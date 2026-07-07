using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManage : MonoBehaviour
{
    public static GridManage Instance;
    public float gridSize = 4f;
    
    private void Awake()
    {
        Instance = this;
    }
    public Vector3 GridPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / gridSize);
        int y = Mathf.RoundToInt(worldPos.y / gridSize);
        int z = Mathf.RoundToInt(worldPos.z / gridSize);
        Vector3 gridPos = new Vector3(x*gridSize, y*gridSize, z*gridSize);
        return gridPos;
    }

    public Vector3 GridCenterPos(Vector3 worldPos)
    {
        Vector3 gridPos = GridPos(worldPos);
        Vector3 gridCenterPos = new Vector3(gridPos.x + 2, gridPos.y + 2, gridPos.z + 2);
        return gridCenterPos;
    }
}
