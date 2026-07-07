using BehaviorTree;
using UnityEngine;

public class PigAi : MobAi
{
    public void FleeAction()
    {
        Move(hurtDir, 2);
    }
}