using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonAction : MonoBehaviour
{
    private Canvas targetCanvas;

    public void MyAction_ChangeScene(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName);
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void MyAction_SceneReLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
    
    public void MyAction_ShowCanvas(Canvas myCanvas)
    {
        myCanvas.enabled = true;
        UIController.Instance.SubmitAction = null;
        targetCanvas = myCanvas;
        UIController.Instance.CancelAction = HideCanvas;
    }
    private void HideCanvas()
    {
        targetCanvas.enabled = false;
        if (targetCanvas.name == "ManualCanvas")// åªèÛégÇ¡ÇƒÇ¢ÇÈÇÃÇÕManualCanvasÇÃÇ›
        {
            UIController.Instance.CancelAction = GameManager.Instance.Menu;
        }
    }

    public void MyAction_SwitchCanvasVisibilityWithSelectable(CanvasVisibilityChanger cvc)
    {
        if (!cvc.CanvasEnable)
        {
            cvc.ShowCanvas();
        }
        else
        {
            cvc.HideCanvas();
        }
    }

    public void MyAction_Select(CanvasVisibilityChanger cvc)
    {
        cvc.Select();
    }

    public void MyAction_ChangeInput(PlayerData playerData)
    {
        playerData.Player.ChangeInput();
    }

    public void MyAction_GamePause()
    {
        GameManager.Instance.GamePause(Time.timeScale == 1f);
    }
}
