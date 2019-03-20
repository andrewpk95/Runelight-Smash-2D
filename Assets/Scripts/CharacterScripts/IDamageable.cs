using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable : IFreezable
{
    int Weight {get; set;}
    float Percentage {get; set;}
    bool IsHitStunned {get; set;}
    bool IsInvulnerable {get; set;}
    bool IsIntangible {get; set;}
    
    void OnHit(IAttackHitbox hitbox, GameObject entity);

    void TakeDamage(float damage);

    void Heal(float heal);

    GameObject GetOwner();

    void HitStun(IAttackHitbox hitbox);

    void Launch(IAttackHitbox hitbox);

    void SetIntangible();

    void SetTangible();
}
