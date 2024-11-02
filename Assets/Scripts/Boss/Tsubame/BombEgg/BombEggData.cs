using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/BombEggData", fileName = "BombEggData")]
public class BombEggData : ScriptableObject
{
    [Header("Œ©‚½–Ú")]
    public Sprite EggSprite;
    public Sprite BombSprite;
    public Color DefaultColor = Color.white;
    public Color BombColor = Color.red;
    [Header("”š—ô—‘‚ÌXƒfƒtƒHƒ‹ƒgƒTƒCƒY‚ÍZ"), SerializeField, Range(0.1f, 10f)] public float DefaultScale;
    [Header("y”š—ô—‘‚ÌXƒGƒtƒFƒNƒgƒTƒCƒY‚ÍZz"), SerializeField, Range(0.1f, 10f)] public float EffectSize;

    [Space(10f), Header("ƒ_ƒ[ƒW”»’è")]
    [Header("”š—ô—‘‚ÌX”š—ô‘O”ÍˆÍ‚ÍZ"), SerializeField, Range(0.1f, 5f)] public float DefaultRadius;
    [Header("y”š—ô—‘‚ÌX”š—ô”ÍˆÍ‚ÍZz"), SerializeField, Range(0.1f, 5f)] public float BombRadius;

    [Space(10f), Header("”­Ë")]
    [Header("y”š—ô—‘‚ÌX”­Ë(X‘¬“x)‚ÍZz"), SerializeField, Range(0.01f, 10f)] public float SpeedX_Ground;
    [Header("y”š—ô—‘‚ÌX”­Ë(Y‰‘¬“x)‚ÍZz"), SerializeField, Range(1f, 30f)] public float ShootFirstSpeedY;
    [Header("y”š—ô—‘‚ÌX”­Ë(Y‘¬“x)‚ÍZz"), SerializeField, Range(0.01f, 10f)] public float ShootSpeedYDeceleration;
    [Header("y”š—ô—‘‚ÌX”­Ë‰ñ“]‚ÍZz"), SerializeField, Range(0f, 90f)] public float ShootRotationSpeed;
    
    [Space(10f), Header("’…’e")]
    [Header("y”š—ô—‘<color=red>A</color>‚ÌX’…’eˆÊ’u(XÀ•W)‚ÍZz"), SerializeField, Range(-5f, 5f)] public float ImpactXPos_A;
    [Header("y”š—ô—‘<color=red>A</color>‚ÌX’…’eˆÊ’u(XÀ•W)•Ï“®‚ÍZz"), SerializeField, Range(-100f, 100f)] public float ImpactXPosAnother_A;
    [Header("y”š—ô—‘‚ÌX’…’eˆÊ’u(XÀ•W)‚ÍZz"), SerializeField, Range(-5f, 5f)] public float ImpactXPos;
    [Header("y”š—ô—‘‚ÌX’…’eˆÊ’u(XÀ•W)•Ï“®‚ÍZz"), SerializeField, Range(-100f, 100f)] public float ImpactXPosAnother;
    [Header("y”š—ô—‘<color=red>A</color>‚ÌX’…’eˆÊ’u(XÀ•W)ƒvƒ‰ƒX’[‚ÍZz"), SerializeField, Range(0f, 30f)] public float ImpactMaxDistance_A;
    [Header("”š—ô—‘‚ÌX’…’eˆÊ’u(XÀ•W)ƒ}ƒCƒiƒX’[‚ÍZ"), SerializeField, Range(0f, 30f)] public float ImpactMinDistance;
    [HideInInspector]public Transform ImpactBlocker_Left;
    [HideInInspector]public Transform ImpactBlocker_Right;
    [Header("y”š—ô—‘‚ÌX’…’eˆÊ’u(YÀ•W)‚ÍZz"), SerializeField, Range(-30f, 30f)] public float ImpactYPos;

    [Space(10f), Header("’µ’e")]
    [Header("y”š—ô—‘‚ÌX’µ’e(X‘¬“x)‚ÍZz"), SerializeField, Range(0.01f, 10f)] public float SpeedX_Rebound;
    [Header("y”š—ô—‘‚ÌX’µ’e(Y‰‘¬“x)‚ÍZz"), SerializeField, Range(1f, 30f)] public float ReboundFirstSpeed;
    [Header("y”š—ô—‘‚ÌX’µ’e(Y‘¬“x)‚ÍZz"), SerializeField, Range(0.01f, 10f)] public float ReboundSpeedYDeceleration;
    [Header("y”š—ô—‘‚ÌX’µ’e‰ñ“]‚ÍZz"), SerializeField, Range(0f, 90f)] public float ReboundRotationSpeed;
    
