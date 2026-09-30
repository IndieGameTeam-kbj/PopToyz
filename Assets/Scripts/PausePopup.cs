using UnityEngine;

public class PausePopup : MonoBehaviour
{
    public void OnClickResume()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnClickRestart()
    {
        GameManager.Instance.RestartGame();
    }
    public void OnClickHome()
    {
        GameManager.Instance.GoMainMenu();
    }
}