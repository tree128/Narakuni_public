using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/Enemy003", fileName = "Enemy003")]
public class Enemy003Parameter : EnemyParameterBase
{
    [Header("yG003ĚHPÍZz"), Range(1, 100)] public int HP;
    [Header("yG003Ě_[WÍZz"), Range(1, 100)] public int Damage;
    [Header("yG003ĚÚŽŹxÍZz"), Range(1f, 30f)] public float Speed;
    [Header("yG003ĚÁŹxÍZz"), Range(0.01f, 3f)] public float Acceleration;
    [Header("yG003ĚĹŹxÍZz"), Range(1f, 30f)] public float MaxSpeed;
    [Header("yG003Ě˝ÍÍÍZz"), Range(0.01f, 5f)] public float SearchRadius;
    [Header("yG003Ě˝dźÍZbz"), Range(0.01f, 5f)] public float FreezeTime_Discover;
    [Header("yG003ĚŐËdźÍZbz"), Range(0.01f, 5f)] public float FreezeTime_Crash;
}
