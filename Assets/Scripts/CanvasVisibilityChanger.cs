using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///�u�\��/��\���̐؂�ւ�������ASelectable�I�u�W�F�N�g�������Ă���Canvas�v�ɃA�^�b�`����B
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
            Debug.Log(gameObject.name + "��GraphicRaycaster��Remove���܂��傤");
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

    // �O�ʂɕ\������Ă����L�����o�X����\���ɂȂ����ۂ�Select�݂̂��K�v�ɂȂ邽�ߕ���
    public void Select()
    {
        GameManager.Instance.EventSystem.SetSelectedGameObject(firstSelect.gameObject);
    }
}
