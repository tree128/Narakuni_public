using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowMessageObject : ActionButtonTarget
{    
    [SerializeField] private Canvas messageCanvas;
    [SerializeField] private TMP_Text text;
    [SerializeField, TextArea(1, 3)] private string[] message = new string[1];
    private int count = 0;
    [SerializeField] PlayerData playerData;

    protected override void Reset()
    {
        base.Reset();
        if (playerData == null)
        {
            playerData = PlayerData.GetPlayerDataAsset();
        }
    }

    protected override void Start()
    {
        base.Start();
        objectType = ObjectTypeData.ShowMessage;
        myAction = ShowMessage;
    }

    private void ShowMessage()
    {
        if(count < message.Length)
        {
            text.text = message[count];
            if(count == 0)
            {
                playerData.Player.ChangeInput();
                GameManager.Instance.GamePause(true);
                UIController.Instance.SubmitAction = ShowMessage;
                UIController.Instance.CancelAction = ShowMessage;
                messageCanvas.enabled = true;
            }
            count++;
        }
        else
        {
            playerData.Player.ChangeInput();
            GameManager.Instance.GamePause(false);
            messageCanvas.enabled = false;
            count = 0;
        }
    }
}
