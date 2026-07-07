using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BehaviorTree;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeWindow : EditorWindow
{
    public static BehaviorTreeWindow WindowRoot;
    public TreeView TreeView;
    public InspectorView InspectorView;
    
    [MenuItem("MyTools/BehaviorTreeWindow")]
    public static void ShowExample()
    {
        //创建窗口方式
        BehaviorTreeWindow wnd = GetWindow<BehaviorTreeWindow>();
        //窗口命名
        wnd.titleContent = new GUIContent("BehaviorTreeWindow");
    }

    public void CreateGUI()
    {
        int treeID = BTSetting.GetSetting().TreeID;
        IGetBT iGetBT = EditorUtility.InstanceIDToObject(treeID) as IGetBT;
        //单例
        WindowRoot = this;
        
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/View/BehaviorTreeWindow.uxml");
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/View/BehaviorTreeWindow.uss");
        visualTree.CloneTree(root);
        
        TreeView = root.Q<TreeView>();
        InspectorView = root.Q<InspectorView>();
        
        if(iGetBT!=null&&iGetBT.GetRoot()!=null)
            CreateRoot(iGetBT.GetRoot());
        //调用所有的节点连接自己的子集
        TreeView.nodes.OfType<NodeView>().ForEach(n => n.LinkLine());
    }
    
    //通过创建根节点创建树
    public void CreateRoot(BTNodeBase rootNode)
    {
        if(rootNode == null) return;
        NodeView nodeView = new NodeView(rootNode);
        nodeView.SetPosition(new Rect(rootNode.Position,Vector2.one));
        TreeView.AddElement(nodeView);
        //添加字典
        TreeView.NodeViews.Add(rootNode.Guid,nodeView);
        //TODO
        switch (rootNode)
        {
            case BTComposite composite:
                composite.ChildNodes.ForEach(CreateChild);
                break;
            case BTPrecondition precondition:
                CreateChild(precondition.ChildNode);
                break;
        }
    }
    //创建子节点
    public void CreateChild(BTNodeBase nodeData)
    {
        if(nodeData == null) return;
        NodeView nodeView = new NodeView(nodeData);
        nodeView.SetPosition(new Rect(nodeData.Position,Vector2.one));
        TreeView.AddElement(nodeView);
        //添加字典
        TreeView.NodeViews.Add(nodeData.Guid,nodeView);
        switch (nodeData)
        {
            case BTComposite composite:
                composite.ChildNodes.ForEach(CreateChild);
                break;
            case BTPrecondition precondition:
                CreateChild(precondition.ChildNode);
                break;
        }
    }
}

public class RightClickMenu : ScriptableObject, ISearchWindowProvider
{
    public delegate bool SelectEntryDelegate(SearchTreeEntry searchTreeEntry, SearchWindowContext context);
    public SelectEntryDelegate OnSelectEntryHandler;
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var entries = new List<SearchTreeEntry>();
        entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));
        entries = AddNodeType<BTComposite>(entries, "组合节点");
        entries = AddNodeType<BTPrecondition>(entries, "条件节点");
        entries = AddNodeType<BTActionNode>(entries, "行为节点");
        return entries;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        if(OnSelectEntryHandler == null) return false;
        return OnSelectEntryHandler(searchTreeEntry, context);
    }

    /// <summary>
    /// 通过反射获取对应的菜单数据
    /// </summary>
    public List<SearchTreeEntry> AddNodeType<T>(List<SearchTreeEntry> entries, string pathName)
    {
        entries.Add(new SearchTreeGroupEntry(new GUIContent(pathName)){level = 1});
        List<Type> rootNodeTypes = GetDerivedClasses(typeof(T));
        foreach (var rootType in rootNodeTypes)
        {
            string menuName = rootType.Name;
            
            entries.Add(new SearchTreeEntry(new GUIContent(menuName)){level = 2,userData = rootType});
        }
        return entries;
    }

    public static List<Type> GetDerivedClasses(Type type)
    {
        List<Type> derivedClasses = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsClass && !t.IsAbstract && type.IsAssignableFrom(t))
                {
                    derivedClasses.Add(t);
                }
            }
        }

        return derivedClasses;
    }
}
