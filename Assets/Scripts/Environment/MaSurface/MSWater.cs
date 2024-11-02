using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSWater : MonoBehaviour
{    
    [SerializeField] private MSParent mSParent;
    [SerializeField] private PlayerData playerData;
    private float surfaceBottom;
    public float SurfaceBottom => surfaceBottom;
    private float surfaceLeftmost;
    private float surfaceRightmost;
    [SerializeField] private BoxCollider2D myCollider;
    [Header("入水/上陸できる範囲のバッファ"), SerializeField, Range(0f, 0.2f)] private float buffer = 0.2f;
    [SerializeField]private bool onPlayer = false;
    [SerializeField] private MSGround partner;
    private bool isVisible = false;
    public bool IsVisible => isVisible;

    // Start is called before the first frame update
    void Start()
    {
        surfaceBottom = transform.position.y - transform.localScale.y * 0.5f;
        surfaceLeftmost = transform.position.x - transform.localScale.x * 0.5f;
        surfaceRightmost = transform.position.x + transform.localScale.x * 0.5f;
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
        if (collision.gameObject.CompareTag("Player") && surfaceLeftmost - buffer <= playerData.PlayerTransform.position.x - playerData.PlayerCollider.size.x / 2 && playerData.PlayerTransform.position.x + playerData.PlayerCollider.size.x / 2 <= surfaceRightmost + buffer)
        {
            onPlayer = true;
            if (0.5f <= collision.GetContact(0).normal.y)// 下から当たっている時に制限
            {
                if (!playerData.Player.IsEntering && !playerData.Player.IsGoingUp)
                {
                    playerData.TargetPos_ChangeEnvironment.x = playerData.PlayerTransform.position.x;
                    playerData.TargetPos_ChangeEnvironment.y = partner.SurfaceTop + playerData.PlayerCollider.size.y * 0.5f - playerData.PlayerCollider.offset.y;
                    playerData.Player.CanGoUpToGround = true;
                }
            }
            /*
            if(collision.GetContact(0).normal.y == 1)// 下から当たっている時に制限
            {
                if (!playerData.Player.IsEntering && !playerData.Player.IsGoingUp)
                {
                    playerData.TargetPos_ChangeEnvironment.x = playerData.PlayerTransform.position.x;
                    playerData.TargetPos_ChangeEnvironment.y = partner.SurfaceTop + playerData.PlayerCollider.size.y * 0.5f - playerData.PlayerCollider.offset.y;
                    playerData.Player.CanGoUpToGround = true;
                }
            }
            */
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlayer = false;
        playerData.Player.CanGoUpToGround = false;
    }
}
