using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// シーン内のキャンバスを管理するクラス。Interactable = trueは一つのCanvasGroupのみにする。
/// </summary>
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    [SerializeField] private CanvasVisibilityChanger[] canvasChangerArray;
    private List<CanvasVisibilityChanger> visibleCanvasList = new List<CanvasVisibilityChanger>();

    private void Reset()
    {
        canvasChangerArray = FindObjectsByType<CanvasVisibilityChanger>(FindObjectsSortMode.None);
        var comparison = new Comparison<CanvasVisibilityChanger>(CanvasComparison);
        Array.Sort(canvasChangerArray, comparison);
    }

    private int CanvasComparison(CanvasVisibilityChanger a, CanvasVisibilityChanger b)
    {
        // 降順、手前に表示されるものから
        return b.MyCanvas.sortingOrder - a.MyCanvas.sortingOrder;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Check()
    {
        visibleCanvasList.Clear();
        // 表示中キャンバスがあるか
        foreach (var item in canvasChangerArray)
        {
            if (item.CanvasEnable)
            {
                visibleCanvasList.Add(item);
            }
        }

        // あるなら、その中にinteractableがあるか確認
        if (0 < visibleCanvasList.Count)
        {
            bool canIntaract = false;
            foreach (var item in visibleCanvasList)
            {
                if (item.MyCanvasGroup.interactable)
                {
                    canIntaract = true;
                }
            }

            if (!canIntaract)
            {
                visibleCanvasList[0].ShowCanvas();
            }
        }
    }

    public void SetInteractable(CanvasVisibilityChanger target)
    {
        foreach (var item in canvasChangerArray)
        {
            if(item == target)
            {
                item.MyCanvasGroup.interactable = true;
            }
            else
            {
                item.MyCanvasGroup.interactable = false;
            }
        }
    }
}
