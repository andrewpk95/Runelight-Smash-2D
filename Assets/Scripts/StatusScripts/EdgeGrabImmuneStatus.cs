using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeGrabImmuneStatus : TimerStatus
{
    public EdgeGrabImmuneStatus(int duration) : base(duration) {
        frameDuration = duration;
    }

    public override void OnStatusEnter(GameObject entity) {
        base.OnStatusEnter(entity);
        character.CanGrabEdge = false;
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        character.CanGrabEdge = true;
    }
}
