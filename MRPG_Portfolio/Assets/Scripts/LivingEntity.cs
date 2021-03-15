using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float startingHealth = 10f;

    protected bool isDead;
    protected float _health;
    public event Action onDeath;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        _health = startingHealth;
    }

    public virtual void OnDamage(float damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
