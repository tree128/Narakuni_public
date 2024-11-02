using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private Enemy000Parameter param;
    [SerializeField] private PlayerData playerData;
    private int damage;

    private void Start()
    {
        damage = param.Damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerData.Player.GetDamage(transform.position, damage);
        }

        if (collision.CompareTag("PlayerAttack"))
        {
            Destroy(gameObject);
        }
    }
}
