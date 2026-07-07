using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ConstructUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    //列表
    public List<Recipe> recipes = new List<Recipe>();
    //UIPrefab
    public GameObject content;
    public GameObject contentPrefab;
    public GameObject contentBg;
    //Component
    public GameObject view;
    public MyCamera vCamera;
    public Image mainImage;
    public InventoryHander hander;
    //Color
    public Color normalColor;
    public Color enterColor;
    //Variable
    public bool isShow;

    public float force;
    
    
    private void Awake()
    {
        GetRecipes();
        view.transform.localScale = new Vector3(1, 0, 1);
    }

    //相机修改 OnEnable OnDisable
    private void OnEnable()
    {
        vCamera.ToBuildVison();
    }
    private void OnDisable()
    {
        if(vCamera == null) return;
        vCamera.ToPlayerVison();
    }
    
    //Enter Exit PressDownOn Click——>展示View  执行View弹性动画
    public void OnPointerEnter(PointerEventData eventData) => mainImage.color = enterColor;
    public void OnPointerExit(PointerEventData eventData) => mainImage.color = normalColor;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isShow)
        {
            isShow = false;
            view.transform.DOKill();
            mainImage.transform.DOKill();
            mainImage.transform.localScale = new Vector3(1, 1, 1);
            mainImage.transform.DOScale(1.2f, 0.2f).SetLoops(2,LoopType.Yoyo);
            view.transform.DOScaleY(0,0.2f);
        }
        else
        {
            isShow = true;
            mainImage.transform.DOKill();
            mainImage.transform.localScale = new Vector3(1, 1, 1);
            mainImage.transform.DOScale(1.2f, 0.2f).SetLoops(2,LoopType.Yoyo);
            view.transform.DOScaleY(1,0.75f).SetEase(Ease.OutElastic,force,0.8f);
        }
    }
    //注意setflase时 ondisable处理

    public void GetRecipes()
    {
        //读取配方
        recipes = Json2Recipe.Instance.buildRecipes.recipes;
        //根据配方数创建子物体->根据子物体数量调整view-bg
        float offsetY = 125;
        for (int i = 0; i < recipes.Count; i++)
        {
            //获取json 读取id 获得内容写入contentPrefab 名字 sprite 材料
            GameObject newobj = Instantiate(contentPrefab, content.transform);
            newobj.SetActive(false);
            newobj.transform.TryGetComponent(out ConRecipes newRecipe);
            newRecipe.hander = hander;
            NeedMaterial material1 = new NeedMaterial();
            material1.material = ItemDataBaseSO.itemDataBase.GetItem(recipes[i].item1ID);
            material1.needCount = recipes[i].item1Count;
            newRecipe.materials.Add(material1);
            if(recipes[i].item2Count!=0)
            {
                NeedMaterial material2 = new NeedMaterial();
                material2.material = ItemDataBaseSO.itemDataBase.GetItem(recipes[i].item2ID);
                material2.needCount = recipes[i].item2Count;
                newRecipe.materials.Add(material2);
            }
            if(recipes[i].item3Count != 0)
            {
                NeedMaterial material3 = new NeedMaterial();
                material3.material = ItemDataBaseSO.itemDataBase.GetItem(recipes[i].item3ID);
                material3.needCount = recipes[i].item3Count;
                newRecipe.materials.Add(material3);
            }
            newRecipe.result = ItemDataBaseSO.itemDataBase.GetItem(recipes[i].resultID);
            newRecipe.InitRecipes();
            newRecipe.Recipe2TipUI();
            newobj.SetActive(true);
            offsetY += 125;
        }
        contentBg.transform.TryGetComponent(out RectTransform rect);
        rect.offsetMin = new Vector2(rect.offsetMin.x,-offsetY);
    }
    
    //做多个以及滚轴

}
public class NeedMaterial
{
    public ItemSO material;
    public int needCount;
}