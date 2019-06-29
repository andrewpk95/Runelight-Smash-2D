using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGameRule : BaseGameRule
{
    public const int RESPAWN_TIME = 60;
    
    public int time;
    protected Timer timer;
    public TimerUIBehavior timerUI;

    public TimeGameRule(int seconds) {
        time = seconds * 60;
    }

    public override void StartGame() {
        base.StartGame();
        timerUI = (TimerUIBehavior) GameObject.FindObjectOfType(typeof(TimerUIBehavior));
        timer = TimerManager.instance.StartTimer(time, OnTimeStart, OnTimeEnd, "Game Timer");
        timerUI.timer = timer;
    }

    public override void StopGame() {
        base.StopGame();
        TimerManager.instance.StopTimer(timer);
    }

    public override string ToString() {
        int frame = time;
        int minutes = frame / 3600;
        int seconds = (frame % 3600) / 60;
        return string.Format("Time {0}:{1:00}", minutes, seconds);
    }

    void OnTimeStart() {
        Debug.Log("Game Start!");
    }

    void OnTimeEnd() {
        Debug.Log("Time's Up!");
        EventManager.instance.InvokeOnGameOverEvent();
    }

    public override void OnEntityDeath(GameObject entity) {
        base.OnEntityDeath(entity);
        System.Action OnRespawnTimerEnd = () => Respawn(entity);
        TimerManager.instance.StartTimer(RESPAWN_TIME, null, OnRespawnTimerEnd, "Respawn Timer");
    }
}
