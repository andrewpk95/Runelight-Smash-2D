using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public OnPlayerJoinEvent onPlayerJoinEvent;
    public OnPlayerLeaveEvent onPlayerLeaveEvent;
    public OnCharacterSelectEvent onCharacterSelectEvent;
    public OnCharacterDeSelectEvent onCharacterDeSelectEvent;
    public OnHitEvent onHitEvent;
    public OnFreezeEvent onFreezeEvent;
    public OnGrabEvent onGrabEvent;
    public OnDamageEvent onDamageEvent;
    public OnHitStunEvent onHitStunEvent;
    public OnEdgeGrabEvent onEdgeGrabEvent;
    public OnDeathEvent onDeathEvent;
    public OnGameOverEvent onGameOverEvent;

    public delegate void EventCleaner();

    Dictionary<EventType, UnityEventBase> EventDictionary;
    Dictionary<GameObject, EventCleaner> EventCleanerDictionary;

    public static EventManager instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        //Singleton Pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);    
        
        DontDestroyOnLoad(this.gameObject);
        
        Initialize();
    }

    void Initialize() {
        onPlayerJoinEvent = new OnPlayerJoinEvent();
        onPlayerLeaveEvent = new OnPlayerLeaveEvent();
        onCharacterSelectEvent = new OnCharacterSelectEvent();
        onCharacterDeSelectEvent = new OnCharacterDeSelectEvent();
        onHitEvent = new OnHitEvent();
        onFreezeEvent = new OnFreezeEvent();
        onGrabEvent = new OnGrabEvent();
        onDamageEvent = new OnDamageEvent();
        onHitStunEvent = new OnHitStunEvent();
        onEdgeGrabEvent = new OnEdgeGrabEvent();
        onDeathEvent = new OnDeathEvent();
        onGameOverEvent = new OnGameOverEvent();

        EventCleanerDictionary = new Dictionary<GameObject, EventCleaner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearAllEvents() {
        onPlayerJoinEvent.RemoveAllListeners();
        onPlayerLeaveEvent.RemoveAllListeners();
    }

    public void InvokeOnPlayerJoinEvent(Player player) {
        Debug.Log("OnPlayerJoinEvent Invoked");
        onPlayerJoinEvent.Invoke(player);
    }

    public void StartListeningToOnPlayerJoinEvent(GameObject listener, UnityAction<Player> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnPlayerJoinEvent", listener.name, method.Method.Name));
        onPlayerJoinEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnPlayerJoinEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnPlayerJoinEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnPlayerJoinEvent(GameObject listener, UnityAction<Player> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnPlayerJoinEvent", listener.name, method.Method.Name));
        onPlayerJoinEvent.RemoveListener(method);
    }

    public void InvokeOnPlayerLeaveEvent(Player player) {
        Debug.Log("OnPlayerLeaveEvent Invoked");
        onPlayerLeaveEvent.Invoke(player);
    }

    public void StartListeningToOnPlayerLeaveEvent(GameObject listener, UnityAction<Player> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnPlayerLeaveEvent", listener.name, method.Method.Name));
        onPlayerLeaveEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnPlayerLeaveEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnPlayerLeaveEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnPlayerLeaveEvent(GameObject listener, UnityAction<Player> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnPlayerLeaveEvent", listener.name, method.Method.Name));
        onPlayerLeaveEvent.RemoveListener(method);
    }

    public void InvokeOnCharacterSelectEvent(Player player, CharacterType character) {
        Debug.Log("OnCharacterSelectEvent Invoked");
        onCharacterSelectEvent.Invoke(player, character);
    }

    public void StartListeningToOnCharacterSelectEvent(GameObject listener, UnityAction<Player, CharacterType> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnCharacterSelectEvent", listener.name, method.Method.Name));
        onCharacterSelectEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnCharacterSelectEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnCharacterSelectEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnCharacterSelectEvent(GameObject listener, UnityAction<Player, CharacterType> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnCharacterSelectEvent", listener.name, method.Method.Name));
        onCharacterSelectEvent.RemoveListener(method);
    }

    public void InvokeOnCharacterDeSelectEvent(Player player) {
        Debug.Log("OnCharacterDeSelectEvent Invoked");
        onCharacterDeSelectEvent.Invoke(player);
    }

    public void StartListeningToOnCharacterDeSelectEvent(GameObject listener, UnityAction<Player> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnCharacterDeSelectEvent", listener.name, method.Method.Name));
        onCharacterDeSelectEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnCharacterDeSelectEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnCharacterDeSelectEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnCharacterDeSelectEvent(GameObject listener, UnityAction<Player> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnCharacterDeSelectEvent", listener.name, method.Method.Name));
        onCharacterDeSelectEvent.RemoveListener(method);
    }

    public void InvokeOnHitEvent(IAttackHitbox hitbox, GameObject entity) {
        Debug.Log("OnHitEvent Invoked");
        onHitEvent.Invoke(hitbox, entity);
    }

    public void StartListeningToOnHitEvent(GameObject listener, UnityAction<IAttackHitbox, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnHitEvent", listener.name, method.Method.Name));
        onHitEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnHitEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnHitEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnHitEvent(GameObject listener, UnityAction<IAttackHitbox, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnHitEvent", listener.name, method.Method.Name));
        onHitEvent.RemoveListener(method);
    }

    public void InvokeOnFreezeEvent(GameObject entity, int freezeFrame) {
        Debug.Log("OnFreezeEvent Invoked: " + entity.name + " " + freezeFrame);
        onFreezeEvent.Invoke(entity, freezeFrame);
    }

    public void StartListeningToOnFreezeEvent(GameObject listener, UnityAction<GameObject, int> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnFreezeEvent", listener.name, method.Method.Name));
        onFreezeEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnFreezeEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnFreezeEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnFreezeEvent(GameObject listener, UnityAction<GameObject, int> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnFreezeEvent", listener.name, method.Method.Name));
        onFreezeEvent.RemoveListener(method);
    }

    public void InvokeOnGrabEvent(GameObject entity, GameObject target) {
        Debug.Log("OnGrabEvent Invoked");
        onGrabEvent.Invoke(entity, target);
    }

    public void StartListeningToOnGrabEvent(GameObject listener, UnityAction<GameObject, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnGrabEvent", listener.name, method.Method.Name));
        onGrabEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnGrabEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnGrabEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnGrabEvent(GameObject listener, UnityAction<GameObject, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnGrabEvent", listener.name, method.Method.Name));
        onGrabEvent.RemoveListener(method);
    }

    public void InvokeOnDamageEvent(IAttackHitbox hitbox, IDamageable damageable) {
        Debug.Log("OnDamageEvent Invoked");
        onDamageEvent.Invoke(hitbox, damageable);
    }

    public void StartListeningToOnDamageEvent(GameObject listener, UnityAction<IAttackHitbox, IDamageable> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnDamageEvent", listener.name, method.Method.Name));
        onDamageEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnDamageEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnDamageEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnDamageEvent(GameObject listener, UnityAction<IAttackHitbox, IDamageable> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnDamageEvent", listener.name, method.Method.Name));
        onDamageEvent.RemoveListener(method);
    }

    public void InvokeOnHitStunEvent(IAttackHitbox hitbox, GameObject entity) {
        Debug.Log("OnHitStunEvent Invoked");
        onHitStunEvent.Invoke(hitbox, entity);
    }

    public void StartListeningToOnHitStunEvent(GameObject listener, UnityAction<IAttackHitbox, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnHitStunEvent", listener.name, method.Method.Name));
        onHitStunEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnHitStunEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnHitStunEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnHitStunEvent(GameObject listener, UnityAction<IAttackHitbox, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnHitStunEvent", listener.name, method.Method.Name));
        onHitStunEvent.RemoveListener(method);
    }

    public void InvokeOnEdgeGrabEvent(GameObject entity, GameObject edge) {
        Debug.Log("OnEdgeGrabEvent Invoked");
        onEdgeGrabEvent.Invoke(entity, edge);
    }

    public void StartListeningToOnEdgeGrabEvent(GameObject listener, UnityAction<GameObject, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnEdgeGrabEvent", listener.name, method.Method.Name));
        onEdgeGrabEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnEdgeGrabEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnEdgeGrabEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnEdgeGrabEvent(GameObject listener, UnityAction<GameObject, GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnEdgeGrabEvent", listener.name, method.Method.Name));
        onEdgeGrabEvent.RemoveListener(method);
    }

    public void InvokeOnDeathEvent(GameObject entity) {
        Debug.Log("OnDeathEvent Invoked: " + entity.name);
        onDeathEvent.Invoke(entity);
    }

    public void StartListeningToOnDeathEvent(GameObject listener, UnityAction<GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnDeathEvent", listener.name, method.Method.Name));
        onDeathEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnDeathEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnDeathEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnDeathEvent(GameObject listener, UnityAction<GameObject> method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnDeathEvent", listener.name, method.Method.Name));
        onDeathEvent.RemoveListener(method);
    }

    public void InvokeOnGameOverEvent() {
        Debug.Log("OnGameOverEvent Invoked");
        onGameOverEvent.Invoke();
    }

    public void StartListeningToOnGameOverEvent(GameObject listener, UnityAction method) {
        Debug.Log(string.Format("{0}'s {1} method subscribed to OnGameOverEvent", listener.name, method.Method.Name));
        onGameOverEvent.AddListener(method);
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod += () => StopListeningToOnGameOverEvent(listener, method);
            EventCleanerDictionary[listener] = eventCleanerMethod;
        }
        else {
            eventCleanerMethod += () => StopListeningToOnGameOverEvent(listener, method);
            EventCleanerDictionary.Add(listener, eventCleanerMethod);
        }
    }

    public void StopListeningToOnGameOverEvent(GameObject listener, UnityAction method) {
        Debug.Log(string.Format("{0}'s {1} method unsubscribed to OnGameOverEvent", listener.name, method.Method.Name));
        onGameOverEvent.RemoveListener(method);
    }

    public void UnsubscribeAll(GameObject listener) {
        EventCleaner eventCleanerMethod;
        if (EventCleanerDictionary.TryGetValue(listener, out eventCleanerMethod)) {
            eventCleanerMethod();
            EventCleanerDictionary.Remove(listener);
        }
    }
}
