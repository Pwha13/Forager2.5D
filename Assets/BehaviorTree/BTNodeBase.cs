using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>配套中断接口，所有持续节点（移动等）实现，Abort时停刚体速度</summary>
    public interface IAbortable
    {
        void Abort();
    }
    //状态枚举
    public enum BehaviorState
    {
        Stop,Success,Failure,Running
    }
    [BoxGroup]
    [HideReferenceObjectPicker]
    //基类
    public abstract class BTNodeBase
    {
        [FoldoutGroup("@NodeName"),LabelText("唯一标识")]
        public string Guid;
        [FoldoutGroup("@NodeName"),LabelText("名字")]
        public string NodeName;
        [FoldoutGroup("@NodeName"),LabelText("位置")]
        public Vector2 Position;
        public abstract BehaviorState Tick();
    }
    //组合节点
    public abstract class BTComposite : BTNodeBase
    {
        [FoldoutGroup("@NodeName"), LabelText("子节点")]
        public List<BTNodeBase> ChildNodes = new();
    }
    //装饰节点
    public abstract class BTPrecondition : BTNodeBase
    {
        [FoldoutGroup("@NodeName"), LabelText("子节点")]
        public BTNodeBase ChildNode;
    }
    //行为节点
    public abstract class BTActionNode : BTNodeBase { }
    
    //顺序节点
    public class Sequence : BTComposite,IAbortable
    {
        [LabelText("执行参数"), FoldoutGroup("@NodeName")]
        private int _index;
        public override BehaviorState Tick()
        {
            var state = ChildNodes[_index].Tick();
            switch (state)
            {
                case BehaviorState.Success:
                    _index++;
                    if (_index >= ChildNodes.Count)
                    {
                        _index = 0;
                        return BehaviorState.Success;
                    }
                    return BehaviorState.Running;
                
                case BehaviorState.Failure:
                    _index = 0;
                    return BehaviorState.Failure;
                
                case BehaviorState.Running:
                    return BehaviorState.Running;
            }
            return BehaviorState.Stop;
        }

        public void Abort()
        {
            if(_index < ChildNodes.Count&&ChildNodes[_index] is IAbortable abortNode)
                abortNode.Abort();
            _index = 0;
        }
    }
    public class PrioritySequence : BTComposite,IAbortable
    {
        [LabelText("执行参数"), FoldoutGroup("@NodeName")]
        private int _runningIndex = -1;
        public override BehaviorState Tick()
        {
            int newRunningIndex = -1;
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                var state = ChildNodes[i].Tick();
                if (state == BehaviorState.Running)
                {
                    newRunningIndex = i;
                    break;
                }
            }

            if (_runningIndex != -1 && _runningIndex != newRunningIndex)
            {
                if (ChildNodes[_runningIndex] is IAbortable oldChild)
                    oldChild.Abort();
            }
            
            _runningIndex = newRunningIndex;
            return _runningIndex != -1 ? BehaviorState.Running : BehaviorState.Success;
        }

        public void Abort()
        {
            if(_runningIndex!=-1 && _runningIndex < ChildNodes.Count)
            {
                if (ChildNodes[_runningIndex] is IAbortable abortNode)
                    abortNode.Abort();
            }

            _runningIndex = -1;
        }
    }
    //选择节点
    public class Selector : BTComposite,IAbortable
    {
        [LabelText("选择参数"), FoldoutGroup("@NodeName")]
        private int _selectIndex;

        public override BehaviorState Tick()
        {
            var state = ChildNodes[_selectIndex].Tick();
            switch (state)
            {
                case BehaviorState.Success:
                    _selectIndex = 0;
                    return state;
                case BehaviorState.Failure:
                    _selectIndex++;
                    if (_selectIndex >= ChildNodes.Count)
                    {
                        _selectIndex = 0;
                        return BehaviorState.Failure;
                    }
                    return state;
                case BehaviorState.Running:
                    return state;
            }
            return BehaviorState.Stop;
        }

        public void Abort()
        {
            if(_selectIndex < ChildNodes.Count&&ChildNodes[_selectIndex] is IAbortable abortNode)
                abortNode.Abort();
            _selectIndex = 0;
        }
    }
    public class Parallel : BTComposite, IAbortable
    {
        public override BehaviorState Tick()
        {
            if (ChildNodes.Count == 0)
                return BehaviorState.Success;

            bool anyRunning = false;

            for (int i = 0; i < ChildNodes.Count; i++)
            {
                var state = ChildNodes[i].Tick();

                if (state == BehaviorState.Failure)
                {
                    // 任一失败 → 立即中止其余子节点
                    for (int j = 0; j < ChildNodes.Count; j++)
                    {
                        if (j != i && ChildNodes[j] is IAbortable abortNode)
                            abortNode.Abort();
                    }
                    return BehaviorState.Failure;
                }

                if (state == BehaviorState.Running)
                    anyRunning = true;
            }

            return anyRunning ? BehaviorState.Running : BehaviorState.Success;
        }

        public void Abort()
        {
            foreach (var child in ChildNodes)
            {
                if (child is IAbortable abortNode)
                    abortNode.Abort();
            }
        }
    }

    //TODO:循环节点Repeat
    //延时节点
    public class Delay : BTPrecondition,IAbortable
    {
        [LabelText("延时"), SerializeField,FoldoutGroup("@NodeName")] 
        private float _timer;
        
        private float _currentTimer;
        public override BehaviorState Tick()
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= _timer)
            {
                _currentTimer = 0;
                ChildNode.Tick();
                return BehaviorState.Success;
            }
            return BehaviorState.Running;
        }

        public void Abort()
        {
            _currentTimer = 0;
            if(ChildNode is IAbortable abortNode)
                abortNode.Abort();
        }
    }
    /// <summary>
    /// 限时装饰器：子树运行开始计时，超时强制Abort子树返回Failure
    /// 被外部打断/超时都会重置状态，下次进入从头执行子节点
    /// </summary>
    public class TimeLimit : BTPrecondition,IAbortable
    {
        [LabelText("最大持续时长"),SerializeField,FoldoutGroup("@NodeName")]
        public float duration;
        [LabelText("超时返回的结果"), FoldoutGroup("@NodeName")]
        public BehaviorState timeoutResult = BehaviorState.Failure;
        
        private float _startTime;
        private bool _isRunning;

        public override BehaviorState Tick()
        {
            // 无子节点直接失败
            if (ChildNode == null)
                return BehaviorState.Failure;

            // 首次进入，初始化计时
            if (!_isRunning)
            {
                _startTime = Time.time;
                _isRunning = true;
            }

            float runTime = Time.time - _startTime;
            // 判断超时
            if (runTime >= duration)
            {
                AbortChild();
                ResetState();
                return timeoutResult;
            }

            // 执行子节点
            BehaviorState childState = ChildNode.Tick();
            // 子树正常跑完（成功/失败），重置计时
            if (childState != BehaviorState.Running)
            {
                ResetState();
            }
            return childState;
        }

        /// <summary>强制终止子节点</summary>
        private void AbortChild()
        {
            // 规范：你框架需要给所有节点加Abort重置接口，这里示意调用
            if (ChildNode is IAbortable abortNode)
                abortNode.Abort();
        }

        /// <summary>清空计时运行状态</summary>
        private void ResetState()
        {
            _isRunning = false;
            _startTime = 0f;
        }

        public void Abort()
        {
            ResetState();
            AbortChild();
        }
    }

    /// <summary>
    /// 冷却装饰器：仅子树完整返回Success后才开启冷却；冷却期间直接返回Failure，不执行子树
    /// 中途被打断（子树Failure）不会触发冷却惩罚
    /// </summary>
    public class Cooldown : BTPrecondition,IAbortable
    {
        [LabelText("冷却时长(秒)"),SerializeField,FoldoutGroup("@NodeName")]
        public float coolTime;

        private float _coolEndTime;
        private bool _isCooling;

        public override BehaviorState Tick()
        {
            if (ChildNode == null)
                return BehaviorState.Failure;

            // 处于冷却阶段，直接拦截，不执行子节点
            if (_isCooling)
            {
                if (Time.time < _coolEndTime)
                    return BehaviorState.Failure;
                // 冷却结束，解除冷却标记
                _isCooling = false;
            }

            // 冷却放行，执行子节点
            BehaviorState childState = ChildNode.Tick();

            // 只有子树完整成功跑完，才开启冷却计时(改成不管跑不跑完都
            if (childState != BehaviorState.Running)
            {
                _coolEndTime = Time.time + coolTime;
                _isCooling = true;
            }

            return childState;
        }

        public void Abort()
        {
            _isCooling = false;
            _coolEndTime = 0;
            if(ChildNode is IAbortable abortNode)
                abortNode.Abort();
        }
    }
}