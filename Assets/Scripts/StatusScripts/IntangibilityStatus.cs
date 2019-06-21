using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntangibilityStatus : TimerStatus
{
    protected IDamageable damageable;
    
    public IntangibilityStatus(int duration) : base(duration) {
        frameDuration = duration;
    }

    public override void OnStatusEnter(GameObject entity) {
        base.OnStatusEnter(entity);
        damageable = entity.GetComponent<IDamageable>();
        damageable.SetIntangible();
    }

    public override void OnStatusExit(GameObject entity) {
        base.OnStatusExit(entity);
        damageable.SetTangible();
    }

    public override void OnStatusInterrupt(GameObject entity) {
        OnStatusExit(entity);
    }
}
