using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "Game/Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemSprite;
    public GameObject itemPrefab;
    public bool stackable;
    public int maxStackCount;
}
