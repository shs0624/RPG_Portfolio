using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;
    private int comboCnt = 0;
    private bool comboPossible = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    public void Attack()
    {
        if(comboCnt == 0)
        {
            _animator.SetTrigger("Attack");
            comboCnt = 1;
        }
        else
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboCnt += 1;
            }
        }
    }

    public void Combo()
    {
        if(comboCnt == 2)
        {
            _animator.SetTrigger("Combo");
        }
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
