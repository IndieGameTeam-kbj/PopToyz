using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Image[] _lifeHearts;

    [SerializeField] private int _CorkMagazineSize = 6;
    [SerializeField] private LayerMask _hitLayer;
    [SerializeField] private CorkPool _corkPool;
    [SerializeField] private Image[] _corkImages;

    private int _life;
    private int _currentCorkCount;
    private bool _isReloading;
    private Coroutine _reloadCoroutine;

    public static event Action LifeDepleted;
    public static event Action GameOverAnimationCompleted;

    private void Awake()
    {
        _life = _lifeHearts.Length;
        _currentCorkCount = _CorkMagazineSize;

        for (int i = 0; i < _corkImages.Length; i++)
        {
            _corkImages[i].gameObject.SetActive(true);
            _corkImages[i].transform.localScale = Vector3.one;
        }
    }

    public void Reset()
    {
        if (_reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
        }

        for (int i = 0; i < _lifeHearts.Length; i++)
        {
            Transform target = _lifeHearts[i].transform;
            target.DOKill();
            target.localScale = Vector3.one;
            target.localRotation = Quaternion.identity;
            _lifeHearts[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _corkImages.Length; i++)
        {
            Transform target = _corkImages[i].transform;
            target.DOKill();
            target.localScale = Vector3.one;
            target.localPosition = _corkImages[i].rectTransform.anchoredPosition;
            _corkImages[i].gameObject.SetActive(true);
        }

        _life = _lifeHearts.Length;
        _currentCorkCount = _CorkMagazineSize;
        _isReloading = false;
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

    private void Shoot(Vector2 screenPosition)
    {
        if (_isReloading) return;
        if (_currentCorkCount <= 0) return;

        int index = _currentCorkCount - 1;
        _currentCorkCount--;

        Fire(screenPosition);
        PlayConsumeCorkAnimation(index);

        if (_currentCorkCount <= 0)
        {
            StartReload();
        }
    }

    private void Fire(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _hitLayer)) return;

        Cork cork = _corkPool.Get();

        if (cork != null)
        {
            cork.Hit(hit.point, hit.normal, ray.direction);
        }

        int layer = hit.collider.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Target"))
        {
            Target target = hit.collider.GetComponentInParent<Target>();

            if (target != null && target.State == TargetState.Idle)
            {
                target.Hit(hit.point, ray.direction);
            }
        }
    }

    private void StartReload()
    {
        if (_isReloading) return;
        if (_currentCorkCount >= _CorkMagazineSize) return;

        _reloadCoroutine = StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(0.2f);

        while (_currentCorkCount < _CorkMagazineSize)
        {
            int index = _currentCorkCount;
            PlayReloadCorkAnimation(index);
            yield return new WaitForSeconds(0.2f);
            _currentCorkCount++;
        }

        _isReloading = false;
        _reloadCoroutine = null;
    }

    private void Update()
    {
        if (InputManager.Instance.IsShootPressed)
        {
            Shoot(InputManager.Instance.PointerScreenPosition);
        }
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

    private void PlayConsumeCorkAnimation(int index)
    {
        Image cork = _corkImages[index];
        Transform target = cork.transform;
        target.DOKill();
        Vector3 startPosition = target.localPosition;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOScale(1.1f, 0.05f));
        sequence.Join(target.DOLocalMoveX(startPosition.x + 100.0f, 0.2f).SetEase(Ease.OutQuad));
        sequence.Join(target.DOLocalMoveY(startPosition.y - 30.0f, 0.2f).SetEase(Ease.InQuad));
        sequence.OnComplete(() =>
        {
            cork.gameObject.SetActive(false);
            target.localPosition = startPosition;
            target.localRotation = Quaternion.identity;
            target.localScale = Vector3.one;
        });
    }

    private void PlayReloadCorkAnimation(int index)
    {
        Image cork = _corkImages[index];
        Transform target = cork.transform;
        target.DOKill();
        Vector3 targetPosition = target.localPosition;
        Vector3 startPosition = targetPosition + Vector3.right * 100.0f;
        
        cork.gameObject.SetActive(true);
        target.localPosition = startPosition;
        target.localScale = Vector3.one;
        target.localRotation = Quaternion.identity;
        target.DOLocalMove(targetPosition, 0.2f).SetEase(Ease.OutQuad);
    }

    private void OnEnable()
    {
        ReturnFloor.TargetMissed += LoseLife;
    }

    private void OnDisable()
    {
        ReturnFloor.TargetMissed -= LoseLife;
    }

}
