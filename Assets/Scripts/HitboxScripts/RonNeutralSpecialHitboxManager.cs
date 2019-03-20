using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonNeutralSpecialHitboxManager : HitboxManager
{
    public int numberOfHits;
    int hitsLeft;

    public RonPassive passive;
    
    // Start is called before the first frame update
    void Start()
    {
        list = GetComponentsInChildren<IAttackHitbox>();
        collisionDictionary = new Dictionary<GameObject, List<IHitbox>>();
        victims = new List<GameObject>();

        passive = GetComponentInParent<RonPassive>();
    }

    void FixedUpdate() {
        if (activated) {
            hitsLeft--;
            CheckHitboxes();
            DeactivateHitboxes();
            if (hitsLeft <= 0) {
                activated = false;
            }
        }
    }

    public override void ActivateHitboxes() {
        activated = true;
        numberOfHits = passive.ConsumeCharge();
        if (numberOfHits >= passive.maxStaticCharge) {
            foreach(IAttackHitbox hitbox in list) {
                hitbox.Stats.HitStun = true;
            }
        }
        hitsLeft = numberOfHits;
    }

    public override void DeactivateHitboxes() {
        collisionDictionary.Clear();
        victims.Clear();
        foreach(IAttackHitbox hitbox in list) {
            hitbox.Stats.HitStun = false;
            hitbox.Reset();
        }
    }
}
