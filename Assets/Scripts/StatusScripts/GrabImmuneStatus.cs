using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabImmuneStatus : TimerStatus
{
    public GrabImmuneStatus(int duration) : base(duration) {
        frameDuration = duration;
    }

    public override void OnStatusEnter(GameObject entity) {
        base.OnStatusEnter(entity);
        character.CanBeGrabbed = false;
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        character.CanBeGrabbed = true;
    }
}
