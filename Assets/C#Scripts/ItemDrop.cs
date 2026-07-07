using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour
{
    [Serializable]
    public class DropEntry
    {
        public int itemID;
        [Range(0,1)] public float dropChance;
        public int minCount;
        public int maxCount;
    }
    public List<DropEntry> dropTable;
    public bool destroy = true;
    public Vector3 offset;
    public void Drop()
    {
        foreach (var entry in dropTable)
        {
            if(Random.value > entry.dropChance)
                return;
            int count = Random.Range(entry.minCount, entry.maxCount + 1);
            ItemSO item = ItemDataBaseSO.itemDataBase.GetItem(entry.itemID);
            Spawn(item,count);
        }
    }

    public void Drop(float mul)
    {
        foreach (var entry in dropTable)
        {
            if(Random.value > entry.dropChance)
                return;
            int count = Random.Range(entry.minCount, entry.maxCount + 1)*(int)mul;
            ItemSO item = ItemDataBaseSO.itemDataBase.GetItem(entry.itemID);
            Spawn(item,count);
        }
    }

    private void Spawn(ItemSO item,int count)
    {
        for (int i = 0; i <= count - 1; i++)
        {
            GameObject obj = ObjectPool.Instance.GetObject(item.itemPrefab, transform);
            obj.transform.position += offset;
            if(obj.TryGetComponent(out DropItem drop)) drop.PlayUpwardThrow();
            obj.transform.SetParent(null, worldPositionStays: true);
        }
        if(destroy)
            ObjectPool.Instance.RecycleObject(gameObject);
    }
}
