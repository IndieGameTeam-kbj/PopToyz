using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private RectTransform _logo;

    public void OnClickPlay()
    {
        GameManager.Instance.StartGame();
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

    public void PlayLogoAnimation()
    {
        _logo.DOKill();
        _logo.localScale = Vector3.one;

        Sequence sequence = DOTween.Sequence();
        sequence.SetTarget(_logo);
        sequence.SetUpdate(true);
        sequence.Append(_logo.DOScale(1.15f, 0.2f).SetEase(Ease.OutQuad));
        sequence.Append(_logo.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack));

        StartCoroutine(PlayLogoSoundNextFrame());
    }

    private IEnumerator PlayLogoSoundNextFrame()
    {
        yield return null;
        SoundManager.Instance.Play(SoundType.Logo);
    }

    private void OnEnable()
    {
        PlayLogoAnimation();
    }

}
