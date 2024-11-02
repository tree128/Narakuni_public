using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/Enemy003", fileName = "Enemy003")]
public class Enemy003Parameter : EnemyParameterBase
{
    [Header("�y�G003��HP�́Z�z"), Range(1, 100)] public int HP;
    [Header("�y�G003�̃_���[�W�́Z�z"), Range(1, 100)] public int Damage;
    [Header("�y�G003�̈ړ����x�́Z�z"), Range(1f, 30f)] public float Speed;
    [Header("�y�G003�̉����x�́Z�z"), Range(0.01f, 3f)] public float Acceleration;
    [Header("�y�G003�̍ō����x�́Z�z"), Range(1f, 30f)] public float MaxSpeed;
    [Header("�y�G003�̔����͈͂́Z�z"), Range(0.01f, 5f)] public float SearchRadius;
    [Header("�y�G003�̔����d���́Z�b�z"), Range(0.01f, 5f)] public float FreezeTime_Discover;
    [Header("�y�G003�̏Փˍd���́Z�b�z"), Range(0.01f, 5f)] public float FreezeTime_Crash;
}
