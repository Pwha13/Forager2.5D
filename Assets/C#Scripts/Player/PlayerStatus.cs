using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour,ISavable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxStamina;
    [SerializeField] private float invincibleTime;
    [SerializeField] private float hungryTime;
    private float _currentTime;
    
    public float Health{get; private set;}
    public float Stamina{get; private set;}
    public bool isInvincible;

    private void Awake()
    {
        Health = maxHealth;
        Stamina = maxStamina;
        _currentTime = hungryTime;
    }

    private void Update()
    {
        HungryLogic();
    }

    private void HungryLogic()
    {
        if (Stamina > 0)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                if (Stamina >= maxStamina&&Health<=maxHealth)
                {
                    Health += maxHealth;
                    if(Health>maxHealth)
                        Health = maxHealth;
                }
                Stamina -= 0.5f;
                if (Stamina <= 0)
                    Stamina = 0;
                PlayerEventCenter.Instance.RaiseOnPlayerStmChange(Stamina);
                _currentTime = hungryTime;
            }
        }
        else
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                Health -= 0.5f;
                _currentTime = hungryTime;
                if (Health <= 0)
                {
                    Health = 0;
                    Dead();
                }
                PlayerEventCenter.Instance.RaiseOnPlayerIsHurt();
                PlayerEventCenter.Instance.RaiseOnPlayerHpChange(Health);
            }
        }
    }

    private void Hurt(float enemyDamage)
    {
        if(isInvincible)
        {
            //Invincible anima
            return;
        }
        StartCoroutine(Invincible());
        Health -= enemyDamage;
        if(Health <= 0)
        {
            Health = 0;
            Dead();
        }
        PlayerEventCenter.Instance.RaiseOnPlayerIsHurt();
        PlayerEventCenter.Instance.RaiseOnPlayerHpChange(Health);
    }

    private void Eat(ToolSO food)
    {
        Stamina += food.num1;
        if(Stamina >= maxStamina)
        {
            _currentTime = hungryTime;
            if(Health < maxHealth)
            {
                Health += Stamina - maxStamina;
                if (Health > maxHealth)
                    Health = maxHealth;
                PlayerEventCenter.Instance.RaiseOnPlayerHpChange(Health);
            }
            Stamina = maxStamina;
        }
        PlayerEventCenter.Instance.RaiseOnPlayerStmChange(Stamina);
    }

    private void Dead()
    {
        PlayerEventCenter.Instance.RaiseOnPlayerIsDead();
        StartCoroutine(Revive());
    }
    private IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private IEnumerator Revive()
    {
        yield return new WaitForSeconds(5f);
        Health = 1;
        Stamina = 2.5f;
        PlayerEventCenter.Instance.RaiseOnPlayerHpChange(Health);
        PlayerEventCenter.Instance.RaiseOnPlayerStmChange(Stamina);
        PlayerEventCenter.Instance.RaiseOnPlayerIsRevive();
    }
    private void OnEnable()
    {
        ISavable savable = this;
        savable.RegisterSaveData();
        PlayerEventCenter.OnPlayerHurt += Hurt;
        MouseManage.OnEat += Eat;
    }

    private void OnDisable()
    {
        ISavable savable = this;
        savable.UnRegisterSaveData();
        PlayerEventCenter.OnPlayerHurt -= Hurt;
        MouseManage.OnEat -= Eat;
    }

    public void Save(Data data)
    {
        data.PlayerHp = Health;
        data.PlayerStm = Stamina;
        data.PlayerHungryTime = _currentTime;
        data.PlayerPos = transform.position;
    }

    public void Load(Data data)
    {
        if(Health <= 0) return;
        Health = data.PlayerHp;
        PlayerEventCenter.Instance.RaiseOnPlayerHpChange(Health);
        Stamina = data.PlayerStm;
        PlayerEventCenter.Instance.RaiseOnPlayerStmChange(Stamina);
        _currentTime = data.PlayerHungryTime;
        transform.position = data.PlayerPos;
    }
}
