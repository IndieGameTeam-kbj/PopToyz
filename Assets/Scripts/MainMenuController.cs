using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private LogoAnimationController _logoAnimation;

    private void OnEnable()
    {
        _logoAnimation.Play();
    }

    public void OnClickPlay()
    {
        GameManager.Instance.StartGame();
    }
}