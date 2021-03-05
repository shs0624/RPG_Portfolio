using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float startingHealth = 10f;

    private bool dead;
    [SerializeField]protected float _health;
    public event Action onDeath; // 사망시 발동할 이벤트

    protected virtual void OnEnable()
    {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        _health = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        dead = true;
    }
}
