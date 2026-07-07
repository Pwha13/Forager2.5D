using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]private Image health;
    [SerializeField]private Image stamina;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;

    private void OnEnable()
    {
        PlayerEventCenter.OnPlayerHpChange += ChangeHealthBar;
        PlayerEventCenter.OnPlayerStmChange += ChangeStaminaBar;
        UIEventCenter.OnUiOpen += SetImage;
    }

    private void OnDisable()
    {
        PlayerEventCenter.OnPlayerHpChange -= ChangeHealthBar;
        PlayerEventCenter.OnPlayerStmChange -= ChangeStaminaBar;
        UIEventCenter.OnUiOpen -= SetImage;
    }

    private void ChangeHealthBar(float getHealth)
    {
        this.health.fillAmount = getHealth/5;
    }
    private void ChangeStaminaBar(float getStamina)
    {
        this.stamina.fillAmount = getStamina/5;
    }

    public void SetImage(bool b)
    {
        health.gameObject.SetActive(!b);
        stamina.gameObject.SetActive(!b);
        healthBar.gameObject.SetActive(!b);
        staminaBar.gameObject.SetActive(!b);
    }
}
