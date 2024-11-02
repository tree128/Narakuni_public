using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy002 : EnemyBase
{
    [SerializeField] private Enemy002Parameter param;
    private Vector3 speed;

    protected override void Start()
    {
        base.Start();
        knockBackParam = param.KnockBackParameters;
        currentHp = param.HP;
        damage = param.Damage;
        speed = new Vector3(-param.Speed, 0, 0);
    }

    protected override void Update()
    {
        base.Update();
        if (onGround)
        {
            transform.position += speed * Time.deltaTime;
        }
    }

    protected override void WallCheck(Collision2D collision)
    {
        base.WallCheck(collision);
        if (atWall)
        {
            if (wallDirection != 2)
            {
                transform.localScale = new Vector2(wallDirection, transform.localScale.y);
                speed = new Vector3(-wallDirection * param.Speed, 0, 0);
            }
            else
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
            atWall = false;
            wallDirection = 0;
        }
    }
}
