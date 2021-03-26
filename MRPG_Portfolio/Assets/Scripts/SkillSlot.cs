using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image skillImage;
    public CoolTimePanel _coolTimePanel;

    private Skill registeredSkill;

    public bool skillCheck(Skill skill)
    {
        return (registeredSkill == skill);
    }

    public void PanelInitial()
    {
        _coolTimePanel.timeSetting(registeredSkill.CoolTime);
    }

    public void RegisterSkill(Skill skill)
    {
        registeredSkill = skill;

        skillImage.sprite = skill.SkillImage;
        _coolTimePanel.timeSetting(skill.CoolTime);
    }

    public void SpellSkill()
    {
        PlayerSkillManager.instance.FindSkillController(registeredSkill);
    }
}
