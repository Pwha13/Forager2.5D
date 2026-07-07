using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generate : MonoBehaviour
{
    private GameObject prefab;
    private int totalWeight;
    public List<Spawn> theSpawn;
    private float speed;
    public float generateTime;
    private float CurrentTime;
    public Block block;

    [Header("hit")] 
    public float hitdistance;
    public bool ishit;
    
    [Serializable]
    public class Spawn
    {
        public GameObject prefab;
        public int weight;
        public bool nightSpawn;
    }
    
    private void Start()
    {
        block = GetComponent<Block>();
        CurrentTime = generateTime;
        speed = UnityEngine.Random.Range(0.5f, 5f);
        GetTotalWeight();
        GetPrefab();
        StartCoroutine(HitCheck());
    }

    private void Update()
    {
        Judge();
    }

    private void GetTotalWeight()
    {
        foreach (var item in theSpawn)
        {
            totalWeight += item.weight;
        }
    }
    private void GetPrefab()
    {
        float value = Random.Range(0, totalWeight);
        int currentWeight = 0;
        foreach (var item in theSpawn)
        {
            currentWeight += item.weight;
            if (value < currentWeight)
            {
                if (item.nightSpawn)
                {
                    if (!TimeManage.timeManage.isNight)
                    {
                        GetPrefab();
                        break;
                    }
                }
                prefab = item.prefab;
                break;
            }
        }
    }
    private void Judge()
    {
        if (!ishit&&!block.isSand)
        {
            CurrentTime -= speed * Time.deltaTime;
            if (CurrentTime <= 0)
            {
                CurrentTime = generateTime;
                GameObject child = ObjectPool.Instance.GetObject(prefab, transform);
                child.transform.localPosition = new Vector3(2, 4, 2);
                speed = UnityEngine.Random.Range(0.5f, 3);
                GetPrefab();
            }
        }
    }

    private void Hit()
    {
        ishit = Physics.BoxCast(transform.position + new Vector3(2, 2, 2), Vector3.zero, Vector3.up, out RaycastHit hit,
            new Quaternion(0, 0, 0, 0), hitdistance, LayerMask.GetMask("Player","Spawn","Construct"));
    }

    IEnumerator HitCheck()
    {
        var wait = new WaitForSeconds(0.5f);
        while (true)
        {
            Hit();
            yield return wait;
        }
    }
}
