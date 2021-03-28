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
    public bool isImpacted = false;
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
        Impacted,
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

    public override void OnDamage(float damage)
    {
        if (!isDead)
        {
            base.OnDamage(damage);
            GameObject g = ObjectPool.instance.CallObj("MonsterDamage");
            g.transform.position = transform.position + Vector3.up;
            g.GetComponent<DamageText>().SetText(damage);

            if (isDead) return;
            if (!isImpacted)
            {
                StopAllCoroutines();
                isAttacking = false;
                StartCoroutine(ImpactCoroutine());
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetTrigger("Die");
        this.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 5f);
        this.enabled = false;
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

    IEnumerator ImpactCoroutine()
    {
        isImpacted = true;

        _animator.SetTrigger("Impact");

        ChangeState(State.Impacted);

        yield return new WaitForSeconds(2f);

        ChangeState(State.Returning);

        isImpacted = false;
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



