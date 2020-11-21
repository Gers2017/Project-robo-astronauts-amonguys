using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSetter : MonoBehaviour
{
    public Transform[] objects;
    public float positionModifier = 0.2f;
    public float scaleModifier = 0.2f;
    void Awake()
    {
        foreach (var item in objects)
        {
            var position = new Vector3(Random.Range(-1,1) * positionModifier, 0f, 
            Random.Range(-1,1) * positionModifier);
            if(position.magnitude < 1f)
                item.position += position;
            
            var scale = item.localScale.x + (Random.value * scaleModifier);
            item.localScale.Set(scale, scale, scale);
        }
    }
}
