using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager instance;
    public SkillController[] skillControllers;
    public Skill[] firstSkillList;
    public Skill[] skillList;
    public SkillSlot[] slotList;

    private float[] coolTimes;
    //스킬리스트에 등록된 스킬들의 쿨타임 상태를 저장한 배열

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        for(int i = 0; i < firstSkillList.Length; i++)
        {
            if(slotList[i] != null && firstSkillList[i] != null)
            {
                slotList[i].RegisterSkill(firstSkillList[i]);
            }
        }

        coolTimes = new float[skillList.Length];
        for(int i = 0; i < skillList.Length; i++)
        {
            coolTimes[i] = skillList[i].CoolTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < skillList.Length; i++)
        {
            if (coolTimes[i] != 0f)
            {
                coolTimes[i] -= Time.smoothDeltaTime;
                if (coolTimes[i] <= 0) coolTimes[i] = 0f;
            }
        }
    }

    public void FindSkillController(Skill _skill)
    {
        for(int i = 0; i < skillControllers.Length; i++)
        {
            if(skillControllers[i].skill == _skill)
            {
                if(coolTimes[i] == 0)
                {
                    skillControllers[i].SpellSkill();
                    return;
                }
                return;
            }
        }
    }

    public void CooltimeInitialization(Skill skill)
    {
        int cnt = 0;
        for(int i = 0; i < skillList.Length; i++)
        {
            if(skill == skillList[i])
            {
                cnt = i; break;
            }
        }

        for(int i = 0; i < slotList.Length; i++)
        {
            if(slotList[i].skillCheck(skillList[cnt]))
            {
                slotList[i].PanelInitial();
            }
        }

        coolTimes[cnt] = skillList[cnt].CoolTime;
    }

    public float GetCoolTime(Skill skill)
    {
        int cnt = 0;
        for (int i = 0; i < skillList.Length; i++)
        {
            if (skill == skillList[i])
            {
                cnt = i; break;
            }
        }

        return coolTimes[cnt];
    }
}

