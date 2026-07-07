using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float playerDamage { get; private set; }
    private Vector3 mousePos;
    private BoxCollider boxCollider;
    private Coroutine coroutine;
    private bool isattacking;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }
    
    private void Update()
    {
        if(isattacking)
        {
            Vector2 a = new Vector2(mousePos.x, mousePos.z);
            Vector2 b = new Vector2(transform.position.x, transform.position.z);
            float z = Vector2.SignedAngle(Vector2.right, a - b);
            transform.localRotation = Quaternion.Euler(0, 0, z);
        }
    }
    //接受攻击预备指令
    private void GetAttack(bool attack)
    {
        isattacking = attack;
        if (attack)
            MouseManage.OnClick += DoAttack;
        else
            MouseManage.OnClick -= DoAttack;
    }

    private void DoAttack()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Attack());
        }
    }

    private void GetDamage(ToolSO tool) => playerDamage = tool.num1;
    IEnumerator Attack()
    {
        boxCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        boxCollider.enabled = false;
        coroutine = null;
    }
    private void OnEnable()
    {
        MouseManage.MouseHitInfo += GetMousePos;
        MouseManage.AttackHander += GetAttack;
        InventoryHander.OnSwordAdd += GetDamage;
    }

    private void OnDisable()
    {
        MouseManage.MouseHitInfo -= GetMousePos;
        MouseManage.AttackHander -= GetAttack;
        InventoryHander.OnSwordAdd -= GetDamage;
    }

    private void GetMousePos(Vector3 getMousePos)
    {
        mousePos = getMousePos;
    }
}
