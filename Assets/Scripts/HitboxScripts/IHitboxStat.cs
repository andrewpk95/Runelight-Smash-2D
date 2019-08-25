using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitboxStat
{
    float Damage {get; set;}
    bool HitStun {get; set;}
    int MinimumHitStunFrame {get; set;}
    int Angle {get; set;}
    float BaseKnockback {get; set;}
    float KnockbackGrowth {get; set;}
    int FreezeFrame {get; set;}
    bool FaceOwnerWhenHit {get; set;}
    float ShieldStunMultiplier {get; set;}
    bool WeightIndependentKnockback {get; set;}

    void AddModifier(IModifier modifier);

    void AddModifiers(List<IModifier> modifier);

    void RemoveModifier(IModifier modifier);

    void RemoveModifiers(List<IModifier> modifier);
}
