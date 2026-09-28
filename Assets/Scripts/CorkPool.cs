using System.Collections.Generic;
using UnityEngine;

public class CorkPool : MonoBehaviour
{
    [SerializeField] private Cork _corkPrefab;
    [SerializeField] private int _poolSize = 20;

    private Queue<Cork> _pool = new Queue<Cork>();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Cork cork = Instantiate(_corkPrefab, transform);

            cork.Init(this);
            cork.gameObject.SetActive(false);

            _pool.Enqueue(cork);
        }
    }

    public Cork Get()
    {
        if (_pool.Count == 0)
            return null;

        Cork cork = _pool.Dequeue();

        cork.gameObject.SetActive(true);

        return cork;
    }

    public void Return(Cork cork)
    {
        cork.gameObject.SetActive(false);

        _pool.Enqueue(cork);
    }
}