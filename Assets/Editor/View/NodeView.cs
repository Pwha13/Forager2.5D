using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class NodeView : Node
{
    public BTNodeBase NodeData;
    
    public Port InputPort;
    public Port OutputPort;
    
    public NodeView(BTNodeBase nodeData)
    {
        this.NodeData = nodeData;
        InitNodeView(nodeData);
    }
    //TODO：待优化为switch case
    public void InitNodeView(BTNodeBase nodeData)
    {
        //名字赋值
        title = nodeData.NodeName;
        //创建端口
        InputPort = +this;
        inputContainer.Add(InputPort);
        if(!(nodeData is BTActionNode))
        {
            OutputPort = this - (nodeData is BTPrecondition);
            outputContainer.Add(OutputPort);
        }
        //
    }
    //连线方法
    public void LinkLine()
    {
        TreeView graphView = BehaviorTreeWindow.WindowRoot.TreeView;
        switch (NodeData)
        {
            case BTComposite composite:
                composite.ChildNodes.ForEach(n =>
                {
                    graphView.AddElement(PortLink(OutputPort, graphView.NodeViews[n.Guid].InputPort));
                });
                break;
            case BTPrecondition precondition:
                graphView.AddElement(PortLink(OutputPort, graphView.NodeViews[precondition.ChildNode.Guid].InputPort));
                break;
        }
    }

    public Edge PortLink(Port p1, Port p2)
    {
        var tempEdge = new Edge()
        {
            output = p1,
            input = p2
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        return tempEdge;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        evt.menu.AppendAction("Set Root",SetRoot);
    }

    private void SetRoot(DropdownMenuAction obj) => BTSetting.GetSetting().SetRoot(NodeData);

    //保存位置
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        NodeData.Position = new Vector2(newPos.xMin, newPos.yMin);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        BehaviorTreeWindow.WindowRoot.InspectorView.UpdateViewData(); 
    }

    public static Port operator +(NodeView nodeView)
    {
        Port port = Port.Create<Edge>(Orientation.Horizontal,Direction.Input,Port.Capacity.Single,typeof(NodeView));
        return port;
    }

    public static Port operator -(NodeView nodeView, bool isSingle)
    {
        Port port = Port.Create<Edge>(Orientation.Horizontal,Direction.Output,isSingle?Port.Capacity.Single:Port.Capacity.Multi,typeof(NodeView));
        return port;
    }
}
