using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public TrailRenderer swordTrail;
    public BoxCollider attackCollider;

    private Animator _animator;
    private int comboCnt = 0;
    private bool comboPossible = false;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    // 1. 공격중일땐 이동을 못하게 하는 코드 추가하기
    // 2. 공격할때 실제로 적 콜라이더를 얻어오는 코드 추가하기.

    
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.35f);
        Debug.Log("Attack!");

        while(_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"))
        {
            yield return null;
        }


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
        yield return new WaitForSeconds(0.5f);
        Debug.Log("ComboAttack!");

        isAttacking = false;
        comboPossible = false;
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

    public void EnemyDetect()
    {
        attackCollider.enabled = true;

    }

    public void TrailOn()
    {
        swordTrail.enabled = true;
    }

    public void TrailOff()
    {
        swordTrail.enabled = false;
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void ComboReset()
    {
        comboCnt = 0;
    }
}
