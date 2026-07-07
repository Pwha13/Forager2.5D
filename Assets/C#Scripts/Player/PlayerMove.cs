using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour,IPlayerDataProvider
{
    private PlayerInput _input;
    private Rigidbody _rb;
    private SpriteRenderer _sr;
    private PlayerAnim _anim;
    [SerializeField]private HandToolShow hand;
    
    private bool _isGround;
    [SerializeField] private float speed;
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private float checkDistance = 3f;
    [SerializeField] private float groundOffset = 0.2f;

    public void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<PlayerAnim>();
    }

    private void Update()
    {
        FaceDir(_input.MousePos);
        MoveJudge(_input.MoveIndex);
    }

    private void FixedUpdate()
    {
        Move(_input.MoveIndex);
    }

    public void FaceDir(Vector3 mousePos)
    {
        if (mousePos.x - transform.position.x < 0)
        {
            _sr.flipX = true;
            hand.isLeft = true;
        }
        else
        {
            _sr.flipX = false;
            hand.isLeft = false;
        }
    }

    public void MoveJudge(Vector3 moveIndex)
    {
        Vector3 faceDir = new Vector3(moveIndex.x, -1, moveIndex.z);
        _isGround = Physics.SphereCast(transform.position + Vector3.up * groundOffset, radius, faceDir, out RaycastHit hit,checkDistance,LayerMask.GetMask("Ground"));
    }
    public void Move(Vector3 moveIndex)
    {
        
        if (moveIndex != Vector3.zero) _anim.isWalking = true;
        else _anim.isWalking = false;
        
        if (!_isGround)
            _rb.velocity = Vector3.zero;
        else
            _rb.velocity = new Vector3(moveIndex.x*speed, 0, moveIndex.z*speed);
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
