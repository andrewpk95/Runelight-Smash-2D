using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGameRule : BaseGameRule
{
    public const int RESPAWN_TIME = 60;
    
    public int time;
    protected Timer timer;

    public TimeGameRule(int seconds) {
        time = seconds * 60;
        timer = TimerManager.instance.StartTimer(time, OnTimeStart, OnTimeEnd, "Game Timer");
    }

    void OnTimeStart() {
        Debug.Log("Game Start!");
    }

    void OnTimeEnd() {
        Debug.Log("Time's Up!");
        EventManager.instance.InvokeOnGameOverEvent();
    }

    protected override void OnEntityDeath(GameObject entity) {
        base.OnEntityDeath(entity);
        System.Action OnRespawnTimerEnd = () => Respawn(entity);
        TimerManager.instance.StartTimer(RESPAWN_TIME, null, OnRespawnTimerEnd, "Respawn Timer");
    }
}
