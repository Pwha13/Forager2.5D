using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class TableController : MonoBehaviour,IMouseRightClickHandler,IMouseClickHandler
{
    public TableModel model;
    public TableView view;
    public GameObject viewUI;
    public GameObject mainUI;
    public MakeMainUI main;
    public int currentIndex;

    private void Start()
    {
        viewUI = UIManage.Instance.makeViewUI;
        mainUI = UIManage.Instance.makeMainUI;
        view = UIManage.Instance.makeViewUI.GetComponent<TableView>();
        main = UIManage.Instance.makeMainUI.GetComponent<MakeMainUI>();
    }

    public void OnMyMouseRightClick(ToolSO tool)
    {
        if(model.isMaking) return;
        MakeMaterialUI.OnClick += MainShow;
        model.InvokeOnMake(true);
        main.button.onClick.AddListener(Make);
        model.CheckRecipes();
        view.GetInfo(model.lastRecipes,model.recipesMakable);
        viewUI.SetActive(true);
    }

    public void Make()
    {
        model.Make(currentIndex);
        Exit();
    }

    public void MainShow(int index)
    {
        if(!mainUI.activeSelf) mainUI.SetActive(true);
        currentIndex = index;
        ItemSO result = ItemDataBaseSO.itemDataBase.GetItem(model.lastRecipes[index].resultID);
        main.image.sprite = result.itemSprite;
        main.text.text = result.itemName;
        ItemSO material1 = ItemDataBaseSO.itemDataBase.GetItem(model.lastRecipes[index].item1ID);
        main.material1.image.sprite = material1.itemSprite;
        main.material1.text.text = material1.itemName + "\n" +
                                   model.hander.GetDict(model.lastRecipes[index].item1ID) + "/" + model.lastRecipes[index].item1Count;
        if(model.lastRecipes[index].item2ID == 0)
        {
            main.material2UI.SetActive(false);
            main.material3UI.SetActive(false);
            main.button.interactable = model.recipesMakable[index];
            return;
        }
        main.material2UI.SetActive(true);
        ItemSO material2 = ItemDataBaseSO.itemDataBase.GetItem(model.lastRecipes[index].item2ID);
        main.material2.image.sprite = material2.itemSprite;
        main.material2.text.text = material2.itemName + "\n" +
                                   model.hander.GetDict(model.lastRecipes[index].item2ID) + "/" + model.lastRecipes[index].item2Count;
        if(model.lastRecipes[index].item3ID == 0)
        {
            main.material3UI.SetActive(false);
            main.button.interactable = model.recipesMakable[index];
            return;
        }
        main.material3UI.SetActive(true);
        ItemSO material3 = ItemDataBaseSO.itemDataBase.GetItem(model.lastRecipes[index].item3ID);
        main.material3.image.sprite = material3.itemSprite;
        main.material3.text.text = material3.itemName + "\n" +
                                   model.hander.GetDict(model.lastRecipes[index].item3ID) + "/" + model.lastRecipes[index].item3Count;
        main.button.interactable = model.recipesMakable[index];
    }

    public void Exit()
    {
        MakeMaterialUI.OnClick -= MainShow;
        model.InvokeOnMake(false);
        main.button.onClick.RemoveListener(Make);
        viewUI.SetActive(false);
        mainUI.SetActive(false);
    }

    private void Update()
    {
        if(viewUI.activeSelf)
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                model.InvokeOnMake(false);
                view.transform.gameObject.SetActive(false);
                mainUI.SetActive(false);
            }
    }

    public void OnMyMouseClick(ToolSO tool)
    {
        if(tool.toolType == ToolType.Hammer)
        {
            if(model.isMaking)
                model.TimeBoost(tool.num1);
        }
    }
}
