using UnityEngine;

public class Cork : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 2.0f;

    private Rigidbody _rigidbody;

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Hit(Vector3 position, Vector3 normal, Vector3 direction)
    {
        transform.position = position + normal * 0.02f;
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Vector3 tangent = Vector3.Cross(normal, Vector3.up);
        if (tangent.sqrMagnitude < 0.01f)
        {
            tangent = Vector3.Cross(normal, Vector3.right);
        }
        tangent.Normalize();

        Vector3 bitangent = Vector3.Cross(normal, tangent);
        Vector3 randomDirection = normal + tangent * Random.Range(-0.4f, 0.4f) + bitangent * Random.Range(-0.2f, 0.4f);
        _rigidbody.AddForce(randomDirection.normalized * _bounceForce, ForceMode.Impulse);
        _rigidbody.AddTorque(Random.insideUnitSphere * 2.0f, ForceMode.Impulse);

        SoundManager.Instance.PlayAtPosition(SoundType.Hit, position);
    }

    public void ResetPhysics()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

}
