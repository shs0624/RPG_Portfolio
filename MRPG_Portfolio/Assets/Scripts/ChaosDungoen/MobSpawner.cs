using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public int numToSpawn;
    public float spawnOffset;
    public Transform[] spawnPoints;
    public string[] mobNames;

    // Start is called before the first frame update
    void Start()
    {
        SpawnMonsters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMonsters()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            for(int j = 0; j < numToSpawn; j++)
            {
                float xOffset = Random.Range(-spawnOffset, spawnOffset);
                float zOffset = Random.Range(-spawnOffset, spawnOffset);
                Debug.Log(xOffset + " / " + zOffset);

                Vector3 pos = new Vector3(spawnPoints[i].position.x + xOffset, spawnPoints[i].position.y, spawnPoints[i].position.z + zOffset);
                Debug.Log(pos);

                GameObject g = ObjectPool.instance.CallObj("Skeleton");
                g.transform.position = pos;
            }
        }
    }
}
