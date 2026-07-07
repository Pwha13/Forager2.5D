using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTrap : MonoBehaviour,IMouseRightClickHandler
{
    public float trapTime = 40f;
    private bool isTrap;
    public GameObject tip;
    private float _currentTime;

    public ItemDrop drop;

    private void Awake()
    {
        _currentTime = trapTime;
    }

    private void Update()
    {
        if(_currentTime > 0 && !isTrap)
            _currentTime -= Time.deltaTime;
        else
        {
            _currentTime = trapTime;
            isTrap = true;
            tip.SetActive(true);
        }
    }

    private void Trap()
    {
        drop.Drop();
        isTrap = false;
        tip.SetActive(false);
    }

    public void OnMyMouseRightClick(ToolSO tool)
    {
        if (!isTrap) return;
        Trap();
    }
}
