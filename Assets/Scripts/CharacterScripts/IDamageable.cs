using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int Weight {get; set;}
    float Percentage {get; set;}
    bool IsHitStunned {get; set;}
    bool IsFrozen {get; set;}
    bool IsInvulnerable {get; set;}
    
    void OnHit(IHitbox hitbox, IDamageable damageable);

    void TakeDamage(float damage);

    void Heal(float heal);

    GameObject GetOwner();

    void HitStun(IHitbox hitbox);

    void Launch(IHitbox hitbox);

    void Freeze(int freezeFrameDuration);
}
