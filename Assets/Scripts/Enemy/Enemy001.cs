using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy001 : EnemyBase
{
    [SerializeField] private Enemy001Parameter param;

    protected override void Start()
    {
        base.Start();
        knockBackParam = param.KnockBackParameters;
        currentHp = param.HP;
        damage = param.Damage;
    }
}
