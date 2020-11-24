using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
using CustomUI;
using Cinemachine;
using GameManagement;

public class PlayerController : MonoBehaviour, IDamagable
{
    const int EYES_MATERIAL_INDEX = 2;
    const float GRAVITY = -9.8f;

    CharacterController character;
    Animator animator;
    bool is_alive = true;

    [Header("Movement")]
    [SerializeField] float normal_speed = 12f;
    [SerializeField] float shooting_speed = 6f;
    float character_speed;
    [SerializeField] float move_turn_speed = 120f;
    [SerializeField] float shoot_turn_speed = 100f;
    float turn_speed;

    [Header("Health")]
    [SerializeField] int max_health = 100;
    [SerializeField] SlideBar health_bar;
    int health;
    public int Health { get => health; set => SetHealth(value); }
    public event Action<float> OnHpPercentage;
    public static event Action<PlayerController> OnPlayerDie;
    float time_to_damage = 0;
    float time_damage_again = 1.5f;
    public bool is_damaged  => Time.time < time_to_damage;

    [Header("Shooting")]
    [SerializeField] int damage_amount = 10;
    [SerializeField] float shoot_distance = 100f;
    [SerializeField] float time_shoot_again = 0.1f;
    float shoot_time;
    public bool can_shoot => shoot_time < Time.time;
    [SerializeField] LayerMask target_layer;
    [SerializeField] ParticleSystem flash_particles;
    ObjectPooler hit_particles_pooler;
    float mouse_sensitivity;

    private Camera main_camera;
    [SerializeField] CinemachineVirtualCamera CM_movement;
    [SerializeField] CinemachineVirtualCamera CM_shooting;
    CinemachineImpulseSource impulse_source;

    int velocity_hash, damage_hash;

    [Header("Audio")]
    [SerializeField] AudioSource audio_src_shoot;
    [SerializeField] AudioSource audio_src_effects;

    [Header("Audio clips")]
    [SerializeField] AudioClip damage_audio;
    [SerializeField] AudioClip laser_audio;

    [Header("Mesh and materials")]
    [SerializeField] SkinnedMeshRenderer robi_renderer;
    [SerializeField] Light robi_light;
    [SerializeField] Material normal_mat, danger_mat;
    [SerializeField] Color normal_light_color = Color.green, danger_light_color = Color.red;

    void Awake()
    {
        character = GetComponent<CharacterController>();
        hit_particles_pooler = GetComponent<ObjectPooler>();
        impulse_source = GetComponent<CinemachineImpulseSource>();
        mouse_sensitivity = GameSettings.GetMouseSensitivity();
    }

    void Start()
    {
        SetUpSpeed();
        SetupAnimator();
        SetupHealth();
        GetMainCamera();
    }

    private void SetUpSpeed()
    {
        character_speed = normal_speed;
        turn_speed = move_turn_speed;
    }

    void GetMainCamera()
    {
        main_camera = Camera.main;
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
                SetPlayerMode(true);
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                SetPlayerMode(false);
            }
            
            Movement();
            HandleAttack();
        }
        else
        {
            DeactivatePlayer();
        }

        if(!character.isGrounded)
            character.Move(Vector3.up * GRAVITY * Time.deltaTime);
    }

    private void SetPlayerMode(bool is_shoot_mode)
    {
        CM_movement.gameObject.SetActive(!is_shoot_mode);
        CM_shooting.gameObject.SetActive(is_shoot_mode);
        character_speed *= is_shoot_mode ? 0.5f : 2f;
        turn_speed = is_shoot_mode ? shoot_turn_speed : move_turn_speed;
    }

    private void Movement()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        float mouse_x = Input.GetAxis("Mouse X") * mouse_sensitivity;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var input = new Vector3(horizontal, 0f, vertical);

        if(Mathf.Abs(mouse_x) > 0f)
        {
            float y_angle = mouse_x * turn_speed;
            float smooth = Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + y_angle, 2f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, smooth, 0);
        }

        if(input.magnitude > 0)
        {
            var move_dir = transform.forward * input.z + transform.right * input.x;
            character.Move(move_dir.normalized * Time.deltaTime * character_speed);
        }

        animator.SetFloat(velocity_hash, input.magnitude);
    }

    void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 0, max_health);
        OnHpPercentage?.Invoke((float)health / max_health);
    }

    void HandleAttack()
    {
        if(is_damaged) return;

        if(Input.GetButtonDown("Fire1") && can_shoot)
        {           
            //Viewport goes from 0 to 1 on x and y
            //So Vector3.one * 0.5 is the center
            var cam_ray = main_camera.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hitInfo;
            if(Physics.Raycast(cam_ray, out hitInfo, 100, target_layer))
            {
                //Debug.DrawRay(cam_ray.origin, cam_ray.direction * shoot_distance, Color.cyan, 5f); 
                hit_particles_pooler.GetInstance(hitInfo.point);
                shoot_time = Time.time + time_shoot_again;

                var amonguys = hitInfo.collider.transform.gameObject.GetComponent<IDamagable>();
                amonguys?.TakeDamage(damage_amount);
            }

            flash_particles.Play();
            audio_src_shoot.PlayOneShot(laser_audio);
            GenerateImpulse();
        }
    }

    public void TakeDamage(int amount)
    {
        if (is_damaged || !is_alive) return;

        Health -= amount;

        animator.SetTrigger(damage_hash);
        audio_src_effects.PlayOneShot(damage_audio);
        GenerateImpulse();
        StartCoroutine(ChangeToDamageMaterials(time_damage_again + 0.5f));

        time_to_damage = Time.time + time_damage_again;

        if (Health <= 0)
        {
            start_rotation = transform.rotation;
            is_alive = false;
            OnPlayerDie.Invoke(this);
        }
    }

    private void GenerateImpulse()
    {
        impulse_source.GenerateImpulse(main_camera.transform.forward);
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

    private Quaternion start_rotation;
    private Quaternion new_rotation = Quaternion.Euler(90,0,-90);
    void DeactivatePlayer()
    {
        transform.rotation = Quaternion.Slerp(start_rotation, new_rotation, Time.deltaTime * 7f);
    }
}
