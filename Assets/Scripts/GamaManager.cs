using UnityEngine;
using System;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public GameState State { get; private set; }
    
    public static event Action GameReset;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        int targetWidth = 1080;
        int targetHeight = (int)(((float)Screen.height / Screen.width) * targetWidth);
        Screen.SetResolution(targetWidth, targetHeight, true);
    }

    private void Start()
    {
        GoMainMenu();
    }

    public void GoMainMenu()
    {
        State = GameState.MainMenu;
        Time.timeScale = 1.0f;
        ViewManager.Instance.ShowMainMenu();
    }

    public void StartGame()
    {
        State = GameState.Playing;
        Time.timeScale = 1.0f;
        GameReset?.Invoke();
        ViewManager.Instance.ShowGame();
    }

    public void PauseGame()
    {
        State = GameState.Paused;
        Time.timeScale = 0.0f;
        ViewManager.Instance.ShowPause();
    }

    public void ResumeGame()
    {
        State = GameState.Playing;
        Time.timeScale = 1.0f;
        ViewManager.Instance.HidePause();
    }

    public void RestartGame()
    {
        State = GameState.Playing;
        Time.timeScale = 1.0f;
        GameReset?.Invoke();
        ViewManager.Instance.ShowGame();
    }

    public void GameOver()
    {
        State = GameState.GameOver;
        Time.timeScale = 1.0f;
        ViewManager.Instance.ShowGameOver();
    }

    private void OnEnable()
    {
        PlayerController.GameOverAnimationCompleted += GameOver;
    }

    private void OnDisable()
    {
        PlayerController.GameOverAnimationCompleted -= GameOver;
    }

}
