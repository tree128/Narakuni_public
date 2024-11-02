using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyKnockBackParameter
{
    [Header("y“GƒmƒbƒNƒoƒbƒN‚ÌX‰‘¬“x‚ÍZz"), Range(0.1f, 100f)] public float KnockBackPower;
    [Header("“GƒmƒbƒNƒoƒbƒN(•ÇÛ)‚ÌX‰‘¬“x‚ÍZ"), Range(0.1f, 100f)] public float KnockBackPower_atWall;
    [Header("y“GƒmƒbƒNƒoƒbƒN‚ÌXŠp“x‚ÍZz"), Range(0f, 90f)] public float KnockBackAngle;
    [Header("y“GƒmƒbƒNƒoƒbƒN‚ÌX‘±‚ÍZ•bz"), Range(0.01f, 3f)] public float SecondsForKnockBack;
    [Header("y“GƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬‚ğn‚ß‚éis‹ï‡‚ÍZz"), Range(0.1f, 1f)] public float WeakneningStartRate;
    [Header("y“GƒmƒbƒNƒoƒbƒN‚ÌXŒ¸‘¬‚Ì‹­‚³‚ÍZz"), Range(0.1f, 0.99f)] public float WeakeningPower;
}
