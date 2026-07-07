using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Game/InventorySO")]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public Dictionary<int, int> SlotsDict = new Dictionary<int, int>();
    public int maxSlots;
    public List<ItemSO> handList = new List<ItemSO>(){null,null,null,null,null};
    public List<ItemSO> equalList = new List<ItemSO>(){null,null};

    public int GetDict(int id)
    {
        if (SlotsDict.TryGetValue(id, out int count))
            return count;
        return 0;
    }

    public int UsedSlots()
    {
        int total = 0;
        foreach (var kv in SlotsDict)
        {
            ItemSO item = ItemDataBaseSO.itemDataBase.GetItem(kv.Key);
            if (item != null)
                total += (kv.Value + item.maxStackCount - 1) / item.maxStackCount;
        }
        return total;
    }
}

public class InventorySlot
{
    public ItemSO item;
    public int count;
    public bool IsEmpty => item == null || count <= 0;
}
