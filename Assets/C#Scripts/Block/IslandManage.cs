using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManage : MonoBehaviour, ISavable
{
    [SerializeField]private List<Island> islands;
    [SerializeField]private List<bool> islandStates;
    public static IslandManage Instance;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < islands.Count; i++)
        {
            islands[i].Id = i;
            if(islandStates[i])
                islands[i].Buy();
        }
    }

    private void OnEnable()
    {
        ISavable savable = this;
        savable.RegisterSaveData();
    }

    private void OnDisable()
    {
        ISavable savable = this;
        savable.UnRegisterSaveData();
    }

    public void Register(int id) => islandStates[id] = true;

    public void Save(Data data)
    {
        data.SaveIsland = new Dictionary<int, bool>();
        foreach (var island in islands)
            data.SaveIsland[island.Id] = island.isBought;
    }

    public void Load(Data data)
    {
        if (data.SaveIsland == null) return;
        foreach (var island in islands)
        {
            if (data.SaveIsland.TryGetValue(island.Id, out bool bought) && bought && !island.isBought)
                island.Buy();
        }
    }
}
