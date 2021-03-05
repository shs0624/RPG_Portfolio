using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;

    private Vector3 moveDir;
    private float Timer;

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer += Time.deltaTime;

        transform.position += moveDir * speed * Time.deltaTime;

        if (Timer > 6f) ObjectPool.ReturnArrowObject(gameObject);
    }

    public void Init(Vector3 vec)
    {
        moveDir = vec;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>().OnDamage(damage);

            ObjectPool.ReturnArrowObject(gameObject);
        }
    }
}
