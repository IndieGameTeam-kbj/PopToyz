using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [SerializeField] private int _CorkMagazineSize = 6;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _reloadTime = 2f;

    [SerializeField] private LayerMask _hitLayer;
    [SerializeField] private CorkPool _corkPool;

    [SerializeField] private CorkUI _corkUI;

    private int _currentCorkCount;
    private bool _isReloading;
    private float _nextFireTime;

    private void Start()
    {
        _currentCorkCount = _CorkMagazineSize;
        _corkUI.SetCorkCount(_currentCorkCount);
    }

    private void Update()
    {
        if (Pointer.current == null)
            return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPosition = Pointer.current.position.ReadValue();
            Shoot(screenPosition);
        }
    }

    private void Shoot(Vector2 screenPosition)
    {
        if (_isReloading)
            return;

        if (Time.time < _nextFireTime)
            return;

        if (_currentCorkCount <= 0)
            return;

        _currentCorkCount--;
        _corkUI.SetCorkCount(_currentCorkCount);
        _nextFireTime = Time.time + _fireRate;

        Fire(screenPosition);

        if (_currentCorkCount <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void Fire(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _hitLayer))
        {
            Cork cork = _corkPool.Get();

            if (cork != null)
            {
                cork.Hit(hit.point, hit.normal);
            }

            int layer = hit.collider.gameObject.layer;

            if (layer == LayerMask.NameToLayer("Target"))
            {
                Debug.Log("Target Hit");
            }
            else if (layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("Wall Hit");
            }
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;

        yield return new WaitForSeconds(_reloadTime);

        _currentCorkCount = _CorkMagazineSize;
        _corkUI.SetCorkCount(_currentCorkCount);
        _isReloading = false;
    }
}