using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "Game/Item/ToolSO")]
public class ToolSO : ItemSO
{
    [HideInInspector]
    public ToolType toolType;
    [HideInInspector]
    public float num1;
    [HideInInspector]
    public float num2;
}

public enum ToolType
{
    Pickaxe,Sword,None,Food,Build,Shovel,Hammer,Glove,Backpack    
}