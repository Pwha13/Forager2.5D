using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyUI : MonoBehaviour
{
    private void OnEnable() => UIEventCenter.Instance.RaiseOnBuy(true);
    private void OnDisable() =>UIEventCenter.Instance.RaiseOnBuy(false);
}
