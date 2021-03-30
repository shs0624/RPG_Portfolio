using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public float maxGuage;
    public float bossSpawnGuage;
    public int numToSpawn;
    public float spawnOffset;
    public Transform[] spawnPoints;
    public string[] mobNames;

    [SerializeField]private float _guage = 0f;
    [SerializeField]private int _remainingMonsters = 0;
    private bool isCleard = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        SpawnMonsters();
    }

    private void Update()
    {
        if(_remainingMonsters == 0 && !isCleard)
        {
            if(_guage >= bossSpawnGuage)
            {
                //보스를 스폰
            }
            else
            {
                //몬스터 스폰
                SpawnMonsters();
            }
        }
    }

    public void GuageUp(float _get)
    {
        _guage += _get;
        if(_guage >= bossSpawnGuage)
        {
            //보스 스폰
        }
    }

    public void SpawnMonsters()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            for (int j = 0; j < numToSpawn; j++)
            {
                float xOffset = Random.Range(-spawnOffset, spawnOffset);
                float zOffset = Random.Range(-spawnOffset, spawnOffset);
                Debug.Log(xOffset + " / " + zOffset);

                Vector3 pos = new Vector3(spawnPoints[i].position.x + xOffset, spawnPoints[i].position.y, spawnPoints[i].position.z + zOffset);
                Debug.Log(pos);

                GameObject g = ObjectPool.instance.CallObj("Skeleton");
                g.GetComponent<Monster>().PosSetUp(pos);
                g.GetComponent<Monster>().onDeath += () => _remainingMonsters--; 
                g.GetComponent<Monster>().onDeath += () => GuageUp(1); 
                _remainingMonsters++;
            }
        }
    }
}
