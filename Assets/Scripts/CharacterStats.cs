using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterStats : MonoBehaviour
{
    Dictionary<Stat, List<IModifier>> modifiers;
    Dictionary<Stat, float> stats;

    public float MaxWalkSpeed {get {return GetStat(Stat.MaxWalkSpeed);} set {SetStat(Stat.MaxWalkSpeed, value);}}
    public float WalkAccelerationRate {get {return GetStat(Stat.WalkAccelerationRate);} set {SetStat(Stat.WalkAccelerationRate, value);}}
    public float GroundDecelerationRate {get {return GetStat(Stat.GroundDecelerationRate);} set {SetStat(Stat.GroundDecelerationRate, value);}}
    public float InitialDashSpeed {get {return GetStat(Stat.InitialDashSpeed);} set {SetStat(Stat.InitialDashSpeed, value);}}
    public float MaxDashSpeed {get {return GetStat(Stat.MaxDashSpeed);} set {SetStat(Stat.MaxDashSpeed, value);}}
    public float DashAccelerationRate {get {return GetStat(Stat.DashAccelerationRate);} set {SetStat(Stat.DashAccelerationRate, value);}}
    
    public float MaxAirSpeed {get {return GetStat(Stat.MaxAirSpeed);} set {SetStat(Stat.MaxAirSpeed, value);}}
    public float AirAccelerationRate {get {return GetStat(Stat.AirAccelerationRate);} set {SetStat(Stat.AirAccelerationRate, value);}}
    public float AirDecelerationRate {get {return GetStat(Stat.AirDecelerationRate);} set {SetStat(Stat.AirDecelerationRate, value);}}
    public float Gravity {get {return GetStat(Stat.Gravity);} set {SetStat(Stat.Gravity, value);}}
    public float MaxFallSpeed {get {return GetStat(Stat.MaxFallSpeed);} set {SetStat(Stat.MaxFallSpeed, value);}}
    public float MaxFastFallSpeed {get {return GetStat(Stat.MaxFastFallSpeed);} set {SetStat(Stat.MaxFastFallSpeed, value);}}
    
    public float ShortHopHeight {get {return GetStat(Stat.ShortHopHeight);} set {SetStat(Stat.ShortHopHeight, value);}}
    public float FullHopHeight {get {return GetStat(Stat.FullHopHeight);} set {SetStat(Stat.FullHopHeight, value);}}
    public float DoubleJumpHeight {get {return GetStat(Stat.DoubleJumpHeight);} set {SetStat(Stat.DoubleJumpHeight, value);}}

    public float Attack {get {return GetStat(Stat.Attack);} set {SetStat(Stat.Attack, value);}}
    public float Defense {get {return GetStat(Stat.Defense);} set {SetStat(Stat.Defense, value);}}
    public float Knockback {get {return GetStat(Stat.Knockback);} set {SetStat(Stat.Knockback, value);}}
    public float Resistance {get {return GetStat(Stat.Resistance);} set {SetStat(Stat.Resistance, value);}}
    
    // Start is called before the first frame update
    void Awake()
    {
        modifiers = new Dictionary<Stat, List<IModifier>>();
        stats = new Dictionary<Stat, float>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
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
