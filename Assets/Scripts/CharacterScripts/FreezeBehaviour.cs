﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FreezeBehaviour : MonoBehaviour, IFreezable
{
    [SerializeField] protected bool isFrozen;
    public bool IsFrozen {get {return isFrozen;} set {isFrozen = value;}}
    protected int freezeFrameLeft;
    
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize() {
        EventManager.instance.StartListeningToOnFreezeEvent(this.gameObject, new UnityAction<GameObject, int>(OnFreeze));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Tick();
    }

    protected virtual void Tick() {
        if (IsFrozen) UpdateFreezeFrame();
        else UpdateOtherBehaviour();
    }

    protected virtual void UpdateOtherBehaviour() {

    }

    protected virtual void UpdateFreezeFrame() {
		freezeFrameLeft--;
		if (freezeFrameLeft <= 0){ //FreezeFrame Over
			UnFreeze();
		}
    }

    protected virtual void OnFreeze(GameObject entity, int freezeFrameDuration) {
        if (entity.Equals(this.gameObject.transform.root.gameObject)) {
            if (!IsFrozen) { //If not frozen before
                Freeze();
            }
            freezeFrameLeft = freezeFrameDuration;
        }
    }

    protected virtual void Freeze() {
        //Debug.Log(this.gameObject.name + " Freeze!");
        IsFrozen = true;
    }

    protected virtual void UnFreeze() {
        //Debug.Log(this.gameObject.name + " Unfreeze!");
        freezeFrameLeft = 0;
        IsFrozen = false;
    }

    void OnDisable() {
        EventManager.instance.UnsubscribeAll(this.gameObject);
    }
}
