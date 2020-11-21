using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUI;
public class PlayerController : MonoBehaviour, IDamagable
{
    const int EYES_MATERIAL_INDEX = 2;
    const float GRAVITY = -9.8f;
    CharacterController character;
    Animator animator;
    bool is_alive = true;
    bool is_shoot_mode;
    float speed = 10f;
    int damage_amount = 10;
    float shoot_distance = 100f;
    float shoot_time;
    float shoot_time_delay = 0.13f;
    public bool can_shoot => shoot_time < Time.time;
    //Health section
    public int max_health = 100;
    public int health;
    public int Health { get => health; set => SetHealth(value); }
    public event Action<float> OnHpPercentage;
    float time_hit_again = 0;
    float hit_time_offset = 1.5f;
    public bool is_damaged  => Time.time < time_hit_again;
    [SerializeField] LayerMask target_layer;
    float turn_smooth_vel;
    float turn_smooth_time = 0.1f;
    int velocity_hash, damage_hash;

    //Camera settings
    float max_shoot_angle_x = 2.5f;
    float min_shoot_angle_x = -1f;
    float shoot_turn_speed = 7.5f;

    float max_move_angle_x = 15f;
    float min_move_angle_x = -5f;
    float move_turn_speed = 15f;


    [Header("Camera")]
    [SerializeField] CameraController camController;

    [Header("References")]
    [SerializeField] Transform shoot_point;
    [SerializeField] SlideBar health_bar;
    [SerializeField] AudioSource audio_src_shoot;
    [SerializeField] AudioSource audio_src_effects;


    [Header("Particles")]
    [SerializeField] ParticleSystem flash_particles;
    ObjectPooler hit_particles_pooler;


    [Header("Audio clips")]
    [SerializeField] AudioClip damage_audio;
    [SerializeField] AudioClip laser_audio;

    [Header("Mesh and materials")]
    [SerializeField] SkinnedMeshRenderer robi_renderer;
    [SerializeField] Light robi_light;
    [SerializeField] Material normal_mat, danger_mat;
    [SerializeField] Color normal_light_color = Color.green, danger_light_color = Color.red;

    void OnApplicationFocus(bool focusStatus) {
        if(focusStatus)
        {
            Cursor.visible = false;
        }
    }

    void Awake()
    {
        character = GetComponent<CharacterController>();
        hit_particles_pooler = GetComponent<ObjectPooler>();
    }
    void Start()
    {
        SetupAnimator();
        SetupHealth();
    }

    private void SetupAnimator()
    {
        animator = GetComponentInChildren<Animator>();
        velocity_hash = Animator.StringToHash("velocity");
        damage_hash = Animator.StringToHash("damage");
    }

    private void SetupHealth()
    {
        Health = max_health;
        health_bar.SetStartValue(max_health);
        OnHpPercentage += health_bar.SetBarValue;
    }

    void Update()
    {
        if (is_alive)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                HandlePlayerMode(true);
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                HandlePlayerMode(false);
            }

            Vector3 move_direction = GetInputDirection();

            Move(move_direction);
            HandleAttack();
        }

        if(!character.isGrounded)
            character.Move(Vector3.up * GRAVITY * Time.deltaTime);
    }

    private void HandlePlayerMode(bool isShootMode)
    {
        camController.SetZoom(isShootMode);
        camController.MaxAngleX = isShootMode ? max_shoot_angle_x : max_move_angle_x;
        camController.MinAngleX = isShootMode? min_shoot_angle_x : min_move_angle_x;
        camController.TurnSpeed = isShootMode? shoot_turn_speed : move_turn_speed;
        speed *= isShootMode ? 0.5f : 2f;
    }

    private static Vector3 GetInputDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 move_direction = new Vector3(horizontal, 0f, vertical).normalized;
        return move_direction;
    }

    void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 0, max_health);
        OnHpPercentage?.Invoke((float)health / max_health);
    }

    void Move(Vector3 direction)
    {
        Vector3 movement = (transform.forward * direction.z + transform.right * direction.x);
        character.Move(movement.normalized * speed * Time.deltaTime);
        animator.SetFloat(velocity_hash, character.velocity.magnitude);
    }

    void HandleAttack()
    {
        if(is_damaged) return;

        if(Input.GetButtonDown("Fire1") && can_shoot)
        {
            RaycastHit hit;
            var ray = Physics.Raycast(shoot_point.position, shoot_point.forward, out hit, shoot_distance, target_layer);

            if(ray)
            {
                //Debug.DrawRay(shoot_point.position, shoot_point.forward * hit.distance, Color.cyan, 2f);
                var amonguys = hit.transform.gameObject.GetComponent<IDamagable>();
                amonguys?.TakeDamage(damage_amount);
                hit_particles_pooler.GetInstance(hit.point);
                shoot_time = Time.time + shoot_time_delay;
            }

            flash_particles.Play();
            audio_src_shoot.PlayOneShot(laser_audio);
        }
    }

    public void TakeDamage(int amount)
    {
        if(is_damaged || !is_alive) return;

        Health -= amount;

        if(Health <= 0)
        {
            is_alive = false;
            Debug.Log("Player Dead");
        }
        
        animator.SetTrigger(damage_hash);
        audio_src_effects.PlayOneShot(damage_audio);
        StartCoroutine(ChangeToDamageMaterials(hit_time_offset + 0.5f));

        time_hit_again = Time.time + hit_time_offset;
    }

    void ChangeMaterial(Material m, int index)
    {
        var materials = robi_renderer.materials;
        materials[index] = m;
        robi_renderer.materials = materials;
    }

    IEnumerator ChangeToDamageMaterials(float t)
    {
        ChangeMaterial(danger_mat, EYES_MATERIAL_INDEX);
        robi_light.color = danger_light_color;
        yield return new WaitForSeconds(t);
        ChangeMaterial(normal_mat, EYES_MATERIAL_INDEX);
        robi_light.color = normal_light_color;

    }

    /*void ThirPersonMovement(Vector3 direction)
    {

        if(direction.z >= 0.1f)
        {
            //x / z to offset the angle 90deg to the left
            float target_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 
            target_angle, ref turn_smooth_vel, turn_smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Forward Direction where target_angle should point
            Vector3 move_dir = Quaternion.Euler(0, target_angle, 0) * Vector3.forward;
            character.Move(move_dir.normalized * speed * Time.deltaTime); 
        }
    }*/
}
