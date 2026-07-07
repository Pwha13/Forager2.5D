using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> prefabList= new List<GameObject>();
    private class PoolInfo
    {
        public Queue<GameObject> Pool;
        public float LastUseTime;
        public float RecycleTime;
    }
    [SerializeField] private int poolSize;
    [SerializeField] private float recycleTime;
    private float _currentCountTime;
    public static ObjectPool Instance;
    private readonly Dictionary<int, PoolInfo> _dict = new();
    private void Awake()
    {
        Instance = this;
        _currentCountTime = recycleTime;
    }

    private void Update()
    {
        _currentCountTime -= Time.deltaTime;
        if (_currentCountTime <= 0)
        {
            CleanTimeCount();
            _currentCountTime = recycleTime;
        }
    }
    
    public GameObject GetObject(GameObject prefab,Transform parent)
    {
        if (!prefab.TryGetComponent(out PoolID poolID))
            return Instantiate(prefab, parent.transform.position,prefab.transform.rotation);
        int key = poolID.id;
        if (!_dict.TryGetValue(key, out PoolInfo poolInfo))
        {
            poolInfo = new PoolInfo { Pool = new Queue<GameObject>(), LastUseTime = Time.time ,RecycleTime = poolID.recycleTime};
            _dict[key] = poolInfo;
        }
        poolInfo.LastUseTime = Time.time;
        GameObject obj = poolInfo.Pool.Count > 0 ? poolInfo.Pool.Dequeue() : CreateObj(prefab,parent);
        obj.transform.position = parent.transform.position;
        obj.transform.SetParent(parent);
        obj.SetActive(true);
        return obj;
    }

    public GameObject GetObject(int id,Vector3 pos)
    {
        GameObject obj = prefabList[id-1];
        return Instantiate(obj, pos, obj.transform.rotation);
    }
    public void RecycleObject(GameObject obj)
    {
        if (!obj.TryGetComponent(out PoolID poolID))
        {
            Destroy(obj);
            return;
        }
        int key = poolID.id;
        if (!_dict.TryGetValue(key, out PoolInfo poolInfo))
        {
            poolInfo = new PoolInfo { Pool = new Queue<GameObject>(), LastUseTime = Time.time ,RecycleTime = poolID.recycleTime};
            _dict[key] = poolInfo;
        }
        poolInfo.LastUseTime = Time.time;
        if (poolInfo.Pool.Count >= poolSize)
        {
            Destroy(obj);
            return;
        }
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        poolInfo.Pool.Enqueue(obj);
    }

    private void CleanTimeCount()
    {
        float now = Time.time;
        var keysToRemove = new List<int>();
        foreach (var pool in _dict)
        {
            if(now - pool.Value.LastUseTime < pool.Value.RecycleTime) continue;
            if (pool.Value.Pool.Count > 0)
                Destroy(pool.Value.Pool.Dequeue());
            if(pool.Value.Pool.Count == 0)
                keysToRemove.Add(pool.Key);
        }
        foreach (var key in keysToRemove)
        {
            _dict.Remove(key);
        }
    }

    private GameObject CreateObj(GameObject prefab,Transform parent)
    {
        GameObject obj = Instantiate(prefab, prefab.transform.position,prefab.transform.rotation);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }

    private void OnDestroy()
    {
        foreach (var pool in _dict.Values)
        {
            while (pool.Pool.Count > 0)
            {
                Destroy(pool.Pool.Dequeue());
            }
        }
        _dict.Clear();
        Instance = null;
    }
}
