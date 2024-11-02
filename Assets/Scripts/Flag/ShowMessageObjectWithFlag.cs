using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowMessageObjectWithFlag : ActionButtonTarget
{
    [SerializeField] private Canvas messageCanvas;
    [SerializeField] private TMP_Text text;
    [SerializeField, TextArea(1, 3)] private string[] FlagOFFMessage = new string[1];
    [SerializeField, TextArea(1, 3)] private string[] FlagONMessage = new string[1];
    private int count = 0;
    [SerializeField] PlayerData playerData;
    [SerializeField] private FlagData flagData;
    [SerializeField] private int targetFlagNumber;
    private FlagData.FlagClass flag;
    private List<string> message = new List<string>();

    protected override void Reset()
    {
        base.Reset();
        if(playerData == null)
        {
            playerData = PlayerData.GetPlayerDataAsset();
        }
        if(flagData == null)
        {
            flagData = FlagData.GetFlagDataAsset();
        }
    }

    protected override void Start()
    {
        base.Start();
        objectType = ObjectTypeData.ShowMessage;
        myAction = ShowMessage;
        flag = flagData.GetFlag(targetFlagNumber);
    }

    private void ShowMessage()
    {
        if(count == 0)
        {
            if (!flag.flag)
            {
                message.AddRange(FlagOFFMessage);
                flagData.SetFlag_True(targetFlagNumber);
            }
            else
            {
                message.AddRange(FlagONMessage);
                flagData.SetFlag_False(targetFlagNumber);
            }
            playerData.Player.ChangeInput();
            GameManager.Instance.GamePause(true);
            UIController.Instance.SubmitAction = ShowMessage;
            UIController.Instance.CancelAction = ShowMessage;
            messageCanvas.enabled = true;
        }

        if (count < message.Count)
        {
            text.text = message[count];
            count++;
        }
        else
        {
            playerData.Player.ChangeInput();
            GameManager.Instance.GamePause(false);
            messageCanvas.enabled = false;
            count = 0;
            message.Clear();
        }
    }
}
