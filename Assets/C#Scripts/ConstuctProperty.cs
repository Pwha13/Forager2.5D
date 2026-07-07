using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstuctProperty : MonoBehaviour
{
    //IMouseRightClick
    public Vector3 posOffset;
    public SpriteRenderer sr;
    public List<MeshRenderer> mr;
    public BuildType buildType;
    public BuildSize buildSize;

    private float gridSize;
    private Color normalColor;
    private Collider myCollider;
    private Collider[] results = new Collider[8];
    private LayerMask groundMask;
    private LayerMask spawnConstructMask;

    void Awake()
    {
        if (buildType == BuildType.Water)
            normalColor = mr[0].material.color;
        myCollider = GetComponent<Collider>();
        if(buildType == BuildType.Ground)
            groundMask = LayerMask.GetMask("Ground");
        else
            groundMask = LayerMask.GetMask("Water");
        spawnConstructMask = LayerMask.GetMask("Spawn", "Construct");
        gridSize = GridManage.Instance.gridSize;
        
    }

    bool OverlapCheck(Vector3 position, LayerMask mask)
    {
        int count = Physics.OverlapSphereNonAlloc(position, 0.5f, results, mask);
        for (int i = 0; i < count; i++)
        {
            if (results[i] != myCollider)
                return true;
        }
        return false;
    }

    public bool PhysicCheck()
    {
        Vector3 mainPos = GridManage.Instance.GridCenterPos(transform.position - posOffset);
        if (buildSize == BuildSize.One)
        {
            if (!OverlapCheck(mainPos + Vector3.down * gridSize, groundMask))
            { Red(); return false; }
            if (OverlapCheck(mainPos + Vector3.down * gridSize, spawnConstructMask))
            { Red(); return false; }

            Normal();
            return true;
        }
        if (buildSize == BuildSize.Four)
        {
            if (!OverlapCheck(mainPos + Vector3.down * gridSize, groundMask))
            { Red(); return false; }
            if (!OverlapCheck(mainPos + (Vector3.down + Vector3.left) * gridSize, groundMask))
            { Red(); return false; }
            if (!OverlapCheck(mainPos + (Vector3.down + Vector3.forward) * gridSize, groundMask))
            { Red(); return false; }
            if (!OverlapCheck(mainPos + (Vector3.down + Vector3.left + Vector3.forward) * gridSize, groundMask))
            { Red(); return false; }

            if (OverlapCheck(mainPos, spawnConstructMask))
            { Red(); return false; }
            if (OverlapCheck(mainPos + Vector3.forward * gridSize, spawnConstructMask))
            { Red(); return false; }
            if (OverlapCheck(mainPos + Vector3.left * gridSize, spawnConstructMask))
            { Red(); return false; }
            if (OverlapCheck(mainPos + (Vector3.forward + Vector3.left) * gridSize, spawnConstructMask))
            { Red(); return false; }

            Normal();
            return true;
        }
        Red();
        return false;
    }

    public void Red()
    {
        if(buildType == BuildType.Water)
            foreach (var r in mr)
            {
                r.material.color = new Color(1, 0, 0);
            }
        else
            sr.color = new Color(1, 0, 0);
    }

    public void Normal()
    {
        if(buildType == BuildType.Water)
            foreach (var r in mr)
            {
                r.material.color = normalColor;
            }
        else 
            sr.color = new Color(1, 1, 1);
    }

    public string GetBuildType()
    {
        return buildType switch
        {
            BuildType.Ground => "Ground",
            BuildType.Water => "Water",
            _ => "Null"
        };
    }
    
}

public enum BuildType
{
    Ground,Water
}

public enum BuildSize
{
    One , Four
}
