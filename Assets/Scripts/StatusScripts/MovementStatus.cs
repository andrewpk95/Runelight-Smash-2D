using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStatus : TimerStatus
{
    protected Vector2 targetVelocity;

    public MovementStatus(Vector2 velocity, int duration) : base(duration) {
        targetVelocity = velocity;
        InitializeStatus(); 
    }

    public MovementStatus(int duration) : base(duration) {
        targetVelocity = Vector2.zero;
        InitializeStatus();
    }

    public override void OnStatusStay(GameObject entity) {
        character.OverrideVelocity(targetVelocity);
        base.OnStatusStay(entity);
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        character.StopOverride();
    }
}
