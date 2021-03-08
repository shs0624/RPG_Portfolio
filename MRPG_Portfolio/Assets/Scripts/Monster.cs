using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public float attackPower;
    public float moveSpeed;
    public float detectDist;
    public float attackDist;
    public float attackSpan;

    private bool dead = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float _distance;
    private float _ReturnDistance;
    private float _chaseDistance;
    private Vector3 _startPos;
    private Transform _target;
    private Animator _animator;
    private NavMeshAgent _nav;

    public enum State
    {
        Wait,
        Chase,
        Attack,
        Attacked,
        Return
    }
    public State _state = State.Wait;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _startPos = transform.position;
        _nav.speed = moveSpeed;
        
        _target = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (dead) return;

        switch(_state)
        {
            case State.Wait:
                UpdateTarget();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Attacked:
                Attacked();
                break;
            case State.Return:
                Return();
                break;
        }
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        if(_state != State.Chase && !dead)
        {
            ChangeState(State.Attacked);
        }
        //그 외 이벤트, 소리 출력
    }

    protected override void Die()
    {
        base.Die();

        _animator.SetTrigger("Die");

        Destroy(gameObject, 2f);

        _nav.enabled = false;

        this.enabled = false;
    }

    public void ChangeState(State state)
    {
        ExitState();
        _state = state;
        EnterState();
    }

    public void EnterState()
    {
        switch (_state)
        {
            case State.Wait:
                {
                    _nav.enabled = false;
                    _animator.SetBool("chasing", false);
                    break;
                }
            case State.Chase:
                {
                    _nav.enabled = true;
                    _nav.isStopped = false;
                    _nav.SetDestination(_target.position);
                    _animator.SetBool("chasing", true);
                    break;
                }
            case State.Attack:
                {
                    _nav.enabled = false;
                    break;
                }
            case State.Attacked:
                {
                    _nav.enabled = true;
                    _nav.isStopped = false;
                    _animator.SetBool("chasing", true);
                    _chaseDistance = Vector3.Distance(transform.position, _target.transform.position);
                    break;
                }
            case State.Return:
                {
                    _nav.enabled = true;
                    _nav.isStopped = false;
                    _nav.SetDestination(_startPos);
                    break;
                }
        }
    }

    public void ExitState()
    {
        switch (_state)
        {
            case State.Wait:
                {
                    _nav.enabled = false;
                    break;
                }
            case State.Chase:
                {
                    _nav.enabled = false;
                    _animator.SetBool("chasing", false);
                    break;
                }
            case State.Attack:
                {
                    _nav.enabled = true;
                    _nav.isStopped = false;
                    break;
                }
            case State.Attacked:
                {
                    break;
                }
            case State.Return:
                {
                    _nav.enabled = false;
                    break;
                }
        }
    }

    public void AttackEvent()
    {
        float dis = Vector3.Distance(_target.position, transform.position);
        
        if(dis < attackDist + 0.5f)
        {
            _target.GetComponent<PlayerHealth>().OnDamage(attackPower);
        }
    }

    private void LookAtTarget()
    {
        transform.LookAt(_target);
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        _animator.SetTrigger("Attack");

        //공격 코드

        yield return new WaitForSeconds(1f);

        attackTimer = 0f;

        isAttacking = false;

        ChangeState(State.Chase);
    }

    private void Attack()
    {
        if(attackTimer > attackSpan && !isAttacking)
        {
            LookAtTarget();
            StartCoroutine(AttackCoroutine()); 
        }
    }

    private void Return()
    {
        _distance = Vector3.Distance(_target.position, transform.position);
        _ReturnDistance = Vector3.Distance(_startPos, transform.position);
        //_nav.SetDestination(_startPos);

        if(_ReturnDistance < 0.1f)
        {
            ChangeState(State.Wait);
        }
        if(_distance < detectDist)
        {
            ChangeState(State.Chase);
        }
    }

    private void Attacked()
    {
        _distance = Vector3.Distance(_target.position, transform.position);
        _nav.SetDestination(_target.position);

        if (_distance < detectDist)
        {
            ChangeState(State.Chase);
        }
        if (_distance > _chaseDistance)
        {
            ChangeState(State.Return);
        }
    }

    private void Chase()
    {
        _distance = Vector3.Distance(_target.position, transform.position);
        _nav.SetDestination(_target.position);

        if (_distance < attackDist)
        {
            ChangeState(State.Attack);
        }
        if(_distance > detectDist)
        {
            ChangeState(State.Return);
        }
    }

    private void UpdateTarget()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if (_distance < detectDist)
        {
            ChangeState(State.Chase);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectDist);
    }
}
