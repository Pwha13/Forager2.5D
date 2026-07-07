using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIManage : MonoBehaviour
{
    public NewInputControl InputControl;
    public GameObject content;
    public GameObject makeMainUI;
    public GameObject makeViewUI;
    public GameObject topUI;
    public GameObject handUI;
    public GameObject constructUI;
    public static UIManage Instance;

    public bool isMaking;

    private void Awake()
    {
        Instance = this;
        InputControl = new NewInputControl();
    }

    private void Start()
    {
        content.SetActive(true);
    }
    private void OnEnable()
    {
        InputControl.Enable();
        UIEventCenter.OnMake += GetIsMake;
        UIEventCenter.OnBuild += OnConstructAnima;
        UIEventCenter.OutBuild += OutConstructAnima;
    }

    private void OnDisable()
    {
        InputControl.Disable();
        UIEventCenter.OnMake -= GetIsMake;
        UIEventCenter.OnBuild -= OnConstructAnima;
        UIEventCenter.OutBuild -= OutConstructAnima;
    }

    private void Update()
    {
        if (InputControl.Player.Backpack.WasPressedThisFrame()&&!isMaking)
        {
            topUI.SetActive(!topUI.activeSelf);
            handUI.SetActive(!handUI.activeSelf);
            UIEventCenter.Instance.RaiseOnUiOpen(topUI.activeSelf);
            if (!topUI.activeSelf)
                UIEventCenter.Instance.RaiseOnUICancel();
        }
    }

    private void GetIsMake(bool b)
    {
        if (b)
        {
            InputControl.Disable();
        }
        else
        {
            InputControl.Enable();
        }
    }

    private void OnConstructAnima(ItemSO _1,ConRecipes _2)
    {
        if (topUI.TryGetComponent(out RectTransform topRect)) 
            topRect.DOAnchorPosY(-180, 1f);
        if (constructUI.TryGetComponent(out RectTransform conRect))
            conRect.DOAnchorPosX(200, 1f);
    }

    private void OutConstructAnima()
    {
        if (topUI.TryGetComponent(out RectTransform topRect))
            topRect.DOAnchorPosY(-285, 0.3f);
        if(constructUI.TryGetComponent(out RectTransform conRect))
            conRect.DOAnchorPosX(0, 0.3f);
    }
}
