using UnityEngine;

public class SlimeAi : MosterAi
{
    [Header("史莱姆特有")]
    public float knockbackForce;
    public float knockbackTime;

    protected override void OnHurtResponse()
    {
        Rb.velocity = hurtDir * knockbackForce;
        if (Anim != null) Anim.SetBool("isMoving", false);
    }

    public override void EndKnockback()
    {
        Rb.velocity = Vector3.zero;
    }
}