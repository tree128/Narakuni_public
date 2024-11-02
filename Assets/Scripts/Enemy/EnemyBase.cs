using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected EnemyKnockBackParameter knockBackParam;
    [SerializeField] protected Rigidbody2D rb;
    protected bool atWall = false;
    /// <summary>
    /// -1:右のみ、0:なし、1:左のみ、2:両方
    /// </summary>
    protected int wallDirection;
    protected bool onGround = false;
    protected bool isFinishedFirstAction = false;
    protected bool isKnockBack = false;
    protected bool isWeakening = false;
    protected float elapsedSeconds_KnockBack;
    protected float knockBackSpeed;
    protected Vector2 knockBackVector;
    protected float attackDirection;
    [SerializeField]protected int currentHp;
    [SerializeField] protected BoxCollider2D myCollider;
    [SerializeField] protected SpriteRenderer myRenderer;
    [SerializeField] protected LayerMask EnvironmentLayer;
    [SerializeField] protected PlayerData playerData;
    [SerializeField] protected EnemyBaseParameter baseParameter;
    protected int damage;
    protected float elapsedTime_Damage;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isKnockBack && isFinishedFirstAction)
        {
            elapsedSeconds_KnockBack += Time.deltaTime;
            if (!isWeakening && elapsedSeconds_KnockBack / knockBackParam.SecondsForKnockBack >= knockBackParam.WeakneningStartRate)
            {
                if (atWall)
                {
                    knockBackSpeed = rb.velocity.y;
                }
                else
                {
                    knockBackSpeed = rb.velocity.x;
                }
                isWeakening = true;
            }
            if (elapsedSeconds_KnockBack >= knockBackParam.SecondsForKnockBack)
            {
                FinishKnockBack();
            }
        }

        if(myRenderer.color == baseParameter.damageColor)
        {
            elapsedTime_Damage += Time.deltaTime;
            if (baseParameter.damageTime <= elapsedTime_Damage)
            {
                myRenderer.color = Color.white;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isKnockBack)
        {
            KnockBack();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        WallCheck(collision);
        if(collision.GetContact(0).normal.y >= 0.95f)
        {
            onGround = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        WallCheck(collision);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.1f), 0, Vector2.down, myCollider.size.y * 0.5f, EnvironmentLayer);
        if (hit.collider == null)
        {
            onGround = false;
        }
    }

    protected virtual void WallCheck(Collision2D collision)
    {
        bool left = false;
        bool right = false;

        // 左チェック
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.1f, myCollider.size.y), 0, Vector2.left, myCollider.size.x * 0.5f, EnvironmentLayer);
        if (hit.collider != null)
        {
            left = true;
        }

        // 右チェック
        hit = Physics2D.BoxCast(transform.position, new Vector2(0.1f, myCollider.size.y), 0, Vector2.right, myCollider.size.x * 0.5f, EnvironmentLayer);
        if (hit.collider != null)
        {
            right = true;
        }

        if (!left && !right)
        {
            atWall = false;
            wallDirection = 0;
        }
        else
        {
            atWall = true;
            if (left && right)
            {
                wallDirection = 2;
            }
            else if (left)
            {
                wallDirection = -1;
            }
            else
            {
                wallDirection = 1;
            }
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            attackDirection = playerData.PlayerTransform.localScale.x;
            if (atWall && (wallDirection == attackDirection || wallDirection == 2))
            {
                // 壁際では真上へ
                knockBackVector = Vector2.up * knockBackParam.KnockBackPower_atWall;
            }
            else
            {
                knockBackVector = Quaternion.Euler(0, 0, knockBackParam.KnockBackAngle * attackDirection) * new Vector2(attackDirection, 0) * knockBackParam.KnockBackPower;
            }
            currentHp -= playerData.AttackPower;
            elapsedTime_Damage = 0f;
            myRenderer.color = baseParameter.damageColor;
            isKnockBack = true;
        }
    }

    protected virtual void KnockBack()
    {
        if (!isFinishedFirstAction)
        {
            //Debug.Log("最初");
            rb.velocity = Vector2.zero;
            rb.AddForce(knockBackVector, ForceMode2D.Impulse);
            isFinishedFirstAction = true;
        }
        else
        {
            if (!isWeakening)
            {
                //Debug.Log("待ち");
            }
            else
            {
                //Debug.Log("減速中");
                knockBackSpeed = knockBackSpeed * (1f - knockBackParam.WeakeningPower);
                if (atWall)
                {
                    rb.velocity = new Vector2(0, knockBackSpeed);
                }
                else
                {
                    rb.velocity = new Vector2(knockBackSpeed, rb.velocity.y);
                }
                if (Mathf.Abs(rb.velocity.x) < 0.1f || Mathf.Abs(rb.velocity.y) < 0.1f)
                {
                    FinishKnockBack();
                }
            }
        }
    }

    protected virtual void FinishKnockBack()
    {
        isFinishedFirstAction = false;
        isWeakening = false;
        isKnockBack = false;
        elapsedSeconds_KnockBack = 0f;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DamageToPlayer()
    {
        playerData.Player.GetDamage(transform.position, damage);
    }
}
