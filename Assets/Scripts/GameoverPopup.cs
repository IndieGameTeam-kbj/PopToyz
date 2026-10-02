using UnityEngine;

public class GameoverPopup : MonoBehaviour
{
    public void OnClickReTry()
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

