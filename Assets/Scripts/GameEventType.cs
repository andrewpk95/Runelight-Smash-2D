using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameEventType {
    OnHitEvent, 
    OnGrabEvent, 
    OnDamageEvent, 
    OnHitStunEvent, 
    OnEdgeGrabEvent, 
    OnDeathEvent,
    OnGameOverEvent
}

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

[System.Serializable]
public class OnDeathEvent : UnityEvent<GameObject>
{
}

[System.Serializable]
public class OnGameOverEvent : UnityEvent
{
}