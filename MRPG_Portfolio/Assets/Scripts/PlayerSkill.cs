using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public int chainLimit = 3;
    public float chainRadius = 4f;
    public Skill[] skillSlots = new Skill[3]; // 스킬 슬롯에 저장된 스킬들 리스트
    public List<Skill> skillList = new List<Skill>(); // 전체 스킬들 리스트
    public List<CoolTimePanel> coolTimePanelList = new List<CoolTimePanel>();
    public Vector3 boxSize = new Vector3(5, 5, 5);
    public Transform chainRange;
    public Vector3 Lightningoffset;

    private int count = 0;
    private float[] coolTimes;
    private List<Monster> targetList = new List<Monster>();
    private Dictionary<Monster, float> targetDictionary = new Dictionary<Monster, float>();
    private Dictionary<Skill, int> skillDictionary = new Dictionary<Skill, int>(); //스킬리스트에 저장된 idx를 포함한 딕셔너리
    private PlayerMovement _playerMov;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMov = GetComponent<PlayerMovement>();
        coolTimes = new float[skillList.Count];

        for(int i = 0; i < coolTimePanelList.Count; i++)
        {
            coolTimePanelList[i].timeSetting(0f);
        }

        for(int i = 0; i < skillList.Count; i++)
        {
            coolTimes[i] = 0f;//skillList[i].CoolTime;
            skillDictionary.Add(skillList[i], i);
        }
    }

    private void Update()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (coolTimes[i] > 0)
            {
                coolTimes[i] -= Time.deltaTime;
                if (coolTimes[i] <= 0) coolTimes[i] = 0;
            }
        }
    }

    public void SpellSkill(int idx)
    {
        if(skillSlots[idx] != null)
        {
            int n = skillDictionary[skillSlots[idx]];
            //n = 스킬리스트에서의 내가 시전한 스킬의 인덱스
            switch(skillSlots[idx].SkillName)
            {
                case "ChainLightning":
                    if (CheckCool(n))
                    {
                        ChainLightning(n);
                        SetCoolTimePanels(n);
                    }
                    break;
                case "WheelWind":
                    break;
            }
        }
    }

    //idx = skillList에서 체인 라이트닝의 인덱스
    public void ChainLightning(int idx)
    {
        SetTargetList();
        // 1. 타겟들을 체크해서 리스트에 넣기.

        MakeLightning();
        // 2. 타겟이하나라도 존재하면, 스킬이 사용되고 해당 위치에 프리팹 생성
        // 3. 손에서 프리팹까지 연결.(라인 렌더러)
        GiveDamage(idx);

        LCInitalization(idx);
    }

    // 스킬의 번호를 받아서, 각 패널에 스킬들의 쿨타임을 적용시키는 함수
    private void SetCoolTimePanels(int skillnum)
    {
        for(int i = 0; i < skillSlots.Length; i++)
        {
            if(skillSlots[i] == skillList[skillnum])
            {
                coolTimePanelList[i].timeSetting(skillList[skillnum].CoolTime);
            }
        }
    }

    private bool CheckCool(int idx)
    {
        if (coolTimes[skillDictionary[skillList[idx]]] <= 0)
        {
            return true;
        }
        return false;
    }

    private void LCInitalization(int idx)
    {
        targetDictionary.Clear();
        targetList.Clear();
        coolTimes[idx] = skillList[idx].CoolTime;
        count = 0;
    }

    // idx = 사용하는 스킬의 skillList에서의 인덱스
    private void GiveDamage(int idx)
    {
        for(int i = 0; i < targetList.Count; i++)
        {
            targetList[i].OnDamage(skillList[idx].Damage);
        }
    }

    private void MakeLightning()
    {
        // 처음엔 플레이어와 리스트 첫번째를 연결.
        GameObject chain = ObjectPool.instance.CallObj("ChainLightning");
        chain.GetComponent<ChainLightning>().PosSetting(transform.position + Lightningoffset, targetList[0].transform.position + Lightningoffset);

        for(int i = 1; i < count; i++)
        {
            chain = ObjectPool.instance.CallObj("ChainLightning");
            chain.GetComponent<ChainLightning>().PosSetting(targetList[i - 1].transform.position + Lightningoffset, targetList[i].transform.position + Lightningoffset);
        }
    }

    private void SetTargetList()
    {
        Collider[] colls = Physics.OverlapBox
            (chainRange.position, boxSize, chainRange.rotation, LayerMask.GetMask("Monster"));

        //거리와 객체를 Dictionary에 넣기.
        foreach (Collider col in colls)
        {
            targetDictionary.Add(col.GetComponent<Monster>(), Vector3.Distance(transform.position, col.transform.position));
        }

        List<Monster> mobList = targetDictionary.Keys.ToList();

        mobList.Sort(delegate (Monster A, Monster B)
        {
            if (targetDictionary[A] > targetDictionary[B]) return 1;
            else if (targetDictionary[A] < targetDictionary[B]) return -1;
            return 0;
        });

        targetList.Add(mobList[0]); count++;

        // 타겟의 근처 적을 탐색해, 그 주변 적에게 사슬을 연결
        ChainEnemies(mobList[0]);
    }

    private void ChainEnemies(Monster center)
    {
        Monster _center = center;
        Dictionary<Monster, float> tempDictionary = new Dictionary<Monster, float>();

        while(count < chainLimit)
        {
            Collider[] colls = Physics.OverlapSphere
                (_center.transform.position, chainRadius, LayerMask.GetMask("Monster"));

            if(colls.Length > 0)
            {
                foreach (Collider col in colls)
                {
                    if (targetList.Contains(col.GetComponent<Monster>())) continue;

                    tempDictionary.Add(col.GetComponent<Monster>(), Vector3.Distance(_center.transform.position, col.transform.position));
                }

                if (tempDictionary.Count == 0) break;

                List<Monster> mobList = tempDictionary.Keys.ToList();

                mobList.Sort(delegate (Monster A, Monster B)
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
        Gizmos.DrawWireSphere(transform.position, chainRadius);
    }
}
