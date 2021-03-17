using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float startingHealth;

    protected bool isDead;
    [SerializeField]protected float _health;
    public event Action onDeath;

    protected void Setup()
    {
        isDead = false;
        _health = startingHealth;
    }

    public virtual void OnDamage(float damage)
    {
        _health -= damage;
        Debug.Log(this.gameObject + " / Get Damaged!");

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
