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

    private bool isAttack = false;
    private float attackTimer = 0f;
    private float _distance;
    private Vector3 _startPos;
    private Transform _target;
    private Animator _animator;
    private NavMeshAgent _nav;

    private State _state;
    public enum State
    {
        Waiting,
        Chasing,
        Attacking,
        AfterAttack,
        Returning
    }

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _startPos = transform.position;
        _nav.speed = moveSpeed;

        _target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
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
                break;
            case State.Attacking:
                break;
            case State.AfterAttack:
                break;
            case State.Returning:
                break;
        }
    }

    private void Chase()
    {
        _distance = Vector3.Distance(_target.position, transform.position);
        _nav.SetDestination(_target.position);

        if(_distance < attackDist)
        {
            //공격상태로 변경
        }
        if(_distance > detectDist)
        {
            //리턴상태로 변경
        }
    }

    private void UpdateTarget()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if(_distance < detectDist)
        {
            //추격상태로 변경.
        }
    }
}
