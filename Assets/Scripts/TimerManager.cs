﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    Dictionary<Timer, Coroutine> TimerDictionary;

    public static TimerManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        //Singleton Pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);    
        
        DontDestroyOnLoad(this.gameObject);
        
        Initialize();
    }

    void Initialize() {
        TimerDictionary = new Dictionary<Timer, Coroutine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Timer StartTimer(int duration, System.Action OnTimerStart, System.Action OnTimerStop, string timerName) {
        Timer newTimer = new Timer(duration, timerName, OnTimerStart, OnTimerStop);
        Coroutine newCoroutine = StartCoroutine(RunTimer(newTimer));
        TimerDictionary.Add(newTimer, newCoroutine);
        return newTimer;
    }

    public Timer StartTimer(Timer newTimer) {
        if (newTimer == null) return null;
        Coroutine newCoroutine = StartCoroutine(RunTimer(newTimer));
        TimerDictionary.Add(newTimer, newCoroutine);
        return newTimer;
    }

    public void StopTimer(Timer timer) {
        if (timer == null) return;
        Debug.Log(timer.name + " Stopped");
        Coroutine timerCoroutine;
        if (TimerDictionary.TryGetValue(timer, out timerCoroutine)) {
            StopCoroutine(timerCoroutine);
            TimerDictionary.Remove(timer);
        }
        //timer.OnTimerStop();
    }

    IEnumerator RunTimer(Timer timer) {
        Debug.Log("Timer Started: " + timer.name);
        timer.OnTimerStart();
        int durationLeft = 0;
        while (durationLeft < timer.duration) {
            //Debug.Log("Timer " + timer.name + " Counting: Frame " + durationLeft);
            durationLeft++;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Timer " + timer.name + " Counting Done!");
        timer.OnTimerStop();
        TimerDictionary.Remove(timer);
    }
}
