using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    Animator _animator;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        Setup();
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        GameObject g = ObjectPool.instance.CallObj("PlayerDamage");

        g.transform.position = transform.position + Vector3.up;
        g.GetComponent<DamageText>().SetText(damage);
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetTrigger("Die");
    }
}
