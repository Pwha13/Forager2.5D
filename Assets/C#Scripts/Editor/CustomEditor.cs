using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[UnityEditor.CustomEditor(typeof(ToolSO),true)]
public class CustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ToolSO toolSO = target as ToolSO;
        toolSO.toolType = (ToolType)EditorGUILayout.EnumPopup("类型", toolSO.toolType);
        switch (toolSO.toolType)
        {
            case ToolType.Pickaxe:
                toolSO.num1 = EditorGUILayout.FloatField("挖掘力量", toolSO.num1);
                break;
            case ToolType.Shovel:
                toolSO.num1 = EditorGUILayout.FloatField("挖掘倍率", toolSO.num1);
                break;
            case ToolType.Hammer:
                toolSO.num1 = EditorGUILayout.FloatField("加速力量", toolSO.num1);
                break;
            case ToolType.Sword:
                toolSO.num1 = EditorGUILayout.FloatField("攻击力量", toolSO.num1);
                break;
            case ToolType.Glove:
                toolSO.num1 = EditorGUILayout.FloatField("点击距离", toolSO.num1);
                toolSO.num2 = EditorGUILayout.FloatField("使用CD", toolSO.num2);
                break;
            case ToolType.Backpack:
                toolSO.num1 = EditorGUILayout.FloatField("额外背包格数", toolSO.num1);
                break;
            case ToolType.Food:
                toolSO.num1 = EditorGUILayout.FloatField("饱腹值", toolSO.num1);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(toolSO);
        }
    }
}
