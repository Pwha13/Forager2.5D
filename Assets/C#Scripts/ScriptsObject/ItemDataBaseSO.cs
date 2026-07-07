using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBaseSO", menuName = "Game/ItemDataBaseSO")]
public class ItemDataBaseSO : ScriptableObject
{
    public static ItemDataBaseSO itemDataBase;
    public List<ItemSO> allItems;
    private Dictionary<int , ItemSO> itemDictionary;

    public void OnEnable()
    {
        itemDataBase = this;
        InitDictionary();
    }

    public void InitDictionary()
    {
        itemDictionary = new Dictionary<int, ItemSO>();
        foreach (ItemSO item in allItems)
        {
            if(!itemDictionary.ContainsKey(item.itemID))
            {
                itemDictionary.Add(item.itemID, item);
            }
        }
    }
    public ItemSO GetItem(int itemID)
    {
        if(itemDictionary.TryGetValue(itemID, out ItemSO item))
            return item;
        return null;
    }
}
