using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PercentageHurtbox : FreezeBehaviour, IDamageable
{
    [SerializeField] protected int weight;
    [SerializeField] protected float percentage;
    [SerializeField] protected bool isHitStunned;
    [SerializeField] protected bool isLaunched;
    [SerializeField] protected bool isInvulnerable;
    [SerializeField] protected bool isIntangible;
    [SerializeField] protected GameObject lastDamagedBy;

    public int Weight {get {return weight;} set {weight = value;}}
    public float Percentage {get {return percentage;} set {percentage = value;}}
    public bool IsHitStunned {get {return isHitStunned;} set {isHitStunned = value;}}
    public bool IsLaunched {get {return isLaunched;} set {isLaunched = value;}}
    public bool IsInvulnerable {get {return isInvulnerable;} set {isInvulnerable = value;}}
    public bool IsIntangible {get {return isIntangible;} set {isIntangible = value;}}
    public GameObject LastDamagedBy {get {return lastDamagedBy;} set {lastDamagedBy = value;}}

    public Color intangibleColor1;
    public Color intangibleColor2;
    public float flashTick;

    protected int hitStunFrameLeft;
    protected Vector2 storedVelocity;

    Animator animator;
    ICharacter character;
    HurtboxManager hurtbox;

    public StatusManager statusManager;
    protected IStatus launchStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<ICharacter>();
        hurtbox = GetComponentInChildren<HurtboxManager>();

        EventManager.instance.StartListeningToOnHitEvent(this.gameObject, new UnityAction<IAttackHitbox, GameObject>(OnHit));
        EventManager.instance.StartListeningToOnDeathEvent(this.gameObject, new UnityAction<GameObject>(OnDeath));

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

    protected override void UpdateOtherBehaviour() {
        UpdateHitStunFrame();
    }

    void UpdateHitStunFrame() {
		if (IsHitStunned) {
			hitStunFrameLeft -= 1;
		}
		if (hitStunFrameLeft < 0) { //Hitstun Over
			IsHitStunned = false;
            if (IsLaunched) {
                Debug.Log("Start Tumbling Down!");
                statusManager.RemoveStatus(launchStatus);
                character.StartTumbling();
            }
			IsLaunched = false;
			hitStunFrameLeft = 0;
			character.IgnoreInput(false);
			character.EnableAirDeceleration(true);
		}
    }

    //IFreezable Overrides

    public override void Freeze(int freezeFrameDuration) {
        base.Freeze(freezeFrameDuration);
        statusManager.Freeze(freezeFrameDuration);
        storedVelocity = character.Velocity;
    }

    protected override void OnFreeze() {
        base.OnFreeze();
        character.Freeze();
    }

    protected override void OnUnFreeze() {
        base.OnUnFreeze();
        character.UnFreeze();
		character.Velocity = storedVelocity;
    }

    //IDamageable Implementations

    public void OnHit(IAttackHitbox hitbox, GameObject entity) {
        if (IsInvulnerable) return;
        if (entity.Equals(this.gameObject)) {
            TakeDamage(hitbox.Stats.Damage);
            lastDamagedBy = hitbox.GetOwner();
            EventManager.instance.InvokeOnDamageEvent(hitbox, this);
            FaceHitbox(hitbox);
            HitStun(hitbox);
            Launch(hitbox);
            Freeze(hitbox.Stats.FreezeFrame);
        }
    }

    public void OnDeath(GameObject entity) {
        if (entity.Equals(this.gameObject)) {
            Percentage = 0;
            IsHitStunned = false;
            IsLaunched = false;
            IsInvulnerable = false;
            IsIntangible = false;
            hitStunFrameLeft = 0;
            storedVelocity = Vector2.zero;
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

    public void FaceHitbox(IAttackHitbox hitbox) {
        if (hitbox.Stats.FaceOwnerWhenHit) character.Face(hitbox.GetOwner());
    }

    public void HitStun(IAttackHitbox hitbox) {
        if (!hitbox.Stats.HitStun) return;
        EventManager.instance.InvokeOnHitStunEvent(hitbox, this.gameObject);
        if (!IsHitStunned) { //If not hitstunned before
            IsHitStunned = true;
            character.IgnoreInput(true);
            character.EnableAirDeceleration(false);
            character.UnPreventWalkOffLedge();
        }
        hitStunFrameLeft = SmashCalculator.HitStunDuration(hitbox, this);
        if (character.IsTumbling) character.StopTumbling();
        if (character.IsTumbled) character.UnTumble();
    }

    public void Launch(IAttackHitbox hitbox) {
        Vector2 launchVector = SmashCalculator.LaunchVector(hitbox, this);
        IsLaunched = SmashCalculator.Tumble(hitbox, this);
        if (IsLaunched) statusManager.AddStatus(launchStatus);
        Debug.Log("Launched! " + launchVector);
        character.Velocity = launchVector;
        //Release from grab if launched with high enough velocity
        if (character.GetGrabber() != null && SmashCalculator.GrabRelease(hitbox, this)) {
            character.GetGrabber().GetComponent<ICharacter>().GrabRelease();
        }
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
    
    //Collision with walls

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.tag == "Wall") {
            //Debug.Log(this.gameObject.name + " collided with wall: " + collision.collider.gameObject.name);
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            Vector2 normal = contacts[0].normal;
            //Reflect character off the wall if being launched
            if (IsLaunched) {
                //If the velocity is not moving into the surface, return
                if (Vector2.Angle(character.Velocity, normal) <= 90.0f) return;
                Vector2 reflectDirection = Vector2.Reflect(character.Velocity, normal);
                character.Velocity = reflectDirection;
                Debug.DrawRay(contacts[0].point, reflectDirection, Color.blue, 0.5f);
            }
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.gameObject.tag == "Wall") {
            
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision) {
        
    }

    void OnDisable() {
        EventManager.instance.UnsubscribeAll(this.gameObject);
    }
}
