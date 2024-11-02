using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/Enemy003", fileName = "Enemy003")]
public class Enemy003Parameter : EnemyParameterBase
{
    [Header("y“G003‚ÌHP‚ÍZz"), Range(1, 100)] public int HP;
    [Header("y“G003‚Ìƒ_ƒ[ƒW‚ÍZz"), Range(1, 100)] public int Damage;
    [Header("y“G003‚ÌˆÚ“®‘¬“x‚ÍZz"), Range(1f, 30f)] public float Speed;
    [Header("y“G003‚Ì‰Á‘¬“x‚ÍZz"), Range(0.01f, 3f)] public float Acceleration;
    [Header("y“G003‚ÌÅ‚‘¬“x‚ÍZz"), Range(1f, 30f)] public float MaxSpeed;
    [Header("y“G003‚Ì”½‰”ÍˆÍ‚ÍZz"), Range(0.01f, 5f)] public float SearchRadius;
    [Header("y“G003‚Ì”½‰d’¼‚ÍZ•bz"), Range(0.01f, 5f)] public float FreezeTime_Discover;
    [Header("y“G003‚ÌÕ“Ëd’¼‚ÍZ•bz"), Range(0.01f, 5f)] public float FreezeTime_Crash;
}
