using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldHurtbox : FreezeBehaviour, IShield
{
    [SerializeField] protected float maxShieldHealth;
    [SerializeField] protected float currentShieldHealth;
    [SerializeField] protected float shieldHealthDegenRate;
    [SerializeField] protected float shieldHealthRegenRate;
    [SerializeField] protected bool isActive;
    [SerializeField] protected bool isShieldStunned;
    [SerializeField] protected bool isShieldBroken;
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
    public bool IsInvulnerable {get {return isInvulnerable;} set {isInvulnerable = value;}}

    protected int shieldStunFrameLeft;
    public int shieldBreakFrameLeft;
    protected Vector2 storedVelocity;

    Animator animator;
    ICharacter character;
    SpriteRenderer sprite;
    Collider2D col;
    Transform trans;

    public StatusManager statusManager;
    protected IStatus shieldBreakStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();

        CurrentShieldHealth = MaxShieldHealth;
        owner = this.gameObject.transform.root.gameObject;
        damageable = owner.GetComponent<IDamageable>();

        animator = GetComponentInParent<Animator>();
        character = GetComponentInParent<ICharacter>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        trans = GetComponent<Transform>();

        EventManager.instance.StartListeningToOnHitEvent(this.gameObject, new UnityAction<IAttackHitbox, GameObject>(OnHit));
        EventManager.instance.StartListeningToOnHitStunEvent(this.gameObject, new UnityAction<IAttackHitbox, GameObject>(OnHitStun));
        EventManager.instance.StartListeningToOnGrabEvent(this.gameObject, new UnityAction<GameObject, GameObject>(OnGrab));

        statusManager = GetComponentInParent<StatusManager>();
        shieldBreakStatus = new ShieldBreakStatus();
    }
    
    void Update() {
        animator.SetBool("isShieldStunned", IsShieldStunned);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Tick();
    }

    protected override void Tick() {
        UpdateShieldSize();
        base.Tick();
    }

    //Freeze Behaviour Overrides

    protected override void UpdateOtherBehaviour() {
        UpdateShieldBreakFrame();
        UpdateShieldStunFrame();
        UpdateShieldHealth();
    }

    protected override void Freeze() {
        base.Freeze();
        storedVelocity = character.Velocity;
    }

    protected override void UnFreeze() {
        base.UnFreeze();
		character.Velocity = storedVelocity;
    }

    //Shield Functions

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
			OnShieldStunOver();
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
        if (isActive) return;
        isActive = true;
        character.IgnoreMainJoystick(true);
        sprite.enabled = true;
        col.enabled = true;
    }

    public void DeactivateShield() {
        if (!isActive) return;
        isActive = false;
        character.IgnoreMainJoystick(false);
        sprite.enabled = false;
        col.enabled = false;
    }
    
    public void OnHit(IAttackHitbox hitbox, GameObject entity) {
        if (IsInvulnerable) return;
        if (entity.Equals(this.gameObject)) {
            TakeDamage(hitbox.Stats.Damage);
            Push(hitbox);
            ShieldStun(hitbox);
            EventManager.instance.InvokeOnFreezeEvent(owner, hitbox.Stats.FreezeFrame);
        }
    }

    //Event Functions

    public void OnHitStun(IAttackHitbox hitbox, GameObject entity) {
        if (entity.Equals(owner)) {
            if (IsActive) DeactivateShield();
            if (IsShieldBroken) OnShieldBreakOver();
        }
    }

    public void OnGrab(GameObject entity, GameObject target) {
        if (target.Equals(owner)) {
            if (IsActive) DeactivateShield();
            if (IsShieldBroken) OnShieldBreakOver();
        }
    }

    //IShield Implementation

    public void TakeDamage(float damage) {
        CurrentShieldHealth = Mathf.Max(CurrentShieldHealth - damage, 0.0f);
    }

    public void Heal(float heal) {
        CurrentShieldHealth = Mathf.Min(CurrentShieldHealth + heal, MaxShieldHealth);
    }

    public GameObject GetOwner() {
        return owner;
    }

    public void ShieldStun(IAttackHitbox hitbox) {
        if (!hitbox.Stats.HitStun) return;
        if (!IsShieldStunned) { //If not shield stunned before
            IsShieldStunned = true;
            character.IgnoreInput(true);
        }
        shieldStunFrameLeft = SmashCalculator.ShieldStunFrame(hitbox, this);
    }

    protected void OnShieldStunOver() {
        IsShieldStunned = false;
		shieldStunFrameLeft = 0;
		character.IgnoreInput(false);
    }

    public void Push(IAttackHitbox hitbox) {
        Vector2 launchVector = SmashCalculator.ShieldKnockbackVector(hitbox, this);
        //Debug.Log(launchVector);
        character.Velocity = launchVector;
    }

    public void OnShieldBreak() {
        Debug.Log(owner.name + "'s Shield Broken!");
        if (IsShieldStunned) OnShieldStunOver();
        DeactivateShield();
        statusManager.AddStatus(shieldBreakStatus);
        character.IgnoreInput(true);
        animator.SetTrigger("ShieldBreak");
        Vector2 launchVector = new Vector2(0.0f, 6.0f);
        character.Velocity = launchVector;
        damageable.SetIntangible();
        EventManager.instance.InvokeOnFreezeEvent(owner, 15);
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

    void OnDisable() {
        EventManager.instance.UnsubscribeAll(this.gameObject);
    }
}
