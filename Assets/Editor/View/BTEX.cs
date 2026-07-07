using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class BTEX
{
    public static void LinkLineAddData(this Edge edge)
    {
        NodeView outputNode = edge.output.node as NodeView;
        NodeView inputNode = edge.input.node as NodeView;
        switch (outputNode.NodeData)
        {
            case BTComposite composite:
                composite.ChildNodes.Add(inputNode.NodeData);
                return;
            case BTPrecondition precondition:
                precondition.ChildNode = inputNode.NodeData;
                return;
        }
    }

    public static void UnLinkLineDelete(this Edge edge)
    {
        NodeView outputNode = edge.output.node as NodeView;
        NodeView inputNode = edge.input.node as NodeView;
        switch (outputNode.NodeData)
        {
            case BTComposite composite:
                composite.ChildNodes.Remove(inputNode.NodeData);
                return;
            case BTPrecondition precondition:
                precondition.ChildNode = null;
                return;
        }
    }
}
