using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttackHitbox : AttackHitbox
{
    public bool releaseOnHit;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override List<GameObject> GetCollisionList() {
        GameObject victim = ownerCharacter.GetGrabbingFighter();
        Debug.Log(victim.name);
        List<GameObject> victims = new List<GameObject>();
        victims.Add(victim);
        
        return victims;
    }

    public override void OnHit(GameObject target) {
        hitboxContainer.DeactivateHitbox(this.GetName());
        if (releaseOnHit) ownerCharacter.GrabRelease();
        base.OnHit(target);
        Reset();
    }
}
