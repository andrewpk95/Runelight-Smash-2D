using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitbox : MonoBehaviour, IHitbox
{
    protected GameObject owner;
    protected ICharacter ownerCharacter;
    protected IDamageable ownerDamageable;
    protected Collider2D hitbox;
    protected List<GameObject> collisions;
    protected HitboxGroup hitboxGroup;
    protected HitboxContainer hitboxContainer;
    
    [SerializeField] protected int id;
    public int ID {get {return id;} set {id = value;}}
    public GameObject GameObject {get {return this.gameObject;}}

    public LayerMask mask;
    protected ContactFilter2D contactFilter;

    protected Collider2D[] result;
    protected int numResults;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize() {
        owner = this.gameObject.transform.root.gameObject;
        ownerCharacter = owner.GetComponent<ICharacter>();
        ownerDamageable = owner.GetComponent<IDamageable>();
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
        collisions = new List<GameObject>();
        hitboxGroup = GetComponentInParent<HitboxGroup>();
        hitboxContainer = GetComponentInParent<HitboxContainer>();
        result = new Collider2D[100];
        contactFilter = new ContactFilter2D();
        mask = Physics2D.GetLayerCollisionMask (gameObject.layer);
        contactFilter.SetLayerMask(mask);
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        numResults = 0;
    }

    protected virtual void CheckCollision() {
        hitbox.enabled = true;
        
        
        Array.Clear(result, 0, numResults);
        numResults = Physics2D.OverlapCollider(hitbox, contactFilter, result);
        //Debug.Log(numResults);
        
        for(int i = 0; i < numResults; i++) {
            if (result[i].gameObject.tag == "GrabHitbox") {
                GameObject fighter = result[i].gameObject.GetComponent<IHitbox>().GetOwner();
                if (fighter.Equals(owner) || collisions.Contains(result[i].gameObject)) {
                    continue;
                }
                else {
                    Debug.Log(this.gameObject.name + " hit " + fighter.name + "'s " + result[i].gameObject.name + " GrabHitbox!");
                    collisions.Add(result[i].gameObject);
                }
            }
            else if (result[i].gameObject.tag == "Hitbox") {
                GameObject fighter = result[i].gameObject.GetComponent<IHitbox>().GetOwner();
                if (fighter.Equals(owner) || collisions.Contains(result[i].gameObject)) {
                    continue;
                }
                else {
                    Debug.Log(this.gameObject.name + " hit " + fighter.name + "'s " + result[i].gameObject.name + " AttackHitbox!");
                    collisions.Add(result[i].gameObject);
                }
            }
            else if (result[i].gameObject.tag == "Shield") {
                GameObject fighter = result[i].gameObject.GetComponent<IShield>().GetOwner();
                if (fighter.Equals(owner) || collisions.Contains(result[i].gameObject)) {
					continue;
				}
                else {
                    
                    Debug.Log(this.gameObject.name + " hit " + fighter.name + "'s Shield!");
                    collisions.Add(result[i].gameObject);
                }
            }
            else if (result[i].gameObject.tag == "Hurtbox") {
                GameObject fighter = result[i].gameObject.GetComponent<IHurtbox>().GetOwner();
				if (fighter.Equals(owner) || collisions.Contains(fighter)) {
					continue;
				}
				else {
					Debug.Log(this.gameObject.name + " hit " + fighter.name + "!");
					collisions.Add(fighter);
				}
            }
        }
    }

    public virtual void OnHit(GameObject target) {
        
    }

    public virtual void OnClash(int clashFrame) {
        Debug.Log("Clashing " + owner.name);
        ownerCharacter.Clash(clashFrame);
    }

    public virtual void Enable() {
        
    }

    public virtual void Reset() {
        collisions.Clear();
        hitboxGroup.Victims.Clear();
        hitbox.enabled = false;
    }

    public virtual string GetName() {
        return this.gameObject.name;
    }

    public virtual GameObject GetOwner() {
        return owner;
    }

    public virtual Vector3 GetWorldPosition() {
        return this.gameObject.transform.position;
    }

    public virtual void SetOwner(GameObject newOwner) {
        owner = newOwner;
    }

    public virtual List<GameObject> GetCollisionList() {
        CheckCollision();
        return collisions;
    }

    public virtual List<GameObject> GetVictimList() {
        return hitboxGroup.Victims;
    }

    public virtual void AddToVictimList(GameObject victim) {
        hitboxGroup.Victims.Add(victim);
    }

    public virtual void Ignore(GameObject fighter) {
        collisions.Add(fighter);
    }

    public virtual void ClearCollisionList() {
        collisions.Clear();
    }

    void OnDisable() {
        Reset();
    }

    void OnTriggerEnter2D(Collider2D col) {
        //Debug.Log(string.Format("{0}'s {1} triggered with {2}'s {3}!", this.GetOwner().name, this.GetName(), col.gameObject.transform.root.gameObject.name, col.gameObject.name));
        
    }
}
