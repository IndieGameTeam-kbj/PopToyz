using DG.Tweening;
using UnityEngine;

public class LogoAnimationController : MonoBehaviour
{
    [SerializeField] private RectTransform _logo;

    public void Play()
    {
        _logo.localScale = Vector3.one;

        _logo.DOScale(1.15f, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _logo.DOScale(1f, 0.25f)
                    .SetEase(Ease.OutBack);
            });
    }
}