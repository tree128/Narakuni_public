using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D myCollider;
    [SerializeField] private Sprite defaultAttackSprite;
    [SerializeField] private Sprite squatAttackSprite;
    [SerializeField] private PlayerData playerData;
    private bool isUpOrDownAttacking;
    private Vector2 attackDirection;
    private Vector3 defaultPlayerOffset;
    [Header("UŒ‚‰¡‚ÆUŒ‚ã‚ÌXÕ“Ë”ÍˆÍ‚ÍZ"), SerializeField] private Vector2 colliderSize_default;
    [Header("UŒ‚‚µ‚á‚ª‚İ‚ÌXÕ“Ë”ÍˆÍ‚ÍZ"), SerializeField] private Vector2 colliderSize_squat;

    // FrontAttack
    [Header("yUŒ‚‰¡‚ÌX(XÀ•W)‚ÍZz"), SerializeField] private float xAxis_FrontAttack;
    [Header("yUŒ‚‰¡‚ÌX(YÀ•W)‚ÍZz"), SerializeField] private float yAxis_FrontAttack;
    [Header("yUŒ‚‰¡‚ÌX•‚ÍZz"), SerializeField, Range(0f, 3f)] private float width_FrontAttack;
    [Header("yUŒ‚‰¡‚ÌXƒŠ[ƒ`‚ÍZz"), SerializeField, Range(0f, 3f)] private float reach_FrontAttack;
    private Vector3 frontAttackScale;

    // UpAttack
    [Header("yUŒ‚ã‚ÌX(XÀ•W)‚ÍZz"), SerializeField] private float xAxis_UpAttack;
    [Header("yUŒ‚ã‚ÌX(YÀ•W)‚ÍZz"), SerializeField] private float yAxis_UpAttack;
    [Header("yUŒ‚ã‚ÌX•‚ÍZz"), SerializeField, Range(0f, 3f)] private float width_UpAttack;
    [Header("yUŒ‚ã‚ÌXƒŠ[ƒ`‚ÍZz"), SerializeField, Range(0f, 3f)] private float reach_UpAttack;
    private Vector3 upAttackScale;

    // SquatAttack
    [Header("yUŒ‚‚µ‚á‚ª‚İ‚ÌX(XÀ•W)‚ÍZz"), SerializeField] private float xAxis_SquatAttack;
    [Header("yUŒ‚‚µ‚á‚ª‚İ‚ÌX(YÀ•W)‚ÍZz"), SerializeField] private float yAxys_SquatAttack;
    [Header("yUŒ‚‚µ‚á‚ª‚İ‚ÌX•‚ÍZz"), SerializeField, Range(0f, 3f)] private float width_SquatAttack;
    [Header("yUŒ‚‚µ‚á‚ª‚İ‚ÌXƒŠ[ƒ`‚ÍZz"), SerializeField, Range(0f, 3f)] private float reach_SquatAttack;
    private Vector3 squatAttackScale;

    // DownAttack
    [Header("y‹ó’†UŒ‚‰º‚ÌX(XÀ•W)‚ÍZz"), SerializeField] private float xAxis_DownAttack;
    [Header("y‹ó’†UŒ‚‰º‚ÌX(YÀ•W)‚ÍZz"), SerializeField] private float yAxis_DownAttack;
    [Header("y‹ó’†UŒ‚‰º‚ÌX•‚ÍZz"), SerializeField, Range(0f, 3f)] private float width_DownAttack;
    [Header("y‹ó’†UŒ‚‰º‚ÌXƒŠ[ƒ`‚ÍZz"), SerializeField, Range(0f, 3f)] private float reach_DownAttack;
    private Vector3 downAttackScale;

    // WaterAttack
    [Header("y…’†UŒ‚‚ÌX(XÀ•W)‚ÍZz"), SerializeField] private float xAxis_WaterAttack;
    [Header("y…’†UŒ‚‚ÌX(YÀ•W)‚ÍZz"), SerializeField] private float yAxis_WaterAttack;
    [Header("y…’†UŒ‚‚ÌX•‚ÍZz"), SerializeField, Range(0f, 3f)] private float width_WaterAttack;
    [Header("y…’†UŒ‚‚ÌXƒŠ[ƒ`‚ÍZz"), SerializeField, Range(0f, 3f)] private float reach_WaterAttack;
    private Vector3 waterAttackPos;
    private Vector3 waterAttackScale;
    private bool isWaterAttacking = false;

    private void OnValidate()
    {
        frontAttackScale = new Vector3(reach_FrontAttack, width_FrontAttack);
        upAttackScale = new Vector3(reach_UpAttack, width_UpAttack);
        squatAttackScale = new Vector3(reach_SquatAttack, width_SquatAttack);
        downAttackScale = new Vector3(reach_DownAttack, width_DownAttack);
        waterAttackPos = new Vector3(xAxis_WaterAttack, yAxis_WaterAttack);
        waterAttackScale = new Vector3(reach_WaterAttack, width_WaterAttack);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
        defaultPlayerOffset = playerData.PlayerCollider.offset;
        frontAttackScale = new Vector3(reach_FrontAttack, width_FrontAttack);
        upAttackScale = new Vector3(reach_UpAttack, width_UpAttack);
        squatAttackScale = new Vector3(reach_SquatAttack, width_SquatAttack);
        downAttackScale = new Vector3(reach_DownAttack, width_DownAttack);
        waterAttackPos = new Vector3(xAxis_WaterAttack, yAxis_WaterAttack);
        waterAttackScale = new Vector3(reach_WaterAttack, width_WaterAttack);
        FinishAttacking();
    }

    private void Update()
    {
        if (myRenderer.enabled)
        {
            if (isWaterAttacking)
            {
                transform.position = playerData.PlayerTransform.position + defaultPlayerOffset + waterAttackPos;
            }
            else if(attackDirection.x != playerData.PlayerTransform.localScale.x)
            {
                if (isUpOrDownAttacking)
                {
                    transform.localScale = new Vector3(1, -transform.localScale.y, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                }
                attackDirection.x = playerData.PlayerTransform.localScale.x;
            }
        }
    }

    public void FrontAttack()
    {
        transform.position = playerData.PlayerTransform.position + defaultPlayerOffset + new Vector3(xAxis_FrontAttack * playerData.PlayerTransform.localScale.x, yAxis_FrontAttack);
        transform.localScale = frontAttackScale;
        attackDirection.x = playerData.PlayerTransform.localScale.x;
        myRenderer.enabled = true;
        myCollider.enabled = true;
    }

    public void UpAttack()
    {
        transform.localRotation = Quaternion.Euler(0, 180, 90);
        transform.position = playerData.PlayerTransform.position + defaultPlayerOffset + new Vector3(xAxis_UpAttack * playerData.PlayerTransform.localScale.x, yAxis_UpAttack);
        transform.localScale = upAttackScale;
        attackDirection.x = playerData.PlayerTransform.localScale.x;
        isUpOrDownAttacking = true;
        myRenderer.enabled = true;
        myCollider.enabled = true;
    }

    public void SquatAttack()
    {
        transform.position = playerData.PlayerTransform.position + (Vector3)playerData.PlayerCollider.offset + new Vector3(xAxis_SquatAttack * playerData.PlayerTransform.localScale.x, yAxys_SquatAttack);
        transform.localScale = squatAttackScale;
        myCollider.size = colliderSize_squat;
        myRenderer.sprite = squatAttackSprite;
        attackDirection.x = playerData.PlayerTransform.localScale.x;
        myRenderer.enabled = true;
        myCollider.enabled = true;
    }

    public void DownAttack()
    {
        transform.localRotation = Quaternion.Euler(0, 0, -90);
        transform.position = playerData.PlayerTransform.position + defaultPlayerOffset + new Vector3(xAxis_DownAttack * playerData.PlayerTransform.localScale.x, yAxis_DownAttack);
        transform.localScale = downAttackScale;
        attackDirection.x = playerData.PlayerTransform.localScale.x;
        isUpOrDownAttacking = true;
        myRenderer.enabled = true;
        myCollider.enabled = true;
    }

    public void WaterAttack()
    {
        isWaterAttacking = true;
        transform.position = playerData.PlayerTransform.position + defaultPlayerOffset + waterAttackPos;
        transform.localScale = waterAttackScale;
        transform.rotation = playerData.PlayerTransform.rotation;
        transform.parent = null;
        myRenderer.enabled = true;
        myCollider.enabled = true;
    }

    public void FinishAttacking()
    {
        myRenderer.enabled = false;
        myCollider.enabled = false;
        attackDirection = Vector2.zero;
        if(myRenderer.sprite == squatAttackSprite)
        {
            myRenderer.sprite = defaultAttackSprite;
        }
        if (isWaterAttacking)
        {
            isWaterAttacking = false;
            transform.parent = playerData.PlayerTransform;
        }
        if (myCollider.size != colliderSize_default)
        {
            myCollider.size = colliderSize_default;
        }
        if(transform.localRotation != Quaternion.identity)
        {
            transform.localRotation = Quaternion.identity;
        }
        if (isUpOrDownAttacking)
        {
            isUpOrDownAttacking = false;
        }
    }
}
