using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUImodel : MonoBehaviour
{
    public int currentBar;
    public InventoryHander hander;
    public HandUIview view;
    public NewInputControl InputControl;

    private void OnEnable()
    {
        InputControl.Enable();
        InventoryHander.OnToolAdd += ChangeTool;
        InventoryHander.OnToolAdd += UpdateBarSprite;
        UpdateBarSprite();
        if (currentBar == 4)
            ChangeTool();
    }

    private void OnDisable()
    {
        InputControl.Disable();
        InventoryHander.OnToolAdd -= ChangeTool;
        InventoryHander.OnToolAdd -= UpdateBarSprite;
    }

    private void Awake()
    {
        view = transform.GetComponent<HandUIview>();
        InputControl = new NewInputControl();
        InputControl.UI.Button1.performed += ctx => ChangeTool(0);
        InputControl.UI.Button2.performed += ctx => ChangeTool(1);
        InputControl.UI.Button3.performed += ctx => ChangeTool(2);
        InputControl.UI.Button4.performed += ctx => ChangeTool(3);
        InputControl.UI.Button5.performed += ctx => ChangeTool(4);
    }

    private void Start()
    {
        hander.InitHand();
        currentBar = 1;
        ChangeTool(0);
        UpdateBarSprite();
    }

    public void ChangeTool()
    {
        UIEventCenter.Instance.RaiseOnToolChange(hander.HandList[currentBar]);
    }

    public void ChangeTool(int i)
    {
        if (i == currentBar) return;
        view.barList[i].SetActive(true);
        view.barList[currentBar].SetActive(false);
        currentBar = i;
        if (i < hander.HandList.Count)
        {
            UIEventCenter.Instance.RaiseOnToolChange(hander.HandList[i]);
            return;
        }

        UIEventCenter.Instance.RaiseOnToolChange(null);
    }

    public void UpdateBarSprite()
    {
        for (int i = 0; i < view.barList.Count; i++)
        {
            if (hander.HandList[i] == null)
                view.toolImageList[i].sprite = view.nullSprite;
            else
                view.toolImageList[i].sprite = hander.HandList[i].itemSprite;
        }
    }
}
