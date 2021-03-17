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
    public Vector3 boxSize = new Vector3(2, 2, 2);
    public Transform attackCol;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float _distance;
    private Vector3 _startPos;
    private Transform _target;
    private Animator _animator;
    private NavMeshAgent _nav;

    public State _state;
    public enum State
    {
        Waiting,
        Chasing,
        Attacking,
        AfterAttack,
        Returning
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _startPos = transform.position;
        _nav.speed = moveSpeed;
        Setup();

        _target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if (isDead) return;

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
            case State.AfterAttack:
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
                //_nav.enabled = false;
                _animator.SetBool("isMoving", false);
                break;
            case State.Chasing:
                _nav.isStopped = false;
                //_nav.enabled = true;
                _animator.SetBool("isMoving", true);
                break;
            case State.Attacking:
                _nav.isStopped = true;
                //_nav.enabled = false;
                break;
            case State.AfterAttack:
                _nav.isStopped = false;
                //_nav.enabled = true;
                break;
            case State.Returning:
                _nav.isStopped = false;
                //_nav.enabled = true;
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
                break;
            case State.AfterAttack:
                break;
            case State.Returning:
                _animator.SetBool("isMoving", false);
                break;
        }
    }

    private void ChangeState(State st)
    {
        ExitState(st);
        _state = st;
        EnterState(st);
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetTrigger("Die");
        this.GetComponent<CapsuleCollider>().isTrigger = true;
        Destroy(gameObject, 5f);
    }

    private void AttackEvent()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if(_distance <= attackDist)
        {
            LivingEntity living = _target.GetComponent<LivingEntity>();
            living.OnDamage(attackDamage);
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackSpeed);

        AttackEvent();

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
        _nav.SetDestination(_target.position);

        if(_distance < attackDist)
        {
            if (attackTimer < attackSpan) ChangeState(State.Waiting);
            else ChangeState(State.Attacking);
        }
        if(_distance > detectDist)
        {
            ChangeState(State.Returning);
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



