using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonAnimationEvents : MonoBehaviour
{
    HitboxManager[] hitboxes;
    HitboxManager hitboxCache;

    ICharacter character;

    RonPassive passive;
    
    // Start is called before the first frame update
    void Start()
    {
        hitboxes = GetComponentsInChildren<HitboxManager>();
        if (hitboxes.Length > 0) {
            hitboxCache = hitboxes[0];
        }
        foreach (HitboxManager hitbox in hitboxes) {
            Debug.Log(hitbox.name);
        }
        character = GetComponent<ICharacter>();
        passive = GetComponent<RonPassive>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {

    }

    public void ActivateHitboxGroup(string hitboxGroupName) {
        if (hitboxes.Length <= 0) {
            return;
        }
        if (hitboxCache.name == hitboxGroupName) {
            hitboxCache.ActivateHitboxes();
            return;
        }
        foreach(HitboxManager hitboxGroup in hitboxes) {
            if (hitboxGroup.name == hitboxGroupName) {
                hitboxGroup.ActivateHitboxes();
                return;
            }
        }
    }

    public void DeactivateHitboxGroup(string hitboxGroupName) {
        if (hitboxes.Length <= 0) {
            return;
        }
        if (hitboxCache.name == hitboxGroupName) {
            hitboxCache.DeactivateHitboxes();
            return;
        }
        foreach(HitboxManager hitboxGroup in hitboxes) {
            if (hitboxGroup.name == hitboxGroupName) {
                hitboxGroup.DeactivateHitboxes();
                break;
            }
        }
    }

    public void DeactivateAllHitbox() {
        foreach(HitboxManager hitboxGroup in hitboxes) {
            hitboxGroup.DeactivateHitboxes();
        }
    }

    public void StartSideSpecial() {
        character.SetVelocity(new Vector2(20, 0));
    }
}
