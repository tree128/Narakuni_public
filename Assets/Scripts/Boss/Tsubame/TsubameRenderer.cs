using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TsubameRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private TsubameParameter param;
    [SerializeField] EnemyBaseParameter baseParam;
    private Status currentStatus = Status.normal;
    public Status CurrentStatus => currentStatus;

    public enum Status
    { 
        normal,
        before,
        attacking,
        damaging
    }

    private void Reset()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        param = GetTsubameParameterAsset();
        baseParam = EnemyBaseParameter.GetEnemyBaseParameterAsset();
    }

#if UNITY_EDITOR
    public static TsubameParameter GetTsubameParameterAsset()
    {
        string tsubameParameterGUID = AssetDatabase.FindAssets("TsubameParameter", new string[1] { "Assets/Data/Boss/Tsubame" })[0];
        string path = AssetDatabase.GUIDToAssetPath(tsubameParameterGUID);
        TsubameParameter asset = AssetDatabase.LoadAssetAtPath(path, typeof(TsubameParameter)) as TsubameParameter;
        return asset;
    }
# endif

    public void StatusChange(Status status)
    {
        currentStatus = status;
        ColorChange();
    }

    private void ColorChange()
    {
        switch (currentStatus)
        {
            case Status.before:
                myRenderer.color = param.BeforeAttackColor;
                break;
            case Status.attacking:
                myRenderer.color = param.AttackingColor;
                break;
            case Status.damaging:
                myRenderer.color = baseParam.damageColor;
                break;
            default:
                myRenderer.color = param.DefaultColor;
                break;
        }
    }
}
