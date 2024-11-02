using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSParent: MonoBehaviour
{
    [SerializeField] private BoxCollider2D MSGround;
    [SerializeField] private BoxCollider2D MSWater;
    [Header("地上用のY座標値は〇"), SerializeField, Range(0, 100f)]private float MSGroundYPos;

    private void OnValidate()
    {
        if(transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        if(MSGround.transform.position.y != MSGroundYPos)
        {
            MSGround.transform.position = new Vector3(transform.position.x, MSGroundYPos, 0f);
            MSWater.transform.position = new Vector3(transform.position.x, -MSGroundYPos, 0f);
        }
    }
}
