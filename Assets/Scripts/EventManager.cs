using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnHitEvent : UnityEvent<IHitbox, GameObject>
{
}

[System.Serializable]
public class OnDamageEvent : UnityEvent<IHitbox, IDamageable>
{
}

[System.Serializable]
public class OnHitStunEvent : UnityEvent<IHitbox, GameObject>
{
}

public class EventManager : MonoBehaviour
{
    public OnHitEvent onHitEvent;
    public OnDamageEvent onDamageEvent;
    public OnHitStunEvent onHitStunEvent;
    
    // Start is called before the first frame update
    void Awake()
    {
        onHitEvent = new OnHitEvent();
        onDamageEvent = new OnDamageEvent();
        onHitStunEvent = new OnHitStunEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeOnHitEvent(IHitbox hitbox, GameObject entity) {
        Debug.Log("OnHitEvent Invoked");
        onHitEvent.Invoke(hitbox, entity);
    }

    public void StartListeningToOnHitEvent(UnityAction<IHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnHitEvent");
        onHitEvent.AddListener(listener);
    }

    public void StopListeningToOnHitEvent(UnityAction<IHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnHitEvent");
        onHitEvent.RemoveListener(listener);
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

    public void InvokeOnHitStunEvent(IHitbox hitbox, GameObject entity) {
        Debug.Log("OnHitStunEvent Invoked");
        onHitStunEvent.Invoke(hitbox, entity);
    }

    public void StartListeningToOnHitStunEvent(UnityAction<IHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnHitStunEvent");
        onHitStunEvent.AddListener(listener);
    }

    public void StopListeningToOnHitStunEvent(UnityAction<IHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnHitStunEvent");
        onHitStunEvent.RemoveListener(listener);
    }
}
