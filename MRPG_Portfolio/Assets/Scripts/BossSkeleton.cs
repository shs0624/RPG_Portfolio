﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossSkeleton : LivingEntity
{
    public float[] patternPercentage;
    public float moveSpeed;
    public float attackDamage;
    public float smashDistance;
    public float attackDistance;
    public float attackSpan;
    public float attackSpeed;
    public float waitingTime;

    private int attackCount = 0;
    private float attackTimer;
    private float _hpPercentage;
    private float _distance;
    private bool isAttacking = false;
    private Color firstColor;
    private Transform _target;
    private Animator _animator;
    private NavMeshAgent _nav;
    private Material _material;

    public State _state;
    public enum State
    {
        Chasing,
        Attacking,
        Smashing,
        Summoning
    }

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        _nav.speed = moveSpeed;
        firstColor = _material.color;

        _target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (isDead || isAttacking) return;

        switch (_state)
        {
            case State.Chasing:
                Chase();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.Smashing:
                Smash();
                break;
            case State.Summoning:
                break;
        }
    }

    public void PosSetUp(Vector3 pos)
    {
        transform.position = pos;
        _nav.enabled = true;
    }

    private void EnterState(State st)
    {
        switch (st)
        {
            case State.Chasing:
                _nav.isStopped = false;
                _animator.SetBool("isMoving", true);
                break;
            case State.Attacking:
                _nav.isStopped = true;
                _animator.SetBool("isMoving", false);
                break;
            case State.Smashing:
                _nav.isStopped = true;
                _animator.SetBool("isMoving", false);
                break;
            case State.Summoning:
                _nav.isStopped = true;
                _animator.SetBool("isMoving", false);
                break;
        }
    }

    private void ExitState(State st)
    {
        switch (st)
        {
            case State.Chasing:
                _animator.SetBool("isMoving", false);
                break;
            case State.Attacking:
                _nav.isStopped = false;
                break;
            case State.Smashing:
                _nav.isStopped = false;
                break;
            case State.Summoning:
                _nav.isStopped = false;
                break;
        }
    }

    private void ChangeState(State st)
    {
        ExitState(_state);
        _state = st;
        EnterState(_state);
    }

    private void Chase()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if (_distance > attackDistance - 0.5f)
        {
            Debug.Log("Chase");
            _animator.SetBool("isMoving", true);
            _nav.SetDestination(_target.position);
        }
        else
        {
            Debug.Log("Stop");
            _animator.SetBool("isMoving", false);
            _nav.SetDestination(transform.position);
        }

        if(_distance <= smashDistance)
        {
            if(attackCount >= 2)
            {
                if (attackTimer < attackSpan) ChangeState(State.Smashing);
            }
            else if(_distance < attackDistance)
            {
                if (attackTimer >= attackSpan) ChangeState(State.Attacking);
            }
        }
    }

    private void Smash()
    {
        if (attackTimer > attackSpan && !isAttacking)
        {
            StartCoroutine(SmashCoroutine());
        }
    }

    private void Attack()
    {
        if(attackTimer > attackSpan && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private void AttackEvent()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if (_distance <= attackDistance)
        {
            LivingEntity living = _target.GetComponent<LivingEntity>();
            living.OnDamage(attackDamage);
        }
    }

    private Vector3 GetLandingPosition(Vector3 input)
    {
        Vector3 heading = _target.position - transform.position;
        heading = heading.normalized;
        heading.y = 0;

        return input - 2 * heading;
    }

    IEnumerator SmashCoroutine()
    {
        isAttacking = true;
        transform.LookAt(_target.transform);
        _animator.SetTrigger("SmashReady");

        yield return new WaitForSeconds(2.5f);

        transform.LookAt(_target.transform);
        _animator.SetTrigger("Smash");
        StartCoroutine(SmashMoveUpdate(_target.position,1.5f));

        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(1.5f);

        attackTimer = 0f;
        attackCount = 0;
        _animator.SetTrigger("Return");
        isAttacking = false;

        ChangeState(State.Chasing);
    }

    IEnumerator SmashMoveUpdate(Vector3 targetPos, float duration)
    {
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 pos = startPos;

        Vector3 _targetPos = GetLandingPosition(targetPos);
        //_targetPos에 빨간색 범위 표시하기.
        Debug.Log(_targetPos);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            pos.x = Mathf.Lerp(startPos.x, _targetPos.x, timer / duration);
            pos.y = Mathf.Lerp(startPos.y, _targetPos.y, timer / duration);
            pos.z = Mathf.Lerp(startPos.z, _targetPos.z, timer / duration);

            transform.localPosition = pos;

            yield return null;
        }

        //_targetPos의 범위 내부 적에게 데미지 주기.
        pos = _targetPos;
        transform.localPosition = pos;
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        _animator.SetTrigger("Attack");
        transform.LookAt(_target.transform);

        yield return new WaitForSeconds(attackSpeed);

        AttackEvent();

        yield return new WaitForSeconds(waitingTime);

        attackTimer = 0f;
        attackCount++;
        isAttacking = false;

        ChangeState(State.Chasing);
    }
}