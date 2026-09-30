using UnityEngine;

public class GameoverPopup : MonoBehaviour
{
    public void OnClickReTry()
    {
        GameManager.Instance.RestartGame();
    }
    public void OnClickHome()
    {
        GameManager.Instance.GoMainMenu();
    }
}
