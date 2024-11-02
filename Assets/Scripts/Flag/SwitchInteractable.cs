using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchInteractable : DependOnFlag
{
    [SerializeField] Selectable selectable;

    protected override void Start()
    {
        base.Start();
        action = SwitchTheInteractable;
    }

    private void SwitchTheInteractable()
    {
        if(selectable.interactable != flag.flag)
        {
            selectable.interactable = flag.flag;
        }
    }

}
