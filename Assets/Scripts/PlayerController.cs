using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Image[] _lifeHearts;

    private int _life;

    public static event Action LifeDepleted;
    public static event Action GameOverAnimationCompleted;

    private void Awake()
    {
        _life = _lifeHearts.Length;
    }

    public void Reset()
    {
        for (int i = 0; i < _lifeHearts.Length; i++)
        {
            Transform target = _lifeHearts[i].transform;
            target.DOKill();
            target.localScale = Vector3.one;
            target.localRotation = Quaternion.identity;
            _lifeHearts[i].gameObject.SetActive(true);
        }

        _life = _lifeHearts.Length;
    }

    public void LoseLife()
    {
        if (_life <= 0) return;

        int index = _lifeHearts.Length - _life;
        _life--;

        if (_life == 0)
        {
            LifeDepleted?.Invoke();
        }

        PlayLoseLifeAnimation(_lifeHearts[index]);
    }

    private void PlayLoseLifeAnimation(Image heart)
    {
        Transform target = heart.transform;

        target.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOScale(1.15f, 0.1f));
        sequence.Append(target.DOShakeRotation(0.2f, new Vector3(0.0f, 0.0f, 15.0f), 10, 90.0f));
        sequence.Append(target.DOScale(0.0f, 0.15f).SetEase(Ease.InBack));
        sequence.OnComplete(() =>
        {
            heart.gameObject.SetActive(false);

            if (_life == 0)
            {
                GameOverAnimationCompleted?.Invoke();
            }
        });
    }
    
    private void OnEnable()
    {
        TargetReturnFloor.TargetMissed += LoseLife;
    }

    private void OnDisable()
    {
        TargetReturnFloor.TargetMissed -= LoseLife;
    }

}
