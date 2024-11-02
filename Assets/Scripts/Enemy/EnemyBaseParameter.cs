using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "MyScriptableObjects/EnemyBase", fileName = "EnemyBase")]
public class EnemyBaseParameter : ScriptableObject
{
    [Header("�G��X���A�N�V�����F�́Z")] public Color damageColor;
    [Header("�y�G��X���A�N�V�����F�ύX���Ԃ́Z�b�z"), Range(0.01f, 1f)] public float damageTime;

#if UNITY_EDITOR
    public static EnemyBaseParameter GetEnemyBaseParameterAsset()
    {
        string enemyBaseParameterGUID = AssetDatabase.FindAssets("EnemyBase", new string[1] { "Assets/Data/EnemyData" })[0];
        string path = AssetDatabase.GUIDToAssetPath(enemyBaseParameterGUID);
        EnemyBaseParameter asset = AssetDatabase.LoadAssetAtPath(path, typeof(EnemyBaseParameter)) as EnemyBaseParameter;
        return asset;
    }
#endif
}
