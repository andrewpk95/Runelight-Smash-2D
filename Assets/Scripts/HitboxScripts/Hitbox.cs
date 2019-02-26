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

    public float damage;
    public bool hitStun;
    public int angle;
    public bool flipAngle;
    public float baseKnockback;
    public float knockbackGrowth;
    public int freezeFrame;
    public int id;

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
        //hitbox.enabled = false;
        collisions = new List<GameObject>();
    }

    void CheckCollision() {
        //hitbox.enabled = true;
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
            eventManager.InvokeOnDamageEvent(this, damageable, damage);
            damageable.TakeDamage(damage);
            if (hitStun) {
                if (flipAngle) {
                    damageable.LaunchAndHitStun(FlipAngle(angle), baseKnockback, knockbackGrowth);
                }
                else {
                    damageable.LaunchAndHitStun(angle, baseKnockback, knockbackGrowth);
                }
            }
            else {
                if (flipAngle) {
                    damageable.Launch(FlipAngle(angle), baseKnockback, knockbackGrowth);
                }
                else {
                    damageable.Launch(angle, baseKnockback, knockbackGrowth);
                }
            }
            damageable.Freeze(freezeFrame);
            ownerDamageable.Freeze(freezeFrame);
            //Debug.Log(angle);
    }

    int FlipAngle(int angle) {
        if (angle > 360) return angle;
        if (ownerCharacter.IsFacingRight()) {
            return angle;
        }
        else {
            return 180 - angle;
        }
    }

    public void Reset() {
        collisions.Clear();
        //hitbox.enabled = false;
    }

    public int GetID() {
        return id;
    }

    public string GetName() {
        return this.gameObject.name;
    }

    public GameObject GetOwner() {
        return owner;
    }

    public void SetHitStun(bool hit) {
        hitStun = hit;
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
