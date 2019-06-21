using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonDownSpecialMovementStatus : MovementStatus
{
    protected float ratio;
    protected float downSpecialFallSpeed;
    
    protected const int MOVEMENT_DURATION = 120;

    public RonDownSpecialMovementStatus(float fallSpeed, float movementRatio, int duration) : base(duration) {
        downSpecialFallSpeed = fallSpeed;
        ratio = movementRatio;
        targetVelocity = new Vector2(0.0f, -downSpecialFallSpeed) * ratio;
        frameDuration = MOVEMENT_DURATION;
        InitializeStatus();
    }

    public override void OnStatusEnter(GameObject entity) {
        base.OnStatusEnter(entity);
        character.CanGrabEdge = true;
    }

    public override void OnStatusStay(GameObject entity) {
        base.OnStatusStay(entity);
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        Debug.Log("Down Special Done");
        character.CanGrabEdge = false;
    }

    public override void OnStatusInterrupt(GameObject entity) {
        OnStatusExit(entity);
    }
}
