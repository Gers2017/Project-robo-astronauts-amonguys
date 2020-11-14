using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmonguysSpawner : MonoBehaviour
{
    ObjectPooler pooler;
    public float spawn_time = 5f;
    public int max_instances = 10;
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
        InvokeRepeating("SpawnAmonguy", spawn_time, spawn_time);
    }
    
    void SpawnAmonguy()
    {
        if(pooler.GetActiveInstances() >= max_instances)
        {
            return;
        }

        Vector3 spawn_pos = transform.position + 
        new Vector3(Random.value * 20f, 0f, Random.value * 20f);

        GameObject instance = pooler.GetInstance(spawn_pos);
        
        Amonguys amonguys = instance.GetComponent<Amonguys>();
        amonguys?.Revive();
    }
}
