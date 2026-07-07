using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Player
    public float playerHp;
    public float playerStm;
    public float playerHungryTime;
    public float posX, posY, posZ;

    // Inventory
    public List<IntPair> saveBackpack;
    public List<int> saveTool;
    public List<int> saveEqual;
    
    //Spawn
    public List<IntXVector3> saveSpawn;

    //Island
    public List<IntPair> saveIsland;

    //Time
    public float timeAngle;
    public bool isNight;
    public float day;
}
[Serializable]                                                                                                                                                                                                           
public struct IntPair
{
    public int key;
    public int value;
}
[Serializable]
public struct IntXVector3
{
    public float posX, posY, posZ;
    public int id;
}

