using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConRecipes : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    //component
    public Image bgImage;
    public Image resultImage;
    public GameObject resultUI;
    public GameObject resultText;
    public TextMeshProUGUI text;
    public InventoryHander hander;
    public GameObject TipUI;
    public MaterialUI UI;
    //item
    public List<NeedMaterial> materials = new List<NeedMaterial>();
    public ItemSO result;
    //variable
    public Sprite lockSprite;
    public Sprite unlockSprite;
    public bool isUnlock ;
    public bool isMakable;

    private void OnEnable()
    {
        MouseManage.OnConstructFinish += MakableJudge;

        if (!isUnlock)
        {
            bgImage.sprite = lockSprite;
            resultUI.SetActive(false);
            resultText.SetActive(false);
            for (int i = 0; i < materials.Count; i++)
            {
                if (hander.GetDict(materials[i].material.itemID) != 0)
                {
                    isUnlock = true;
                    bgImage.sprite = unlockSprite;
                    resultUI.SetActive(true);
                    resultText.SetActive(true);
                    break;
                }
            }
        }
        MakableJudge();
    }

    private void MakableJudge()
    {
        isMakable = false;
        if(materials.Count == 0) return;
        for (int j = 0; j < materials.Count; j++)
        {
            if(hander.GetDict(materials[j].material.itemID) < materials[j].needCount) return;
        }
        isMakable = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isUnlock) return;
        Count2TipUI();
        TipUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isUnlock) return;
        TipUI.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isMakable) return;
        UIEventCenter.Instance.RaiseOnBuild(result,this);
    }


    public void InitRecipes()
    {
        resultImage.sprite = result.itemSprite;
        text.text = result.itemName;
    }

    public void SubMaterial()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            hander.SubDict(materials[i].material.itemID,materials[i].needCount);
        }
    }

    public void Recipe2TipUI()
    {
        foreach (var needMaterial in materials)
        {
            NeedHaveMaterial material = new NeedHaveMaterial();
            material.needMaterial = needMaterial;
            material.haveCount = hander.GetDict(needMaterial.material.itemID);
            UI.materials.Add(material);
        }
        UI.GetInfo();
    }

    public void Count2TipUI()
    {
        for(int i = 0;i< materials.Count; i++)
        {
            UI.materials[i].haveCount = hander.GetDict(materials[i].material.itemID);
        }
        UI.UpdateInfo();
    }
}
public class NeedHaveMaterial
{
    public NeedMaterial needMaterial;
    public int haveCount;
}
