using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Amonguys : MonoBehaviour
{
    bool is_alive = true;
    int velocity, is_dead, attack;
    Transform target;
    int start_health = 60;
    int health = 60;
    float attack_distance = 2.5f;
    float search_time = 1.25f;

    Collider amonguys_collider;
    NavMeshAgent agent;
    [SerializeField] Animator animator;
    
    //Get gameObject components in Awake
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        amonguys_collider = GetComponent<Collider>();
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
            animator.SetTrigger(attack);
        }
    }

    public void Revive()
    {
        animator.SetBool(is_dead, false);
        agent.isStopped = false;
        amonguys_collider.enabled = true;
        health = start_health;
        is_alive = true;
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
