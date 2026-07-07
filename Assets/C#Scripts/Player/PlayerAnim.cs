using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    public bool isWalking;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetStatus();
    }

    private void OnEnable()
    {
        PlayerEventCenter.OnPlayerIsHurt += HurtAnim;
    }

    private void OnDisable()
    {
        PlayerEventCenter.OnPlayerIsHurt -= HurtAnim;
    }
    public void GetStatus()
    {
        anim.SetBool("isWalking", isWalking);
    }

    private void HurtAnim()
    {
        sr.DOKill();
        sr.DOColor(Color.red, 0.1f).SetLoops(10,LoopType.Yoyo);
    }
}
