using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : BaseHitbox, IAttackHitbox
{
    [SerializeField] protected float damage;
    [SerializeField] protected bool hitStun;
    [SerializeField] protected int minimumHitStunFrame;
    [SerializeField] protected int angle;
    [SerializeField] protected float baseKnockback;
    [SerializeField] protected float knockbackGrowth;
    [SerializeField] protected int freezeFrame;
    [SerializeField] protected bool faceOwnerWhenHit;
    [SerializeField] protected float shieldStunMultiplier;

    protected IHitboxStat stats;

    public IHitboxStat Stats {get {return stats;} set {stats = value;}}
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();
        Stats = new HitboxStat();
        InitializeStats();
    }

    protected virtual void InitializeStats() {
        Stats.Damage = damage;
        Stats.HitStun = hitStun;
        Stats.MinimumHitStunFrame = minimumHitStunFrame;
        Stats.Angle = angle;
        Stats.BaseKnockback = baseKnockback;
        Stats.KnockbackGrowth = knockbackGrowth;
        Stats.FreezeFrame = freezeFrame;
        Stats.FaceOwnerWhenHit = faceOwnerWhenHit;
        Stats.ShieldStunMultiplier = shieldStunMultiplier;
        if (shieldStunMultiplier == 0.0f) {
            Stats.ShieldStunMultiplier = 1.0f;
        }
    }

    public override void OnHit(GameObject target) {
        EventManager.instance.InvokeOnHitEvent(this, target);
        if (target.tag == "Shield") {
            Debug.Log("Shielded");
            ownerDamageable.Freeze(Stats.FreezeFrame);
        }
        else if (target.tag == "Entity") {
            ownerDamageable.Freeze(Stats.FreezeFrame);
        }
    }

    void OnDisable() {
        Reset();
    }
}
