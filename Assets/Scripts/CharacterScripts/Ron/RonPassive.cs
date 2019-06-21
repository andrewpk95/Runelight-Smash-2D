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

    public StatusManager statusManager;
    public IStatus staticSpeedBuff;
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.StartListeningToOnDamageEvent(new UnityAction<IAttackHitbox, IDamageable>(SaveDamage));
        EventManager.instance.StartListeningToOnDeathEvent(new UnityAction<GameObject>(OnDeath));

        statusManager = GetComponent<StatusManager>();
        staticSpeedBuff = new RonPassiveBuffStatus(1.5f, this);
        statusManager.AddStatus(staticSpeedBuff);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveDamage(IAttackHitbox hitbox, IDamageable damageable) {
        if (hitbox.GetOwner().Equals(this.gameObject)) {
            if (hitbox.GetName() == "NeutralSpecial" || hitbox.GetName() == "SideSpecial1" || hitbox.GetName() == "SideSpecial2") return;
            damageDealt += hitbox.Stats.Damage;
            ConvertDamageToStaticCharge();
        }
        
    }

    public void OnDeath(GameObject entity) {
        if (entity.Equals(this.gameObject)) {
            staticCharge = 0;
            damageDealt = 0.0f;
        }
    }

    void ConvertDamageToStaticCharge() {
        float result = damageDealt / damageUnitForStaticCharge;
        int charge = Mathf.FloorToInt(result);
        staticCharge += charge;
        damageDealt = damageDealt - charge * damageUnitForStaticCharge;
        Clamp();
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
