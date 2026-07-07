using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialUI : MonoBehaviour
{
    public List<NeedHaveMaterial> materials = new List<NeedHaveMaterial>();
    public List<ContructMaterial> ContructMaterials = new List<ContructMaterial>();
    public Image contentBg;
    public GameObject _contentBg;
    public GameObject contentPrefab;
    public GameObject content;

    private void OnEnable()
    {
        _contentBg.SetActive(true);
    }

    private void OnDisable()
    {
        _contentBg.SetActive(false);
    }

    public void GetInfo()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            GameObject newobj = Instantiate(contentPrefab, content.transform);
            newobj.transform.TryGetComponent(out ContructMaterial contructMaterial);
            ContructMaterials.Add(contructMaterial);
            contructMaterial.image.sprite = materials[i].needMaterial.material.itemSprite;
            contructMaterial.text.text = materials[i].needMaterial.material.itemName + "\n" +
                                         materials[i].needMaterial.needCount + "/" + materials[i].haveCount;
            contentBg.rectTransform.offsetMin = new Vector2(contentBg.rectTransform.offsetMin.x,
                contentBg.rectTransform.offsetMin.y - 100);
        }
    }

    public void UpdateInfo()
    {
        for (int i = 0; i < ContructMaterials.Count; i++)
        {
            ContructMaterials[i].text.text = materials[i].needMaterial.material.itemName + "\n" +
                                             materials[i].needMaterial.needCount + "/" + materials[i].haveCount;
        }
    }
}
