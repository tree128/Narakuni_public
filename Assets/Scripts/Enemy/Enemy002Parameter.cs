using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/Enemy002", fileName = "Enemy002")]
public class Enemy002Parameter : EnemyParameterBase
{
    [Header("y“G002‚ÌHP‚ÍZz"), Range(1, 100)] public int HP;
    [Header("y“G002‚Ìƒ_ƒ[ƒW‚ÍZz"), Range(1, 100)] public int Damage;
    [Header("y“G002‚ÌˆÚ“®‘¬“x‚ÍZz"), Range(1f, 30f)] public float Speed;
}
