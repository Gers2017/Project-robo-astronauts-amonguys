using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    public class Collectable : MonoBehaviour
    {
        public AudioSource audioSource;
        public event Action<Collectable> OnCollected;
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                OnCollected?.Invoke(this);
                audioSource.Play();
                GetComponent<BoxCollider>().enabled = false;
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                Invoke("Disable", audioSource.clip.length + 0.5f);
            }
        }
        float start_pos_y;
        private void Start() {
            start_pos_y = transform.position.y;
        }
        float max_y_offset = 0.5f;
        float offset_speed = 5f;

        void FixedUpdate()
        {
            float y_offset = Mathf.Sin(Time.time * offset_speed) * max_y_offset;
            var new_position = transform.position;
            new_position.y = start_pos_y + y_offset;
            transform.position = new_position;
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}