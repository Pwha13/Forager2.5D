using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour,IMouseClickHandler,IMouseEnterHandler,IMouseExitHandler
{
    public int Id;
    public GameObject tip;
    public bool isBought;
    public BlockManage block;
    
    public void Buy()
    {
        block.SpawnIsland();
        isBought = true;
    }
    public void RegisterState()
    {
        IslandManage.Instance.Register(Id);
    }

    public void OnMyMouseClick(ToolSO tool)
    {
        if(isBought) return;
        Buy();
        RegisterState();
    }

    public void OnMyMouseEnter(ToolSO tool)
    {
        tip.SetActive(true);
    }

    public void OnMyMouseExit(ToolSO tool)
    {
        tip.SetActive(false);
    }
}
