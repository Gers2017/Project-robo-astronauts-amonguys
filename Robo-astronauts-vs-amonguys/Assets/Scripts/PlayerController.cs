using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    CharacterController character;
    ObjectPooler hitParticlesPooler;
    Animator animator;
    float speed = 10f;
    int damage_amount = 10;
    float shoot_distance = 25f;
    LayerMask target_layer;
    [SerializeField] Transform shoot_point;
    [SerializeField] ParticleSystem flashParticles;
    int velocity_hash, damage_hash;
    
    //Health section
    int start_health = 100;
    int health;
    float time_hit_again;
    float hit_time_offset = 2f;
    public int Max_health => start_health;
    public int Health => health;

    void Awake()
    {
        character = GetComponent<CharacterController>();
        hitParticlesPooler = GetComponent<ObjectPooler>();
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        target_layer = LayerMask.GetMask("Amonguys");
        velocity_hash = Animator.StringToHash("velocity");
        damage_hash = Animator.StringToHash("damage");

        health = start_health;
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
        animator.SetFloat(velocity_hash, character.velocity.magnitude);
    }

    private void HandleAttack()
    {
        bool trigger_attack = Input.GetButtonDown("Fire1");

        if(trigger_attack)
        {
            RaycastHit hit;
            var ray = Physics.Raycast(shoot_point.position, shoot_point.forward, out hit, shoot_distance, target_layer);
            flashParticles.Play();

            if(ray)
            {
                Debug.DrawRay(shoot_point.position, shoot_point.forward * hit.distance, Color.yellow, 2f);
                var amonguys = hit.transform.gameObject.GetComponent<Amonguys>();
                amonguys?.TakeDamage(damage_amount);
                hitParticlesPooler.GetInstance(hit.point);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if(Time.time < time_hit_again) return;
        health -= amount;
        if(health <= 0)
        {
            Debug.Log("Player Dead");
        }
        animator.SetTrigger(damage_hash);

        time_hit_again = Time.time + hit_time_offset;
    }
}
