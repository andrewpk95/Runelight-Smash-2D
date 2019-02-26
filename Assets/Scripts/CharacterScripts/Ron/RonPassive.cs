using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RonPassive : MonoBehaviour, IPassive
{
    public float damageDealt;
    public float damageUnitForStaticCharge;
    public int staticCharge;
    public int maxStaticCharge;

    public EventManager eventManager;
    
    // Start is called before the first frame update
    void Start()
    {
        eventManager = (EventManager) GameObject.FindObjectOfType(typeof(EventManager));
        eventManager.SubscribeToOnDamageEvent(new UnityAction<IHitbox, IDamageable, float>(SaveDamage));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveDamage(IHitbox hitbox, IDamageable damageable, float damage) {
        if (hitbox.GetOwner().Equals(this.gameObject)) {
            if (hitbox.GetName() == "NeutralSpecial") return;
        if (staticCharge >= maxStaticCharge) {
            Clamp();
            return;
        }
        damageDealt += damage;
        ConvertDamageToStaticCharge();
        }
        
    }

    void ConvertDamageToStaticCharge() {
        float result = damageDealt / damageUnitForStaticCharge;
        int charge = Mathf.FloorToInt(result);
        staticCharge += charge;
        Clamp();
        damageDealt = damageDealt - charge * damageUnitForStaticCharge;
    }

    void Clamp() {
        if (staticCharge >= maxStaticCharge) {
            staticCharge = maxStaticCharge;
            damageDealt = 0.0f;
            return;
        }
    }

    public int ConsumeCharge() {
        int charge = staticCharge;
        staticCharge = 0;
        return charge;
    }

    public int GetStaticCharge() {
        return staticCharge;
    }
}
