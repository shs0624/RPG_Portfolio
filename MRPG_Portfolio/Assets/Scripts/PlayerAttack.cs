using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage;
    public TrailRenderer swordTrail;
    public Vector3 boxSize = new Vector3(5, 5, 5);
    public Transform attackCol;

    private PlayerMovement _playerMov;
    private Animator _animator;
    private int comboCnt = 0;
    private bool comboPossible = false;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMov = GetComponent<PlayerMovement>();
    }
    
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.3f);

        Debug.Log("AttackEvent");
        TrailOn();
        AttackEvent();

        while(_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"))
        {
            Debug.Log("In Animation");
            yield return null;
        }
        TrailOff();

        //공격이 끝나고, 콤보가 입력됐다면 콤보 애니메이션과 코루틴 실행
        if(comboPossible)
        {
            _animator.Play("Player_Combo");
            StartCoroutine(Combo1Coroutine());
        }
        else
        {
            isAttacking = false;
            comboPossible = false;
        }
    }
    
    IEnumerator Combo1Coroutine()
    {
        _playerMov.canMove = false;

        yield return new WaitForSeconds(0.3f);
        AttackEvent();
        TrailOn();

        while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Combo"))
        {
            Debug.Log("In Animation");
            yield return null;
        }
        TrailOff();

        isAttacking = false;
        comboPossible = false;
        _playerMov.canMove = true;
    }

    private void AttackEvent()
    {
        Collider[] colls = Physics.OverlapBox
            (attackCol.transform.position, boxSize, attackCol.transform.rotation, LayerMask.GetMask("Monster"));

        foreach (Collider coll in colls)
        {
            GameObject hitEffect = ObjectPool.instance.CallObj("HitEffect");
            hitEffect.transform.position = coll.transform.position + new Vector3(0,1f,0);
            hitEffect.GetComponent<ParticleSystem>().Play();
            coll.GetComponent<LivingEntity>().OnDamage(attackDamage, "Monster");
        }
    }

    public void Attack()
    {
        if(isAttacking)
        {
            //공격중이라면, 다음 콤보가 가능하도록 바꾼다.
            comboPossible = true;
        }
        else
        {
            comboPossible = false;
            _animator.Play("Player_Attack");
            StartCoroutine(AttackCoroutine());
        }
    }

    public void TrailOn()
    {
        swordTrail.enabled = true;
    }

    public void TrailOff()
    {
        if(swordTrail.enabled)
        {
            swordTrail.enabled = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCol.transform.position, boxSize);
    }
}
