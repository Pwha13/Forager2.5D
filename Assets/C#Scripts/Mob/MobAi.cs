using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MobAi : MonoBehaviour
{
    [Header("Attributes")]
    public float moveSpeed;
    public int maxHealth;
    public float offset;

    [Header("References")]
    public int health;
    public bool isGround;
    public bool isHurt;
    public bool isDead;
    public float detectGroundDistance;
    public Vector3 moveDir;
    public Vector3 theScale;
    public Vector3 hurtDir;

    public Animator Anim;
    protected Rigidbody Rb;
    protected SpriteRenderer Sr;
    protected ItemDrop ItemDrop;

    public virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        ItemDrop = GetComponent<ItemDrop>();
        Sr = GetComponent<SpriteRenderer>();
        theScale = transform.localScale;
        health = maxHealth;
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Attack"))
            Hurt(other);
    }
    public void Move(){Move(moveDir, 1);}

    public virtual void Move(Vector3 dir, int mul)
    {
        transform.position += dir * (mul * moveSpeed * Time.deltaTime);
        if (Anim != null)
            Anim.SetBool("isMoving", true);
    }

    public virtual void ChangeDir()
    { Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        if (Sr != null) Sr.flipX = randomDir.x < 0;
        moveDir = new Vector3(randomDir.x, 0, randomDir.y);
    }

    public virtual bool CheckGround()
    {
        Vector3 origin = transform.position + offset * transform.up;
        Vector3 direction = moveDir + Vector3.down;
        bool check = Physics.Raycast(origin, direction, detectGroundDistance, LayerMask.GetMask("Ground"));
        isGround = check;
        return check;
    }

    public virtual void Hurt(Collider other)
    {
        Vector3 tempDir = (transform.position - other.transform.position).normalized;
        if (Sr != null) Sr.flipX = tempDir.x < 0;
        hurtDir = new Vector3(tempDir.x, 0, tempDir.z);

        int damage = 1;
        if (other.TryGetComponent(out PlayerAttack attacker))
            damage = (int)attacker.playerDamage;

        health -= damage;
        if (health <= 0)
        {
            Dead();
        }
        else
        {
            transform.DOScale(theScale * 1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
            HurtAnim();
            isHurt = true;
            OnHurtResponse();
        }
    }

    /// <summary>子类覆写以在受伤时做额外响应（如 Slime 击退力）</summary>
    protected virtual void OnHurtResponse() { }

    /// <summary>子类覆写以在击退结束时清理（如 Slime 停刚体速度）</summary>
    public virtual void EndKnockback() { }

    public virtual void HurtAction()
    {
        isHurt = false;
    }

    public virtual void Dead()
    {
        isDead = true;
        transform.DOKill();
        if (Anim != null)
            Anim.SetBool("isMoving", false);
        if (ItemDrop != null)
            ItemDrop.Drop();
    }

    public void HurtAnim()
    {
        if (Sr != null)
            Sr.DOColor(Color.red, 0.1f).SetLoops(10, LoopType.Yoyo);
    }
}