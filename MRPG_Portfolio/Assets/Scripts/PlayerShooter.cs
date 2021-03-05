using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject Arrow;
    public Transform shotPoint;
    public float timeBetweenShot = 0.5f;

    private float lastShotTime;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Fire()
    {
        if(Time.time > lastShotTime + timeBetweenShot)
        {
            Shot();
            lastShotTime = Time.time;
        }
    }

    private void Shot()
    {
        //GameObject arrow = Instantiate(Arrow, shotPoint.position, Quaternion.Euler(-90,transform.rotation.eulerAngles.y,0));
        GameObject arrow = ObjectPool.instance.GetArrowObj();
        arrow.transform.position = shotPoint.position;
        arrow.transform.rotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, 0);

        Debug.Log(transform.rotation.y);
        arrow.GetComponent<Projectile>().Init(transform.forward);
        animator.SetTrigger("Attack");
    }
}
