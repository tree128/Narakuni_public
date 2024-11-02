using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/Enemy001", fileName = "Enemy001")]
public class Enemy001Parameter : EnemyParameterBase
{
    [Header("y“G001‚ÌHP‚ÍZz"), Range(1, 100)]public int HP;
    [Header("y“G001‚Ìƒ_ƒ[ƒW‚ÍZz"), Range(1, 100)]public int Damage;
}
