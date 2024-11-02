using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///「表示/非表示の切り替えがあり、Selectableオブジェクトを持っているCanvas」にアタッチする。
/// </summary>
public class CanvasVisibilityChanger : MonoBehaviour
{
    [SerializeField] private Canvas myCanvas;
    public Canvas MyCanvas => myCanvas;
    [SerializeField] private CanvasGroup myCanvasGroup;
    public CanvasGroup MyCanvasGroup => myCanvasGroup;
    public bool CanvasEnable => myCanvas.enabled;
    [SerializeField] private Selectable firstSelect;

    private void Reset()
    {
        myCanvas = GetComponent<Canvas>();
        GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        if (!TryGetComponent(out myCanvasGroup))
        {
            CanvasGroup cg = gameObject.AddComponent<CanvasGroup>();
            cg.ignoreParentGroups = true;
            myCanvasGroup = cg;
        }
        GraphicRaycaster gr;
        if(TryGetComponent(out gr))
        {
            Debug.Log(gameObject.name + "のGraphicRaycasterをRemoveしましょう");
        }
    }

    private void Start()
    {
        myCanvasGroup.interactable = myCanvas.enabled;
    }

    public void ShowCanvas()
    {
        myCanvas.enabled = true;
        CanvasManager.Instance.SetInteractable(this);
        Select();
        UIController.Instance.SubmitAction = null;
        if (myCanvas.name == "MenuCanvas")
        {
            UIController.Instance.CancelAction = GameManager.Instance.Menu;
        }
        else
        {
            UIController.Instance.CancelAction = HideCanvas;
        }
    }

    public void HideCanvas()
    {
        myCanvas.enabled = false;
        myCanvasGroup.interactable = false;
        CanvasManager.Instance.Check();
    }

    // 前面に表示されていたキャンバスが非表示になった際はSelectのみが必要になるため分離
    public void Select()
    {
        GameManager.Instance.EventSystem.SetSelectedGameObject(firstSelect.gameObject);
    }
}
