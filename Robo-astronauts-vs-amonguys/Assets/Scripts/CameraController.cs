using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform character_body;
    float x_angle;
    Vector3 offset;
    float damping = 1.5f;

    void Start()
    {
        offset = character_body.position - transform.position;
        x_angle = transform.eulerAngles.x;
        transform.SetParent(null);
    }

    void LateUpdate()
    {

        float current_rotation = transform.eulerAngles.y;
        float desired_rotation = character_body.eulerAngles.y;
        float y_angle = Mathf.LerpAngle(current_rotation, desired_rotation, damping * Time.deltaTime);

        //transform.Rotate(Vector3.up, y_angle);
        Quaternion rotation = Quaternion.Euler(x_angle, y_angle, 0f);
        transform.rotation = rotation;

        transform.position = character_body.position - (rotation * offset);
        transform.LookAt(character_body);
    }
}
