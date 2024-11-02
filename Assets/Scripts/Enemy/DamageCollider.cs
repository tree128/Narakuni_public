using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private PlayerData playerData;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerData.Player.IsPlayerInvincible)
        {
            enemy.DamageToPlayer();
        }
    }
}
