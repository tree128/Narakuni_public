using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/Enemy000", fileName = "Enemy000")]
public class Enemy000Parameter : ScriptableObject
{
    [Header("�G000��X�_���[�W�́Z"), SerializeField, Range(1, 100)] public int Damage;
}
