using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonUpSpecialMovementStatus : MovementStatus
{
    protected float ratio;
    protected float upSpecialHorizontalSpeed;
    protected float upSpecialVerticalAcceleration;
    protected float upSpecialDecelerationRate;
    
    protected const int START_DURATION = 6;
    protected const int MOVEMENT_DURATION = 14;
    protected const int END_DURATION = 10;

    protected const int EDGE_SNAP_FRAME = 15;
    protected const int EDGE_SNAP_DURATION = 15;
    protected bool snapToEdge;

    public RonUpSpecialMovementStatus(float horizontalSpeed, float verticalAcceleration, float movementRatio, int duration) : base(duration) {
        upSpecialHorizontalSpeed = horizontalSpeed;
        upSpecialVerticalAcceleration = verticalAcceleration;
        upSpecialDecelerationRate = 70.0f;
        ratio = movementRatio;
        frameDuration = START_DURATION + MOVEMENT_DURATION + END_DURATION;
        InitializeStatus();
    }

    public override void OnStatusStay(GameObject entity) {
        //Start of the up special
        if (durationLeft < START_DURATION) {
            targetVelocity = Vector2.zero;
        }
        //Movement of the up special
        else if (durationLeft < START_DURATION + MOVEMENT_DURATION) {
            targetVelocity = new Vector2(character.IsFacingRight ? upSpecialHorizontalSpeed * ratio : -upSpecialHorizontalSpeed * ratio, 
                                            character.GetTargetVelocity(targetVelocity.y, 50.0f, upSpecialVerticalAcceleration * ratio));
        }
        //End of the up special
        else if (durationLeft < frameDuration) {
            targetVelocity = new Vector2(character.GetTargetVelocity(character.Velocity.x, 0.0f, upSpecialDecelerationRate),
                                            character.GetTargetVelocity(character.Velocity.y, 0.0f, upSpecialDecelerationRate));
        }
        if (durationLeft == EDGE_SNAP_FRAME) {
            snapToEdge = true;
            character.CanGrabEdge = true;
        }
        if (durationLeft == EDGE_SNAP_FRAME + EDGE_SNAP_DURATION) {
            snapToEdge = false;
            character.CanGrabEdge = false;
        }
        base.OnStatusStay(entity);
    }

    public override void OnTimerOver(GameObject entity) {
        
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        Debug.Log("Up Special Done");
        character.Helpless();
        if (snapToEdge) character.CanGrabEdge = false;
        snapToEdge = false;
    }
}
