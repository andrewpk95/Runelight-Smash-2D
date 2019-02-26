using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SmashCalculator
{
    public static float HitStunDuration(float percentage, float baseKnockback, float KnockbackGrowth, int weight) {
        return LaunchStrength(percentage, baseKnockback, KnockbackGrowth, weight) * 0.04f;
    }

    public static Vector2 LaunchVector(int angle, float percentage, float baseKnockback, float KnockbackGrowth, int weight) {
        Vector2 dir;
        if (angle == 361) { //Sakurai Angle
            dir = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);
            return dir * LaunchStrength(percentage, baseKnockback, KnockbackGrowth, weight);
        }
        dir = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);
        return dir * LaunchStrength(percentage, baseKnockback, KnockbackGrowth, weight);
    }

    public static float LaunchStrength(float percentage, float baseKnockback, float KnockbackGrowth, int weight) {
        return (baseKnockback + percentage * KnockbackGrowth * 0.01f) * (200 / (100 + weight)) * 0.2f;
    }
}
