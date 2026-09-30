using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenu;
    [SerializeField] private GameObject _gameView;
    [SerializeField] private GameObject _dimBackground;
    [SerializeField] private PausePopup _pausePopup;
    [SerializeField] private GameoverPopup _gameOverPopup;

    public static ViewManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
    }

}
