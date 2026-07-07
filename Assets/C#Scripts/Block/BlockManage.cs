using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BlockManage : MonoBehaviour
{
    public GameObject block;
    public float GenerateTime = 1f;
    public int size = 6;
    public Vector2Int spawnPoint;
    
    private HashSet<Vector2Int> generatedPositions = new HashSet<Vector2Int>();
    private List<List<Vector2Int>> rings = new List<List<Vector2Int>>();
    
    public void SpawnIsland()
    {
        StartCoroutine(Generate(spawnPoint));
    }
    [Header("仅最外两圈不规则删减")]
    [Range(0f, 1f)] public float lastRingChance = 0.6f;      // 最外圈保留概率
    [Range(0f, 1f)] public float secondLastRingChance = 0.8f; // 开启：越外层越稀疏（更自然）

    // 固定方向（只创建一次，无GC）
    private static readonly Vector2Int[] Directions = { new(0,4), new(0,-4), new(4,0), new(-4,0) };
    
     private IEnumerator Generate(Vector2Int center)
    {
        ClearGeneratedData();
        CalculateFullRings(center);

        // ========== 核心：仅删除最外两圈，内部全部不动 ==========
        TrimLastTwoRings();

        // 按环逐层生成
        for (int i = 0; i < rings.Count; i++)
        {
            foreach (Vector2Int pos in rings[i])
            {
                Vector3 worldPos = new Vector3(pos.x, 0, pos.y);
                Instantiate(block, worldPos, Quaternion.identity, transform);
            }
            yield return new WaitForSeconds(GenerateTime);
        }
    }

    // 第一步：完整BFS，全部圈100%生成，不做任何概率截断
    private void CalculateFullRings(Vector2Int center)
    {
        if (!IsInBounds(center)) return;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, int> distanceDict = new Dictionary<Vector2Int, int>();

        queue.Enqueue(center);
        distanceDict[center] = 0;
        generatedPositions.Add(center);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDist = distanceDict[current];

            while (rings.Count <= currentDist)
                rings.Add(new List<Vector2Int>());
            rings[currentDist].Add(current);

            foreach (Vector2Int dir in Directions)
            {
                Vector2Int neighbor = current + dir;
                if (IsInBounds(neighbor) && !generatedPositions.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    distanceDict[neighbor] = currentDist + 1;
                    generatedPositions.Add(neighbor);
                }
            }
        }
    }

    // 第二步：生成完所有环之后，单独剔除【最后两圈】的随机节点
    private void TrimLastTwoRings()
    {
        int totalRingCount = rings.Count;
        // 圈数不够两层就不删减
        if (totalRingCount < 3) return;

        int lastIndex = totalRingCount - 1;        // 最外圈
        int secondLastIndex = totalRingCount - 2;  // 倒数第二圈

        // 处理倒数第二圈
        List<Vector2Int> secondLastRing = rings[secondLastIndex];
        secondLastRing.RemoveAll(pos => Random.value > secondLastRingChance);

        // 处理最外圈
        List<Vector2Int> lastRing = rings[lastIndex];
        lastRing.RemoveAll(pos => Random.value > lastRingChance);
    }

    private bool IsInBounds(Vector2Int pos)
    {
        // 以 spawnPoint 为中心，size 为范围
        int minX = spawnPoint.x - size;
        int maxX = spawnPoint.x + size;
        int minY = spawnPoint.y - size;
        int maxY = spawnPoint.y + size;

        return pos.x >= minX && pos.x <= maxX &&
               pos.y >= minY && pos.y <= maxY;
    }

    private void ClearGeneratedData()
    {
        rings.Clear();
        generatedPositions.Clear();
    }
    
}
