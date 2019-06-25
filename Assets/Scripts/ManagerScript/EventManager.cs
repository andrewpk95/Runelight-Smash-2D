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
    public OnGrabEvent onGrabEvent;
    public OnDamageEvent onDamageEvent;
    public OnHitStunEvent onHitStunEvent;
    public OnEdgeGrabEvent onEdgeGrabEvent;
    public OnDeathEvent onDeathEvent;
    public OnGameOverEvent onGameOverEvent;

    Dictionary<EventType, UnityEventBase> eventDictionary;

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
        onGrabEvent = new OnGrabEvent();
        onDamageEvent = new OnDamageEvent();
        onHitStunEvent = new OnHitStunEvent();
        onEdgeGrabEvent = new OnEdgeGrabEvent();
        onDeathEvent = new OnDeathEvent();
        onGameOverEvent = new OnGameOverEvent();
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

    public void StartListeningToOnPlayerJoinEvent(UnityAction<Player> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnPlayerJoinEvent");
        onPlayerJoinEvent.AddListener(listener);
    }

    public void StopListeningToOnPlayerJoinEvent(UnityAction<Player> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnPlayerJoinEvent");
        onPlayerJoinEvent.RemoveListener(listener);
    }

    public void InvokeOnPlayerLeaveEvent(Player player) {
        Debug.Log("OnPlayerLeaveEvent Invoked");
        onPlayerLeaveEvent.Invoke(player);
    }

    public void StartListeningToOnPlayerLeaveEvent(UnityAction<Player> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnPlayerLeaveEvent");
        onPlayerLeaveEvent.AddListener(listener);
    }

    public void StopListeningToOnPlayerLeaveEvent(UnityAction<Player> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnPlayerLeaveEvent");
        onPlayerLeaveEvent.RemoveListener(listener);
    }

    public void InvokeOnCharacterSelectEvent(Player player, CharacterType character) {
        Debug.Log("OnCharacterSelectEvent Invoked");
        onCharacterSelectEvent.Invoke(player, character);
    }

    public void StartListeningToOnCharacterSelectEvent(UnityAction<Player, CharacterType> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnCharacterSelectEvent");
        onCharacterSelectEvent.AddListener(listener);
    }

    public void StopListeningToOnCharacterSelectEvent(UnityAction<Player, CharacterType> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnCharacterSelectEvent");
        onCharacterSelectEvent.RemoveListener(listener);
    }

    public void InvokeOnCharacterDeSelectEvent(Player player) {
        Debug.Log("OnCharacterDeSelectEvent Invoked");
        onCharacterDeSelectEvent.Invoke(player);
    }

    public void StartListeningToOnCharacterDeSelectEvent(UnityAction<Player> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnCharacterDeSelectEvent");
        onCharacterDeSelectEvent.AddListener(listener);
    }

    public void StopListeningToOnCharacterDeSelectEvent(UnityAction<Player> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnCharacterDeSelectEvent");
        onCharacterDeSelectEvent.RemoveListener(listener);
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

    public void InvokeOnDeathEvent(GameObject entity) {
        Debug.Log("OnDeathEvent Invoked: " + entity.name);
        onDeathEvent.Invoke(entity);
    }

    public void StartListeningToOnDeathEvent(UnityAction<GameObject> listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnDeathEvent");
        onDeathEvent.AddListener(listener);
    }

    public void StopListeningToOnDeathEvent(UnityAction<GameObject> listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnDeathEvent");
        onDeathEvent.RemoveListener(listener);
    }

    public void InvokeOnGameOverEvent() {
        Debug.Log("OnGameOverEvent Invoked");
        onGameOverEvent.Invoke();
    }

    public void StartListeningToOnGameOverEvent(UnityAction listener) {
        Debug.Log(listener.Method.Name + " Subscribed to OnGameOverEvent");
        onGameOverEvent.AddListener(listener);
    }

    public void StopListeningToOnGameOverEvent(UnityAction listener) {
        Debug.Log(listener.Method.Name + " Unsubscribed to OnGameOverEvent");
        onGameOverEvent.RemoveListener(listener);
    }
}
