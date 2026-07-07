using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuyVisonMove : MonoBehaviour
{
    public bool isControling;

    private void OnEnable()
    {
        MyCamera.ToBuyVisonHandler+= isControlTrue;
        MyCamera.ToPlayerVisonHandler+= isControlFalse;
    }
    private void OnDisable()
    {
        MyCamera.ToBuyVisonHandler-= isControlTrue;
        MyCamera.ToPlayerVisonHandler-= isControlFalse;
    }

    private void Update()
    {
        if (isControling)
        {
            Move();
        }
    }

    public void isControlTrue()
    {
        isControling = true;
    }
    public void isControlFalse()
    {
        isControling = false;
        transform.position = new Vector3(0, 40, 5);
    }
    public void Move()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            transform.position= new Vector3(transform.position.x,transform.position.y,transform.position.z+80);
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            transform.position= new Vector3(transform.position.x,transform.position.y,transform.position.z-80);
        }

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            transform.position= new Vector3(transform.position.x-80,transform.position.y,transform.position.z);
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            transform.position= new Vector3(transform.position.x+80,transform.position.y,transform.position.z);
        }
    }
}
