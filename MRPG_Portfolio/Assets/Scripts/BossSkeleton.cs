using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossSkeleton : LivingEntity
{
    public float[] patternPercentage;
    public float moveSpeed;
    public float attackDamage;
    public float attackDist;
    public float attackSpan;
    public float attackSpeed;

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
                Attack();
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
        _nav.SetDestination(_target.position);

        if (_distance < attackDist)
        {
            if (attackTimer < attackSpan) ChangeState(State.Chasing);
            else ChangeState(State.Attacking);
        }
    }

    private void Attack()
    {
        if(attackTimer > attackSpan && !isAttacking)
        {
            if (attackCount >= 2) StartCoroutine(SmashCoroutine());
            else StartCoroutine(AttackCoroutine());
        }
    }

    private void AttackEvent()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if (_distance <= attackDist)
        {
            LivingEntity living = _target.GetComponent<LivingEntity>();
            living.OnDamage(attackDamage);
        }
    }

    IEnumerator SmashCoroutine()
    {
        isAttacking = true;

        _animator.SetTrigger("Smash");

        yield return new WaitForSeconds(1f);

        attackTimer = 0f;

        attackCount = 0;

        isAttacking = false;

        ChangeState(State.Chasing);
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackSpeed);

        attackTimer = 0f;

        attackCount++;
        //if(attackCount >= 2)
        //{
        //    점프공격 준비는 어택코루틴 밖에서 실행하기
        //}

        isAttacking = false;

        ChangeState(State.Chasing);
    }
}
