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
    private bool isSpawning = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SpawnMonsters();
    }

    private void Update()
    {
        if(_remainingMonsters == 0 && isSpawning)
        {
            SpawnMonsters();
        }
    }

    public void GuageUp(float _get)
    {
        _guage += _get;
        if(_guage >= maxGuage)
        {
            //게임 클리어
            Debug.Log("Clear!");
        }
        else if(_guage >= bossSpawnGuage)
        {
            isSpawning = false;
            SpawnBoss();
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

                Vector3 pos = new Vector3(spawnPoints[i].position.x + xOffset, spawnPoints[i].position.y, spawnPoints[i].position.z + zOffset);

                GameObject g = ObjectPool.instance.CallObj(mobNames[0]);
                g.GetComponent<Monster>().PosSetUp(pos);
                g.GetComponent<Monster>().onDeath += () => _remainingMonsters--; 
                g.GetComponent<Monster>().onDeath += () => GuageUp(10); 
                _remainingMonsters++;
            }
        }
    }

    private void SpawnBoss()
    {
        int rand = Random.Range(0,spawnPoints.Length);

        Vector3 pos = new Vector3(spawnPoints[rand].position.x, spawnPoints[rand].position.y, spawnPoints[rand].position.z);

        GameObject g = ObjectPool.instance.CallObj("Boss");

        g.GetComponent<BossSkeleton>().PosSetUp(pos);
        g.GetComponent<BossSkeleton>().onDeath += () => GuageUp(50);
    }
}
