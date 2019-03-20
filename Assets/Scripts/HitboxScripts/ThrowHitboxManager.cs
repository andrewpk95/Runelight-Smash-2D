using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHitboxManager : HitboxManager //Throw Hitbox Manager simply applies all hitboxes to the grabbed opponent and releases
{
    ICharacter ownerCharacter;

    public bool releaseOnHit;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();
        ownerCharacter = owner.GetComponent<ICharacter>();
    }

    void FixedUpdate() {
        
    }
    
    public override void ActivateHitboxes() {
        activated = true;
        GameObject victim = ownerCharacter.GetGrabbingFighter();
        if (victim == null) {
            //Debug.LogError("Error while throwing: No Grabbed Opponent!");
            return;
        }
        if (releaseOnHit) ownerCharacter.GrabRelease();
        foreach (IHitbox hitbox in list) {
            hitbox.OnHit(victim);
        }
    }
}
