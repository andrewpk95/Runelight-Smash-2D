using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldHurtbox : MonoBehaviour, IShield
{
    [SerializeField] protected float maxShieldHealth;
    [SerializeField] protected float currentShieldHealth;
    [SerializeField] protected float shieldHealthDegenRate;
    [SerializeField] protected float shieldHealthRegenRate;
    [SerializeField] protected bool isActive;
    [SerializeField] protected bool isShieldStunned;
    [SerializeField] protected bool isShieldBroken;
    [SerializeField] protected bool isFrozen;
    [SerializeField] protected bool isInvulnerable;
    public GameObject owner;
    public IDamageable damageable;
    
    public float MaxShieldHealth {get {return maxShieldHealth;} set {maxShieldHealth = value;}}
    public float CurrentShieldHealth {get {return currentShieldHealth;} set {currentShieldHealth = value;}}
    public float ShieldHealthDegenRate {get {return shieldHealthDegenRate;} set {shieldHealthDegenRate = value;}}
    public float ShieldHealthRegenRate {get {return shieldHealthRegenRate;} set {shieldHealthRegenRate = value;}}
    public bool IsActive {get {return isActive;} set{isActive = value;}}
    public bool IsShieldStunned {get {return isShieldStunned;} set {isShieldStunned = value;}}
    public bool IsShieldBroken {get {return isShieldBroken;} set {isShieldBroken = value;}}
    public bool IsFrozen {get {return isFrozen;} set {isFrozen = value;}}
    public bool IsInvulnerable {get {return isInvulnerable;} set {isInvulnerable = value;}}

    protected int shieldStunFrameLeft;
    public int shieldBreakFrameLeft;
    protected int freezeFrameLeft;
    protected Vector2 storedVelocity;

    Animator animator;
    ICharacter character;
    SpriteRenderer sprite;
    Collider2D col;
    Transform trans;

    public EventManager eventManager;
    public StatusManager statusManager;
    protected IStatus shieldBreakStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentShieldHealth = MaxShieldHealth;
        owner = this.gameObject.transform.root.gameObject;
        damageable = owner.GetComponent<IDamageable>();

        animator = GetComponentInParent<Animator>();
        character = GetComponentInParent<ICharacter>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        trans = GetComponent<Transform>();

        eventManager = (EventManager) GameObject.FindObjectOfType(typeof(EventManager));
        eventManager.StartListeningToOnHitEvent(new UnityAction<IHitbox, GameObject>(OnHit));
        eventManager.StartListeningToOnHitStunEvent(new UnityAction<IHitbox, GameObject>(OnHitStun));

        statusManager = GetComponentInParent<StatusManager>();
        shieldBreakStatus = new ShieldBreakStatus();
    }
    
    void Update() {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Tick();
    }

    void Tick() {
        UpdateShieldSize();
        if (IsFrozen) {
            UpdateFreezeFrame();
        }
        else {
            UpdateShieldBreakFrame();
            UpdateShieldStunFrame();
            UpdateShieldHealth();
        }
    }

    void UpdateFreezeFrame() {
        if (IsFrozen) freezeFrameLeft--;
		if (freezeFrameLeft < 0){ //FreezeFrame Over
            IsFrozen = false;
            freezeFrameLeft = 0;
			character.EnableMovement();
			character.SetVelocity(storedVelocity);
		}
    }

    void UpdateShieldBreakFrame() {
        if (IsShieldBroken) shieldBreakFrameLeft--;
        if (shieldBreakFrameLeft < 0) { //Shield Break Over
            IsShieldBroken = false;
            shieldBreakFrameLeft = 0;
            animator.SetTrigger("ShieldBreakOver");
        }
    }

    void UpdateShieldStunFrame() {
		if (IsShieldStunned) shieldStunFrameLeft--;
		if (shieldStunFrameLeft < 0) { //Hitstun Over
			IsShieldStunned = false;
			shieldStunFrameLeft = 0;
			character.IgnoreInput(false);
		}
    }

    void UpdateShieldHealth() {
        if (isShieldBroken) return;
        if (IsActive) { //Degen shield health
            if (!IsShieldStunned) {
                CurrentShieldHealth = Mathf.Max(CurrentShieldHealth - ShieldHealthDegenRate * Time.fixedDeltaTime, 0.0f);
            }
            if (CurrentShieldHealth <= 0) {
                OnShieldBreak();
            }
        }
        else { //Regen shield health
            CurrentShieldHealth = Mathf.Min(CurrentShieldHealth + ShieldHealthRegenRate * Time.fixedDeltaTime, MaxShieldHealth);
        }
    }

    void UpdateShieldSize() {
        float radius = Mathf.Clamp(Mathf.Sqrt(CurrentShieldHealth / MaxShieldHealth), 0.1f, 1.0f);
        trans.localScale = new Vector3(radius, radius, 1);
    }

    public void ActivateShield() {
        isActive = true;
        character.IgnoreInput(true);
        sprite.enabled = true;
        col.enabled = true;
    }

    public void DeactivateShield() {
        isActive = false;
        character.IgnoreInput(false);
        sprite.enabled = false;
        col.enabled = false;
    }
    
    public void OnHit(IHitbox hitbox, GameObject entity) {
        if (IsInvulnerable) return;
        if (entity.Equals(this.gameObject)) {
            TakeDamage(hitbox.Damage);
            Push(hitbox);
            ShieldStun(hitbox);
            Freeze(hitbox.FreezeFrame);
        }
    }

    public void OnHitStun(IHitbox hitbox, GameObject entity) {
        if (entity.Equals(owner)) {
            if (IsActive) DeactivateShield();
            if (IsShieldBroken) OnShieldBreakOver();
        }
    }

    public void TakeDamage(float damage) {
        CurrentShieldHealth = Mathf.Max(CurrentShieldHealth - damage, 0.0f);
    }

    public void Heal(float heal) {
        CurrentShieldHealth = Mathf.Min(CurrentShieldHealth + heal, MaxShieldHealth);
    }

    public GameObject GetOwner() {
        return owner;
    }

    public void ShieldStun(IHitbox hitbox) {
        if (!hitbox.HitStun) return;
        IsShieldStunned = true;
        character.IgnoreInput(true);
        shieldStunFrameLeft = SmashCalculator.ShieldStunFrame(hitbox, this);
    }

    public void Push(IHitbox hitbox) {
        Vector2 launchVector = SmashCalculator.ShieldKnockbackVector(hitbox, this);
        Debug.Log(launchVector);
        character.SetVelocity(launchVector);
    }

    public void Freeze(int freezeFrameDuration) {
        freezeFrameLeft = freezeFrameDuration;
        storedVelocity = character.GetVelocity();
        character.DisableMovement();
        IsFrozen = true;
    }

    public void OnShieldBreak() {
        Debug.Log(owner.name + "'s Shield Broken!");
        IsShieldStunned = false;
        DeactivateShield();
        statusManager.AddStatus(shieldBreakStatus);
        character.IgnoreInput(true);
        animator.SetTrigger("ShieldBreak");
        Vector2 launchVector = new Vector2(0.0f, 5.0f);
        character.SetVelocity(launchVector);
        damageable.SetIntangible();
        Freeze(15);
    }

    public void OnShieldBreakStart() {
        Debug.Log("Shield Break Counter Start");
        damageable.SetTangible();
        IsShieldBroken = true;
        shieldBreakFrameLeft = 150; //damageable.Percentage
    }

    public void OnShieldBreakOver() {
        Debug.Log("Shield Break Over");
        IsShieldBroken = false;
        shieldBreakFrameLeft = 0;
        statusManager.RemoveStatus(shieldBreakStatus);
        character.IgnoreInput(false);
        CurrentShieldHealth = MaxShieldHealth / 2;
    }
}
