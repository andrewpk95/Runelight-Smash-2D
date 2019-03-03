using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonAnimationEvents : MonoBehaviour
{
    Dictionary<string, HitboxManager> hitboxes;
    List<HitboxManager> activatedHitboxes;
    IShield shield;

    ICharacter character;

    RonPassive passive;
    
    // Start is called before the first frame update
    void Start()
    {
        hitboxes = new Dictionary<string, HitboxManager>();
        HitboxManager[] list = GetComponentsInChildren<HitboxManager>();
        foreach (HitboxManager hitbox in list) {
            hitboxes.Add(hitbox.name, hitbox);
            //Debug.Log(hitbox.name);
        }
        activatedHitboxes = new List<HitboxManager>();

        shield = GetComponentInChildren<IShield>();

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
        if (hitboxes.Count <= 0) {
            return;
        }
        hitboxes[hitboxGroupName].ActivateHitboxes();
        activatedHitboxes.Add(hitboxes[hitboxGroupName]);
    }

    public void DeactivateHitboxGroup(string hitboxGroupName) {
        if (hitboxes.Count <= 0) {
            return;
        }
        hitboxes[hitboxGroupName].DeactivateHitboxes();
        activatedHitboxes.Remove(hitboxes[hitboxGroupName]);
    }

    public void DeactivateAllHitbox() {
        foreach(HitboxManager hitboxGroup in activatedHitboxes) {
            hitboxGroup.DeactivateHitboxes();
        }
        activatedHitboxes.Clear();
    }

    public void ActivateShield() {
        shield.ActivateShield();
    }

    public void DeactivateShield() {
        shield.DeactivateShield();
    }

    public void OnShieldBreakStart() {
        shield.OnShieldBreakStart();
    }

    public void OnShieldBreakOver() {
        shield.OnShieldBreakOver();
    }
}
