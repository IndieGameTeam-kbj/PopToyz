using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Image[] _lifeHearts;
    [SerializeField] private LayerMask _hitLayer;
    [SerializeField] private CorkPool _corkPool;
    [SerializeField] private Image[] _corkImages;
    [SerializeField] private Image _reloadImage;

    private int _life;
    private int _corkMagazineSize = 6;
    private int _currentCorkCount;
    private bool _isReloading;
    private Coroutine _reloadCoroutine;
    private float _reloadInterval = 0.2f;
    private Vector3[] _corkPositions;

    public static event Action LifeDepleted;
    public static event Action GameOverAnimationCompleted;

    private void Awake()
    {
        _life = _lifeHearts.Length;
        _currentCorkCount = _corkMagazineSize;
        _corkPositions = new Vector3[_corkImages.Length];

        for (int i = 0; i < _corkImages.Length; i++)
        {
            _corkPositions[i] = _corkImages[i].transform.localPosition;
            _corkImages[i].gameObject.SetActive(true);
            _corkImages[i].transform.localScale = Vector3.one;
        }

        _reloadImage.gameObject.SetActive(false);
        _reloadImage.fillAmount = 0.0f;
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
            _lifeHearts[i].DOKill();
            target.localScale = Vector3.one;
            target.localRotation = Quaternion.identity;
            _lifeHearts[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _corkImages.Length; i++)
        {
            Transform target = _corkImages[i].transform;
            target.DOKill();
            _corkImages[i].DOKill();
            target.localPosition = _corkPositions[i];
            target.localScale = Vector3.one;
            target.localRotation = Quaternion.identity;
            _corkImages[i].color = new Color(_corkImages[i].color.r, _corkImages[i].color.g, _corkImages[i].color.b, 1.0f);
            _corkImages[i].gameObject.SetActive(true);
        }

        _reloadImage.DOKill();
        _reloadImage.gameObject.SetActive(false);
        _reloadImage.fillAmount = 0.0f;

        _life = _lifeHearts.Length;
        _currentCorkCount = _corkMagazineSize;
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
                ScoreManager.Instance.AddScore(1);
            }
        }
    }

    private void StartReload()
    {
        if (_isReloading) return;
        if (_currentCorkCount >= _corkMagazineSize) return;

        int reloadCount = _corkMagazineSize - _currentCorkCount;
        float reloadDuration = reloadCount * _reloadInterval;

        _reloadImage.DOKill();
        _reloadImage.fillAmount = 0.0f;
        _reloadImage.gameObject.SetActive(true);

        _reloadImage.DOFillAmount(1.0f, reloadDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _reloadImage.gameObject.SetActive(false);
            });

        _reloadCoroutine = StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(_reloadInterval);

        while (_currentCorkCount < _corkMagazineSize)
        {
            int index = _currentCorkCount;
            PlayReloadCorkAnimation(index);
            yield return new WaitForSeconds(_reloadInterval);
            _currentCorkCount++;
        }

        _isReloading = false;
        _reloadCoroutine = null;
    }

    private void Update()
    {
        if (!InputManager.Instance.IsShootPressed) return;

        if (Input.touchCount > 0)
        {
            int fingerId = Input.GetTouch(0).fingerId;
            if (EventSystem.current.IsPointerOverGameObject(fingerId)) return;
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
        }

        Shoot(InputManager.Instance.PointerScreenPosition);
    }

    private void PlayLoseLifeAnimation(Image heart)
    {
        Transform target = heart.transform;
        target.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.SetTarget(target);
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
        cork.DOKill();

        Vector3 startPosition = target.localPosition;
        Vector3 recoilPosition = startPosition + Vector3.right * 60.0f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOLocalMoveX(recoilPosition.x, 0.08f).SetEase(Ease.OutQuad));
        sequence.Append(target.DOLocalMoveY(startPosition.y - 40.0f, 0.12f).SetEase(Ease.InQuad));
        sequence.Join(cork.DOFade(0.0f, 0.12f));
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
        cork.DOKill();

        Vector3 targetPosition = target.localPosition;
        Vector3 startPosition = targetPosition + Vector3.right * 100.0f;
        cork.gameObject.SetActive(true);
        cork.DOFade(0.0f, 0.0f);
        target.localPosition = startPosition;
        target.localScale = Vector3.one;
        target.localRotation = Quaternion.identity;

        Sequence sequence = DOTween.Sequence();
        sequence.SetTarget(target);
        sequence.Append(target.DOLocalMove(targetPosition, 0.2f).SetEase(Ease.OutQuad));
        sequence.Join(cork.DOFade(1.0f, 0.2f));
    }

    private void OnEnable()
    {
        ReturnFloor.TargetMissed += LoseLife;
        GameManager.GameReset += Reset;
    }

    private void OnDisable()
    {
        ReturnFloor.TargetMissed -= LoseLife;
        GameManager.GameReset -= Reset; 
    }

}
