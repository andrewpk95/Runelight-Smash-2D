using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnDamageEvent : UnityEvent<IHitbox, IDamageable>
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

    public void InvokeOnDamageEvent(IHitbox hitbox, IDamageable damageable) {
        Debug.Log("OnDamageEvent Invoked");
        onDamageEvent.Invoke(hitbox, damageable);
    }

    public void StartListeningToOnDamageEvent(UnityAction<IHitbox, IDamageable> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnDamageEvent");
        onDamageEvent.AddListener(listener);
    }

    public void StopListeningToOnDamageEvent(UnityAction<IHitbox, IDamageable> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnDamageEvent");
        onDamageEvent.RemoveListener(listener);
    }
}
