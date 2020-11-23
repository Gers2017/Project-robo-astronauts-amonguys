
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            transform.GetChild(0).gameObject.SetActive(false);
            Invoke("Disable", 0.3f);
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
