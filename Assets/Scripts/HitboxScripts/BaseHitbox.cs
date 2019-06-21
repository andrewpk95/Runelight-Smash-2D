using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitbox : MonoBehaviour, IHitbox
{
    protected GameObject owner;
    protected ICharacter ownerCharacter;
    protected IDamageable ownerDamageable;
    protected HitboxManager manager;
    protected Collider2D hitbox;
    protected List<GameObject> collisions;
    
    [SerializeField] protected int id;
    public int ID {get {return id;} set {id = value;}}

    public LayerMask mask;

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
        manager = GetComponentInParent<HitboxManager>();
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
        collisions = new List<GameObject>();
        result = new Collider2D[100];
        numResults = 0;
    }

    protected virtual void CheckCollision() {
        hitbox.enabled = true;
        ContactFilter2D contactFilter = new ContactFilter2D();
        mask = Physics2D.GetLayerCollisionMask (gameObject.layer);
        contactFilter.SetLayerMask(mask);
        contactFilter.useLayerMask = true;
        Array.Clear(result, 0, numResults);
        numResults = Physics2D.OverlapCollider(hitbox, contactFilter, result);
        //Debug.Log(numResults);
        
        for(int i = 0; i < numResults; i++) {
            if (result[i].gameObject.tag == "Shield") {
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

    public virtual void Reset() {
        collisions.Clear();
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

    public virtual void Ignore(GameObject fighter) {
        collisions.Add(fighter);
    }

    public virtual void ClearCollisionList() {
        collisions.Clear();
    }

    void OnDisable() {
        Reset();
    }
}
