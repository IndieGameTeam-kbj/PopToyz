using Unity.VisualScripting;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }
    [SerializeField] private GameObject _mainMenu;
    //[SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _pausePopup;
    [SerializeField] private GameObject _gameOverPopup;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
       // _gameUI.SetActive(false);
        _pausePopup.SetActive(false);
        _gameOverPopup.SetActive(false);
    }

    public void ShowGame()
    {
        _mainMenu.SetActive(false);
       // _gameUI.SetActive(true);
        _pausePopup.SetActive(false);
        _gameOverPopup.SetActive(false);
    }

    public void ShowPause()
    {
        _pausePopup.SetActive(true);
    }

    public void HidePause()
    {
        _pausePopup.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverPopup.SetActive(true);
    }
}