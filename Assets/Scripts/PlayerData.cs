using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "MyScriptableObjects/PlayerData", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("yƒvƒŒƒCƒ„[‚ÌUŒ‚—Í‚ÍZz"), Range(1, 100)] public int AttackPower;
    private Player player;
    public Player Player => player;
    private Transform playerTransform;
    public Transform PlayerTransform => playerTransform;
    private BoxCollider2D playerCollider;
    public BoxCollider2D PlayerCollider => playerCollider;

    [HideInInspector]public Vector2 TargetPos_ChangeEnvironment;

    public void Init(Player _player, BoxCollider2D _playerCollider)
    {
        player = _player;
        playerTransform = player.transform;
        playerCollider = _playerCollider;
    }

#if UNITY_EDITOR
    public static PlayerData GetPlayerDataAsset()
    {
        string playerDataGUID = AssetDatabase.FindAssets("PlayerData", new string[1] { "Assets/Data" })[0];
        string path = AssetDatabase.GUIDToAssetPath(playerDataGUID);
        PlayerData asset = AssetDatabase.LoadAssetAtPath(path, typeof(PlayerData)) as PlayerData;
        return asset;
    }
# endif
}
