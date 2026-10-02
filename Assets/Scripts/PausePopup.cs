using UnityEngine;

public class PausePopup : MonoBehaviour
{
    public void OnClickResume()
    {
        GameManager.Instance.ResumeGame();
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

    public void OnClickRestart()
    {
        GameManager.Instance.RestartGame();
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

    public void OnClickHome()
    {
        GameManager.Instance.GoMainMenu();
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

}
