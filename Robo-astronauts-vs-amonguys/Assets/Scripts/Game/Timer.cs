using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float time = 0;
    private float startTime;
    public bool autoStart = true;
    public bool loop = false;
    public UnityEvent OnTimeOut;

    void Awake()
    {
        startTime = time;
    }

    public void Play()
    {
        autoStart = true;
    }

    public void ResetTimer()
    {
        time = startTime;
        Play();
    }

    void Update()
    {
        if(!autoStart) return;
        UpdateTime();
    }

    void UpdateTime()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            InvokeOnTimeOut();
            if(loop) time = startTime;
            else autoStart = false;
        }
    }

    void InvokeOnTimeOut()
    {
        OnTimeOut.Invoke();
    }

}
