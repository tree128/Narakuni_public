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
    [Header("【翼叩きつけのX基準角度は〇】"), SerializeField] private float defaultAngle;
    [Header("【翼叩きつけのX振り上げ角度は〇】"), SerializeField] private float swingUpAngle;
    [Header("【翼叩きつけのX振り下げ角度は〇】"), SerializeField] private float swingDownAngle;
    private bool isAttacking;
    private float elapsedTime;
    private float secondsForSwingUpBeforeAttack;
    private float secondsUntilHitGround;
    private float secondsForWaitOnGround;
    [Header("【翼叩きつけ水中のX回転初速度は〇】"), SerializeField, Range(1f, 100f)] private float firstRotateAngle;
    [Header("【翼叩きつけ水中のX回転角度は〇】"), SerializeField] private float targetAngle;
    [Header("【翼叩きつけ水中のX回転減速開始角度は〇】"), SerializeField] private float decelerationStartAngle;
    [Header("【翼叩きつけ水中のX回転減速度は〇】"), SerializeField, Range(1f, 100f)] private float decelerationAngle;
    private float rotateAngle;
    [Header("【翼叩きつけ水中のX振り戻し時間は〇秒】"), SerializeField, Range(0.1f, 5f)] private float secondsForRotate_wingAttackInWater;
    [Header("【翼叩きつけ水中のX振り戻し加速度は〇】"), SerializeField, Range(1f, 500f)] private float acceleration_Return;
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
            //Debug.Log("振り上げ");
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
            //Debug.Log("振り下ろし");
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
        // 以下水中
        if(secondsForSwingUpBeforeAttack + secondsUntilHitGround + secondsForWaitOnGround < elapsedTime && elapsedTime <= secondsForSwingUpBeforeAttack + secondsUntilHitGround + secondsForWaitOnGround + secondsForRotate_wingAttackInWater)
        {
            if (root.localEulerAngles.z <= targetAngle)
            {
                //Debug.Log("水中抉り");
                root.rotation = Quaternion.AngleAxis(rotateAngle * Time.deltaTime, Vector3.forward * Mathf.Sign(-root.localPosition.x)) * root.rotation;
                if(decelerationStartAngle <= root.localEulerAngles.z)
                {
                    //Debug.Log("水中抉り減速");
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

            //Debug.Log("戻る");
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
