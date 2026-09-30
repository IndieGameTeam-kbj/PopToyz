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
    public static event Action GameReset;
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }

    private void Awake()
    {
        Instance = this;
        State = GameState.MainMenu;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        ViewManager.Instance.ShowMainMenu();
        PlayerController.GameOverAnimationCompleted += GameOver;
    }

    public void StartGame()
    {
        State = GameState.Playing;
        Time.timeScale = 1f;

        ViewManager.Instance.ShowGame();
    }

    public void PauseGame()
    {
        State = GameState.Paused;
        Time.timeScale = 0f;

        ViewManager.Instance.ShowPause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        State = GameState.Playing;

        ViewManager.Instance.HidePause();
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameReset?.Invoke();

        State = GameState.Playing;
        ViewManager.Instance.ShowGame();
    }

    public void GameOver()
    {
        State = GameState.GameOver;
        Time.timeScale = 1f;

        ViewManager.Instance.ShowGameOver();
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        GameReset?.Invoke();

        State = GameState.MainMenu;

        ViewManager.Instance.ShowMainMenu();
    }
}