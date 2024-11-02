using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsubameWing : MonoBehaviour
{
    [SerializeField] private Tsubame tsubame;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform root;
    [SerializeField] private TsubameRenderer myRenderer;
    [SerializeField] private BoxCollider2D myCollider;
    [SerializeField]private BoxCollider2D rootCollider;
    //[SerializeField] private Color defaultColor;
    //[SerializeField] private Color beforeAttackColor;
    //[SerializeField] private Color attackColor;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌXŠî€Šp“x‚ÍZz"), SerializeField] private float defaultAngle;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌXU‚èã‚°Šp“x‚ÍZz"), SerializeField] private float swingUpAngle;
    [Header("y—ƒ’@‚«‚Â‚¯‚ÌXU‚è‰º‚°Šp“x‚ÍZz"), SerializeField] private float swingDownAngle;
    private bool isAttacking;
    private float elapsedTime;
    private float secondsForSwingUpBeforeAttack;
    private float secondsUntilHitGround;
    private float secondsForWaitOnGround;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌX‰ñ“]‰‘¬“x‚ÍZz"), SerializeField, Range(1f, 100f)] private float firstRotateAngle;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌX‰ñ“]Šp“x‚ÍZz"), SerializeField] private float targetAngle;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌX‰ñ“]Œ¸‘¬ŠJnŠp“x‚ÍZz"), SerializeField] private float decelerationStartAngle;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌX‰ñ“]Œ¸‘¬“x‚ÍZz"), SerializeField, Range(1f, 100f)] private float decelerationAngle;
    private float rotateAngle;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌXU‚è–ß‚µŠÔ‚ÍZ•bz"), SerializeField, Range(0.1f, 5f)] private float secondsForRotate_wingAttackInWater;
    [Header("y—ƒ’@‚«‚Â‚¯…’†‚ÌXU‚è–ß‚µ‰Á‘¬“x‚ÍZz"), SerializeField, Range(1f, 500f)] private float acceleration_Return;
    private bool isReturn = false;

    private void Start()
    {
        if (myCollider.enabled)
        {
            myCollider.enabled = false;
        }
        if (rootCollider.enabled)
        {
            rootCollider.enabled = false;
        }
    }

    private void OnValidate()
    {
        if (root != null && root.rotation.z != defaultAngle)
        {
            root.rotation = Quaternion.Euler(0f, root.eulerAngles.y, defaultAngle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            SwingAttack();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerData.Player.IsPlayerInvincible)
        {
            tsubame.DamageToPlayer(transform.position, 3);
        }
    }

    private void SwingAttack()
    {
        elapsedTime += Time.deltaTime;

        if(0 < elapsedTime && elapsedTime <= secondsForSwingUpBeforeAttack)
        {
            //Debug.Log("U‚èã‚°");
            root.rotation = Quaternion.Lerp(Quaternion.Euler(0f, root.eulerAngles.y, defaultAngle), Quaternion.Euler(0f, root.eulerAngles.y, swingUpAngle), elapsedTime / secondsForSwingUpBeforeAttack);
        }

        if(secondsForSwingUpBeforeAttack < elapsedTime)
        {
            if (myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.attacking)
            {
                myRenderer.StatusChange(TsubameRenderer.Status.attacking);
            }
        }

        if(secondsForSwingUpBeforeAttack < elapsedTime && elapsedTime <= secondsForSwingUpBeforeAttack + secondsUntilHitGround)
        {
            //Debug.Log("U‚è‰º‚ë‚µ");
            if (!myCollider.enabled)
            {
                myCollider.enabled = true;
            }
            /*if (myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.attacking)
            {
                myRenderer.StatusChange(TsubameRenderer.Status.attacking);
            }*/
            root.rotation = Quaternion.Lerp(Quaternion.Euler(0f, root.eulerAngles.y, swingUpAngle), Quaternion.Euler(0f, root.eulerAngles.y, swingDownAngle), (elapsedTime - secondsForSwingUpBeforeAttack) / secondsUntilHitGround);
        }
        // ˆÈ‰º…’†
        if(secondsForSwingUpBeforeAttack + secondsUntilHitGround + secondsForWaitOnGround < elapsedTime && elapsedTime <= secondsForSwingUpBeforeAttack + secondsUntilHitGround + secondsForWaitOnGround + secondsForRotate_wingAttackInWater)
        {
            if (root.localEulerAngles.z <= targetAngle)
            {
                //Debug.Log("…’†P‚è");
                root.rotation = Quaternion.AngleAxis(rotateAngle * Time.deltaTime, Vector3.forward * Mathf.Sign(-root.localPosition.x)) * root.rotation;
                if(decelerationStartAngle <= root.localEulerAngles.z)
                {
                    //Debug.Log("…’†P‚èŒ¸‘¬");
                    rotateAngle = Mathf.Clamp(rotateAngle - decelerationAngle * Time.deltaTime, 0, rotateAngle);
                }
            }

            /*if (myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.attacking)
            {
                myRenderer.StatusChange(TsubameRenderer.Status.attacking);
            }*/
        }
        if (secondsForSwingUpBeforeAttack + secondsUntilHitGround + secondsForWaitOnGround + secondsForRotate_wingAttackInWater < elapsedTime)
        {
            if (!isReturn)
            {
                rotateAngle = 0f;
                isReturn = true;
            }

            /*if(myRenderer.CurrentStatus != TsubameRenderer.Status.damaging && myRenderer.CurrentStatus != TsubameRenderer.Status.attacking)
            {
                myRenderer.StatusChange(TsubameRenderer.Status.attacking);
            }*/

            //Debug.Log("–ß‚é");
            root.rotation = Quaternion.AngleAxis(-rotateAngle * Time.deltaTime, Vector3.forward * Mathf.Sign(-root.localPosition.x)) * root.rotation;
            rotateAngle += acceleration_Return * Time.deltaTime;
            if(root.localEulerAngles.z <= defaultAngle)
            {
                root.rotation = Quaternion.Euler(0f, root.eulerAngles.y, defaultAngle);
                myRenderer.StatusChange(TsubameRenderer.Status.normal);
                myCollider.enabled = false;
                rootCollider.enabled = false;
                isAttacking = false;
                tsubame.WingAttackFinish();
            }
        }
    }

    public void InitSwingAttack(float beforeTime, float untilHitTime, float waitTime)
    {
        secondsForSwingUpBeforeAttack = beforeTime;
        secondsUntilHitGround = untilHitTime;
        secondsForWaitOnGround = waitTime;
        if(myRenderer.CurrentStatus != TsubameRenderer.Status.damaging)
        {
            myRenderer.StatusChange(TsubameRenderer.Status.before);
        }
        rootCollider.enabled = true;
        rotateAngle = firstRotateAngle;
        isReturn = false;
        elapsedTime = 0f;
        isAttacking = true;
    }
}
