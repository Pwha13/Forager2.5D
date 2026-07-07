using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AphlaAnim : MonoBehaviour
{
    public ImageType type;
    Image image;
    SpriteRenderer sprite;
    private void Awake()
    {
        GetImage();
    }

    void GetImage()
    {
        if (type == ImageType.Image)
        {
            image = GetComponent<Image>();
        }
        else
        {
            sprite = GetComponent<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        Doanim();
    }

    private void OnDisable()
    {
        this.DOKill();
    }

    private void OnDestroy()
     {
         if (transform != null) transform.DOKill(true);
         this.DOKill(true); // 关键！杀死当前脚本的动画
     }

    void Doanim()
    {
        if (transform != null) transform.DOKill(true);
        this.DOKill(true); // 关键！杀死当前脚本的动画
        if(this == null || gameObject == null) return;
        if (type == ImageType.Image)
        {
            image.color = Color.white;
            image.DOFade(0, 0.5f).SetTarget(this).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }
        else
        {
            sprite.color = Color.white;
            sprite.DOFade(0, 0.5f).SetTarget(this).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }
    }
}

public enum ImageType
{
    Image,Sprite
}