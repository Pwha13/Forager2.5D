using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UITop : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    public bool first;
    public bool isSelect;
    private bool _isConstructing;
    public GameObject showUI;
    
    public Color normalColor;
    public Color selectColor;
    public Color enterColor;
    public Color pressColor;
    
    private static UITop _lastUITop;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = normalColor;
    }

    private void OnEnable()
    {
        UIEventCenter.OnBuild += IsConstruct;
        UIEventCenter.OutBuild += OutConstruct;
        
        if (first)
        {
            _lastUITop = this;
            _image.color = selectColor;
            isSelect = true;
            showUI.SetActive(true);
        }
    }

    private void OnDisable()
    {
        UIEventCenter.OnBuild -= IsConstruct;
        UIEventCenter.OutBuild -= OutConstruct;
        
        _image.color = normalColor;
        isSelect = false;
        showUI.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_isConstructing) return;
        if (_lastUITop != this && _lastUITop != null)
        {
            _lastUITop._image.color = normalColor;
            _lastUITop.isSelect = false;
            _lastUITop.showUI.SetActive(false);
        }
        
        _lastUITop = this;
        _image.color = selectColor;
        isSelect = true;
        showUI.SetActive(true);
        
        transform.DOScale(2.0f, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isSelect) return;
        _image.color = enterColor;
        
        transform.DOScale(1.8f, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelect)
        {
            _image.color = selectColor;
            transform.DOKill();
            transform.DOScale(1.7f, 0.2f);
        }
        else
        {
            _image.color = normalColor;
            transform.DOScale(1.7f, 0.1f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isSelect) return;
        _image.color = pressColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isSelect)
        {
            _image.color = selectColor;
        }
        else
        {
            _image.color = normalColor;
        }
    }

    private void IsConstruct(ItemSO _1,ConRecipes _2) => _isConstructing = true;
    private void OutConstruct() => _isConstructing = false;
}
