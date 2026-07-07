using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadEventSO", menuName = "Game/Event/SaveLoadEventSO")]
public class SaveLoadEventSo : ScriptableObject
{
    public static event Action SaveEvent;
    public static event Action LoadEvent;
    public void RaiseSaveEvent() => SaveEvent?.Invoke();
    public void RaiseLoadEvent() => LoadEvent?.Invoke();
}
