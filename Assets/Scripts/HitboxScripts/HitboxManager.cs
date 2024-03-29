﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    protected GameObject owner;
    protected IHitbox[] list;
    protected Dictionary<GameObject, List<IHitbox>> collisionDictionary;
    protected List<GameObject> victims;

    public bool activated;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize() {
        owner = this.gameObject.transform.root.gameObject;
        list = GetComponentsInChildren<IHitbox>();
        collisionDictionary = new Dictionary<GameObject, List<IHitbox>>();
        victims = new List<GameObject>();
    }

    void FixedUpdate() {
        if (activated) {
            CheckHitboxes();
        }
    }
    
    public virtual void ActivateHitboxes() {
        activated = true;
        CheckHitboxes();
    }

    protected void CheckHitboxes() {
        collisionDictionary.Clear();
        //Create dictionary of hitboxes that hit fighters to sort them with hitbox id
        foreach (IHitbox hitbox in list) {
            List<GameObject> collisions = hitbox.GetCollisionList();
            foreach(GameObject collider in collisions) {
                if (collisionDictionary.ContainsKey(collider)) {
                    collisionDictionary[collider].Add(hitbox);
                }
                else {
                    List<IHitbox> value = new List<IHitbox>();
                    value.Add(hitbox);
                    collisionDictionary.Add(collider, value);
                }
            }
        }
        //Remove Fighters who have shield collision
        List<GameObject> shieldingFighters = new List<GameObject>();
        foreach (KeyValuePair<GameObject, List<IHitbox>> p in collisionDictionary) {
            if (p.Key.tag == "Shield") {
                shieldingFighters.Add(p.Key.transform.root.gameObject);
                victims.Add(p.Key.transform.root.gameObject);
            }
        }
        foreach (GameObject fighter in shieldingFighters) {
            collisionDictionary.Remove(fighter);
        }
        //Remove all but Hitbox with lowest ID for each fighter and process the highest ID hitbox
        foreach (KeyValuePair<GameObject, List<IHitbox>> p in collisionDictionary) {
            //Find hitbox with lowest ID
            IHitbox topIDHitbox = p.Value[0];
            int topID = 10;
            foreach(IHitbox hitbox in p.Value) {
                if(hitbox.ID < topID) {
                    topIDHitbox = hitbox;
                    topID = hitbox.ID;
                }
            }
            //Debug.Log("Fighter: " + p.Key.name + ", Hitbox Chosen: " + topID);
            //Add to the victims list to prevent hitting again
            if (victims.Contains(p.Key)) continue;
            victims.Add(p.Key);
            topIDHitbox.OnHit(p.Key);
        }
    }

    public virtual void DeactivateHitboxes() {
        activated = false;
        collisionDictionary.Clear();
        victims.Clear();
        foreach(IHitbox hitbox in list) {
            hitbox.Reset();
        }
    }
    
}
