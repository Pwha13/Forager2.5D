using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadUI : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private Image image;

    private void OnEnable()
    {
        PlayerEventCenter.OnPlayerIsDead += Load;
        PlayerEventCenter.OnPlayerIsRevive += Unload;
    }

    private void OnDisable()
    {
        PlayerEventCenter.OnPlayerIsDead -= Load;
        PlayerEventCenter.OnPlayerIsRevive -= Unload;
    }

    public void Load()
    {
        UI.SetActive(true);
        image.DOFade(1f, 0.5f);
    }
    public void Unload()
    {
        image.DOFade(0f, 0.5f).OnComplete(() => UI.SetActive(false));
    }
}
