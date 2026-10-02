using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenu;
    [SerializeField] private GameObject _gameView;
    [SerializeField] private GameObject _dimBackground;
    [SerializeField] private PausePopup _pausePopup;
    [SerializeField] private GameoverPopup _gameOverPopup;
    [SerializeField] private Image _screenTransition;

    private float _transitionDuration = 0.2f;
    private float _popupDuration = 0.2f;
    private float _popupStartScale = 0.9f;

    public static ViewManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetTransitionAlpha(0.0f);
    }

    public void ShowMainMenu()
    {
        _mainMenu.gameObject.SetActive(true);
        _gameView.SetActive(false);
        _pausePopup.gameObject.SetActive(false);
        _gameOverPopup.gameObject.SetActive(false);
        _dimBackground.SetActive(false);
    }

    public void ShowGame()
    {
        _mainMenu.gameObject.SetActive(false);
        _gameView.SetActive(true);
        _pausePopup.gameObject.SetActive(false);
        _gameOverPopup.gameObject.SetActive(false);
        _dimBackground.SetActive(false);
    }

    public void ShowPause()
    {
        _pausePopup.gameObject.SetActive(true);
        _dimBackground.SetActive(true);
        PlayPopupOpenAnimation(_pausePopup.GetComponent<RectTransform>(), null);
    }

    public void HidePause()
    {
        _pausePopup.gameObject.SetActive(false);
        _dimBackground.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverPopup.gameObject.SetActive(true);
        _dimBackground.SetActive(true);
        PlayPopupOpenAnimation(_gameOverPopup.GetComponent<RectTransform>(), null);
    }

    public void Transition(Action onSwap, Action onComplete)
    {
        _screenTransition.DOKill();
        SetTransitionAlpha(0.0f);

        _screenTransition
            .DOFade(1.0f, _transitionDuration)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                onSwap?.Invoke();
                _screenTransition.DOFade(0.0f, _transitionDuration).SetEase(Ease.InOutQuad).SetUpdate(true)
                    .OnComplete(() =>
                    {
                        onComplete?.Invoke();
                    });
            });
    }

    private void PlayPopupOpenAnimation(RectTransform popup, Action onComplete)
    {
        popup.DOKill();
        popup.localScale = Vector3.one * _popupStartScale;
        popup.DOScale(Vector3.one, _popupDuration).SetEase(Ease.OutBack, 1.2f).SetUpdate(true)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    private void SetTransitionAlpha(float alpha)
    {
        Color color = _screenTransition.color;
        color.a = alpha;
        _screenTransition.color = color;
    }

}
