using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvasObject : ActionButtonTarget
{
    [SerializeField] private CanvasVisibilityChanger canvasChanger;
    [SerializeField] private PlayerData playerData;

    protected override void Reset()
    {
        base.Reset();
        if(playerData == null)
        {
            playerData = PlayerData.GetPlayerDataAsset();
        }
    }

    protected override void Start()
    {
        base.Start();
        objectType = ObjectTypeData.ShowCanvas;
        myAction = SwitchCanvasVisibility;
    }

    private void SwitchCanvasVisibility()
    {
        if (!canvasChanger.CanvasEnable)// ï\é¶Ç∑ÇÈéû
        {
            canvasChanger.ShowCanvas();
            UIController.Instance.CancelAction = SwitchCanvasVisibility;// è„èëÇ´
            GameManager.Instance.GamePause(true);
            playerData.Player.ChangeInput();
        }
        else
        {
            canvasChanger.HideCanvas();
            GameManager.Instance.GamePause(false);
            playerData.Player.ChangeInput();
        }
    }
}
