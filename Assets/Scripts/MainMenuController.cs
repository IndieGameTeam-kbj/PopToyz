using DG.Tweening;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private RectTransform _logo;

    public void OnClickPlay()
    {
        GameManager.Instance.StartGame();
    }

    public void PlayLogoAnimation()
    {
        _logo.localScale = Vector3.one;

        _logo.DOScale(1.15f, 0.2f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _logo.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
            });
    }

    private void OnEnable()
    {
        PlayLogoAnimation();
    }

}
