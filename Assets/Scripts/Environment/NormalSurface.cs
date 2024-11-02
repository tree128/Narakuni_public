using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSurface : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private float surfaceTop;
    private float surfaceBottom;
    private float surfaceLeftmost;
    private float surfaceRightmost;
    [SerializeField] private BoxCollider2D myCollider;
    [Header("入水/上陸できる範囲のバッファ"), SerializeField, Range(0f, 0.2f)] private float buffer = 0.2f;
    private bool onPlayer = false;
    private bool isVisible = false;
    private float playerDefaultHeight;
    private float playerDefaultOffset;

    // Start is called before the first frame update
    void Start()
    {
        surfaceTop = transform.position.y + transform.localScale.y / 2;
        surfaceBottom = transform.position.y - transform.localScale.y / 2;
        surfaceLeftmost = transform.position.x - transform.localScale.x / 2;
        surfaceRightmost = transform.position.x + transform.localScale.x / 2;
        playerDefaultHeight = playerData.PlayerCollider.size.y * 0.5f;
        playerDefaultOffset = playerData.PlayerCollider.offset.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
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
        else if(myCollider.enabled)
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
        if (collision.gameObject.tag == "Player" && surfaceLeftmost - buffer <= playerData.PlayerTransform.position.x - playerData.PlayerCollider.size.x / 2 && playerData.PlayerTransform.position.x + playerData.PlayerCollider.size.x / 2 <= surfaceRightmost + buffer)
        {
            onPlayer = true;
            if (!playerData.Player.IsEntering && !playerData.Player.IsGoingUp)
            {
                playerData.TargetPos_ChangeEnvironment.x = playerData.PlayerTransform.position.x;
                if (playerData.Player.CurrentEnvironment == "Ground")
                {
                    playerData.TargetPos_ChangeEnvironment.y = surfaceBottom - playerDefaultHeight - playerDefaultOffset;
                }
                else if (playerData.Player.CurrentEnvironment == "Water")
                {
                    playerData.TargetPos_ChangeEnvironment.y = surfaceTop + playerDefaultHeight - playerDefaultOffset;
                }

                if (playerData.Player.CurrentEnvironment == "Ground")
                {
                    playerData.Player.CanEnterTheWater = true;
                }
                else if (playerData.Player.CurrentEnvironment == "Water")
                {
                    playerData.Player.CanGoUpToGround = true;
                }
            }
        }
        /*if (collision.gameObject.tag == "Player" && surfaceLeftmost - buffer <= playerData.PlayerTransform.position.x - playerData.PlayerCollider.size.x / 2 && playerData.PlayerTransform.position.x + playerData.PlayerCollider.size.x / 2 <= surfaceRightmost + buffer)
        {
            onPlayer = true;
            if(!playerData.Player.IsEntering && !playerData.Player.IsGoingUp)
            {
                if (playerData.Player.CurrentEnvironment == "Ground")
                {
                    destination = new Vector2(playerData.PlayerTransform.position.x, surfaceBottom - playerData.PlayerCollider.size.y / 2 - playerData.PlayerCollider.offset.y);
                }
                else if (playerData.Player.CurrentEnvironment == "Water")
                {
                    destination = new Vector2(playerData.PlayerTransform.position.x, surfaceTop + playerData.PlayerCollider.size.y / 2 - playerData.PlayerCollider.offset.y);
                }

                playerData.Player.TargetPos = destination;

                if(playerData.Player.CurrentEnvironment == "Ground")
                {
                    playerData.Player.CanEnterTheWater = true;
                }
                else if(playerData.Player.CurrentEnvironment == "Water")
                {
                    playerData.Player.CanGoUpToGround = true;
                }
            }
        }*/
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlayer = false;
        playerData.Player.CanEnterTheWater = false;
        playerData.Player.CanGoUpToGround = false;
    }
}
