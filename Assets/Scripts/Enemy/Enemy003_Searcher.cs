using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy003_Searcher : MonoBehaviour
{
    [SerializeField] private CircleCollider2D searchCollider;
    [SerializeField] private Enemy003 enemy003;

    public void Init(float radius)
    {
        searchCollider.enabled = false;
        searchCollider.radius = radius;
        searchCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy003.Discover();
        }
    }
}
