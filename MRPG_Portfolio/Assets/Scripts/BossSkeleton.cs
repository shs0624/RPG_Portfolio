using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossSkeleton : LivingEntity
{
    public float[] patternPercentage;
    public float moveSpeed;
    public float attackDamage;
    public float smashDamage;
    public float smashDistance;
    public float smashRange;
    public float attackDistance;
    public float attackSpan;
    public float attackSpeed;
    public float waitingTime;
    public Transform[] SpawnLocations;
    public Image bossHitBar;
    public Text bossHitText;
    public event Action onDeath;

    private int patternIdx = 0;
    private int attackCount = 0;
    private float attackTimer;
    private float _hpPercentage;
    private float _distance;
    private bool isAttacking = false;
    private bool TurnToSpawn = false;
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
        base.Setup();

        bossHitBar = GameObject.Find("UICanvas").transform.Find("BossHitFrame").transform.Find("BossHitBar").GetComponent<Image>();
        bossHitText = bossHitBar.transform.Find("BossHitText").GetComponent<Text>();
        _target = GameObject.Find("Player").transform;
    }

    private void OnEnable()
    {
        this.GetComponent<CapsuleCollider>().enabled = true;
        isAttacking = false;
        TurnToSpawn = false;
        attackTimer = 0f;
        _nav.enabled = false;
        ChangeState(State.Chasing);
        base.Setup();
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
                Summon();
                break;
        }
    }

    public override void OnDamage(float damage, string tag)
    {
        base.OnDamage(damage, tag);
        UpdateBossHitBar(startingHealth, _health);

        if (patternIdx >= patternPercentage.Length) return;

        float per = (_health / startingHealth) * 100f;

        if (per <= patternPercentage[patternIdx])
        {
            TurnToSpawn = true;
            patternIdx++;
        }
    }

    protected override void Die()
    {
        base.Die();
        if (onDeath != null)
        {
            onDeath();
            onDeath = null;
        }
        _animator.SetTrigger("Die");
        this.GetComponent<CapsuleCollider>().enabled = false;
        _nav.enabled = false;
        Invoke("TurnOff", 3f);
    }

    public void InitSetting(Vector3 pos)
    {
        transform.position = pos;
        _nav.enabled = true;
        UpdateBossHitBar(startingHealth, _health);
    }   

    private void UpdateBossHitBar(float max, float current)
    {
        float ratio = current / max;
        string str = string.Format("{0:0.00}", float.Parse((ratio*100f).ToString())) + "%";
        bossHitBar.fillAmount = ratio;

        bossHitText.text = str;
    }

    private void TurnOff()
    {
        this.gameObject.SetActive(false);
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
            if(TurnToSpawn)
            {
                if (attackTimer < attackSpan) ChangeState(State.Summoning);
                TurnToSpawn = false;
            }
            else if(attackCount >= 2)
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

    private void Summon()
    {
        if(!isAttacking)
        {
            StartCoroutine(SummonCoroutine());
        }
    }

    private void AttackEvent()
    {
        _distance = Vector3.Distance(_target.position, transform.position);

        if (_distance <= attackDistance)
        {
            LivingEntity living = _target.GetComponent<LivingEntity>();
            living.OnDamage(attackDamage, "Player");
        }
    }

    private Vector3 GetLandingPosition(Vector3 input)
    {
        Vector3 heading = _target.position - transform.position;
        heading = heading.normalized;
        heading.y = 0;

        return input - 2 * heading;
    }

    private void SmashEvent(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, smashRange, LayerMask.GetMask("Player"));

        foreach (Collider coll in colls)
        {
            PlayerHealth pHealth = coll.GetComponent<PlayerHealth>();
            pHealth.OnDamage(smashDamage, "Player");
        }
    }

    //1. 소환 위치에 스켈레톤 프리팹을 소환.
    //2. 소환 당시 보스는 소환 애니메이션 재생
    //3. 소환 위치에 소환 이펙트를 재생.
    //4. 소환이 끝나고, 일정시간 Idle을 유치하고 Chase 상태로 변경.
    IEnumerator SummonCoroutine()
    {
        Debug.Log("소환");
        isAttacking = true;

        _animator.SetTrigger("Summon");

        for(int i = 0; i < SpawnLocations.Length; i++)
        {
            GameObject s = ObjectPool.instance.CallObj("Skeleton");
            s.GetComponent<Monster>().PosSetUp(SpawnLocations[i].position);

            this.onDeath += () => s.SetActive(false);
        }
        //소환코드 여기
        for (int i = 0; i < SpawnLocations.Length; i++)
        {
            GameObject e = ObjectPool.instance.CallObj("SummonEffect");
            e.transform.position = SpawnLocations[i].position;
        }
        //이펙트도 여기

        yield return new WaitForSeconds(3f);

        _animator.SetTrigger("Return");

        ChangeState(State.Chasing);
        Debug.Log("소환끝");
        isAttacking = false;
    }

    IEnumerator SmashEffectCoroutine(float time, Vector3 pos)
    {
        GameObject effect = ObjectPool.instance.CallObj("SmashEffect");
        effect.transform.position = pos;
        effect.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(time);

        effect.SetActive(false);
    }

    IEnumerator ShowIndicator(float time)
    {
        GameObject indicator = ObjectPool.instance.CallObj("Indicator");
        Vector3 pos = _target.position;
        indicator.transform.position = pos;
        Vector3 firstscale = indicator.transform.localScale;
        indicator.transform.localScale = new Vector3(smashRange, smashRange, indicator.transform.localScale.z);

        yield return new WaitForSeconds(time - 0.2f);
        indicator.transform.localScale = firstscale;
        indicator.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        SmashEvent(pos);
        StartCoroutine(SmashEffectCoroutine(time, pos));
    }

    IEnumerator SmashCoroutine()
    {
        isAttacking = true;
        transform.LookAt(_target.transform);
        _animator.SetTrigger("SmashReady");

        yield return new WaitForSeconds(2.5f);

        transform.LookAt(_target.transform);
        _animator.SetTrigger("SmashJump");
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
        bool triggerd = false;

        Vector3 startPos = transform.position;
        Vector3 pos = startPos;

        Vector3 _targetPos = GetLandingPosition(targetPos);
        //_targetPos에 빨간색 범위 표시하기.
        StartCoroutine(ShowIndicator(duration));

        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (timer >= duration - 0.3f && !triggerd)
            {
                _animator.SetTrigger("SmashAttack");
                triggerd = true;
            }

            pos.x = Mathf.Lerp(startPos.x, _targetPos.x, timer / duration);
            pos.y = Mathf.Lerp(startPos.y, _targetPos.y, timer / duration);
            pos.z = Mathf.Lerp(startPos.z, _targetPos.z, timer / duration);

            transform.localPosition = pos;

            yield return null;
        }

        //_targetPos의 범위 내부 적에게 데미지 주기.
        pos = _targetPos;
        //transform.localPosition = pos;
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
