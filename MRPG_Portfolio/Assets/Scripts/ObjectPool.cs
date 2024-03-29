﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject MobDamageText;
    public GameObject PlayerDamageText;
    public GameObject IndicatorPrefab;
    public GameObject SmashCrackEffect;
    public GameObject HitEffect;
    public GameObject SummonEffect;
    public GameObject chainLightningPrefab;
    public GameObject skeletonPrefab;
    public GameObject bossPrefab;
    public Transform DamageParent;
    public Transform SkillParent;
    public Transform ObjectParent;
    public Transform EffectParent;
    public Transform IndicatorParent;

    GameObject[] MobDamageArr = new GameObject[100];
    GameObject[] PlayerDamageArr = new GameObject[100];
    GameObject[] ChainLightningArr = new GameObject[50];
    GameObject[] IndicatorArr = new GameObject[5];
    GameObject[] SmashEffectArr = new GameObject[5];
    GameObject[] HitEffectArr = new GameObject[50];
    GameObject[] SummonEffectArr = new GameObject[10];
    GameObject[] SkeletonArr = new GameObject[100];
    GameObject[] BossArr = new GameObject[5];

    GameObject[] target;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        Setup();
    }

    private void Setup()
    {
        for(int i = 0; i < MobDamageArr.Length; i++)
        {
            MobDamageArr[i] = Instantiate(MobDamageText);
            MobDamageArr[i].transform.parent = DamageParent;
            MobDamageArr[i].SetActive(false);
        }

        for (int i = 0; i < PlayerDamageArr.Length; i++)
        {
            PlayerDamageArr[i] = Instantiate(PlayerDamageText);
            PlayerDamageArr[i].transform.parent = DamageParent;
            PlayerDamageArr[i].SetActive(false);
        }

        for (int i = 0; i < ChainLightningArr.Length; i++)
        {
            ChainLightningArr[i] = Instantiate(chainLightningPrefab);
            ChainLightningArr[i].transform.parent = SkillParent;
            ChainLightningArr[i].SetActive(false);
        }

        for (int i = 0; i < IndicatorArr.Length; i++)
        {
            IndicatorArr[i] = Instantiate(IndicatorPrefab);
            IndicatorArr[i].transform.parent = IndicatorParent;
            IndicatorArr[i].transform.Rotate(new Vector3(90f, 0, 0));
            IndicatorArr[i].SetActive(false);
        }

        for (int i = 0; i < SmashEffectArr.Length; i++)
        {
            SmashEffectArr[i] = Instantiate(SmashCrackEffect);
            SmashEffectArr[i].transform.parent = EffectParent;
            SmashEffectArr[i].transform.Rotate(new Vector3(90f, 0, 0));
            SmashEffectArr[i].SetActive(false);
        }

        for (int i = 0; i < HitEffectArr.Length; i++)
        {
            HitEffectArr[i] = Instantiate(HitEffect);
            HitEffectArr[i].transform.parent = EffectParent;
            HitEffectArr[i].SetActive(false);
        }

        for (int i = 0; i < SummonEffectArr.Length; i++)
        {
            SummonEffectArr[i] = Instantiate(SummonEffect);
            SummonEffectArr[i].transform.parent = EffectParent;
            SummonEffectArr[i].SetActive(false);
        }

        for (int i = 0; i < SkeletonArr.Length; i++)
        {
            SkeletonArr[i] = Instantiate(skeletonPrefab);
            SkeletonArr[i].transform.parent = ObjectParent;
            SkeletonArr[i].SetActive(false);
        }

        for (int i = 0; i < BossArr.Length; i++)
        {
            BossArr[i] = Instantiate(bossPrefab);
            BossArr[i].transform.parent = ObjectParent;
            BossArr[i].SetActive(false);
        }
    }

    public GameObject CallObj(string name)
    {
        switch(name)
        {
            case "MonsterDamage":
                target = MobDamageArr;
                break;
            case "PlayerDamage":
                target = PlayerDamageArr;
                break;
            case "ChainLightning":
                target = ChainLightningArr;
                break;
            case "Indicator":
                target = IndicatorArr;
                break;
            case "SmashEffect":
                target = SmashEffectArr;
                break;
            case "HitEffect":
                target = HitEffectArr;
                break;
            case "SummonEffect":
                target = SummonEffectArr;
                break;
            case "Skeleton":
                target = SkeletonArr;
                break;
            case "Boss":
                target = BossArr;
                break;
        }

        Debug.Log(target);

        for(int i = 0; i < target.Length; i++)
        {
            if(!target[i].activeSelf)
            {
                target[i].SetActive(true);
                return target[i];
            }
        }

        return null;
    }
}
