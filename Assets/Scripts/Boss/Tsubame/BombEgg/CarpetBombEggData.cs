using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/CarpetBombEggData", fileName = "CarpetBombEggData")]
public class CarpetBombEggData : ScriptableObject
{
    [Header("yãO~ÌXnãôm¦ÍZz"), SerializeField, Range(0f, 100f)] public float Rate;
    [Header("yãO~ÌXº¬xÍZz"), SerializeField, Range(0.1f, 20f)] public float FirstSpeed_Ground;
    [Header("yãO~ÌXºÁ¬xÍZz"), SerializeField, Range(0f, 1f)] public float Acceleration;
    [Header("yãO~ÌXºÅ¬xÍZz"), SerializeField, Range(1f, 20f)] public float MaxSpeed;

    [Space(10f), Header("nãôÁLÌp[^[")]
    [Header("yãO~ÌX_ÅJnÔÍZbz"), SerializeField, Range(0.01f, 5f)] public float SecondsForStartingToBlink_Ground;
    [Header("yãO~ÌXnãôÊuÍZz"), SerializeField, Range(-3f, 3f)] public float GroundBombYpos;
}
