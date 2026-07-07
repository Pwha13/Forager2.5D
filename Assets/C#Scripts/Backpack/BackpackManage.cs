using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BackpackManage : MonoBehaviour
{
    public List<SlotManage> slots = new List<SlotManage>();
    public InventoryHander hander;
    public ScrollRect scrollRect;
    public int columnCount;
    public float slotHeight;
    public float contentHeight;
    public float maxHeight;
    public float maxSlotHeight;
    public float minHeight;
    public int lastSlotCount;
    public Sprite nullSprite;

    public List<Image> equal;
    public List<Image> tool;

    private void Awake()
    {
        InitSlotsList();
    }

    public void Start()
    {
        contentHeight = scrollRect.content.anchoredPosition.y;
        maxHeight = contentHeight;
        minHeight = maxHeight + (hander.MaxSlots - 16) / columnCount * slotHeight;
    }

    public void Update()
    {
        CheckViewHeight();
    }

    public void OnEnable()
    {
        hander.OnInventoryChanged += GetInventory;
        hander.OnInventoryChanged += GetEqual;
        UIEventCenter.OnFoodGet += GetFood;
        minHeight = maxHeight + (hander.MaxSlots - 16) / columnCount * slotHeight;
        hander.LoadSlotsByID();
    }

    private void OnDisable()
    {
        hander.OnInventoryChanged -= GetInventory;
        hander.OnInventoryChanged -= GetEqual;
        UIEventCenter.OnFoodGet -= GetFood;
    }

    public void CheckViewHeight()
    {
        if (scrollRect.content.anchoredPosition.y > contentHeight)
        {
            contentHeight = scrollRect.content.anchoredPosition.y;
            ScrollUp();
        }

        if (scrollRect.content.anchoredPosition.y < contentHeight)
        {
            contentHeight = scrollRect.content.anchoredPosition.y;
            ScrollDown();
        }
    }

    public void ScrollDown()
    {
        if (scrollRect.content.anchoredPosition.y < maxHeight)
        {
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, maxHeight);
            return;
        }
        if (lastSlotCount - slots.Count == 0) return;
        slots[0].TryGetComponent(out RectTransform rect);
        if (rect.anchoredPosition.y >= maxSlotHeight) return;
        float testHeight = maxHeight + (lastSlotCount - slots.Count) / columnCount * slotHeight + 175;
        if (contentHeight <= testHeight)
        {
            for (int i = 0; i < columnCount; i++)
            {
                if (!hander.Slots[lastSlotCount - slots.Count - 1].IsEmpty)
                {
                    slots[slots.Count - 1].itemCount.text = hander.Slots[lastSlotCount - slots.Count - 1].count.ToString();
                    slots[slots.Count - 1].item = hander.Slots[lastSlotCount - slots.Count - 1].item;
                    slots[slots.Count - 1].GetImage();
                }
                else
                {
                    SetSlotEmpty(slots.Count - 1);
                }
                SlotManage last = slots[slots.Count - 1];
                slots.RemoveAt(slots.Count - 1);
                slots.Insert(0, last);
                slots[0].TryGetComponent(out RectTransform currentslot);
                currentslot.anchoredPosition = new Vector2(currentslot.anchoredPosition.x, currentslot.anchoredPosition.y + slotHeight * slots.Count / columnCount);
                lastSlotCount--;
            }
        }
    }

    public void ScrollUp()
    {
        if (scrollRect.content.anchoredPosition.y > minHeight)
        {
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, minHeight);
            return;
        }
        if (lastSlotCount == hander.MaxSlots) return;
        float testHeight = minHeight - (hander.MaxSlots - lastSlotCount) / columnCount * slotHeight;
        if (contentHeight >= testHeight)
        {
            for (int i = 0; i < columnCount; i++)
            {
                if (!hander.Slots[lastSlotCount].IsEmpty)
                {
                    slots[0].itemCount.text = hander.Slots[lastSlotCount].count.ToString();
                    slots[0].item = hander.Slots[lastSlotCount].item;
                    slots[0].GetImage();
                }
                else
                {
                    SetSlotEmpty(0);
                }
                slots[0].TryGetComponent(out RectTransform currentslot);
                currentslot.anchoredPosition = new Vector2(currentslot.anchoredPosition.x, currentslot.anchoredPosition.y - slotHeight * slots.Count / columnCount);
                lastSlotCount++;
                var first = slots[0];
                slots.RemoveAt(0);
                slots.Add(first);
            }
        }
    }

    public void InitSlotsList()
    {
        slots.Clear();
        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            if (i >= hander.MaxSlots) break;
            scrollRect.content.GetChild(i).TryGetComponent(out SlotManage slot);
            slots.Add(slot);
            lastSlotCount = i + 1;
            if (lastSlotCount == hander.MaxSlots)
            {
                minHeight = slots[i].GetComponent<RectTransform>().anchoredPosition.y;
            }
        }
        hander.InitSlots();
    }

    public void GetInventory()
    {
        int j = 0;
        for (int i = lastSlotCount - scrollRect.content.childCount; i < lastSlotCount; i++)
        {
            if (i < 0 || i >= hander.Slots.Count) break;
            if (j >= slots.Count) break;

            if (!hander.Slots[i].IsEmpty)
            {
                slots[j].itemCount.text = hander.Slots[i].count.ToString();
                slots[j].item = hander.Slots[i].item;
                slots[j].GetImage();
            }
            else
            {
                SetSlotEmpty(j);
            }
            j++;
        }
    }

    public void SetSlotEmpty(int i)
    {
        slots[i].itemCount.text = null;
        slots[i].item = null;
        slots[i].itemImage.sprite = nullSprite;
    }

    public void GetEqual()
    {
        for (int i = 0; i < equal.Count; i++)
        {
            if (hander.EqualList[i] == null)
                equal[i].sprite = nullSprite;
            else
                equal[i].sprite = hander.EqualList[i].itemSprite;
        }

        for (int i = 0; i < tool.Count; i++)
        {
            if (hander.HandList[i] == null)
                tool[i].sprite = nullSprite;
            else
                tool[i].sprite = hander.HandList[i].itemSprite;
        }
    }

    public void GetFood(ToolSO _)
    {
        if (hander.HandList[4] == null)
            tool[4].sprite = nullSprite;
        else
            tool[4].sprite = hander.HandList[4].itemSprite;
    }
}
