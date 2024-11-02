using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirinn : MonoBehaviour
{
    [Header("HP")]
    [Header("yƒLƒŠƒ“‚ÌX‘Ì—Í‚ÍZz"), SerializeField, Range(0, 1000)] private int currentHP;
    private bool isDamage = false;
    private float elapsedTime_Damage = 0f;

    [Space(20f), Header("UŒ‚‚PuˆÚ“®v")]
    [Header("yUŒ‚‚P‚ÌX‰ÁŒ¸‘¬“x‚ÍZz"), SerializeField, Range(0.01f, 10f)] private float acceleration;
    private Vector2 moveVelocity = Vector2.zero;
    [Header("yUŒ‚‚P‚ÌX‰ÁŒ¸‘¬’n“_‚ÍZz"), SerializeField, Range(0, 100)] private float speedKeepPosAbs;
    [Header("yUŒ‚‚P‚ÌX‰æ–ÊŠOˆÚ“®’n“_‚ÍZz"), SerializeField, Range(0, 200)] private float translatePosAbs;

    [SerializeField] private Transform myTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer[] renderers;
    [SerializeField] private Color defaultColor_Body;
    [SerializeField] private Color defaultColor_Other;
    [SerializeField] private KirinnParameter param;
    [SerializeField] private EnemyBaseParameter baseParam;
    [SerializeField] private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamage)
        {
            elapsedTime_Damage += Time.deltaTime;
            if(baseParam.damageTime <= elapsedTime_Damage)
            {
                foreach (var item in renderers)
                {
                    if(item.name == gameObject.name)
                    {
                        item.color = defaultColor_Body;
                    }
                    else
                    {
                        item.color = defaultColor_Other;
                    }
                }
                elapsedTime_Damage = 0f;
                isDamage = false;
            }
        }
    }

    private void FixedUpdate()
    {
        MoveAttack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerData.Player.GetDamage(transform.position, param.MoveDamage);
        }

        if (collision.CompareTag("PlayerAttack"))
        {
            currentHP = Mathf.Clamp(currentHP - playerData.AttackPower, 0, currentHP);
            foreach (var item in renderers)
            {
                item.color = baseParam.damageColor;
            }
            isDamage = true;
            if(currentHP == 0)
            {
                GameManager.Instance.GameClear();
            }
        }
    }

    private void MoveAttack()
    {
        if(moveVelocity.x != 0)
        {
            moveVelocity.x += acceleration * Mathf.Sign(moveVelocity.x);
        }
        else
        {
            moveVelocity.x += acceleration * Mathf.Sign(-myTransform.position.x);
        }
        rb.velocity = moveVelocity;
    }
}
