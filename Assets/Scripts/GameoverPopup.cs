using UnityEngine;

public class GameoverPopup : MonoBehaviour
{
    private void OnEnable()
    {
    }
    public void OnClickReTry()
    {
        GameManager.Instance.RestartGame();
    }
    public void OnClickHome()
    {
        GameManager.Instance.GoMainMenu();
    }
}
