using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultihitHitbox : AttackHitbox, IFreezable
{
    public int multihitIntervalFrame;
    public int hitIntervalFrameLeft;

    [SerializeField] protected bool isFrozen;
    public bool IsFrozen {get {return isFrozen;} set {isFrozen = value;}}
    public int freezeFrameLeft;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();
        EventManager.instance.StartListeningToOnFreezeEvent(this.gameObject, new UnityAction<GameObject, int>(OnFreeze));
        if (multihitIntervalFrame <= 0) {
            multihitIntervalFrame = 120;
        }
        hitIntervalFrameLeft = multihitIntervalFrame;
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
        //Clear victim list every cycle
        if (hitbox.enabled) {
            hitIntervalFrameLeft--;
            if (hitIntervalFrameLeft <= 0) {
                collisions.Clear();
                hitboxGroup.Victims.Clear();
                hitIntervalFrameLeft = multihitIntervalFrame;
            }
        }
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

    public override void Reset() {
        base.Reset();
        hitIntervalFrameLeft = multihitIntervalFrame;
    }

    void OnDisable() {
        Reset();
        EventManager.instance.UnsubscribeAll(this.gameObject);
    }
}
