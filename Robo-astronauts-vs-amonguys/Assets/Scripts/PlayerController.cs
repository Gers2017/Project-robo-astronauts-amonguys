using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController character;
    [SerializeField] Transform camera_body;
    float speed = 10f;
    float turn_speed = 0.5f;
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float current_angle = transform.eulerAngles.y;
        float desired_agle = camera_body.eulerAngles.y;
        float angle_y = Mathf.LerpAngle(desired_agle, current_angle, Time.deltaTime * turn_speed);
        transform.rotation = Quaternion.Euler(0, desired_agle, 0);

        Vector3 movement = (transform.forward * vertical) + (transform.right * horizontal);
        character.Move(movement * speed * Time.deltaTime);
        
    }
}
