using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyKnockBackParameter
{
    [Header("�y�G�m�b�N�o�b�N��X�����x�́Z�z"), Range(0.1f, 100f)] public float KnockBackPower;
    [Header("�G�m�b�N�o�b�N(�Ǎ�)��X�����x�́Z"), Range(0.1f, 100f)] public float KnockBackPower_atWall;
    [Header("�y�G�m�b�N�o�b�N��X�p�x�́Z�z"), Range(0f, 90f)] public float KnockBackAngle;
    [Header("�y�G�m�b�N�o�b�N��X�����́Z�b�z"), Range(0.01f, 3f)] public float SecondsForKnockBack;
    [Header("�y�G�m�b�N�o�b�N��X�������n�߂�i�s��́Z�z"), Range(0.1f, 1f)] public float WeakneningStartRate;
    [Header("�y�G�m�b�N�o�b�N��X�����̋����́Z�z"), Range(0.1f, 0.99f)] public float WeakeningPower;
}
