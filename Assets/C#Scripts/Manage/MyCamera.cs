using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform playerFollow;
    public Transform buyFollow;
    public CinemachineVirtualCamera vCamera;
    public Vector3 buildFollowOffset;
    public Vector3 playerFollowOffset;
    public Vector3 buyFollowOffset;
    public static event Action ToPlayerVisonHandler;
    public static event Action ToBuyVisonHandler;
    public static event Action ToBuildVisonHandler;

    private void Awake()
    {
        vCamera = GetComponent<CinemachineVirtualCamera>();
    }

    //Dotween
    public void ToPlayerVison()
    {
        vCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = playerFollowOffset;
        transform.localEulerAngles = new Vector3(45, 0, 0);
        vCamera.Follow = playerFollow;
        vCamera.LookAt = playerFollow;
        ToPlayerVisonHandler?.Invoke();
    }
    public void ToBuyVison()
    {
        vCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = buyFollowOffset;
        transform.localEulerAngles = new Vector3(75, 0, 0);
        vCamera.Follow = buyFollow;
        vCamera.LookAt = buyFollow;
        ToBuyVisonHandler?.Invoke();
    }
    public void ToBuildVison()
    {
        vCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = buildFollowOffset;
        vCamera.Follow = playerFollow;
        vCamera.LookAt = playerFollow;
        ToBuildVisonHandler?.Invoke();
    }

    private void Player2Buy(bool state)
    {
        if(state) ToBuyVison();
        else ToPlayerVison();
    }
    private void OnEnable()
    {
        UIEventCenter.OnBuy += Player2Buy;
    }
    private void OnDisable()
    {
        UIEventCenter.OnBuy -= Player2Buy;
    }
}
