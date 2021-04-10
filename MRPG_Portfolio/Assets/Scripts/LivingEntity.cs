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

    public virtual void OnDamage(float damage, string tag)
    {
        GameObject g;
        _health -= damage;
        Debug.Log(this.gameObject + " / Get Damaged!");

        if (tag == "Monster") g = ObjectPool.instance.CallObj("MonsterDamage");
        else g = ObjectPool.instance.CallObj("PlayerDamage");
        g.transform.position = transform.position + Vector3.up;
        g.GetComponent<DamageText>().SetText(damage);

        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
