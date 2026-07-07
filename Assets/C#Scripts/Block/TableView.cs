using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TableView : MonoBehaviour
{
    public RectTransform bg;
    public List<GameObject> uiList;
    public List<MakeMaterialUI> materials = new List<MakeMaterialUI>();

    private void Awake()
    {
        for(int i = 0;i<uiList.Count; i++)
        {
            uiList[i].TryGetComponent(out MakeMaterialUI material);
            material.count = i;
            materials.Add(material);
        }
    }

    //遍历传递过来的boolrecipe
    public void GetInfo(List<Recipe> getRecipes,List<bool> getMakable)
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            if (i >= getRecipes.Count)
            {
                uiList[i].SetActive(false);
                continue;
            }
            materials[i].recipe = getRecipes[i];
            materials[i].makable = getMakable[i];
            materials[i].GetInfo();
            uiList[i].SetActive(true);
        }   
        float y = 125 * (getRecipes.Count+1);
        bg.offsetMin= new Vector2(bg.offsetMin.x,-y);
    }
}
