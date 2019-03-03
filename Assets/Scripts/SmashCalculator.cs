using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SmashCalculator
{
    //Knockback Ratios
    public const float KNOCKBACK_TO_UNIT_RATIO = 0.1f;
    public const float KNOCKBACK_TO_HITSTUN_RATIO = 2.0f;
    public const float KNOCKBACK_TO_SHIELDSTUN_RATIO = 4.0f;
    public const float MINIMUM_KNOCKBACK_TO_TUMBLE = 80.0f;
    
    //Sakurai Angle Constants
    public const float LOW_KNOCKBACK_VALUE = 60f;
    public const float HIGH_KNOCKBACK_VALUE = 88f;
    public const int MAXIMUM_SAKURAI_ANGLE = 45;
    
    public static int HitStunDuration(IHitbox hitbox, IDamageable damageable) {
        return Mathf.RoundToInt(LaunchStrength(hitbox, damageable) * KNOCKBACK_TO_HITSTUN_RATIO);
    }

    public static Vector2 LaunchVector(IHitbox hitbox, IDamageable damageable) {
        Vector2 dir;
        float knockback = KnockbackValue(hitbox, damageable);
        if (hitbox.Angle == 361) { //Sakurai Angle
            float sakuraiAngle;
            if (knockback <= LOW_KNOCKBACK_VALUE) { //Does not lift off victim when knockback is low
                sakuraiAngle = 0;
            }
            else if (knockback >= HIGH_KNOCKBACK_VALUE) { //Launch at maximum sakurai angle if knockback is high
                sakuraiAngle = MAXIMUM_SAKURAI_ANGLE;
            }
            else { //Launch at angle between 0 and MAXIMUM_SAKURAI_ANGLE
                sakuraiAngle = (knockback - LOW_KNOCKBACK_VALUE) * MAXIMUM_SAKURAI_ANGLE / (HIGH_KNOCKBACK_VALUE - LOW_KNOCKBACK_VALUE);
            }
            //Flip Angle based on the hitbox owner position and victim position
            sakuraiAngle = FlipAngle(sakuraiAngle, hitbox.GetOwner(), damageable.GetOwner());
            dir = (Vector2)(Quaternion.Euler(0, 0, sakuraiAngle) * Vector2.right);
        }
        else if (hitbox.Angle == 366) { //Autolink Angle that pulls opponents into the hitbox
            float autolinkAngle;
            Vector2 attackerVelocity = hitbox.GetOwner().GetComponent<ICharacter>().GetVelocity();
            dir = attackerVelocity;
            return dir;
        }
        else {
            float launchAngle = hitbox.Angle;
            launchAngle = FlipAngle(launchAngle, hitbox.GetOwner(), damageable.GetOwner());
            dir = (Vector2)(Quaternion.Euler(0, 0, launchAngle) * Vector2.right);
        }
        
        return dir * LaunchStrength(knockback);
    }

    public static float LaunchStrength(IHitbox hitbox, IDamageable damageable) {
        return KnockbackValue(hitbox, damageable) * KNOCKBACK_TO_UNIT_RATIO;
    }

    public static float LaunchStrength(float knockbackValue) {
        return knockbackValue * KNOCKBACK_TO_UNIT_RATIO;
    }

    public static float KnockbackValue(IHitbox hitbox, IDamageable damageable) {
        return (((damageable.Percentage / 10.0f + damageable.Percentage * hitbox.Damage / 20) * (200 / (100 + damageable.Weight)) * 1.4f) + 18.0f) * hitbox.KnockbackGrowth * 0.01f + hitbox.BaseKnockback;
    }

    public static bool Tumble(IHitbox hitbox, IDamageable damageable) {
        return hitbox.HitStun && KnockbackValue(hitbox, damageable) >= MINIMUM_KNOCKBACK_TO_TUMBLE;
    }

    public static float FlipAngle(float angle, GameObject attacker, GameObject victim) {
        if (victim.transform.position.x - attacker.transform.position.x >= 0) { //Victim is on the right side of the attacker
            return angle;
        }
        else { //Victim is on the left side of the attacker
            return 180 - angle;
        }
    }

    public static int ShieldStunFrame(IHitbox hitbox, IShield shield) {
        return Mathf.RoundToInt(hitbox.Damage * 0.8f * hitbox.ShieldStunMultiplier) + 2;
    }

    public static Vector2 ShieldKnockbackVector(IHitbox hitbox, IShield shield) {
        float pushAngle = FlipAngle(0.0f, hitbox.GetOwner(), shield.GetOwner());
        Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, pushAngle) * Vector2.right);
        return dir * ShieldKnockbackStrength(hitbox, shield);
    }

    public static float ShieldKnockbackStrength(IHitbox hitbox, IShield shield) {
        return ShieldKnockbackValue(hitbox, shield) * KNOCKBACK_TO_UNIT_RATIO;
    }

    public static float ShieldKnockbackValue(IHitbox hitbox, IShield shield) {
        return (((hitbox.Damage / 10.0f + hitbox.Damage * hitbox.Damage / 20) * 1.4f) + 18.0f) * hitbox.KnockbackGrowth * 0.01f + hitbox.BaseKnockback;
    }
    /* 
    public static Vector2 PredictPositionAfterLaunch(IHitbox hitbox, IDamageable damageable) {
        Vector3 startPosition = damageable.GetOwner().transform.position;
        float launchStrength = LaunchStrength(hitbox, damageable);
    }
    */
}
