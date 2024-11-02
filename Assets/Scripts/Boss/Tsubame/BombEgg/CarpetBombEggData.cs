using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/CarpetBombEggData", fileName = "CarpetBombEggData")]
public class CarpetBombEggData : ScriptableObject
{
    [Header("�y�O�~������X�n�㔚��m���́Z���z"), SerializeField, Range(0f, 100f)] public float Rate;
    [Header("�y�O�~������X���������x�́Z�z"), SerializeField, Range(0.1f, 20f)] public float FirstSpeed_Ground;
    [Header("�y�O�~������X���������x�́Z�z"), SerializeField, Range(0f, 1f)] public float Acceleration;
    [Header("�y�O�~������X�����ō����x�́Z�z"), SerializeField, Range(1f, 20f)] public float MaxSpeed;

    [Space(10f), Header("�n�㔚�������L�̃p�����[�^�[")]
    [Header("�y�O�~������X�_�ŊJ�n���Ԃ́Z�b�z"), SerializeField, Range(0.01f, 5f)] public float SecondsForStartingToBlink_Ground;
    [Header("�y�O�~������X�n�㔚��ʒu�́Z�z"), SerializeField, Range(-3f, 3f)] public float GroundBombYpos;
}