    [Space(10f), Header("—…")]
    [Header("y”š—ô—‘‚ÌX—…ˆÊ’u(XÀ•W)‚ÍZz"), SerializeField, Range(-5f, 5f)] public float FallWaterXPos;
    [Header("y”š—ô—‘‚ÌX—…ˆÊ’u(XÀ•W)•Ï“®‚ÍZz"), SerializeField, Range(-100f, 100f)] public float FallWaterXPosAnother;
    [Header("y”š—ô—‘‚ÌX—…ˆÊ’u(XÀ•W)ƒ}ƒCƒiƒX’[‚ÍZz"), SerializeField, Range(0f, 10f)] public float FallWaterXPosShort;
    [Header("y”š—ô—‘‚ÌX—…ˆÊ’u(XÀ•W)ƒvƒ‰ƒX’[‚ÍZz"), SerializeField, Range(0f, 20f)] public float FallWaterXPosLong;
    [Header("y”š—ô—‘‚ÌX—…ˆÊ’u(YÀ•W)‚ÍZz"), SerializeField, Range(-30, 30)] public float FallWaterYPos;
    [Header("y”š—ô—‘‚ÌX—…(X‘¬“x)‚ÍZ“z"), SerializeField, Range(0f, 100f)] public float WaterSpeedXRate;
    [Header("y”š—ô—‘‚ÌX—…(XŒ¸‘¬“x)‚ÍZz"), SerializeField, Range(0.001f, 1f)] public float WaterSpeedXDeceleration;
    [Header("y”š—ô—‘‚ÌX—…(Y‰‘¬“x)‚ÍZz"), SerializeField, Range(0.01f, 10f)] public float WaterFirstSpeedY;
    [Header("y”š—ô—‘‚ÌX—…(YŒ¸‘¬)ŠJn‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] public float SecondsForWaterDeceleration;
    [Header("y”š—ô—‘‚ÌX—…(YŒ¸‘¬“x)‚ÍZz"), SerializeField, Range(0.001f, 1f)] public float WaterSpeedYDeceleration;
    [Header("y”š—ô—‘‚ÌX—…(YŒ¸‘¬“x)•Ï“®‚ÍZz"), SerializeField, Range(0.001f, 1f)] public float WaterSpeedYDecelerationAnother;
    [Header("y”š—ô—‘‚ÌX—…‰ñ“]‚ÍZz"), SerializeField, Range(0f, 90f)] public float WaterRotationSpeed;
    
    [Space(10f), Header("“_–Å")]
    [Header("y”š—ô—‘‚ÌX“_–ÅŠJnŠÔ‚ÍZ•bz"), SerializeField, Range(0f, 10f)] public float SecondsForStartingToBlink;
    [Header("y”š—ô—‘‚Ì“_–Å‚P’iŠK–Ú‚ÌX“_–Å’†ŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] public float SecondsForFirstBlink;
    [Header("y”š—ô—‘‚Ì“_–Å‚P’iŠK–Ú‚ÌX’ÊíŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] public float SecondsForFirstDefault;
    [Header("y”š—ô—‘‚Ì“_–Å‚P’iŠK–Ú‚ÌX‰ñ”‚ÍZz"), SerializeField, Range(1, 10)] public int BlinkNum;
    [Header("y”š—ô—‘‚Ì“_–Å‚Q’iŠK–Ú‚ÌX“_–Å’†ŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] public float SecondsForSecondBlink;
    [Header("y”š—ô—‘‚Ì“_–Å‚Q’iŠK–Ú‚ÌX’ÊíŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] public float SecondsForSecondDefault;

    [Space(10f), Header("”š”­")]
    [Header("y”š—ô—‘‚ÌX”š—ôŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] public float SecondsForBomb;
    [Header("y”š—ô—‘‚ÌX”š—ô‘±‚ÍZ•bz"), SerializeField, Range(0.01f, 3f)] public float SecondsForKeepingDamage;
    [Header("y”š—ô—‘‚ÌXƒGƒtƒFƒNƒgƒtƒF[ƒhƒAƒEƒgŠÔ‚ÍZ•bz"), SerializeField, Range(0.01f, 5f)] public float SecondsForFadeOut;
}
