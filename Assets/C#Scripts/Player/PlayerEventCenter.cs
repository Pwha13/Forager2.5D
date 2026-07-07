using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventCenter : MonoBehaviour
{
    public static PlayerEventCenter Instance;
    public static event Action<float> OnPlayerHurt;
    public static event Action OnPlayerIsHurt;
    public static event Action OnPlayerIsRevive;
    public static event Action OnPlayerIsDead;
    public static event Action<float> OnPlayerHpChange;
    public static event Action<float> OnPlayerStmChange;

    private void Awake() => Instance = this;
    public void RaiseOnPlayerHurt(float damage) => OnPlayerHurt?.Invoke(damage);
    public void RaiseOnPlayerIsHurt() => OnPlayerIsHurt?.Invoke();
    public void RaiseOnPlayerIsDead() => OnPlayerIsDead?.Invoke();
    public void RaiseOnPlayerIsRevive() => OnPlayerIsRevive?.Invoke();
    public void RaiseOnPlayerHpChange(float hp) => OnPlayerHpChange?.Invoke(hp);
    public void RaiseOnPlayerStmChange(float stm) => OnPlayerStmChange?.Invoke(stm);
}
