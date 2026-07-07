using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using UnityEngine;

/// 持续移动：每帧沿当前方向移动。永远返回 Running，需配合 TimeLimit 使用
public class Move : BTActionNode,IAbortable
{
    [LabelText("启用对象"),SerializeField,FoldoutGroup("@NodeName")]
    private MobAi _target;
    public override BehaviorState Tick()
    {
        _target.Move();
        _target.Anim.SetBool("isMoving",true);
        return BehaviorState.Running;
    }

    public void Abort()
    {
        _target.Anim.SetBool("isMoving",false);
    }
}
/// 转向
public class ChangeDir : BTActionNode
{
    [LabelText("启用对象"),SerializeField,FoldoutGroup("@NodeName")]
    private MobAi _target;
    public override BehaviorState Tick()
    {
        _target.ChangeDir();
        return BehaviorState.Success;
    }
}
/// 检查地面，有地则返回失败，无则返回成功
public class CheckGround : BTActionNode
{
    [LabelText("启用对象"),SerializeField,FoldoutGroup("@NodeName")]
    private MobAi _target;
    public override BehaviorState Tick()
    {
        if (!_target.CheckGround())
            return BehaviorState.Failure;
        return BehaviorState.Success;
    }
}
/// 检测受伤
public class CheckHurt : BTActionNode
{
    [LabelText("启用对象"),SerializeField,FoldoutGroup("@NodeName")]
    private MobAi _target;

    public override BehaviorState Tick()
    {
        if (!_target.isHurt) return BehaviorState.Failure;
        return BehaviorState.Success;
    }
}
/// 清除受伤标记
public class HurtAction : BTActionNode
{
    [LabelText("启用对象"),SerializeField,FoldoutGroup("@NodeName")]
    private MobAi _target;

    public override BehaviorState Tick()
    {
        _target.HurtAction();
        return BehaviorState.Success;
    }
}
/// 逃跑，持续返回Running，需配合TimeLimit
public class FleeAction : BTActionNode,IAbortable
{
    [LabelText("启用对象"),SerializeField,FoldoutGroup("@NodeName")]
    private PigAi _target;
    public override BehaviorState Tick()
    {
        _target.FleeAction();
        _target.Anim.SetBool("isMoving",true);
        return BehaviorState.Running;
    }
    public void Abort()
    {
        _target.Anim.SetBool("isMoving",false);
    }
}

/// 检测目标（OverlapSphere 检测玩家），发现返回 Success，否则 Failure
public class CheckTarget : BTActionNode
{
    [LabelText("启用对象"), SerializeField, FoldoutGroup("@NodeName")]
    private MosterAi _target;

    public override BehaviorState Tick()
    {
        return _target.CheckTarget() ? BehaviorState.Success : BehaviorState.Failure;
    }
}

/// 追击玩家，每帧朝目标移动并检查脱战距离，丢失目标返回 Failure，否则 Running
public class ChaseAction : BTActionNode, IAbortable
{
    [LabelText("启用对象"), SerializeField, FoldoutGroup("@NodeName")]
    private MosterAi _target;

    public override BehaviorState Tick()
    {
        if (!_target.isFound || _target.target == null)
            return BehaviorState.Failure;

        if (_target.LoseTarget())
        {
            _target.Anim.SetBool("isMoving", false);
            return BehaviorState.Failure;
        }

        _target.ChaseAction();
        return BehaviorState.Running;
    }

    public void Abort()
    {
        _target.Anim.SetBool("isMoving", false);
    }
}

/// 击退等待，力已由 Hurt() 施加，此节点仅保持 Running 直到 TimeLimit 超时
public class KnockbackAction : BTActionNode, IAbortable
{
    [LabelText("启用对象"), SerializeField, FoldoutGroup("@NodeName")]
    private MobAi _target;

    public override BehaviorState Tick()
    {
        return BehaviorState.Running;
    }

    public void Abort()
    {
        _target.EndKnockback();
    }
}

/// 等待
public class Wait : BTActionNode, IAbortable
{
    [LabelText("等待时长"), SerializeField, FoldoutGroup("@NodeName")]
    public float waitForTime;

    private float _timer;
    public override BehaviorState Tick()
    {
        _timer += Time.deltaTime;
        if (_timer >= waitForTime)
        {
            _timer = 0;
            return BehaviorState.Success;
        }
        return BehaviorState.Running;
    }

    public void Abort()
    {
        _timer = 0;
    }
}

