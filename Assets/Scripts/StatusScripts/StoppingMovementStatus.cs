using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingMovementStatus : MovementStatus
{
    protected float decelerationRate;
    
    public StoppingMovementStatus(float decelRate, int duration) : base(duration) {
        decelerationRate = decelRate;
        InitializeStatus();
    }

    public override void OnStatusStay(GameObject entity) {
        targetVelocity = new Vector2(character.GetTargetVelocity(character.Velocity.x, 0.0f, decelerationRate),
                                        character.GetTargetVelocity(character.Velocity.y, 0.0f, decelerationRate));
        base.OnStatusStay(entity);
    }
}
