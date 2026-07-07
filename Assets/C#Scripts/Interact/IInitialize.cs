using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInitialize : MonoBehaviour
{
    //Initialize
    public MouseManage MouseManage;
    public PlayerMove PlayerMove;
    private void Awake()
    {
        MouseManage.Initialize(PlayerMove);
    }
}
