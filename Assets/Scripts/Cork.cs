using System.Collections;
using UnityEngine;

public class Cork : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 2f;
    [SerializeField] private float _returnTime = 3f;

    private Rigidbody _rigidbody;
    private CorkPool _pool;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(CorkPool pool)
    {
        _pool = pool;
    }

    public void Hit(Vector3 position, Vector3 normal)
    {
        transform.position = position + normal * 0.02f;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Vector3 randomDirection = normal
            + Vector3.right * Random.Range(-0.4f, 0.4f)
            + Vector3.up * Random.Range(-0.2f, 0.4f);

        _rigidbody.AddForce(
            randomDirection.normalized * _bounceForce,
            ForceMode.Impulse
        );

        _rigidbody.AddTorque(
            Random.insideUnitSphere * 2f,
            ForceMode.Impulse
        );

        StartCoroutine(ReturnRoutine());
    }

    private IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(_returnTime);

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _pool.Return(this);
    }
}