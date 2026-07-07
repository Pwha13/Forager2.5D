using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Construct",menuName = "Game/Item/Construct")]
public class ConstructSO : ItemSO
{ 
    public ConstructType constructType;
}
public enum ConstructType
{
    Water,Ground    
}
