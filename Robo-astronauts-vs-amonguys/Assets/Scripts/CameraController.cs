using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float x_rotation;
    Vector3 offset;
    [SerializeField] Transform target;
    [SerializeField] float turn_speed = 10f;
    [Range(0.1f, 1f)] public float sensitibity = 0.5f;
    [SerializeField] Vector2 camera_offset;
    void Start()
    {
        transform.SetParent(null);
        x_rotation = transform.eulerAngles.x;
        offset = target.position - transform.position;
    }

    void LateUpdate()
    {
        Orbit();
    }

    void Orbit()
    {
        float mouse_x = Input.GetAxis("Mouse X") * sensitibity;
        float angle = mouse_x * turn_speed;
        target.Rotate(Vector3.up, angle);

        //Y rotation of the target
        float desired_rotation = target.eulerAngles.y;
        //The desired_rotation for the camera
        Quaternion rotation = Quaternion.Euler(x_rotation, desired_rotation, 0);
        //position where the camera should be in base of the rotation
        var rotation_position = rotation * offset;
        transform.position = target.transform.position - rotation_position + (Vector3)camera_offset;
        transform.rotation = rotation;
    }
}
