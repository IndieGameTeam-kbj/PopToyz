using UnityEngine;

public class TargetReturnFloor : MonoBehaviour
{
    [SerializeField] private TargetPool _targetPool;

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();
        if (target == null) return;

        _targetPool.Return(target);
    }

}
