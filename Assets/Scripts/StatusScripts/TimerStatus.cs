using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStatus : BaseStatus
{
    protected int frameDuration;
    protected int durationLeft = 0;
    
    public TimerStatus(int duration) {
        frameDuration = duration;
    }

    public override void OnStatusEnter(GameObject entity) {
        base.OnStatusEnter(entity);
        durationLeft = 0;
    }

    public override void OnStatusStay(GameObject entity) {
        base.OnStatusStay(entity);
        durationLeft++;
        if (durationLeft >= frameDuration) {
            EndStatus = true;
            OnTimerOver(entity);
        }
    }

    public virtual void OnTimerOver(GameObject entity) {
        
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        durationLeft = 0;
    }

    public override void OnStatusInterrupt(GameObject entity) {
        OnStatusExit(entity);
    }
}
