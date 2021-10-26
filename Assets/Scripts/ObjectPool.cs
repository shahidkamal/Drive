using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Pool { get; private set; }

    private List<GameObject> _objectPool;

#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Game Object to pool")] [SerializeField]
    private GameObject prefab;

    [Tooltip("Maximum pool size")] [SerializeField] 
    private int poolSize;
#pragma warning restore 649 // 'field' is never assigned to

    public int Count { get; private set; }

    private void Awake()
    {
        Pool = this;
        Count = 0;
        _objectPool = new List<GameObject>();
        for (var i = 0; i < poolSize; ++i)
        {
            var go = Instantiate(prefab, gameObject.transform, true);
            go.SetActive(false);
            _objectPool.Add(go);
        }
    }

    public GameObject Get()
    {
        for (var i = 0; i < poolSize; ++i)
        {
            if (!_objectPool[i].activeInHierarchy)
            {
                ++Count;
                return _objectPool[i];
            }
        }

        return null;
    }

    public void Release(GameObject go)
    {
        for (var i = 0; i < poolSize; ++i)
        {
            if (go == _objectPool[i] && _objectPool[i].activeInHierarchy)
            {
                --Count;
                go.SetActive(false);
                return;
            }
        }
    }
    
    public IEnumerable<GameObject> Iterator()
    {
        foreach (var go in _objectPool)
        {
            yield return go;
        }
    }
}
