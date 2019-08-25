using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEvent {

}

public enum GameEventType {
    OnPlayerJoinEvent,
    OnPlayerLeaveEvent,
    OnCharacterSelectEvent,
    OnCharacterDeSelectEvent,
    OnHitEvent, 
    OnFreezeEvent,
    OnGrabEvent, 
    OnDamageEvent, 
    OnHitStunEvent, 
    OnEdgeGrabEvent, 
    OnDeathEvent,
    OnGameOverEvent
}

[System.Serializable]
public class OnPlayerJoinEvent : UnityEvent<Player>, IEvent
{
}

[System.Serializable]
public class OnPlayerLeaveEvent : UnityEvent<Player>
{
}

[System.Serializable]
public class OnCharacterSelectEvent : UnityEvent<Player, CharacterType>
{
}

[System.Serializable]
public class OnCharacterDeSelectEvent : UnityEvent<Player>
{
}

[System.Serializable]
public class OnHitEvent : UnityEvent<IAttackHitbox, GameObject>
{
}

[System.Serializable]
public class OnFreezeEvent : UnityEvent<GameObject, int>
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