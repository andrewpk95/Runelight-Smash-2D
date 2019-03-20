using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShield : IFreezable
{
    float MaxShieldHealth {get; set;}
    float CurrentShieldHealth {get; set;}
    float ShieldHealthDegenRate {get; set;}
    float ShieldHealthRegenRate {get; set;}
    bool IsActive {get; set;}
    bool IsShieldStunned {get; set;}
    bool IsInvulnerable {get; set;}
    
    void ActivateShield();

    void DeactivateShield();
    
    void OnHit(IAttackHitbox hitbox, GameObject damageable);

    void TakeDamage(float damage);

    void Heal(float heal);

    GameObject GetOwner();

    void ShieldStun(IAttackHitbox hitbox);

    void Push(IAttackHitbox hitbox);

    void OnShieldBreak();

    void OnShieldBreakStart();

    void OnShieldBreakOver();
}
