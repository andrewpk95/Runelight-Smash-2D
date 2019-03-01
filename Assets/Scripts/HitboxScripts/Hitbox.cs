using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour, IHitbox
{
    public GameObject owner;
    ICharacter ownerCharacter;
    IDamageable ownerDamageable;
    public HitboxManager manager;
    public EventManager eventManager;
    public Collider2D hitbox;
    List<GameObject> collisions;

    [SerializeField] protected float damage;
    [SerializeField] protected bool hitStun;
    [SerializeField] protected int angle;
    [SerializeField] protected float baseKnockback;
    [SerializeField] protected float knockbackGrowth;
    [SerializeField] protected int freezeFrame;
    [SerializeField] protected int id;
    
    public float Damage {get {return damage;} set {damage = value;}}
    public bool HitStun {get {return hitStun;} set {hitStun = value;}}
    public int Angle {get {return angle;} set {angle = value;}}
    public float BaseKnockback {get {return baseKnockback;} set {baseKnockback = value;}}
    public float KnockbackGrowth {get {return knockbackGrowth;} set {knockbackGrowth = value;}}
    public int FreezeFrame {get {return freezeFrame;} set {freezeFrame = value;}}
    public int ID {get {return id;} set {id = value;}}

    public LayerMask mask;
    
    // Start is called before the first frame update
    void Start()
    {
        owner = this.gameObject.transform.root.gameObject;
        ownerCharacter = owner.GetComponent<ICharacter>();
        ownerDamageable = owner.GetComponent<IDamageable>();
        manager = GetComponentInParent<HitboxManager>();
        eventManager = (EventManager) GameObject.FindObjectOfType(typeof(EventManager));
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
        collisions = new List<GameObject>();
    }

    void CheckCollision() {
        hitbox.enabled = true;
        ContactFilter2D contactFilter = new ContactFilter2D();
        mask = Physics2D.GetLayerCollisionMask (gameObject.layer);
        contactFilter.SetLayerMask(mask);
        contactFilter.useLayerMask = true;
        Collider2D[] result = new Collider2D[500];
        int numResults = Physics2D.OverlapCollider(hitbox, contactFilter, result);
        //Debug.Log(numResults);
        
        for(int i = 0; i < numResults; i++) {
            GameObject fighter = result[i].gameObject.transform.root.gameObject;
            //Debug.Log(fighter.name);
            if (fighter.Equals(owner) || collisions.Contains(fighter)) {
                continue;
            }
            else {
                Debug.Log(this.gameObject.name + " hit " + fighter.name + "!");
                collisions.Add(fighter);
            }
        }
    }

    public void Hit(GameObject target) {
        IDamageable damageable = target.GetComponent<IDamageable>();
        eventManager.InvokeOnDamageEvent(this, damageable);
        
        ownerDamageable.Freeze(FreezeFrame);
    }

    public void Reset() {
        collisions.Clear();
        hitbox.enabled = false;
    }

    public string GetName() {
        return this.gameObject.name;
    }

    public GameObject GetOwner() {
        return owner;
    }

    public Vector3 GetWorldPosition() {
        return this.gameObject.transform.position;
    }

    public void SetOwner(GameObject newOwner) {
        owner = newOwner;
    }

    public List<GameObject> GetCollisionList() {
        CheckCollision();
        return collisions;
    }

    public void Ignore(GameObject fighter) {
        collisions.Add(fighter);
    }

    public void ClearCollisionList() {
        collisions.Clear();
    }

    void OnDisable() {
        Reset();
    }

    
}
