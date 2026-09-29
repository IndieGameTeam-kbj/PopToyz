using UnityEngine;

public enum TargetState
{
    Idle,
    Hit,
}

public class Target : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private TargetState _state;

    public TargetState State => _state;

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _state = TargetState.Idle;
    }

    public void Launch(Vector3 position, Vector3 velocity)
    {
        _state = TargetState.Idle;
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

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _rigidbody.AddForceAtPosition(hitDirection * 5.0f, hitPoint, ForceMode.Impulse);
    }

}
