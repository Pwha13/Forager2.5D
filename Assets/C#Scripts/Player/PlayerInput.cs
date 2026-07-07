using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private NewInputControl _input;
    public Vector3 MoveIndex { get; private set; }
    public Vector3 MousePos { get; private set; }

    private void Awake()
    {
        _input = new NewInputControl();
    }

    private void Update()
    {
        GetMoveIndex();
    }

    private void GetMoveIndex()
    {
        Vector2 inputMoveIndex = _input.Player.Move.ReadValue<Vector2>();
        MoveIndex = new Vector3(inputMoveIndex.x, 0, inputMoveIndex.y);
    }
    private void GetMouseInfo(Vector3 getMousePos)
    {
        MousePos = getMousePos;
    }
    public void SetInput(bool b)
    {
        if(!b) _input.Enable();
        else _input.Disable();
    }
    public void SetInputTrue() => SetInput(true);
    public void SetInputFalse() => SetInput(false);
    private void OnEnable()
    {
        _input.Enable();
        UIEventCenter.OnUiOpen += SetInput;
        UIEventCenter.OnMake += SetInput;
        PlayerEventCenter.OnPlayerIsDead += SetInputFalse;
        PlayerEventCenter.OnPlayerIsRevive += SetInputTrue;
        MouseManage.MouseHitInfo += GetMouseInfo;
    }
    private void OnDisable()
    {
        _input.Disable();
        UIEventCenter.OnUiOpen -= SetInput;
        UIEventCenter.OnMake -= SetInput;
        PlayerEventCenter.OnPlayerIsDead -= SetInputFalse;
        PlayerEventCenter.OnPlayerIsRevive -= SetInputTrue;
        MouseManage.MouseHitInfo -= GetMouseInfo;
    }
}
