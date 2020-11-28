using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ObjectPooling;
using UnityEditor;

namespace Amonguys
{
    public class Amonguys : MonoBehaviour, IDamagable, IPoolObject
    {
        public static event Action OnAnyAmonguyDie;
        [SerializeField] protected AmonguysStats[] amonguys_stats;
        [SerializeField] float angleOfView = 45f;
        protected int start_health = 60;
        protected int health;
        protected int damage_amount = 10;
        [SerializeField] protected Transform attack_point;
        [SerializeField] protected Vector3 attack_size = Vector3.one;
        [SerializeField] protected float attack_distance = 3f;
        protected bool is_alive = true;

        protected Transform target;
        protected float distance_to_target;
        protected int velocity, is_dead, attack;
        protected NavMeshAgent agent;
        protected Collider amonguys_collider;
        protected AudioSource audio_source;
        protected LayerMask target_layer;
        protected Vector3 start_scale = Vector3.one;
        [SerializeField] protected Animator animator;

        [Header("Audio clips")]
        [SerializeField] protected AudioClip[] spawn_audios;
        [SerializeField] protected AudioClip damage_audio;
        [SerializeField] protected AudioClip faint_audio;
        [SerializeField] float min_pitch = 0.7f;
        [SerializeField] float max_pitch = 1.2f;

        [Header("Meshes")]
        [SerializeField] protected SkinnedMeshRenderer amonguy_renderer;


        //Get gameObject components in Awake
        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            amonguys_collider = GetComponent<Collider>();
            audio_source = GetComponent<AudioSource>();
            agent.stoppingDistance = attack_distance;
        }

        protected void Start()
        {
            PlayerController player = FindObjectOfType<PlayerController>();

            if (player != null)
            {
                target = player.transform;
            }

            velocity = Animator.StringToHash("velocity");
            is_dead = Animator.StringToHash("is_dead");
            attack = Animator.StringToHash("attack");
            target_layer = LayerMask.GetMask("Player");
        }

        public virtual void OnActivation()
        {
            SetUpAmonguy();
            animator.SetBool(is_dead, false);
            agent.isStopped = false;
            amonguys_collider.enabled = true;
            is_alive = true;
            transform.localScale = start_scale;
        }

        protected void SetUpAmonguy()
        {
            var stats = amonguys_stats[UnityEngine.Random.Range(0, amonguys_stats.Length)];

            start_health = stats.health;
            health = start_health;
            damage_amount = stats.damage;
            agent.speed = stats.speed;

            SetColor(stats.amonguy_color);

            InvokeRepeating("SearchTarget", 0f, stats.GetSearchTime());
            animator.SetFloat("m_speed", stats.GetAnimationSpeed());

            int audio_index = UnityEngine.Random.Range(0, spawn_audios.Length);
            PlaySound(spawn_audios[audio_index]);
        }

        public void PlaySound(AudioClip clip)
        {

            audio_source.pitch = UnityEngine.Random.Range(min_pitch, max_pitch);
            audio_source.PlayOneShot(clip);
        }

        protected void SetColor(Color color)
        {
            amonguy_renderer.materials[0].SetColor("_Color", color);
        }
        
        protected void SearchTarget()
        {
            if (target != null && is_alive)
            {
                agent.SetDestination(target.position);
            }
        }

        protected virtual void LateUpdate()
        {
            if(is_alive)
            {
                OnAmonguyAlive();
            }
            else
            {
                ScaleToDeath();
            }
        }

        protected virtual void OnAmonguyAlive()
        {
            SetDistanceToTarget();
            SetAnimatorVelocity();

            if(distance_to_target <= attack_distance)
            {
                ExecuteAttack();
                animator.SetTrigger(attack);
            }
        }

        protected void SetAnimatorVelocity()
        {
            animator.SetFloat(velocity, agent.velocity.magnitude);
        }

        protected void ScaleToDeath()
        {
            var scale = transform.localScale;

            if (scale.y > 0.1f)
            {
                float amount = Time.deltaTime * 0.5f;
                scale.y -= amount;
                scale.z += amount;
                scale.x += amount;
                transform.localScale = scale;

                var new_pos = transform.position;
                if (new_pos.y > -0.5f)
                {
                    new_pos.y -= amount;
                    transform.position = new_pos;
                }
            }
        }

        protected void SetDistanceToTarget()
        {
            if(target == null) return;
            distance_to_target = Vector3.Distance(transform.position, target.position);
        }

        protected virtual void ExecuteAttack()
        {
            float target_angle = GetTargetAngle();
            bool target_is_visible = IsTargetVisible(target_angle);

            if(!target_is_visible) return;

            var colliders = Physics.OverlapBox(attack_point.position, attack_size, Quaternion.identity, target_layer);
            if(colliders == null || colliders.Length <= 0) return;

            foreach (var collider in colliders)
            {
                IDamagable player;
                bool is_player = collider.TryGetComponent<IDamagable>(out player);
                if(is_player)
                {
                    player.TakeDamage(damage_amount);
                }
            }
        }


        #if UNITY_EDITOR
        protected void OnDrawGizmosSelected()
        {
            //Attack area
            Gizmos.color = new Color(0.7f, 0.7f, 0f, 0.8f);
            Gizmos.DrawWireSphere(transform.position + Vector3.up, attack_distance);

            if(attack_point != null)
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawCube(attack_point.position, attack_size);
            }

            if(target == null) return;

            Vector3 dir = target.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            float target_angle = GetTargetAngle();
            Color c = IsTargetVisible(target_angle) ? Color.green : Color.magenta;
            Gizmos.color = c;
            Handles.color = c;
            Gizmos.DrawLine(transform.position, target.position);
            Handles.DrawWireArc(transform.position, Vector2.up, dir, target_angle, 2f);
            Handles.DrawWireArc(transform.position, Vector2.up, dir, -target_angle, 2f);
        }
        
        #endif

        protected float GetTargetAngle()
        {
            Vector3 lookDir = transform.forward;
            Vector3 diff = (target.position - transform.position);
            Vector3 dirToPoint = new Vector3(diff.x, 0f, diff.z).normalized;

            float dot = Vector3.Dot(lookDir, dirToPoint);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            return angle;
        }

        protected bool IsTargetVisible(float angle)
        {
            return angle <= angleOfView;
        }

        public void TakeDamage(int amount)
        {
            if(!is_alive) return;

            health -= amount;

            //play damage sound
            if(!audio_source.isPlaying)
                PlaySound(damage_audio);

            if(health <= 0)
            {
                KillAmonguy();
            }
        }

        protected void KillAmonguy()
        {
            OnAnyAmonguyDie?.Invoke();
            //Stop the agent and disable the collider
            is_alive = false;
            agent.isStopped = true;
            amonguys_collider.enabled = false;
            CancelInvoke("SearchTarget");

            //Set the parameters to the animator
            animator.SetBool(is_dead, true);
            float faint_time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            PlaySound(faint_audio);
            StartCoroutine(OnAnimationEnd(faint_time + faint_audio.length * 0.5f));
        }

        IEnumerator OnAnimationEnd(float t)
        {
            yield return new WaitForSeconds(t);
            ReturnToPool();
        }
        protected void ReturnToPool()
        {
            gameObject.SetActive(false);
        }
    }
}