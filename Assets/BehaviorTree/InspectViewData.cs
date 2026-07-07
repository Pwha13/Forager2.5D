using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "InspectViewData",menuName = "InspectViewData")]
public class InspectViewData : SerializedScriptableObject
{
    public HashSet<BTNodeBase> DataView = new HashSet<BTNodeBase>();
    
    
}
