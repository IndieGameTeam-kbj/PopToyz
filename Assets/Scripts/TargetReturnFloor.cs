using System;
using UnityEngine;

public class TargetReturnFloor : MonoBehaviour
{
    [SerializeField] private TargetPool _targetPool;

    public static event Action TargetMissed;

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();
        if (target == null) return;

        if(target.State == TargetState.Idle)
        {
            TargetMissed?.Invoke();
        }
        _targetPool.Return(target);
    }

}
