using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/BombEggData", fileName = "BombEggData")]
public class BombEggData : ScriptableObject
{
    [Header("������")]
    public Sprite EggSprite;
    public Sprite BombSprite;
    public Color DefaultColor = Color.white;
    public Color BombColor = Color.red;
    [Header("��������X�f�t�H���g�T�C�Y�́Z"), SerializeField, Range(0.1f, 10f)] public float DefaultScale;
    [Header("�y��������X�G�t�F�N�g�T�C�Y�́Z�z"), SerializeField, Range(0.1f, 10f)] public float EffectSize;

    [Space(10f), Header("�_���[�W����")]
    [Header("��������X�����O�͈͂́Z"), SerializeField, Range(0.1f, 5f)] public float DefaultRadius;
    [Header("�y��������X�����͈͂́Z�z"), SerializeField, Range(0.1f, 5f)] public float BombRadius;

    [Space(10f), Header("����")]
    [Header("�y��������X����(X���x)�́Z�z"), SerializeField, Range(0.01f, 10f)] public float SpeedX_Ground;
    [Header("�y��������X����(Y�����x)�́Z�z"), SerializeField, Range(1f, 30f)] public float ShootFirstSpeedY;
    [Header("�y��������X����(Y���x)�́Z�z"), SerializeField, Range(0.01f, 10f)] public float ShootSpeedYDeceleration;
    [Header("�y��������X���ˉ�]�́Z�z"), SerializeField, Range(0f, 90f)] public float ShootRotationSpeed;
    
    [Space(10f), Header("���e")]
    [Header("�y������<color=red>A</color>��X���e�ʒu(X���W)�́Z�z"), SerializeField, Range(-5f, 5f)] public float ImpactXPos_A;
    [Header("�y������<color=red>A</color>��X���e�ʒu(X���W)�ϓ��́Z�z"), SerializeField, Range(-100f, 100f)] public float ImpactXPosAnother_A;
    [Header("�y��������X���e�ʒu(X���W)�́Z�z"), SerializeField, Range(-5f, 5f)] public float ImpactXPos;
    [Header("�y��������X���e�ʒu(X���W)�ϓ��́Z�z"), SerializeField, Range(-100f, 100f)] public float ImpactXPosAnother;
    [Header("�y������<color=red>A</color>��X���e�ʒu(X���W)�v���X�[�́Z�z"), SerializeField, Range(0f, 30f)] public float ImpactMaxDistance_A;
    [Header("��������X���e�ʒu(X���W)�}�C�i�X�[�́Z"), SerializeField, Range(0f, 30f)] public float ImpactMinDistance;
    [HideInInspector]public Transform ImpactBlocker_Left;
    [HideInInspector]public Transform ImpactBlocker_Right;
    [Header("�y��������X���e�ʒu(Y���W)�́Z�z"), SerializeField, Range(-30f, 30f)] public float ImpactYPos;

    [Space(10f), Header("���e")]
    [Header("�y��������X���e(X���x)�́Z�z"), SerializeField, Range(0.01f, 10f)] public float SpeedX_Rebound;
    [Header("�y��������X���e(Y�����x)�́Z�z"), SerializeField, Range(1f, 30f)] public float ReboundFirstSpeed;
    [Header("�y��������X���e(Y���x)�́Z�z"), SerializeField, Range(0.01f, 10f)] public float ReboundSpeedYDeceleration;
    [Header("�y��������X���e��]�́Z�z"), SerializeField, Range(0f, 90f)] public float ReboundRotationSpeed;
    
    [Space(10f), Header("����")]
    [Header("�y��������X�����ʒu(X���W)�́Z�z"), SerializeField, Range(-5f, 5f)] public float FallWaterXPos;
    [Header("�y��������X�����ʒu(X���W)�ϓ��́Z�z"), SerializeField, Range(-100f, 100f)] public float FallWaterXPosAnother;
    [Header("�y��������X�����ʒu(X���W)�}�C�i�X�[�́Z�z"), SerializeField, Range(0f, 10f)] public float FallWaterXPosShort;
    [Header("�y��������X�����ʒu(X���W)�v���X�[�́Z�z"), SerializeField, Range(0f, 20f)] public float FallWaterXPosLong;
    [Header("�y��������X�����ʒu(Y���W)�́Z�z"), SerializeField, Range(-30, 30)] public float FallWaterYPos;
    [Header("�y��������X����(X���x)�́Z���z"), SerializeField, Range(0f, 100f)] public float WaterSpeedXRate;
    [Header("�y��������X����(X�����x)�́Z�z"), SerializeField, Range(0.001f, 1f)] public float WaterSpeedXDeceleration;
    [Header("�y��������X����(Y�����x)�́Z�z"), SerializeField, Range(0.01f, 10f)] public float WaterFirstSpeedY;
    [Header("�y��������X����(Y����)�J�n�́Z�b�z"), SerializeField, Range(0.01f, 5f)] public float SecondsForWaterDeceleration;
    [Header("�y��������X����(Y�����x)�́Z�z"), SerializeField, Range(0.001f, 1f)] public float WaterSpeedYDeceleration;
    [Header("�y��������X����(Y�����x)�ϓ��́Z�z"), SerializeField, Range(0.001f, 1f)] public float WaterSpeedYDecelerationAnother;
    [Header("�y��������X������]�́Z�z"), SerializeField, Range(0f, 90f)] public float WaterRotationSpeed;
    
    [Space(10f), Header("�_��")]
    [Header("�y��������X�_�ŊJ�n���Ԃ́Z�b�z"), SerializeField, Range(0f, 10f)] public float SecondsForStartingToBlink;
    [Header("�y�������̓_�łP�i�K�ڂ�X�_�Œ����Ԃ́Z�b�z"), SerializeField, Range(0.01f, 3f)] public float SecondsForFirstBlink;
    [Header("�y�������̓_�łP�i�K�ڂ�X�ʏ펞�Ԃ́Z�b�z"), SerializeField, Range(0.01f, 3f)] public float SecondsForFirstDefault;
    [Header("�y�������̓_�łP�i�K�ڂ�X�񐔂́Z�z"), SerializeField, Range(1, 10)] public int BlinkNum;
    [Header("�y�������̓_�łQ�i�K�ڂ�X�_�Œ����Ԃ́Z�b�z"), SerializeField, Range(0.01f, 3f)] public float SecondsForSecondBlink;
    [Header("�y�������̓_�łQ�i�K�ڂ�X�ʏ펞�Ԃ́Z�b�z"), SerializeField, Range(0.01f, 3f)] public float SecondsForSecondDefault;

    [Space(10f), Header("����")]
    [Header("�y��������X���􎞊Ԃ́Z�b�z"), SerializeField, Range(0.01f, 5f)] public float SecondsForBomb;
    [Header("�y��������X���􎝑��́Z�b�z"), SerializeField, Range(0.01f, 3f)] public float SecondsForKeepingDamage;
    [Header("�y��������X�G�t�F�N�g�t�F�[�h�A�E�g���Ԃ́Z�b�z"), SerializeField, Range(0.01f, 5f)] public float SecondsForFadeOut;
}
