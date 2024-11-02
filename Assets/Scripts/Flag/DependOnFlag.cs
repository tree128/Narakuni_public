using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DependOnFlag : MonoBehaviour
{
    [SerializeField] protected FlagData flagData;
    [SerializeField] protected int targetFlagNumber;
    protected FlagData.FlagClass flag;
    protected Action action;

    protected virtual void Start()
    {
        foreach (var item in flagData.flagList.flags)
        {
            if(item.number == targetFlagNumber)
            {
                flag = item;
                break;
            }
        }
    }

    protected virtual void Update()
    {
        action();
    }
}
