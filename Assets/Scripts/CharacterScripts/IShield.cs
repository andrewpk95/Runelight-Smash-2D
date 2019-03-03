using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShield
{
    float MaxShieldHealth {get; set;}
    float CurrentShieldHealth {get; set;}
    float ShieldHealthDegenRate {get; set;}
    float ShieldHealthRegenRate {get; set;}
    bool IsActive {get; set;}
    bool IsShieldStunned {get; set;}
    bool IsFrozen {get; set;}
    bool IsInvulnerable {get; set;}
    
    void ActivateShield();

    void DeactivateShield();
    
    void OnHit(IHitbox hitbox, GameObject damageable);

    void TakeDamage(float damage);

    void Heal(float heal);

    GameObject GetOwner();

    void ShieldStun(IHitbox hitbox);

    void Push(IHitbox hitbox);

    void Freeze(int freezeFrameDuration);

    void OnShieldBreak();

    void OnShieldBreakStart();

    void OnShieldBreakOver();
}
