using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController character;
    float speed = 10f;
    int damage = 10;
    float shoot_distance = 25f;
    LayerMask target_layer;
    [SerializeField] Transform shoot_point;

    void Start()
    {
        character = GetComponent<CharacterController>();
        target_layer = LayerMask.GetMask("Amonguys");
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = (transform.forward * vertical + transform.right * horizontal);
        character.Move(movement.normalized * speed * Time.deltaTime);
    }

    private void HandleAttack()
    {
        bool trigger_attack = Input.GetButtonDown("Fire1");

        if(trigger_attack)
        {
            RaycastHit hit;
            var ray = Physics.Raycast(shoot_point.position, shoot_point.forward, out hit, shoot_distance, target_layer);
            if(ray)
            {
                Debug.DrawRay(shoot_point.position, shoot_point.forward * hit.distance, Color.yellow, 2f);
                var amonguys = hit.transform.gameObject.GetComponent<Amonguys>();
                amonguys?.TakeDamage(damage);
            }
        }
    }
}
