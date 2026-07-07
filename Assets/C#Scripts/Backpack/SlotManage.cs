using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotManage : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public ItemSO item;
    public Image itemImage;
    public GameObject tip;
    public TextMeshProUGUI itemCount;

    public void GetImage()
    {
        itemImage.sprite = item.itemSprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tip.SetActive(false);
    }
    
    private void OnDisable()
    {
        tip.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            if (item is ToolSO { toolType: ToolType.Food } tool)
                UIEventCenter.Instance.RaiseOnFoodGet(tool);
    }
}
