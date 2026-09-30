using System;
using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public enum GameState
{
    MainMenu,
    Countdown,
    Playing,
    Paused,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _countdownText;

    private float _countdownDuration = 0.5f;
    private Coroutine _countdownCoroutine;

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

        _countdownText.gameObject.SetActive(false);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
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
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        State = GameState.MainMenu;
        Time.timeScale = 0.0f;
        _countdownText.gameObject.SetActive(false);
        ViewManager.Instance.ShowMainMenu();
    }

    public void StartGame()
    {
        if (_countdownCoroutine != null) return;

        State = GameState.Countdown;
        Time.timeScale = 1.0f;
        GameReset?.Invoke();
        ViewManager.Instance.ShowGame();
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    public void RestartGame()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        State = GameState.Countdown;
        Time.timeScale = 1.0f;
        GameReset?.Invoke();
        ViewManager.Instance.ShowGame();
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    public void PauseGame()
    {
        State = GameState.Paused;
        Time.timeScale = 0.0f;
        ViewManager.Instance.ShowPause();
    }

    public void ResumeGame()
    {
        if (_countdownCoroutine != null)
        {
            State = GameState.Countdown;
        }
        else
        {
            State = GameState.Playing;
        }
        Time.timeScale = 1.0f;
        ViewManager.Instance.HidePause();
    }

    public void GameOver()
    {
        State = GameState.GameOver;
        Time.timeScale = 0.0f;
        ViewManager.Instance.ShowGameOver();
        ScoreManager.Instance.PlayGameOverScoreAnimation();
    }

    private IEnumerator Countdown()
    {
        _countdownText.gameObject.SetActive(true);

        PlayCountdownAnimation("3");
        yield return new WaitForSeconds(_countdownDuration);

        PlayCountdownAnimation("2");
        yield return new WaitForSeconds(_countdownDuration);

        PlayCountdownAnimation("1");
        yield return new WaitForSeconds(_countdownDuration);

        PlayCountdownAnimation("Start!");
        yield return new WaitForSeconds(_countdownDuration);

        _countdownText.gameObject.SetActive(false);
        State = GameState.Playing;
        _countdownCoroutine = null;
    }

    private void PlayCountdownAnimation(string text)
    {
        Transform target = _countdownText.transform;
        target.DOKill();

        _countdownText.text = text;
        target.localScale = Vector3.one * 0.5f;

        Sequence sequence = DOTween.Sequence();
        sequence.SetTarget(target);
        sequence.Append(target.DOScale(1.2f, 0.15f).SetEase(Ease.OutBack));
        sequence.Append(target.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad));
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
