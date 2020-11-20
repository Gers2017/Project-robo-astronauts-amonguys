using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmonguysSpawner : MonoBehaviour
{
    ObjectPooler pooler;
    [SerializeField] float spawn_time = 5f;
    [SerializeField] int max_instances = 10;
    [SerializeField] Transform[] spawn_points;
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
        InvokeRepeating("SpawnAmonguy", spawn_time, spawn_time);
        InvokeRepeating("ReduceSpawnTime", spawn_time, spawn_time + 1f);
    }

    void SpawnAmonguy()
    {
        if(pooler.GetActiveInstances() >= max_instances)
        {
            return;
        }

        Vector3 spawn_pos = spawn_points[Random.Range(0, spawn_points.Length)].position;

        GameObject instance = pooler.GetInstance(spawn_pos);
        
        Amonguys amonguys = instance.GetComponent<Amonguys>();
        amonguys?.Revive();
    }

    void ReduceSpawnTime()
    {
        if(spawn_time <= 0f)
        {
            CancelInvoke("ReduceSpawnTime");
        }

        bool chance = Random.value > 0.5f;
        if(chance)
            spawn_time -= 0.5f;
    }

}
