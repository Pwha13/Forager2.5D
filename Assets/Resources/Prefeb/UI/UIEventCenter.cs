using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventCenter : MonoBehaviour
{
    public static UIEventCenter Instance;
    public static event Action<bool> OnUiOpen;
    public static event Action<ItemSO,ConRecipes> OnBuild;
    public static event Action OutBuild;
    public static event Action<bool> OnMake;
    public static event Action<ItemSO> OnToolChange;
    public static event Action OnUICancel;
    public static event Action<ToolSO> OnFoodGet;
    public static event Action<bool> OnBuy;

    private void Awake() => Instance = this;
    public void RaiseOnUiOpen(bool state) => OnUiOpen?.Invoke(state);
    public void RaiseOnBuild(ItemSO item,ConRecipes conRecipes) => OnBuild?.Invoke(item,conRecipes);
    public void RaiseOnMake(bool state) => OnMake?.Invoke(state);
    public void RaiseOnToolChange(ItemSO item) => OnToolChange?.Invoke(item);
    public void RaiseOnUICancel() => OnUICancel?.Invoke();
    public void RaiseOutBuild() => OutBuild?.Invoke();
    public void RaiseOnFoodGet(ToolSO tool) => OnFoodGet?.Invoke(tool);
    public void RaiseOnBuy(bool state) => OnBuy?.Invoke(state);
}
