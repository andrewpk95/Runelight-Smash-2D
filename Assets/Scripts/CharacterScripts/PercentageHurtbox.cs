using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PercentageHurtbox : MonoBehaviour, IDamageable
{
    [SerializeField] protected int weight;
    [SerializeField] protected float percentage;
    [SerializeField] protected bool isHitStunned;
    [SerializeField] protected bool isLaunched;
    [SerializeField] protected bool isFrozen;
    [SerializeField] protected bool isInvulnerable;

    public int Weight {get {return weight;} set {weight = value;}}
    public float Percentage {get {return percentage;} set {percentage = value;}}
    public bool IsHitStunned {get {return isHitStunned;} set {isHitStunned = value;}}
    public bool IsLaunched {get {return isLaunched;} set {isLaunched = value;}}
    public bool IsFrozen {get {return isFrozen;} set {isFrozen = value;}}
    public bool IsInvulnerable {get {return isInvulnerable;} set {isInvulnerable = value;}}

    protected float hitStunDurationLeft;
    protected int freezeFrameLeft;
    protected Vector2 storedVelocity;

    Animator animator;
    ICharacter character;

    public EventManager eventManager;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<ICharacter>();

        eventManager = (EventManager) GameObject.FindObjectOfType(typeof(EventManager));
        eventManager.StartListeningToOnDamageEvent(new UnityAction<IHitbox, IDamageable>(OnHit));
    }

    // Update is called once per frame
    void Update() 
    {
        animator.SetBool("isHitStunned", IsHitStunned);
        animator.SetBool("isLaunched", IsLaunched);
    }

    //Hitstun and Freeze Frame duration update
    void FixedUpdate()
    {
        if (IsFrozen) {
            freezeFrameLeft--;
            if (freezeFrameLeft < 0) { //FreezeFrame Over
                character.EnableMovement();
                character.SetVelocity(storedVelocity);
                IsFrozen = false;
            }
        }
        else {
            if (IsHitStunned) {
                hitStunDurationLeft -= Time.fixedDeltaTime;
            }
            if (hitStunDurationLeft < 0) { //Hitstun Over
                IsHitStunned = false;
                IsLaunched = false;
                hitStunDurationLeft = 0.0f;
                character.IgnoreInput(false);
                character.EnableAirDeceleration(true);
            }
        }
    }

    public void OnHit(IHitbox hitbox, IDamageable damageable) {
        if (IsInvulnerable) return;
        if (damageable.GetOwner().Equals(this.gameObject)) {
            TakeDamage(hitbox.Damage);
            Launch(hitbox);
            HitStun(hitbox);
            Freeze(hitbox.FreezeFrame);
        }
    }

    public void TakeDamage(float damage) {
        Percentage += damage;
    }

    public void Heal(float heal) {
        Percentage -= heal;
    }

    public GameObject GetOwner() {
        return this.gameObject;
    }

    public void HitStun(IHitbox hitbox) {
        if (!hitbox.HitStun) return;
        IsHitStunned = true;
        hitStunDurationLeft = SmashCalculator.HitStunDuration(hitbox, this);
        character.IgnoreInput(true);
        character.EnableAirDeceleration(false);
        character.SetPreventWalkOffLedge(false);
    }

    public void Launch(IHitbox hitbox) {
        Vector2 launchVector = SmashCalculator.LaunchVector(hitbox, this);
        IsLaunched = SmashCalculator.Tumble(hitbox, this);
        Debug.Log(launchVector);
        character.SetVelocity(launchVector);
    }

    public void Freeze(int freezeFrameDuration) {
        freezeFrameLeft = freezeFrameDuration;
        storedVelocity = character.GetVelocity();
        character.DisableMovement();
        IsFrozen = true;
    }
    /*
    public void HitStun(float duration) {
        IsHitStunned = true;
        hitStunDurationLeft = duration;
        character.IgnoreInput(true);
        character.EnableAirDeceleration(false);
    }

    public void Launch(int angle, float baseKnockback, float knockbackGrowth) {
        Vector2 launchVector = SmashCalculator.LaunchVector(angle, percentage, baseKnockback, knockbackGrowth, weight)
        Debug.Log(launchVector);
        character.SetVelocity(launchVector);
    }

    
    */

}
