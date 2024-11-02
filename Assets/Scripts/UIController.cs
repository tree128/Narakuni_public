using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Action SubmitAction = null;
    public Action CancelAction = null;

    //[SerializeField] private string submit;
    //[SerializeField] private string cancel;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    /*
    private void Update()
    {
        if(SubmitAction == null && submit != "")
        {
            submit = "";
        }
        if(SubmitAction != null && submit != SubmitAction.Method.Name)
        {
            submit = SubmitAction.Method.Name;
        }

        if (CancelAction == null && cancel != "")
        {
            cancel = "";
        }
        if (CancelAction != null && cancel != CancelAction.Method.Name)
        {
            cancel = CancelAction.Method.Name;
        }
    }
    */
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(SubmitAction != null)
            {
                SubmitAction.Invoke();
            }
            else
            {
                GameObject selectableObj = GameManager.Instance.EventSystem.currentSelectedGameObject;
                if (selectableObj != null)
                {
                    Button button = selectableObj.GetComponent<Button>();
                    if (button.onClick != null)
                    {
                        button.onClick.Invoke();
                    }
                }
            }
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(CancelAction != null)
            {
                CancelAction.Invoke();
            }
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.Menu();
        }
    }
}
