using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonSideSpecialHitbox : MultihitHitbox
{
    public RonPassive passive;
    protected IModifier modifier;

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
        modifier = new MultiplyModifier(Stat.Damage, 1.0f);
        Stats.AddModifier(modifier);
    }

    public override void OnHit(GameObject target) {
        float strengthMultiplier = 1.0f + 0.5f * passive.GetStaticCharge() / passive.maxStaticCharge;
        modifier.ModifyValue = strengthMultiplier;
        base.OnHit(target);
    }
}
