using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/KirinnParameter", fileName = "KirinnParameter")]
public class KirinnParameter : ScriptableObject
{
    [Header("UŒ‚‚P•‚Q‚ÌXƒ_ƒ[ƒW‚ÍZ"), SerializeField, Range(1, 100)] private int moveDamage;
    public int MoveDamage => moveDamage;
}
