using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //PlayerData
    public float PlayerHp;
    public float PlayerStm;
    public float PlayerHungryTime;
    public Vector3 PlayerPos;
    
    //Inventory
    //id count
    public Dictionary<int, int> SaveBackpack;
    //id
    public List<int> SaveTool = new List<int>(){0,0,0,0,0};
    //index id
    public List<int> SaveEqual = new List<int>(){0,0};
    
    // //Island
    // //index state
    // public Dictionary<int,bool> SaveIsland;
    // //pos recoverTime
    // public Dictionary<Vector3,float> SaveIsSandBlock;
    //
    //Island
    //id state
    public Dictionary<int, bool> SaveIsland;

    //AllSpawn Vector3
    //Pos Type
    public Dictionary<Vector3, int> SaveSpawn = new Dictionary<Vector3, int>();

    //Time
    public float TimeAngle;
    public bool IsNight;
    public float Day;
}

// public class TableData
// {
//     public int ID;
//     public int MakeID;
//     public float MakeTime;
// }
