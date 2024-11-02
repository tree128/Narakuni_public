using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private FlagData flagData;
    [SerializeField] private int flagNumber;
   
    public void FlagUpdate()
    {
        flagData.SetFlag_True(flagNumber);
    }
}
