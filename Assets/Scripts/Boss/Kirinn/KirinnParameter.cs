using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/KirinnParameter", fileName = "KirinnParameter")]
public class KirinnParameter : ScriptableObject
{
    [Header("�U���P���Q��X�_���[�W�́Z"), SerializeField, Range(1, 100)] private int moveDamage;
    public int MoveDamage => moveDamage;
}
