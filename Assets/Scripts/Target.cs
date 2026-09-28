using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 position, Vector3 velocity)
    {
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

}
