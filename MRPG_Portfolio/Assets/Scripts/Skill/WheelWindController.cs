using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWindController : SkillController
{
    public float rangeRadius;
    public float damagePerTick;
    public float spinTime; //총 회전하는 시간 3.0초로 생각.
    public float tickTime; //데미지가 들어가는 시간. 0.6초로 생각
    public GameObject lineRenderer;

    private GameObject _player;
    private Animator _animator;
    private TrailRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = _player.GetComponent<Animator>();
        _lineRenderer = lineRenderer.GetComponent<TrailRenderer>();
    }

    public override void SpellSkill()
    {
        StartCoroutine(WheelWindCoroutine());
        // 휠윈드 도중엔, 다른 동작(공격)이 불가능하게끔 해야됨
        PlayerSkillManager.instance.CooltimeInitialization(skill);
    }

    IEnumerator WheelWindCoroutine()
    {
        float Timer = 0f;
        float tickTimer = 0f;
        _player.GetComponent<PlayerAttack>().enabled = false;
        _animator.SetTrigger("WheelWindTrigger");
        _lineRenderer.enabled = true;

        while (Timer < spinTime)
        {
            yield return new WaitForSeconds(0.1f);
            Timer += 0.1f;
            tickTimer += 0.1f;

            if (tickTimer >= tickTime)
            {
                damageTargets();
                tickTimer = 0f;
            }
        }

        _lineRenderer.enabled = false;
        _animator.SetTrigger("WheelWindOutTrigger");
        _player.GetComponent<PlayerAttack>().enabled = true;
        yield return null;
    }

    private void damageTargets()
    {
        Collider[] colls = 
            Physics.OverlapSphere(_player.transform.position, rangeRadius, LayerMask.GetMask("Monster"));

        //감지된 적에게 데미지주기.
        foreach (Collider col in colls)
        {
            col.GetComponent<LivingEntity>().OnDamage(damagePerTick, "Monster");
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_player.transform.position, rangeRadius);
    }
    */
}
