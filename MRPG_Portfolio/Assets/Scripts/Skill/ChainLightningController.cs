using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainLightningController : SkillController
{
    public int chainLimit = 3;
    public float chainRadius = 4f;
    public Transform chainRange;
    public Vector3 Lightningoffset;
    public Vector3 boxSize = new Vector3(5, 5, 5);

    private int count = 0;
    private float _damage;
    private float _coolTime;
    private GameObject _player;
    private List<LivingEntity> targetList = new List<LivingEntity>();

    // Start is called before the first frame update
    void Start()
    {
        _damage = skill.Damage;
        _coolTime = skill.CoolTime;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void SpellSkill()
    {
        SetTargetList();

        MakeLightning();

        GiveDamage();

        LCInitalization();
    }

    private void LCInitalization()
    {
        targetList.Clear();
        PlayerSkillManager.instance.CooltimeInitialization(skill);
        count = 0;
    }

    // idx = 사용하는 스킬의 skillList에서의 인덱스
    private void GiveDamage()
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            targetList[i].OnDamage(_damage, "Monster");
        }
    }

    private void MakeLightning()
    {
        // 처음엔 플레이어와 리스트 첫번째를 연결.
        GameObject chain = ObjectPool.instance.CallObj("ChainLightning");
        chain.GetComponent<ChainLightningEffect>().PosSetting(_player.transform.position + Lightningoffset, targetList[0].transform.position + Lightningoffset);

        for (int i = 1; i < count; i++)
        {
            chain = ObjectPool.instance.CallObj("ChainLightning");
            chain.GetComponent<ChainLightningEffect>().PosSetting(targetList[i - 1].transform.position + Lightningoffset, targetList[i].transform.position + Lightningoffset);
        }
    }

    private void SetTargetList()
    {
        Dictionary<LivingEntity, float> targetDictionary = new Dictionary<LivingEntity, float>();

        Collider[] colls = Physics.OverlapBox
            (chainRange.position, boxSize, chainRange.rotation, LayerMask.GetMask("Monster"));

        //거리와 객체를 Dictionary에 넣기.
        foreach (Collider col in colls)
        {
            targetDictionary.Add(col.GetComponent<LivingEntity>(), Vector3.Distance(_player.transform.position, col.transform.position));
        }

        List<LivingEntity> mobList = targetDictionary.Keys.ToList();

        mobList.Sort(delegate (LivingEntity A, LivingEntity B)
        {
            if (targetDictionary[A] > targetDictionary[B]) return 1;
            else if (targetDictionary[A] < targetDictionary[B]) return -1;
            return 0;
        });

        targetList.Add(mobList[0]); count++;

        // 타겟의 근처 적을 탐색해, 그 주변 적에게 사슬을 연결
        ChainEnemies(mobList[0]);
    }

    private void ChainEnemies(LivingEntity center)
    {
        LivingEntity _center = center;
        Dictionary<LivingEntity, float> tempDictionary = new Dictionary<LivingEntity, float>();

        while (count < chainLimit)
        {
            Collider[] colls = Physics.OverlapSphere
                (_center.transform.position, chainRadius, LayerMask.GetMask("Monster"));

            if (colls.Length > 0)
            {
                foreach (Collider col in colls)
                {
                    if (targetList.Contains(col.GetComponent<LivingEntity>())) continue;

                    tempDictionary.Add(col.GetComponent<LivingEntity>(), Vector3.Distance(_center.transform.position, col.transform.position));
                }

                if (tempDictionary.Count == 0) break;

                List<LivingEntity> mobList = tempDictionary.Keys.ToList();

                mobList.Sort(delegate (LivingEntity A, LivingEntity B)
                {
                    if (tempDictionary[A] > tempDictionary[B]) return 1;
                    else if (tempDictionary[A] < tempDictionary[B]) return -1;
                    return 0;
                });

                targetList.Add(mobList[0]);
                count++;

                tempDictionary.Clear();
            }
            else
            {
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(chainRange.position, boxSize);
    }
}
