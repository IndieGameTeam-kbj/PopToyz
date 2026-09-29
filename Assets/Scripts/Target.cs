using UnityEngine;
using DG.Tweening;

public enum TargetState
{
    Idle,
    Hit,
}

public class Target : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private Color _originalColor;
    private Color _hitColor = Color.black;
    private TargetState _state;

    public TargetState State => _state;

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _state = TargetState.Idle;
    }

    public void Launch(Vector3 position, Vector3 velocity)
    {
        _state = TargetState.Idle;
        _renderer.material.DOKill();
        _renderer.material.color = _originalColor;
        transform.SetPositionAndRotation(position, Random.rotation);
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        gameObject.SetActive(true);
        _rigidbody.linearVelocity = velocity;
        _rigidbody.angularVelocity = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

    public void ResetPhysics()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void Hit(Vector3 hitPoint, Vector3 hitDirection)
    {
        _state = TargetState.Hit;
        _renderer.material.DOKill();
        _renderer.material.DOColor(_hitColor, 1.0f);
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.AddForceAtPosition(hitDirection * 5.0f, hitPoint, ForceMode.Impulse);
    }

}
