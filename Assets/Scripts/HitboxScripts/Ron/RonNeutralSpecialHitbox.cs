using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonNeutralSpecialHitbox : MultihitHitbox
{
    public RonPassive passive;

    public int numberOfHits;
    public int hitsLeft;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Tick();
    }

    protected override void Initialize() {
        base.Initialize();
        passive = GetComponentInParent<RonPassive>();
    }

    protected override void UpdateOtherBehaviour() {
        //Clear victim list every cycle
        if (hitsLeft <= 0) return;
        if (hitbox.enabled) {
            hitIntervalFrameLeft--;
            if (hitIntervalFrameLeft <= 0) {
                hitsLeft--;
                collisions.Clear();
                hitboxGroup.Victims.Clear();
                hitIntervalFrameLeft = multihitIntervalFrame;
            }
        }
    }

    public override void OnHit(GameObject target) {
        base.OnHit(target);
    }

    public override void Enable() {
        numberOfHits = passive.ConsumeCharge();
        if (numberOfHits >= passive.maxStaticCharge) {
            Stats.HitStun = true;
        }
        hitsLeft = numberOfHits;
    }

    public override void Reset() {
        base.Reset();
        
    }
}
