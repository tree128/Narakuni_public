using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "MyScriptableObjects/EnemyBase", fileName = "EnemyBase")]
public class EnemyBaseParameter : ScriptableObject
{
    [Header("“G‚ÌXƒŠƒAƒNƒVƒ‡ƒ“F‚ÍZ")] public Color damageColor;
    [Header("y“G‚ÌXƒŠƒAƒNƒVƒ‡ƒ“F•ÏXŠÔ‚ÍZ•bz"), Range(0.01f, 1f)] public float damageTime;

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
