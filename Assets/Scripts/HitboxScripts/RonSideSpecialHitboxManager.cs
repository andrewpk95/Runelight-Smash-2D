using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonSideSpecialHitboxManager : HitboxManager
{
    public RonPassive passive;
    
    // Start is called before the first frame update
    void Start()
    {
        list = GetComponentsInChildren<IHitbox>();
        collisionDictionary = new Dictionary<GameObject, List<IHitbox>>();
        victims = new List<GameObject>();

        passive = GetComponentInParent<RonPassive>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activated) {
            CheckHitboxes();
        }
    }

    public override void ActivateHitboxes() {
        activated = true;
        float strengthMultiplier = 1.0f + 0.5f * passive.GetStaticCharge() / passive.maxStaticCharge;
        foreach(IHitbox hitbox in list) {
            //hitbox.SetDamage(hitbox.GetDamage() * strengthMultiplier);
        }
        CheckHitboxes();
    }
}
