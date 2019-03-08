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
    [SerializeField] protected bool isIntangible;

    public int Weight {get {return weight;} set {weight = value;}}
    public float Percentage {get {return percentage;} set {percentage = value;}}
    public bool IsHitStunned {get {return isHitStunned;} set {isHitStunned = value;}}
    public bool IsLaunched {get {return isLaunched;} set {isLaunched = value;}}
    public bool IsFrozen {get {return isFrozen;} set {isFrozen = value;}}
    public bool IsInvulnerable {get {return isInvulnerable;} set {isInvulnerable = value;}}
    public bool IsIntangible {get {return isIntangible;} set {isIntangible = value;}}

    public Color intangibleColor1;
    public Color intangibleColor2;
    public float flashTick;

    protected int hitStunFrameLeft;
    protected int freezeFrameLeft;
    protected Vector2 storedVelocity;

    Animator animator;
    ICharacter character;
    HurtboxManager hurtbox;

    public EventManager eventManager;
    public StatusManager statusManager;
    protected IStatus launchStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<ICharacter>();
        hurtbox = GetComponentInChildren<HurtboxManager>();

        eventManager = (EventManager) GameObject.FindObjectOfType(typeof(EventManager));
        eventManager.StartListeningToOnHitEvent(new UnityAction<IHitbox, GameObject>(OnHit));

        statusManager = GetComponent<StatusManager>();
        launchStatus = new LaunchStatus();
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
        Tick();
    }

    void Tick() {
        if (IsFrozen) {
            UpdateFreezeFrame();
        }
        else {
            UpdateHitStunFrame();
        }
    }

    void UpdateFreezeFrame() {
		freezeFrameLeft--;
		if (freezeFrameLeft < 0){ //FreezeFrame Over
			character.EnableMovement();
			character.SetVelocity(storedVelocity);
			IsFrozen = false;
		}
    }

    void UpdateHitStunFrame() {
		if (IsHitStunned) {
			hitStunFrameLeft -= 1;
		}
		if (hitStunFrameLeft < 0) { //Hitstun Over
			IsHitStunned = false;
            if (IsLaunched) character.Tumble();
			IsLaunched = false;
			hitStunFrameLeft = 0;
			character.IgnoreInput(false);
			character.EnableAirDeceleration(true);
		}
    }

    public void OnHit(IHitbox hitbox, GameObject entity) {
        if (IsInvulnerable) return;
        if (entity.Equals(this.gameObject)) {
            TakeDamage(hitbox.Damage);
            eventManager.InvokeOnDamageEvent(hitbox, this);
            HitStun(hitbox);
            Launch(hitbox);
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
        eventManager.InvokeOnHitStunEvent(hitbox, this.gameObject);
        IsHitStunned = true;
        hitStunFrameLeft = SmashCalculator.HitStunDuration(hitbox, this);
        character.IgnoreInput(true);
        character.EnableAirDeceleration(false);
        character.SetPreventWalkOffLedge(false);
    }

    public void Launch(IHitbox hitbox) {
        Vector2 launchVector = SmashCalculator.LaunchVector(hitbox, this);
        IsLaunched = SmashCalculator.Tumble(hitbox, this);
        if (IsLaunched) statusManager.AddStatus(launchStatus);
        Debug.Log(launchVector);
        character.SetVelocity(launchVector);
    }

    public void Freeze(int freezeFrameDuration) {
        freezeFrameLeft = freezeFrameDuration;
        storedVelocity = character.GetVelocity();
        character.DisableMovement();
        IsFrozen = true;
    }

    public void SetIntangible() {
        IsIntangible = true;
        hurtbox.SetIntangible(true);
        hurtbox.StartFlashing(intangibleColor1, intangibleColor2, flashTick);
    }

    public void SetTangible() {
        IsIntangible = false;
        hurtbox.SetIntangible(false);
        hurtbox.StopFlashing();
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
