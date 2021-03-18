using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject MobDamageText;
    public GameObject PlayerDamageText;
    public Transform DamageParent;

    GameObject[] MobDamageArr = new GameObject[100];
    GameObject[] PlayerDamageArr = new GameObject[100];

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
        for(int i = 0; i < 100; i++)
        {
            MobDamageArr[i] = Instantiate(MobDamageText);
            MobDamageArr[i].transform.parent = DamageParent;
            MobDamageArr[i].SetActive(false);
        }

        for (int i = 0; i < 100; i++)
        {
            PlayerDamageArr[i] = Instantiate(PlayerDamageText);
            PlayerDamageArr[i].transform.parent = DamageParent;
            PlayerDamageArr[i].SetActive(false);
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
        }

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
