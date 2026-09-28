using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{
    [SerializeField] private Target[] _targetPrefabs;

    private Dictionary<Target, Queue<Target>> _pools = new Dictionary<Target, Queue<Target>>();
    private Dictionary<Target, Target> _targetOwners = new Dictionary<Target, Target>();

    public Target Get()
    {
        Target prefab = _targetPrefabs[Random.Range(0, _targetPrefabs.Length)];

        if (!_pools.TryGetValue(prefab, out Queue<Target> pool))
        {
            pool = new Queue<Target>();
            _pools.Add(prefab, pool);
        }

        Target target;

        if (pool.Count > 0)
        {
            target = pool.Dequeue();
            target.gameObject.SetActive(true);
        }
        else
        {
            target = Instantiate(prefab, transform);
            _targetOwners.Add(target, prefab);
        }

        return target;
    }

    public void Return(Target target)
    {
        if (!_targetOwners.TryGetValue(target, out Target prefab))
        {
            return;
        }

        target.gameObject.SetActive(false);

        _pools[prefab].Enqueue(target);
    }

}
