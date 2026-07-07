using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehaviorTree;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public IMGUIContainer InspectorBar;
    public InspectViewData InspectViewData;
    public new class UxmlFactory: UxmlFactory<InspectorView,UxmlTraits>{}
    public InspectorView()
    {
        //创建GUI
        InspectorBar = new IMGUIContainer(){name="InspectorBar"};
        //拉伸到最高
        InspectorBar.style.flexGrow = 1;
        //创建面板
        CreateInspectorView();
        Add(InspectorBar);
    }
    //把InspectViewData的面板放到GUI里
    private async void CreateInspectorView()
    {
        InspectViewData = Resources.Load<InspectViewData>("InspectViewData");
        await Task.Delay(100);
        var odinEditor = UnityEditor.Editor.CreateEditor(InspectViewData);
        InspectorBar.onGUIHandler += () =>
        {
            odinEditor.OnInspectorGUI();
        };
    }

    public void UpdateViewData()
    {
        HashSet<BTNodeBase> dates = BehaviorTreeWindow.WindowRoot.TreeView.selection.OfType<NodeView>()
            .Select(nv => nv.NodeData).ToHashSet();
        InspectViewData.DataView.Clear();
        foreach (var date in dates)
        {
            InspectViewData.DataView.Add(date);
        }
    }
}
