using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitboxStat : IHitboxStat
{
    Dictionary<Stat, List<IModifier>> modifiers;
    Dictionary<Stat, float> stats;
    
    [SerializeField] protected float damage;
    [SerializeField] protected bool hitStun;
    [SerializeField] protected int minimumHitStunFrame;
    [SerializeField] protected int angle;
    [SerializeField] protected float baseKnockback;
    [SerializeField] protected float knockbackGrowth;
    [SerializeField] protected int freezeFrame;
    [SerializeField] protected bool faceOwnerWhenHit;
    [SerializeField] protected float shieldStunMultiplier;
    [SerializeField] protected bool weightIndependentKnockback;
    
    public float Damage {get {return GetStat(Stat.Damage);} set {SetStat(Stat.Damage, value);}}
    public bool HitStun {get {return hitStun;} set {hitStun = value;}}
    public int MinimumHitStunFrame {get {return minimumHitStunFrame;} set {minimumHitStunFrame = value;}}
    public int Angle {get {return angle;} set {angle = value;}}
    public float BaseKnockback {get {return GetStat(Stat.BaseKnockback);} set {SetStat(Stat.BaseKnockback, value);}}
    public float KnockbackGrowth {get {return GetStat(Stat.KnockbackGrowth);} set {SetStat(Stat.KnockbackGrowth, value);}}
    public int FreezeFrame {get {return freezeFrame;} set {freezeFrame = value;}}
    public bool FaceOwnerWhenHit {get {return faceOwnerWhenHit;} set {faceOwnerWhenHit = value;}}
    public float ShieldStunMultiplier {get {return GetStat(Stat.ShieldStunMultiplier);} set {SetStat(Stat.ShieldStunMultiplier, value);}}
    public bool WeightIndependentKnockback {get {return weightIndependentKnockback;} set {weightIndependentKnockback = value;}}

    public HitboxStat() {
        modifiers = new Dictionary<Stat, List<IModifier>>();
        stats = new Dictionary<Stat, float>();
    }

    public float GetStat(Stat stat) {
        float result = stats[stat];
        if (!modifiers.ContainsKey(stat)) return result;
        foreach (IModifier modifier in modifiers[stat]) {
            result = modifier.Apply(result);
        }
        return result;
    }

    public void SetStat(Stat stat, float val) {
        if (stats.ContainsKey(stat)) {
            stats[stat] = val;
        }
        else {
            stats.Add(stat, val);
        }
    }

    public void AddModifier(IModifier modifier) {
        if (modifiers.ContainsKey(modifier.TargetStat)) {
            modifiers[modifier.TargetStat].Add(modifier);
        }
        else {
            List<IModifier> newList = new List<IModifier>();
            newList.Add(modifier);
            modifiers.Add(modifier.TargetStat, newList);
        }
        //Sort modifier list according to priority
        modifiers[modifier.TargetStat] = modifiers[modifier.TargetStat].OrderBy(x => x.Priority).ToList();
    }

    public void AddModifiers(List<IModifier> modifier) {
        foreach (IModifier mod in modifier) {
            AddModifier(mod);
        }
    }

    public void RemoveModifier(IModifier modifier) {
        modifiers[modifier.TargetStat].Remove(modifier);
    }

    public void RemoveModifiers(List<IModifier> modifier) {
        foreach (IModifier mod in modifier) {
            RemoveModifier(mod);
        }
    }
}
