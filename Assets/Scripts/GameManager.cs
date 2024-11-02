using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 全てのシーンに設置する
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private FlagData flagData;
    [SerializeField] private EventSystem eventSystem;
    public EventSystem EventSystem => eventSystem;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField, Range(0, 2)] private int firstActionMapIndex;
    [Header("目標とするフレームレート"), SerializeField, Range(1, 100)] private int targetFrameRate;
    [SerializeField] private TMP_Text FPSText;
    private float elapsedTime_fps;
    private float elapsedTime_pause;
    private float pauseTime;
    private bool isPause = false;

    // GameOver
    [SerializeField] private CanvasVisibilityChanger gameOverCanvasChanger;
    [SerializeField] private Image gameOverImage;
    private Color gameOverColor;
    [Header("ゲームオーバーの暗転にかける時間は〇秒"), SerializeField, Range(0f, 10f)] private float secondsForBlackOut;
    private float elapsedTime_gameOver;
    private bool isGameOver = false;

    [SerializeField] private CanvasVisibilityChanger menuCanvasChanger;
    [SerializeField] private CanvasVisibilityChanger settingCanvasChanger;
    [SerializeField] private Canvas manualCanvas;
    [SerializeField] private CanvasVisibilityChanger retireCanvasChanger;
    [SerializeField] private CanvasVisibilityChanger returnTitleCanvasChanger;
    private bool wasUIMode = false;
    private Action wasSetAction;

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

        // フレームレート設定
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;

        playerInput.currentActionMap = playerInput.actions.actionMaps[firstActionMapIndex];
    }
    
    private void OnValidate()
    {
        if(Application.targetFrameRate != targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause)
        {
            elapsedTime_pause += Time.unscaledDeltaTime;
            if (pauseTime <= elapsedTime_pause)
            {
                isPause = false;
                Time.timeScale = 1f;
            }
        }
        else if(FPSText != null)
        {
            elapsedTime_fps += Time.deltaTime;
            if (elapsedTime_fps >= 1)
            {
                FPSText.text = "FPS:" + (1f / Time.deltaTime).ToString("00");
                elapsedTime_fps = 0;
            }
        }
        if (isGameOver)
        {
            elapsedTime_gameOver += Time.unscaledDeltaTime;
            if(elapsedTime_gameOver <= secondsForBlackOut)
            {
                gameOverColor.a = Mathf.Lerp(0f, 1f, elapsedTime_gameOver / secondsForBlackOut);
                gameOverImage.color = gameOverColor;
            }
            else
            {
                if(gameOverColor.a != 1f)
                {
                    gameOverColor.a = 1f;
                    gameOverImage.color = gameOverColor;
                }
                gameOverCanvasChanger.ShowCanvas();
                playerData.Player.ChangeInput();
                isGameOver = false;
            }
        }
    }

    public void GamePause(float time)
    {
        Time.timeScale = 0f;
        elapsedTime_pause = 0f;
        pauseTime = time;
        isPause = true;
    }

    public void GamePause(bool pause)
    {
        if(pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Menu()
    {
        if (!menuCanvasChanger.CanvasEnable)// 開くとき
        {
            if(playerInput.currentActionMap.name != "UI")
            {
                wasUIMode = false;
                GamePause(true);
                playerData.Player.ChangeInput();
            }
            else
            {
                wasUIMode = true;
                wasSetAction = UIController.Instance.CancelAction;
            }
            menuCanvasChanger.ShowCanvas();
            UIController.Instance.CancelAction = Menu;
        }
        else
        {
            menuCanvasChanger.HideCanvas();
            if (settingCanvasChanger.CanvasEnable)
            {
                settingCanvasChanger.HideCanvas();
            }
            if (manualCanvas.enabled)
            {
                manualCanvas.enabled = false;
            }
            if (retireCanvasChanger.CanvasEnable)
            {
                retireCanvasChanger.HideCanvas();
            }
            if (returnTitleCanvasChanger.CanvasEnable)
            {
                returnTitleCanvasChanger.HideCanvas();
            }

            if (!wasUIMode)
            {
                GamePause(false);
                playerData.Player.ChangeInput();
                UIController.Instance.CancelAction = null;
            }
            else
            {
                UIController.Instance.CancelAction = wasSetAction;
            }
            UIController.Instance.SubmitAction = null;
        }
    }

    public void GameOver()
    {
        GamePause(true);
        elapsedTime_gameOver = 0f;
        isGameOver = true;
    }

    /// <summary>
    /// 自室へシーン遷移する。
    /// </summary>
    public void GameClear()
    {
        SceneManager.LoadScene("TestMyRoom");
    }

    private void OnApplicationQuit()
    {
        flagData.Save();
        Debug.Log("ゲーム終了");
    }
}
