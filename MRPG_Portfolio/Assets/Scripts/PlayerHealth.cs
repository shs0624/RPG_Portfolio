using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public UIManager uIManager;
    private Animator _animator;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        Setup();
    }

    public void HealHitPoint(float heal)
    {
        _health += heal;
        if (_health > startingHealth) _health = startingHealth;

        uIManager.SetHitGuage(_health);
    }

    public override void OnDamage(float damage, string tag)
    {
        if (isDead) return;

        base.OnDamage(damage, tag);
        uIManager.SetHitGuage(_health);
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetTrigger("Die");
        GameManager.instance.GameOver();
    }
}
