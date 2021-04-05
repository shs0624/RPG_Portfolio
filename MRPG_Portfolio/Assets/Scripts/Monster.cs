﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public float attackDamage;
    public float moveSpeed;
    public float detectDist;
    public float attackDist;
    public float attackSpan;
    public float attackSpeed;
    public float attackWaitingTime;
    public Vector3 boxSize = new Vector3(2, 2, 2);
    public Transform attackCol;
    public event Action onDeath;

    private bool isAttacking = false;
    private bool isImpacted = false;
    private float attackTimer = 0f;
    private float _distance;
    private Color firstColor;
    private Vector3 _startPos;
    private Transform _target;
    [SerializeField]private Material _material;
    private Animator _animator;
    private NavMeshAgent _nav;

    public State _state;
    public enum State
    {
        Waiting,
        Chasing,
        Attacking,
        Impacted,
        Returning
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        _startPos = transform.position;
        _nav.speed = moveSpeed;
        firstColor = _material.color;
        Setup();

        _target = GameObject.Find("Player").transform;
    }

    private void OnEnable()
    {
        this.GetComponent<CapsuleCollider>().enabled = true;
        isAttacking = false;
        isImpacted = false;
        attackTimer = 0f;
        _nav.enabled = false;
        ChangeState(State.Waiting);
        Setup();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (isDead || isImpacted) return;

        switch(_state)
        {
            case State.Waiting:
                UpdateTarget();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.Impacted:
                break;
            case State.Returning:
                Return();
                break;
        }
    }

    private void EnterState(State st)
    {
        switch (st)
        {
            case State.Waiting:
                _nav.isStopped = true;
                _animator.SetBool("isMoving", false);
                break;
            case State.Chasing:
                _nav.isStopped = false;
                _animator.SetBool("isMoving", true);
                break;
            case State.Attacking:
                _nav.isStopped = true;
                break;
            case State.Impacted:
                _nav.isStopped = true;
                _animator.SetBool("isMoving", false);
                break;
            case State.Returning:
                _nav.isStopped = false;
                _animator.SetBool("isMoving", true);
                break;
        }
    }

    private void ExitState(State st)
    {
        switch (st)
        {
            case State.Waiting:
                break;
            case State.Chasing:
                _animator.SetBool("isMoving", false);
                break;
            case State.Attacking:
                _nav.isStopped = false;
                break;
            case State.Impacted:
                _nav.isStopped = false;
                break;
            case State.Returning:
                _animator.SetBool("isMoving", false);
                break;
        }
    }

    private void ChangeState(State st)
    {
        ExitState(_state);
        _state = st;
        EnterState(_state);
    }

    public void PosSetUp(Vector3 pos)
    {
        _startPos = pos;
        transform.position = _startPos;
        _nav.enabled = true;
    }

    public override void OnDamage(float damage, string tag)
    {
        if (!isDead)
        {
            base.OnDamage(damage, tag);
            StartCoroutine(FlashCoroutine());

            if (isDead) return;
            if (!isImpacted)
            {
                StopCoroutine(AttackCoroutine());
                isAttacking = false;
                StartCoroutine(ImpactCoroutine());
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        if(onDeath != null)
        {
            onDeath();
            onDeath = null;
        }
        _animator.SetTrigger("Die");
        this.GetComponent<CapsuleCollider>().enabled = false;
        _nav.enabled = false;
        Invoke("TurnOff", 5f);
    }

    private void TurnOff()
    {
        this.gameObject.SetActive(false);
    }

    private void AttackEvent()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if(_distance <= attackDist)
        {
            LivingEntity living = _target.GetComponent<LivingEntity>();
            living.OnDamage(attackDamage, "Player");
        }
    }

    /*
    IEnumerator ActiveCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        _animator.SetTrigger("Active");

        this.GetComponent<CapsuleCollider>().enabled = true;
        isAttacking = false;
        isImpacted = false;
        attackTimer = 0f;
        _nav.enabled = false;
        ChangeState(State.Waiting);
        Setup();
    }
    */

    IEnumerator FlashCoroutine()
    {
        _material.color = new Color(255, 255, 255);

        yield return new WaitForSeconds(0.1f);

        _material.color = firstColor;
    }

    IEnumerator ImpactCoroutine()
    {
        isImpacted = true;

        _animator.SetTrigger("Impact");

        ChangeState(State.Impacted);

        yield return new WaitForSeconds(1.5f);

        ChangeState(State.Returning);

        isImpacted = false;
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        _animator.SetTrigger("Attack");
        transform.LookAt(_target.transform);

        yield return new WaitForSeconds(attackSpeed);

        AttackEvent();

        yield return new WaitForSeconds(attackWaitingTime);

        attackTimer = 0f;
        isAttacking = false;
        _distance = Vector3.Distance(_target.position, transform.position);
        
        ChangeState(State.Chasing);
    }

    private void Attack()
    {
        if(attackTimer > attackSpan && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private void Chase()
    {
        _distance = Vector3.Distance(_target.position, transform.position);
        if (_distance > attackDist - 0.2f)
        {
            _animator.SetBool("isMoving", true);
            _nav.SetDestination(_target.position);
        }
        else
        {
            _animator.SetBool("isMoving", false);
            _nav.SetDestination(transform.position);
        }

        if(_distance < attackDist)
        {
            if (attackTimer >= attackSpan) ChangeState(State.Attacking);
        }
    }

    private void Return()
    {
        _distance = Vector3.Distance(_target.position, transform.position);
        float _returnDist = Vector3.Distance(_startPos, transform.position);
        _nav.SetDestination(_startPos);

        if(_returnDist < 0.1f)
        {
            ChangeState(State.Waiting);
        }
        if(_distance < detectDist)
        {
            ChangeState(State.Chasing);
        }
    }

    private void UpdateTarget()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if(_distance < detectDist)
        {
            if (_distance < attackDist && attackTimer < attackSpan) ChangeState(State.Waiting);
            else ChangeState(State.Chasing);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCol.transform.position, boxSize);
    }
}



