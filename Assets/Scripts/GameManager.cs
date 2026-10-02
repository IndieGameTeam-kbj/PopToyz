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

    private float _countdownDuration = 0.8f;
    private Coroutine _countdownCoroutine;
    private bool _isTransitioning;

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
        if (_isTransitioning) return;

        StopCountdown();
        _isTransitioning = true;

        ViewManager.Instance.Transition(
            () =>
            {
                State = GameState.MainMenu;
                Time.timeScale = 0.0f;
                ViewManager.Instance.ShowMainMenu();
            },
            () =>
            {
                _isTransitioning = false;
            }
        );
    }

    public void StartGame()
    {
        if (_isTransitioning) return;

        StopCountdown();
        _isTransitioning = true;

        ViewManager.Instance.Transition(
            () =>
            {
                State = GameState.Countdown;
                Time.timeScale = 1.0f;
                GameReset?.Invoke();
                ViewManager.Instance.ShowGame();
            },
            () =>
            {
                _countdownCoroutine = StartCoroutine(Countdown());
                _isTransitioning = false;
            }
        );
    }

    public void RestartGame()
    {
        StartGame();
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
        SoundManager.Instance.Play(SoundType.GameOver);
    }

    private void ShowGameOverPopup()
    {
        Time.timeScale = 0.0f;
        ViewManager.Instance.ShowGameOver();
    }

    private void StopCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        Transform target = _countdownText.transform;
        target.DOKill();
        target.localScale = Vector3.one;
        _countdownText.text = string.Empty;
        _countdownText.gameObject.SetActive(false);
    }

    private IEnumerator Countdown()
    {
        _countdownText.gameObject.SetActive(true);

        PlayCountdownAnimation("3");
        SoundManager.Instance.Play(SoundType.Countdown);
        yield return new WaitForSeconds(_countdownDuration);

        PlayCountdownAnimation("2");
        SoundManager.Instance.Play(SoundType.Countdown);
        yield return new WaitForSeconds(_countdownDuration);

        PlayCountdownAnimation("1");
        SoundManager.Instance.Play(SoundType.Countdown);
        yield return new WaitForSeconds(_countdownDuration);

        PlayCountdownAnimation("Start!");
        SoundManager.Instance.Play(SoundType.GameStart);
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
        sequence.Append(target.DOScale(1.2f, _countdownDuration * 0.375f).SetEase(Ease.OutBack));
        sequence.Append(target.DOScale(1.0f, _countdownDuration * 0.125f).SetEase(Ease.OutQuad));
    }

    private void Update()
    {
        if (!InputManager.Instance.IsBackPressed) return;

        switch (State)
        {
            case GameState.MainMenu:
                Application.Quit();
                break;

            case GameState.Countdown:
                PauseGame();
                break;

            case GameState.Playing:
                PauseGame();
                break;

            case GameState.Paused:
                ResumeGame();
                break;

            case GameState.GameOver:
                GoMainMenu();
                break;
        }
    }

    public void OnClickPauseButton()
    {
        PauseGame();
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

    private void OnEnable()
    {
        PlayerController.GameOver += GameOver;
        PlayerController.GameOverAnimationCompleted += ShowGameOverPopup;
    }

    private void OnDisable()
    {
        PlayerController.GameOver -= GameOver;
        PlayerController.GameOverAnimationCompleted -= ShowGameOverPopup;
    }

}
