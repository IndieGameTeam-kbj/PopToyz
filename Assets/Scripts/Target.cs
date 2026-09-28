using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private int _poolId;

    public int PoolId => _poolId;

    public void Initialize(int poolId)
    {
        _poolId = poolId;
    }

    public void Launch(Vector3 position, Vector3 velocity)
    {
        transform.position = position;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        transform.rotation = Random.rotation;

        _rigidbody.linearVelocity = velocity;
        _rigidbody.angularVelocity = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

}
