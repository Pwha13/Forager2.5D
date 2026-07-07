using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class SpawnProperty : MonoBehaviour,IMouseEnterHandler,IMouseExitHandler,IMouseClickHandler
{
    private ItemDrop itemDrop;
    public GameObject tipUI;
    public bool isMakeTable;
    private Vector3 _scale;
    public int durability;
    public int currentDurability;
    
    [Header("hit")] 
    public bool isBridge;
    public float hitDistance;

    private void Awake()
    {
        itemDrop = GetComponent<ItemDrop>();
        currentDurability = durability;
        _scale = transform.localScale;
    }

    public void Broke(float intensity)
    {
        currentDurability -= (int)intensity;
        if (currentDurability <= 0)
        {
            currentDurability = durability;
            itemDrop.Drop();
            transform.DOKill();
        }
        else
        {
            transform.DOKill();
            transform.DOScale(_scale*1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }
    }

    public void OnMyMouseEnter(ToolSO tool)
    {
        if (tool.toolType == ToolType.Pickaxe||(tool.toolType == ToolType.Hammer&&isMakeTable))
        {
            tipUI.SetActive(true);
        }
        else
        {
            tipUI.SetActive(false);
        }
    }

    public void OnMyMouseExit(ToolSO tool)
    {
        tipUI.SetActive(false);
    }

    public void OnMyMouseClick(ToolSO tool)
    {
        if (tool.toolType == ToolType.Pickaxe)
        {
            if(isBridge)
                if(!Hit())
                    return;
            Broke(tool.num1);
        }
    }
    private bool Hit()
    {
        return Physics.BoxCast(transform.position + new Vector3(2, 2, 2), Vector3.zero, Vector3.up, out RaycastHit hit,
            new Quaternion(0, 0, 0, 0), hitDistance, LayerMask.GetMask("Player","Spawn","Construct"));
    }
}
