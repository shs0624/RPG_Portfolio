using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/Skill Data")]
public class Skill : ScriptableObject
{
    [SerializeField]
    private string skillName;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float coolTime;
    

    public string SkillName { get { return skillName; } }
    public float Damage { get { return damage; } }
    public float CoolTime { get { return coolTime; } }
}
