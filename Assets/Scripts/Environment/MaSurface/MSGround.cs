using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSGround : MonoBehaviour
{    
    [SerializeField] private MSParent mSParent;
    [SerializeField] private PlayerData playerData;
    private float surfaceTop;
    public float SurfaceTop => surfaceTop;
    private float surfaceLeftmost;
    private float surfaceRightmost;
    [SerializeField] private BoxCollider2D myCollider;
    [Header("����/�㗤�ł���͈͂̃o�b�t�@"), SerializeField, Range(0f, 0.2f)] private float buffer = 0.2f;
    [SerializeField]private bool onPlayer = false;
    [SerializeField] private MSWater partner;
    private bool isVisible = false;
    public bool IsVisible => isVisible;
    private Vector2 playerColliderSize;
    private float playerColliderOffset;

    // Start is called before the first frame update
    void Start()
    {
        surfaceTop = transform.position.y + transform.localScale.y * 0.5f;
        surfaceLeftmost = transform.position.x - transform.localScale.x * 0.5f;
        surfaceRightmost = transform.position.x + transform.localScale.x * 0.5f;
        // ���Ⴊ��ł��Ȃ���Ԃ̏��łȂ���destination������邽��
        playerColliderSize = playerData.PlayerCollider.size * 0.5f;
        playerColliderOffset = playerData.PlayerCollider.offset.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible || partner.IsVisible)
        {
            if (playerData.Player.IsEntering || playerData.Player.IsGoingUp)
            {
                if (myCollider.enabled && onPlayer)
                {
                    myCollider.enabled = false;
                }
            }
            else
            {
                if (!myCollider.enabled)
                {
                    myCollider.enabled = true;
                }
            }
        }
        else if (myCollider.enabled)
        {
            myCollider.enabled = false;
        }
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && surfaceLeftmost - buffer <= playerData.PlayerTransform.position.x - playerColliderSize.x / 2 && playerData.PlayerTransform.position.x + playerColliderSize.x / 2 <= surfaceRightmost + buffer)
        {
            onPlayer = true;
            if(collision.GetContact(0).normal.y == -1)// ��ɏ���Ă��鎞�ɐ���
            {
                if (!playerData.Player.IsEntering && !playerData.Player.IsGoingUp)
                {
                    playerData.TargetPos_ChangeEnvironment.x = playerData.PlayerTransform.position.x;
                    playerData.TargetPos_ChangeEnvironment.y = partner.SurfaceBottom - playerColliderSize.y - playerColliderOffset;
                    playerData.Player.CanEnterTheWater = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlayer = false;
        playerData.Player.CanEnterTheWater = false;
    }
}
