using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class TestBT : SerializedMonoBehaviour,IGetBT
{
    [OdinSerialize]
    public BTNodeBase rootNode;

    private void Update()
    {
        rootNode?.Tick();
    }
    
    //打开面板
#if UNITY_EDITOR
    [Button]
    public void OpenView()
    {
        //TODO:了解一下其他读取加载传递的方法
        BTSetting.GetSetting().TreeID = GetInstanceID();
        UnityEditor.EditorApplication.ExecuteMenuItem("MyTools/BehaviorTreeWindow");
    }
#endif
    public BTNodeBase GetRoot() => rootNode;
    public void SetRoot(BTNodeBase rootData) => rootNode = rootData;
}

public interface IGetBT
{
    BTNodeBase GetRoot();
    void SetRoot(BTNodeBase rootData);
}
