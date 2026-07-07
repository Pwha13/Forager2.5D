using UnityEngine;

public class MosterAi : MobAi
{
    [Header("怪物属性")]
    public float loseTargetDistance;
    public float detectRadius;
    public float damage;

    [Header("运行时")]
    public Transform target;
    public bool isFound;

    private Collider[] _playerResults = new Collider[1];

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("Player"))
            Attack(other);
    }

    /// <summary>OverlapSphere 检测玩家，发现则记录目标</summary>
    public virtual bool CheckTarget()
    {
        if (!isFound)
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position, detectRadius, _playerResults,
                LayerMask.GetMask("Player"));
            if (count > 0)
            {
                target = _playerResults[0].transform;
                isFound = true;
            }
        }
        return isFound;
    }

    /// <summary>距离检查：超出脱战范围则丢失目标</summary>
    public virtual bool LoseTarget()
    {
        if (!isFound || target == null) return true;
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist >= loseTargetDistance)
        {
            target = null;
            isFound = false;
            return true;
        }
        return false;
    }

    /// <summary>朝向目标移动，每 Tick 调用</summary>
    public virtual void ChaseAction()
    {
        if (target == null) return;
        Vector3 dir = (target.position - transform.position).normalized;
        if (Sr != null) Sr.flipX = dir.x < 0;
        moveDir = new Vector3(dir.x, 0, dir.z);
        Move(moveDir, 1);
        if (Anim != null) Anim.SetBool("isMoving", true);
    }

    public virtual void AttackAction() { }
    public virtual void Attack(Collider other) { }
    public virtual void LoseTargetAction() { }
}