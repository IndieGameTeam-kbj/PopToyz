using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{
    [SerializeField] private Target[] _targetPrefabs;

    private Dictionary<Target, Queue<Target>> _pools = new();
    private Dictionary<Target, Target> _targetOwners = new();

    private void Start()
    {
        GameManager.GameRestarted += Reset;
    }

    private void OnDestroy()
    {
        GameManager.GameRestarted -= Reset;
    }

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
        }
        else
        {
            target = Instantiate(prefab, transform);
            target.Init();
            _targetOwners.Add(target, prefab);
        }

        target.gameObject.SetActive(false);
        return target;
    }

    public void Return(Target target)
    {
        if (!_targetOwners.TryGetValue(target, out Target prefab)) return;

        target.ResetPhysics();
        target.gameObject.SetActive(false);
        _pools[prefab].Enqueue(target);
    }

    public void Reset()
    {
        foreach (Target target in _targetOwners.Keys)
        {
            if (target.gameObject.activeSelf)
            {
                Return(target);
            }
        }
    }

}
