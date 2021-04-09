using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotionController : SkillController
{
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void SpellSkill()
    {
        PlayerHealth ph = _player.GetComponent<PlayerHealth>();
        ph.HealHitPoint(skill.Damage);
        PlayerSkillManager.instance.CooltimeInitialization(skill);
    }
}
