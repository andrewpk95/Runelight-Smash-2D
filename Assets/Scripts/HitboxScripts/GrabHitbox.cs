using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHitbox : BaseHitbox
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void OnHit(GameObject target) {
        eventManager.InvokeOnGrabEvent(this.owner, target);
    }

    void OnDisable() {
        Reset();
    }
}
