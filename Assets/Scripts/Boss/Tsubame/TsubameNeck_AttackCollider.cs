using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsubameNeck_AttackCollider : MonoBehaviour
{
    [SerializeField] private Tsubame tsubame;
    [SerializeField] private PlayerData playerData;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerData.Player.IsPlayerInvincible)
        {
            tsubame.DamageToPlayer(transform.position, 2);
        }
    }
}
