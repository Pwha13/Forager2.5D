using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandToolShow : MonoBehaviour
{
    private SpriteRenderer currentTool;
    private bool isUIing;
    public bool isLeft;
    private void Awake()
    {
        currentTool = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        
        UIEventCenter.OnToolChange += ChangeToolShow;
        UIEventCenter.OnUiOpen += ChangeIsUI;
        UIEventCenter.OnMake += ChangeIsUI;
        MouseManage.OnClick += ClickAnim;
    }
    private void OnDisable()
    {
        UIEventCenter.OnToolChange -= ChangeToolShow;
        UIEventCenter.OnUiOpen -= ChangeIsUI;
        UIEventCenter.OnMake -= ChangeIsUI;
        MouseManage.OnClick -= ClickAnim;
    }

    private void Update()
    {
        ToolDir();
    }
    private void ToolDir()
    {
        float eulerAngle;
        if (isLeft) eulerAngle = 0;
        else eulerAngle = 90;
        transform.localRotation = Quaternion.Euler(0, 0, eulerAngle);
    }

    private void ClickAnim()
    {
        transform.DOKill();
        if (isLeft) transform.DOLocalRotate(new Vector3(0, 0, 135), 0.4f).SetLoops(1, LoopType.Yoyo);
        else transform.DOLocalRotate(new Vector3(0, 0, -45), 0.4f).SetLoops(1, LoopType.Yoyo);
    }

    private void ChangeToolShow(ItemSO item)
    {
        currentTool.sprite = item?.itemSprite;
    }

    private void ChangeIsUI(bool b)
    {
        isUIing = b;
    }
}
