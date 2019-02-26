using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnDamageEvent : UnityEvent<IHitbox, IDamageable, float>
{
}

public class EventManager : MonoBehaviour
{
    public OnDamageEvent onDamageEvent;
    
    // Start is called before the first frame update
    void Awake()
    {
        onDamageEvent = new OnDamageEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeOnDamageEvent(IHitbox hitbox, IDamageable damageable, float damage) {
        Debug.Log("OnDamageEvent Invoked");
        onDamageEvent.Invoke(hitbox, damageable, damage);
    }

    public void SubscribeToOnDamageEvent(UnityAction<IHitbox, IDamageable, float> unityAction) {
        Debug.Log("Subscribed to OnDamageEvent");
        onDamageEvent.AddListener(unityAction);
    }
}
