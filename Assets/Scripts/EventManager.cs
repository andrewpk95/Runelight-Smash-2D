using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnHitEvent : UnityEvent<IAttackHitbox, GameObject>
{
}

[System.Serializable]
public class OnGrabEvent : UnityEvent<GameObject, GameObject>
{
}

[System.Serializable]
public class OnDamageEvent : UnityEvent<IAttackHitbox, IDamageable>
{
}

[System.Serializable]
public class OnHitStunEvent : UnityEvent<IAttackHitbox, GameObject>
{
}

[System.Serializable]
public class OnEdgeGrabEvent : UnityEvent<GameObject, GameObject>
{
}

public class EventManager : MonoBehaviour
{
    public OnHitEvent onHitEvent;
    public OnGrabEvent onGrabEvent;
    public OnDamageEvent onDamageEvent;
    public OnHitStunEvent onHitStunEvent;
    public OnEdgeGrabEvent onEdgeGrabEvent;
    
    // Start is called before the first frame update
    void Awake()
    {
        onHitEvent = new OnHitEvent();
        onGrabEvent = new OnGrabEvent();
        onDamageEvent = new OnDamageEvent();
        onHitStunEvent = new OnHitStunEvent();
        onEdgeGrabEvent = new OnEdgeGrabEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeOnHitEvent(IAttackHitbox hitbox, GameObject entity) {
        Debug.Log("OnHitEvent Invoked");
        onHitEvent.Invoke(hitbox, entity);
    }

    public void StartListeningToOnHitEvent(UnityAction<IAttackHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnHitEvent");
        onHitEvent.AddListener(listener);
    }

    public void StopListeningToOnHitEvent(UnityAction<IAttackHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnHitEvent");
        onHitEvent.RemoveListener(listener);
    }

    public void InvokeOnGrabEvent(GameObject entity, GameObject target) {
        Debug.Log("OnGrabEvent Invoked");
        onGrabEvent.Invoke(entity, target);
    }

    public void StartListeningToOnGrabEvent(UnityAction<GameObject, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnGrabEvent");
        onGrabEvent.AddListener(listener);
    }

    public void StopListeningToOnGrabEvent(UnityAction<GameObject, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnGrabEvent");
        onGrabEvent.RemoveListener(listener);
    }

    public void InvokeOnDamageEvent(IAttackHitbox hitbox, IDamageable damageable) {
        Debug.Log("OnDamageEvent Invoked");
        onDamageEvent.Invoke(hitbox, damageable);
    }

    public void StartListeningToOnDamageEvent(UnityAction<IAttackHitbox, IDamageable> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnDamageEvent");
        onDamageEvent.AddListener(listener);
    }

    public void StopListeningToOnDamageEvent(UnityAction<IAttackHitbox, IDamageable> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnDamageEvent");
        onDamageEvent.RemoveListener(listener);
    }

    public void InvokeOnHitStunEvent(IAttackHitbox hitbox, GameObject entity) {
        Debug.Log("OnHitStunEvent Invoked");
        onHitStunEvent.Invoke(hitbox, entity);
    }

    public void StartListeningToOnHitStunEvent(UnityAction<IAttackHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnHitStunEvent");
        onHitStunEvent.AddListener(listener);
    }

    public void StopListeningToOnHitStunEvent(UnityAction<IAttackHitbox, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnHitStunEvent");
        onHitStunEvent.RemoveListener(listener);
    }

    public void InvokeOnEdgeGrabEvent(GameObject entity, GameObject edge) {
        Debug.Log("OnEdgeGrabEvent Invoked");
        onEdgeGrabEvent.Invoke(entity, edge);
    }

    public void StartListeningToOnEdgeGrabEvent(UnityAction<GameObject, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnEdgeGrabEvent");
        onEdgeGrabEvent.AddListener(listener);
    }

    public void StopListeningToOnEdgeGrabEvent(UnityAction<GameObject, GameObject> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnEdgeGrabEvent");
        onEdgeGrabEvent.RemoveListener(listener);
    }
}
