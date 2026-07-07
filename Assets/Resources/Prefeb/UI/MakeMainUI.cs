using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MakeMainUI : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public ContructMaterial material1;
    public ContructMaterial material2;
    public ContructMaterial material3;
    public GameObject material1UI;
    public GameObject material2UI;
    public GameObject material3UI;
    public Button button;
    //button事件调用TableController.Make()
}
