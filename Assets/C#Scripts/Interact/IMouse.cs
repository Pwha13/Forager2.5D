using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseEnterHandler
{
    void OnMyMouseEnter(ToolSO tool);
}
public interface IMouseExitHandler
{
    void OnMyMouseExit(ToolSO tool);
}
public interface IMouseClickHandler
{
    void OnMyMouseClick(ToolSO tool);
}
public interface IMouseRightClickHandler
{
    void OnMyMouseRightClick(ToolSO tool);
}
