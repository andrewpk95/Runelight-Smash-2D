using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public string name;
    public int duration;
    
    public System.Action OnTimerStart;
    public System.Action OnTimerStop;
    
    public Timer(int frameDuration, string timerName, System.Action onTimerStart, System.Action onTimerStop) {
        name = timerName;
        duration = frameDuration;

        if (onTimerStart == null) OnTimerStart = Empty;
        else OnTimerStart = onTimerStart;

        if (onTimerStop == null) OnTimerStop = Empty;
        else OnTimerStop = onTimerStop;
    }

    void Empty() {
        //Empty Function
    }
}
