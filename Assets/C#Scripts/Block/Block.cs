using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour,IMouseClickHandler
{
    public ItemDrop drop;
    private int isanima = 0;
    public float generateSpeed = 1;
    private Coroutine recoverCoroutine;
    public float recoverTime;
    public float animTime;
    public GameObject Glass;
    public GameObject Sand;
    public bool isSand;

    private void Start()
    {
        drop = GetComponent<ItemDrop>();
    }

    private void Update()
    {
        if (isanima != 3) 
            GenerateAnima();
    }

    private void GenerateAnima()
    {
        float y = transform.position.y;
        float speed = generateSpeed*Time.deltaTime;
        switch (isanima)
        {
            case 0:
                y += speed;
                if (y >= 3) isanima = 1;
                break;
            case 1:
                y -= speed;
                if (y <= -1) isanima = 2;
                break;
            case 2:
                y+= speed;
                if (y >= 0)
                {
                    isanima = 3;
                    y = 0;
                }
                break;
        }
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    public void OnMyMouseClick(ToolSO tool)
    {
        if(recoverCoroutine!=null) return;
        Glass.SetActive(false);
        Sand.SetActive(true);
        drop.Drop(tool.num1);
        isSand = true;
        recoverCoroutine = StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoverTime);
        Glass.SetActive(true);
        Sand.SetActive(false);
        isSand = false;
        recoverCoroutine = null;
    }
}
