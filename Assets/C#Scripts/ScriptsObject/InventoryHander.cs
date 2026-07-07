using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryHander : MonoBehaviour,ISavable
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private List<int> defaultItem;

    public static event Action OnToolAdd;
    public static event Action<ToolSO> OnGloveAdd;
    public static event Action<ToolSO> OnSwordAdd;
    public event Action OnInventoryChanged;
    public ItemDataBaseSO itemdatabase;
    
    public List<InventorySlot> Slots => inventory.slots;
    public List<ItemSO> HandList => inventory.handList;
    public List<ItemSO> EqualList => inventory.equalList;
    
    public int MaxSlots
    {
        get => inventory.maxSlots;
        set => inventory.maxSlots = value;
    }

    public int GetDict(int id) => inventory.GetDict(id);
    public int UsedSlots() => inventory.UsedSlots();

    private int ItemSlots(ItemSO item, int count) =>
        (count + item.maxStackCount - 1) / item.maxStackCount;

    public void AddItem(DropItem dropItem)
    {
        ItemSO currentItem = ItemDataBaseSO.itemDataBase.GetItem(dropItem.itemID);
        if (currentItem is ToolSO tool)
        {
            if (tool.toolType == ToolType.Hammer || tool.toolType == ToolType.Pickaxe ||
                tool.toolType == ToolType.Shovel || tool.toolType == ToolType.Sword)
            {
                GetTool(tool);
                OnToolAdd?.Invoke();
                if (tool.toolType == ToolType.Sword)
                    OnSwordAdd?.Invoke(tool);
            }
            else if (tool.toolType == ToolType.Glove)
            {
                GetTool(tool);
                OnGloveAdd?.Invoke(tool);
            }
            else if (tool.toolType == ToolType.Backpack)
            {
                GetTool(tool);
                inventory.maxSlots = (int)tool.num1;
                AddSlots();
            }
            else
                AddDict(currentItem, dropItem.count);
        }
        else
            AddDict(currentItem, dropItem.count);
    }

    public void AddFood(ToolSO tool)
    {
        GetTool(tool);
    }

    public void EatFood(ToolSO tool)
    {
        SubDict(tool.itemID, 1);
        if (GetDict(tool.itemID) == 0)
        {
            inventory.handList[4] = null;
            OnToolAdd?.Invoke();
        }
    }
    

    public void InitSlots()
    {
        inventory.slots.Clear();
        for (int i = 0; i < inventory.maxSlots; i++)
            inventory.slots.Add(new InventorySlot());
        OnInventoryChanged?.Invoke();
    }

    public void AddSlots()
    {
        int addCount = inventory.maxSlots - inventory.slots.Count;
        for (int i = 0; i < addCount; i++)
            inventory.slots.Add(new InventorySlot());
        OnInventoryChanged?.Invoke();
    }

    public void AddDict(ItemSO item, int count)
    {
        var dict = inventory.SlotsDict;
        if (dict.TryGetValue(item.itemID, out int current))
        {
            int newTotal = current + count;
            int oldSlots = ItemSlots(item, current);
            int newSlots = ItemSlots(item, newTotal);
            if (UsedSlots() + (newSlots - oldSlots) > inventory.maxSlots) return;
            dict[item.itemID] = newTotal;
        }
        else
        {
            if (UsedSlots() + ItemSlots(item, count) > inventory.maxSlots) return;
            dict.Add(item.itemID, count);
        }
        OnInventoryChanged?.Invoke();
    }

    public void SubDict(int id, int count)
    {
        var dict = inventory.SlotsDict;
        if (!dict.TryGetValue(id, out int current)) return;
        int newCount = current - count;
        if (newCount <= 0)
            dict.Remove(id);
        else
            dict[id] = newCount;
        OnInventoryChanged?.Invoke();
    }

    public void GetTool(ToolSO tool)
    {
        switch (tool.toolType)
        {
            case ToolType.Pickaxe: 
                inventory.handList[0] = tool; 
                break;
            case ToolType.Sword: 
                inventory.handList[1] = tool;
                break;
            case ToolType.Shovel:
                inventory.handList[2] = tool;
                break;
            case ToolType.Hammer:
                inventory.handList[3] = tool;
                break;
            case ToolType.Glove:
                inventory.equalList[0] = tool;
                break;
            case ToolType.Backpack:
                inventory.equalList[1] = tool;
                break;
            default:
                inventory.handList[4] = tool;
                break;
            }
        OnInventoryChanged?.Invoke();
    }

    public void InitHand()
    {
        for (int i = 0; i < defaultItem.Count; i++)
        {
            inventory.handList[i] = itemdatabase.GetItem(defaultItem[i]);
        }
    }

    public void LoadSlotsByID()
    {
        var slots = inventory.slots;
        for (int k = 0; k < slots.Count; k++)
        {
            slots[k].item = null;
            slots[k].count = 0;
        }
        int i = 0;
        foreach (var kv in inventory.SlotsDict.OrderBy(k => k.Key))
        {
            ItemSO item = ItemDataBaseSO.itemDataBase.GetItem(kv.Key);
            if (item == null) continue;
            int remain = kv.Value;
            int stack = item.maxStackCount;
            while (remain > 0)
            {
                if (i >= slots.Count)
                {
                    Debug.LogWarning("背包容量不足，无法容纳所有物品");
                    return;
                }
                int put = remain > stack ? stack : remain;
                slots[i].item = item;
                slots[i].count = put;
                i++;
                remain -= put;
            }
        }
        OnInventoryChanged?.Invoke();
    }
    

    private void Start()
    {
        if (inventory.handList[1] != null && inventory.handList[1] is ToolSO tool)
            OnSwordAdd?.Invoke(tool);
    }

    public void OnEnable()
    {
        ISavable savable = this;
        savable.RegisterSaveData();
        UIEventCenter.OnFoodGet += AddFood;
        MouseManage.OnEat += EatFood;
    }

    private void OnDisable()
    {
        ISavable savable = this;
        savable.UnRegisterSaveData();
        UIEventCenter.OnFoodGet -= AddFood;
        MouseManage.OnEat -= EatFood;
    }

    public void Save(Data data)
    {
        data.SaveBackpack = inventory.SlotsDict;
        for (int i = 0; i < inventory.handList.Count; i++)
        {
            int id = 0;
            if (inventory.handList[i] != null)
                id = inventory.handList[i].itemID;
            data.SaveTool[i] = id;
        }
        for (int i = 0; i < inventory.equalList.Count; i++)
        {
            int id = 0;
            if (inventory.equalList[i] != null)
                id = inventory.equalList[i].itemID;
            data.SaveEqual[i] = id;
        }
    }

    public void Load(Data data)
    {
        //可能先取装备再取背包
        if(data.SaveBackpack == null) return;
        inventory.SlotsDict = data.SaveBackpack;
        for (int i = 0; i < data.SaveTool.Count; i++)
            inventory.handList[i] = ItemDataBaseSO.itemDataBase.GetItem(data.SaveTool[i]);
        for (int i = 0; i < data.SaveEqual.Count; i++) 
            inventory.equalList[i] = ItemDataBaseSO.itemDataBase.GetItem(data.SaveEqual[i]);
        
        OnToolAdd?.Invoke();
        OnInventoryChanged?.Invoke();
        if(inventory.handList[1]!=null && inventory.handList[1] is ToolSO sword)
            OnSwordAdd?.Invoke(sword);
        if(inventory.equalList[0]!=null && inventory.equalList[0] is ToolSO glove)
            OnGloveAdd?.Invoke(glove);
        if (inventory.equalList[1] != null && inventory.equalList[1] is ToolSO backpack)
        {
            inventory.maxSlots = (int)backpack.num1;
            AddSlots();
        }
    }
}
