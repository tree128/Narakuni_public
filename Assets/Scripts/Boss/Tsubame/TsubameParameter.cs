using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/TsubameParameter", fileName = "TsubameParameter")]
public class TsubameParameter : ScriptableObject
{
    public Color DefaultColor;
    public Color BeforeAttackColor;
    public Color AttackingColor;
}
