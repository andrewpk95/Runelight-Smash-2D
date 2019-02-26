using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageHurtbox : MonoBehaviour, IDamageable
{
    public int weight;
    public float percentage;
    
    public bool isHitStunned;
    public float hitStunDurationLeft;

    public bool isFrozen;
    public int freezeFrameLeft;
    public Vector2 storedVelocity;

    Animator animator;
    Rigidbody2D rb;
    ICharacter character;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<ICharacter>();
    }

    // Update is called once per frame
    void Update() 
    {
        animator.SetBool("isHitStunned", isHitStunned);
    }

    //Hitstun and Freeze Frame duration update
    void FixedUpdate()
    {
       
        if (isFrozen) {
            freezeFrameLeft--;
            if (freezeFrameLeft < 0) {
                character.EnableMovement();
                rb.velocity = storedVelocity;
                isFrozen = false;
            }
        }
        else {
            if (isHitStunned) {
                hitStunDurationLeft -= Time.fixedDeltaTime;
            }
            if (hitStunDurationLeft < 0) {
                isHitStunned = false;
                hitStunDurationLeft = 0.0f;
                character.IgnoreInput(false);
                character.EnableAirDeceleration(true);
            }
        }
        
    }

    public void TakeDamage(float damage) {
        percentage += damage;
    }

    public void HitStun(float duration) {
        isHitStunned = true;
        hitStunDurationLeft = duration;
        character.IgnoreInput(true);
        character.EnableAirDeceleration(false);
    }

    public void Launch(int angle, float baseKnockback, float knockbackGrowth) {
        Debug.Log(SmashCalculator.LaunchVector(angle, percentage, baseKnockback, knockbackGrowth, weight));
        rb.velocity = SmashCalculator.LaunchVector(angle, percentage, baseKnockback, knockbackGrowth, weight);
    }

    public void LaunchAndHitStun(int angle, float baseKnockback, float knockbackGrowth) {
        Debug.Log(SmashCalculator.LaunchVector(angle, percentage, baseKnockback, knockbackGrowth, weight));
        rb.velocity = SmashCalculator.LaunchVector(angle, percentage, baseKnockback, knockbackGrowth, weight);
        HitStun(SmashCalculator.HitStunDuration(percentage, baseKnockback, knockbackGrowth, weight));
    }

    public void Freeze(int freezeFrameDuration) {
        freezeFrameLeft = freezeFrameDuration;
        storedVelocity = rb.velocity;
        character.DisableMovement();
        isFrozen = true;
    }

    public float GetDamage() {
        return percentage;
    }

}
