using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    public static HitboxManager instance;

    protected Dictionary<GameObject, List<IHitbox>> collisionDictionary;
    
    protected HitboxContainer[] hitboxContainers;
    
    // Start is called before the first frame update
    void Awake()
    {
        //Singleton Pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);    

        Initialize();
    }

    protected virtual void Initialize() {
        hitboxContainers = (HitboxContainer[]) GameObject.FindObjectsOfType(typeof(HitboxContainer));
        collisionDictionary = new Dictionary<GameObject, List<IHitbox>>();
        
    }

    void FixedUpdate() {
        Tick();
    }
    
    void Tick() {
        collisionDictionary.Clear();
        //Create dictionary of hitboxes that hit fighters to sort them with hitbox id
        foreach (HitboxContainer hitboxContainer in hitboxContainers) {
            foreach (IHitbox hitbox in hitboxContainer.GetActivatedHitboxes()) {
                foreach (GameObject collider in hitbox.GetCollisionList()) {
                    if (hitbox.GetVictimList().Contains(collider)) {
                        continue;
                    }
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
        }

        string log = "";
        

        //First, clash grab hitboxes
        HashSet<IHitbox> clashingHitboxes = new HashSet<IHitbox>();
        foreach (KeyValuePair<GameObject, List<IHitbox>> p in collisionDictionary) {
            if (p.Key.tag == "GrabHitbox") {
                clashingHitboxes.Add(p.Key.GetComponent<IHitbox>());
                foreach (IHitbox hitbox in p.Value) {
                    clashingHitboxes.Add(hitbox);
                }
            }
        }
        log = "";
        foreach (IHitbox hitbox in clashingHitboxes) {
            log += string.Format("{0}'s {1}, ", hitbox.GetOwner().name, hitbox.GetName());
            hitbox.OnClash(10);
            collisionDictionary.Remove(hitbox.GameObject);
        }
        if (log.Length > 0) {
            Debug.Log("Clashing GrabHitboxes: " + log);
        }
        //Display Raw result
        log = "";
        foreach (KeyValuePair<GameObject, List<IHitbox>> p in collisionDictionary) {
            string list = "";
            foreach (IHitbox hitbox in p.Value) {
                list += string.Format("{0}'s {1}, ", hitbox.GetOwner().name, hitbox.GetName());
            }
            log += string.Format("[{0}: [{1}]] ", p.Key.name, list);
        }
        if (log.Length > 0) {
            Debug.Log("HitboxManager Dictionary Sort Result: " + log);
        }

        //Remove Fighters who have shield collision
        List<GameObject> shieldingFighters = new List<GameObject>();
        foreach (KeyValuePair<GameObject, List<IHitbox>> p in collisionDictionary) {
            if (p.Key.tag == "Shield") {
                
                shieldingFighters.Add(p.Key.transform.root.gameObject);
                foreach (IHitbox hitbox in p.Value) {
                    hitbox.GetVictimList().Add(p.Key.transform.root.gameObject);
                }
            }
        }
        foreach (GameObject fighter in shieldingFighters) {
            collisionDictionary.Remove(fighter);
            Debug.Log("Removing Shielding Fighter " + fighter.name);
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

            //Add to the victims list to prevent hitting again
            if (topIDHitbox.GetVictimList().Contains(p.Key)) continue;
            foreach (IHitbox hitbox in p.Value) {
                hitbox.AddToVictimList(p.Key);
                Debug.Log(p.Key.name + " added to " + hitbox.GetName() + "'s victim list");
            }

            //Send event for each valid hitbox event that occurred
            Debug.Log("Fighter: " + p.Key.name + ", Hitbox Chosen: " + topID);
            topIDHitbox.OnHit(p.Key);
        }
    }
}
