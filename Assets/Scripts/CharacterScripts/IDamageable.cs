using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);

    void HitStun(float duration);

    void Launch(int angle, float baseKnockback, float knockbackGrowth);

    void LaunchAndHitStun(int angle, float baseKnockback, float knockbackGrowth);

    void Freeze(int freezeFrameDuration);

    float GetDamage();
}
