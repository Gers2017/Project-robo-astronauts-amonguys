using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Amonguys : MonoBehaviour, IDamagable
{
    bool is_alive = true;
    int velocity, is_dead, attack;
    Transform target;
    int start_health = 60;
    int health = 60;
    int damage_amount = 10;
    float attack_distance = 2.5f;
    float search_time = 1.25f;

    Collider amonguys_collider;
    NavMeshAgent agent;
    AudioSource audio_source;
    LayerMask target_layer;
    [SerializeField] Animator animator;
    [Header("Audio clips")]
    [SerializeField] AudioClip[] spawn_audios;
    [SerializeField] AudioClip faint_audio;

    public int Max_health => start_health;
    public int Health { get => health; set => health = value; }

    //Get gameObject components in Awake
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        amonguys_collider = GetComponent<Collider>();
        audio_source = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if(player != null)
        {
            target = player.transform;
        }
        
        velocity = Animator.StringToHash("velocity");
        is_dead = Animator.StringToHash("is_dead");
        attack = Animator.StringToHash("attack");

        InvokeRepeating("SearchTarget", 0f, search_time);

        target_layer = LayerMask.NameToLayer("Player");

        Revive();
    }
    
    void LateUpdate()
    {
        if(is_alive)
        {
            animator.SetFloat(velocity, agent.velocity.magnitude);
            CheckDistance();
        }
    }

    private void SearchTarget()
    {
        if (target != null && is_alive)
        {
            agent.SetDestination(target.position);
        }
    }

    void CheckDistance()
    {
        if(target == null) return;
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance < attack_distance)
        {
            ExecuteAttack();
            animator.SetTrigger(attack);
        }
    }

    void ExecuteAttack()
    {
        var position = transform.position + transform.forward * 1.5f + Vector3.up * 1f;
        var size = Vector3.one * 1.5f;
        var colliders = Physics.OverlapBox(position, size, Quaternion.identity, target_layer);
        if(colliders == null) return;

        foreach (var collider in colliders)
        {
            PlayerController player;
            bool is_player = collider.TryGetComponent<PlayerController>(out player);
            if(is_player)
            {
                player.TakeDamage(damage_amount);
            }
        }
    }

    private void OnDrawGizmos() {
        var position = transform.position + transform.forward * 1.5f + Vector3.up * 1f;
        var size = Vector3.one * 1.5f;
        Gizmos.color = new Color(1,0,0,0.5f);;
        Gizmos.DrawCube(position,size);
    }

    public void Revive()
    {
        animator.SetBool(is_dead, false);
        agent.isStopped = false;
        amonguys_collider.enabled = true;
        health = start_health;
        is_alive = true;
        //Play random spawn audio
        audio_source.PlayOneShot(spawn_audios[Random.Range(0,spawn_audios.Length)]);
    }

    public void TakeDamage(int amount)
    {
        if(!is_alive) return;

        health -= amount;
        if(health <= 0)
        {
            //Stop the agent and disable the collider
            is_alive = false;
            agent.isStopped = true;
            amonguys_collider.enabled = false;

            //Set the parameters to the animator
            animator.SetBool(is_dead, true);
            float faint_time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            audio_source.PlayOneShot(faint_audio);
            StartCoroutine(OnAnimationEnd(faint_time));
        }
    }

    IEnumerator OnAnimationEnd(float t)
    {
        yield return new WaitForSeconds(t);
        ReturnToPool();
    }

    void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
    
}
