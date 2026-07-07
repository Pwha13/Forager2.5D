using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITip : MonoBehaviour
{
    public GameObject tipUI;

    public void ShowTipUI()
    {
        tipUI.SetActive(true);
    }
    public void HideTipUI()
    {
        tipUI.SetActive(false);
    }
}
