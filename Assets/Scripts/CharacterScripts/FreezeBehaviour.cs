using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBehaviour : MonoBehaviour, IFreezable
{
    [SerializeField] protected bool isFrozen;
    public bool IsFrozen {get {return isFrozen;} set {isFrozen = value;}}
    protected int freezeFrameLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
		if (freezeFrameLeft < 0){ //FreezeFrame Over
			OnUnFreeze();
		}
    }

    public virtual void Freeze(int freezeFrameDuration) {
        if (!IsFrozen) { //If not frozen before
            OnFreeze();
        }
        freezeFrameLeft = freezeFrameDuration;
    }

    protected virtual void OnFreeze() {
        IsFrozen = true;
    }

    protected virtual void OnUnFreeze() {
        freezeFrameLeft = 0;
        IsFrozen = false;
    }
}
