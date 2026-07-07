using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeView : GraphView
{
    //原本自带nodes，但是为列表不方便查找
    public Dictionary<string,NodeView> NodeViews = new Dictionary<string, NodeView>();
    
    public new class UxmlFactory: UxmlFactory<TreeView,UxmlTraits>{}
    public TreeView()
    {
        Insert(0,new GridBackground());
        
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/View/BehaviorTreeWindow.uss");
        styleSheets.Add(styleSheet);
        GraphViewMenu();
        
        graphViewChanged += OnGraphViewChanged;
        RegisterCallback<MouseEnterEvent>(MouseEnterControl);
    }

    private void MouseEnterControl(MouseEnterEvent evt)
    {
        BehaviorTreeWindow.WindowRoot.InspectorView.UpdateViewData();
    }
    private GraphViewChange OnGraphViewChanged(GraphViewChange gvc)
    {
        if (gvc.edgesToCreate != null)
        {
            gvc.edgesToCreate.ForEach(edge =>
            {
                edge.LinkLineAddData();
            });
        }

        if (gvc.elementsToRemove != null)
        {
            gvc.elementsToRemove.ForEach(element =>
            {
                if (element is Edge edge)
                {
                    edge.UnLinkLineDelete();
                }
            });
        }
        return gvc; 
    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt); 
        //evt.menu.AppendAction("Create Node",CreateNode);
    }
    
    private void CreateNode(Type type,Vector2 position)
    {
        BTNodeBase nodeData = Activator.CreateInstance(type) as BTNodeBase;
        //赋予uid
        nodeData.Guid = Guid.NewGuid().ToString();
        nodeData.NodeName = type.Name;
        NodeView node = new NodeView(nodeData);
        node.SetPosition(new Rect(position, Vector2.one));
        NodeViews.Add(nodeData.Guid,node);
        this.AddElement(node);
    }
    //右键菜单
    public void GraphViewMenu()
    {
        var menuWindowProvider = ScriptableObject.CreateInstance<RightClickMenu>();
        menuWindowProvider.OnSelectEntryHandler = OnMenuSelectEntry;

        nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
        };
    }
    //覆写定义连接规则
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.Where(endPorts => endPorts.direction!=startPort.direction&&endPorts.node!=startPort.node).ToList();
    }

    //实现创建
    private bool OnMenuSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        var windowRoot = BehaviorTreeWindow.WindowRoot.rootVisualElement;
        var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent,
            context.screenMousePosition - BehaviorTreeWindow.WindowRoot.position.position);
        var graphMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);
        
        CreateNode((Type)searchTreeEntry.userData,graphMousePosition);
        return true;
    }
}
