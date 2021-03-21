using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public int chainLimit = 3;
    public float chainRadius = 4f;
    public List<Skill> skillList;
    public Vector3 boxSize = new Vector3(5, 5, 5);
    public Transform chainRange;

    private int count = 0;
    private List<Monster> targetList = new List<Monster>();
    private Dictionary<Monster, float> targetDictionary = new Dictionary<Monster, float>();
    private PlayerMovement _playerMov;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMov = GetComponent<PlayerMovement>();
    }


    public void ChainLightning()
    {
        SetTargetList();
        // 1. 타겟들을 체크해서 리스트에 넣기.

        MakeLightning();
        // 2. 타겟이하나라도 존재하면, 스킬이 사용되고 해당 위치에 프리팹 생성
        // 3. 손에서 프리팹까지 연결.(라인 렌더러)

        Initalization();
    }

    private void Initalization()
    {
        targetDictionary.Clear();
        targetList.Clear();
        count = 0;
    }

    private void MakeLightning()
    {

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

        Debug.Log("1번 타겟 : " + mobList[0]);
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

                Debug.Log((count + 1) + "번 타겟 : " + mobList[0]);
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
