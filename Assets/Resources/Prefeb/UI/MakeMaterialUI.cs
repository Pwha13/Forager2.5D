using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MakeMaterialUI : MonoBehaviour,IPointerClickHandler
{
    public Image image;
    public Image bg;
    public Color tureColor;
    public Color falseColor;
    public TextMeshProUGUI text;
    public Recipe recipe;
    public bool makable;
    public static event Action<int> OnClick;
    public int count;

    private void Awake()
    {
        bg.GetComponent<Image>();
    }

    public void GetInfo()
    {
        image.sprite = ItemDataBaseSO.itemDataBase.GetItem(recipe.resultID).itemSprite;
        text.text = ItemDataBaseSO.itemDataBase.GetItem(recipe.resultID).itemName;
        bg.color = makable ? tureColor : falseColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(count);
    }
}
