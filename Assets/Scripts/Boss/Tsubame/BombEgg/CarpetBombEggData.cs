using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/CarpetBombEggData", fileName = "CarpetBombEggData")]
public class CarpetBombEggData : ScriptableObject
{
    [Header("yãOŸ~”šŒ‚‚ÌX’nã”š—ôŠm—¦‚ÍZ“z"), SerializeField, Range(0f, 100f)] public float Rate;
    [Header("yãOŸ~”šŒ‚‚ÌX—‰º‰‘¬“x‚ÍZz"), SerializeField, Range(0.1f, 20f)] public float FirstSpeed_Ground;
    [Header("yãOŸ~”šŒ‚‚ÌX—‰º‰Á‘¬“x‚ÍZz"), SerializeField, Range(0f, 1f)] public float Acceleration;
    [Header("yãOŸ~”šŒ‚‚ÌX—‰ºÅ‚‘¬“x‚ÍZz"), SerializeField, Range(1f, 20f)] public float MaxSpeed;

    [Space(10f), Header("’nã”š—ô—‘“Á—L‚Ìƒpƒ‰ƒ[ƒ^[")]
    [Header("yãOŸ~”šŒ‚‚ÌX“_–ÅŠJnŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] public float SecondsForStartingToBlink_Ground;
    [Header("yãOŸ~”šŒ‚‚ÌX’nã”š—ôˆÊ’u‚ÍZz"), SerializeField, Range(-3f, 3f)] public float GroundBombYpos;
}
