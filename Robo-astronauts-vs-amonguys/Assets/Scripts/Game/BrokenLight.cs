using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLight : MonoBehaviour
{
    public Light point_lihgt;
    public float min_light_range = 2.5f;
    public float max_light_range = 12f;
    public float min_light_time = 2.5f;
    public float max_light_time = 4f;
    void Start()
    {
        Invoke("Spark", 2f);
    }

    void Spark()
    {
        point_lihgt.range = Random.Range(min_light_range, max_light_range);
        float t = Random.Range(min_light_time, max_light_time);
        Invoke("Spark", t);
    }
}
