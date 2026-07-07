using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BTSetting", menuName = "BTSetting")]
public class BTSetting : SerializedScriptableObject
{
    public int TreeID;

    public static BTSetting GetSetting()
    {
        return Resources.Load<BTSetting>("BTSetting");
    }
#if UNITY_EDITOR
    public IGetBT GetTree()=>UnityEditor.EditorUtility.InstanceIDToObject(TreeID) as IGetBT;
    public void SetRoot(BTNodeBase rootNode) => GetTree().SetRoot(rootNode);
#endif
}
