using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// アクションボタンで起こすアクションを定義するクラスのベースクラス
/// </summary>
public class ActionButtonTarget : MonoBehaviour
{
    [SerializeField] protected Collider2D myCollider;
    protected ObjectTypeData objectType;
    public ObjectTypeData ObjectType => objectType;
    public enum ObjectTypeData
    {
        ShowMessage = 10,
        ShowCanvas = 20
    }
    protected Action myAction;
    protected Vector3 objectTopPos;
    public Vector3 ObjectTopPos => objectTopPos;

    protected virtual void Reset()
    {
        if (!gameObject.CompareTag("ActionButtonTarget"))
        {
            gameObject.tag = "ActionButtonTarget";
        }
        if (myCollider == null)
        {
            myCollider = GetComponent<Collider2D>();
            myCollider.isTrigger = true;
        }
    }

    protected virtual void Start()
    {
        objectTopPos = myCollider.bounds.center;
        objectTopPos.y += myCollider.bounds.size.y * 0.5f;
    }

    public virtual void Execute()
    {
        myAction.Invoke();
    }
}
