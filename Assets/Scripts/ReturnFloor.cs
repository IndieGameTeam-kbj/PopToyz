using System;
using UnityEngine;

public class ReturnFloor : MonoBehaviour
{
    [SerializeField] private TargetPool _targetPool;
    [SerializeField] private CorkPool _corkPool;

    public static event Action TargetMissed;

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();

        if (target != null)
        {
            if (target.State == TargetState.Idle)
            {
                TargetMissed?.Invoke();
            }

            target.ResetPhysics();
            _targetPool.Return(target);
            return;
        }

        Cork cork = other.GetComponent<Cork>();

        if (cork != null)
        {
            cork.ResetPhysics();
            _corkPool.Return(cork);
        }
    }

}
