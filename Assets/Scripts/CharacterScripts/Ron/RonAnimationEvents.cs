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
