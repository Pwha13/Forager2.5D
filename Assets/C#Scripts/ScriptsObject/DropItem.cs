using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropItem : MonoBehaviour
{
    public int itemID;
    public int count = 1;
    public bool isCollectable = true;
    
    private void OnTriggerEnter(Collider other)
    {
        if(!isCollectable) return;
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerCollect collector))
            {
                transform.DOKill();
                isCollectable = false;
                transform.DOMove(other.transform.position, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(
                        () =>
                        {
                            collector.hander.AddItem(this);
                            isCollectable = true;
                            ObjectPool.Instance.RecycleObject(gameObject);
                        });
            }
        }
    }
    [Header("掉落物参数")]
    [SerializeField] private float upHeight = 2f;  
    [SerializeField] private float duration = 0.4f; 
    
    public void PlayUpwardThrow()
    {
        Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        Vector3 targetPos = transform.position + new Vector3(randomDir.x, upHeight, randomDir.y);
        transform.DOMove(targetPos, duration);
        transform.DORotate(new Vector3(90, 0, 0), duration * 3);
    }
}
