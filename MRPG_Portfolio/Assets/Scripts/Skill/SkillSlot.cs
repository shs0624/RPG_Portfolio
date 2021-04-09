using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler
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
        _coolTimePanel.timeInitialSetting(registeredSkill.CoolTime);
    }

    //스킬 슬롯에 해당 스킬을 넣을때 사용하는 함수
    public void ChangeSkill(Skill skill, float cool)
    {
        registeredSkill = skill;
        skillImage.sprite = skill.SkillImage;
        Debug.Log("cool : " + cool);
        _coolTimePanel.timeSetting(skill.CoolTime, cool);
    }

    public void RegisterSkill(Skill skill)
    {
        registeredSkill = skill;

        skillImage.sprite = skill.SkillImage;
        _coolTimePanel.timeInitialSetting(skill.CoolTime);
    }

    public void SpellSkill()
    {
        PlayerSkillManager.instance.FindSkillController(registeredSkill);
    }

    public void OnDrop(PointerEventData eventData)
    {
        ChangeSkill(DragHandler.instance.GetDraggingSkill(), 
            PlayerSkillManager.instance.GetCoolTime(DragHandler.instance.GetDraggingSkill()));
    }
}
