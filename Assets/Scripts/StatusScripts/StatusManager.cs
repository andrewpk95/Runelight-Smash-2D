using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusManager : FreezeBehaviour
{
    public List<IStatus> statuses;
    List<IStatus> toRemoveStatuses;
    
    // Start is called before the first frame update
    void Awake()
    {
        statuses = new List<IStatus>();
        toRemoveStatuses = new List<IStatus>();
        EventManager.instance.StartListeningToOnDeathEvent(new UnityAction<GameObject>(OnDeath));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Tick();
    }

    //Freeze Behaviour overrides

    protected override void UpdateOtherBehaviour() {
        UpdateStatus();
    }

    //Status Functions

    protected void UpdateStatus() {
        //Process each status tick
        foreach(IStatus status in statuses) {
            status.OnStatusStay(this.gameObject);
            if (status.EndStatus) toRemoveStatuses.Add(status);
        }
        //Remove status that requested to be removed
        foreach(IStatus status in toRemoveStatuses) {
            RemoveStatus(status);
        }
        toRemoveStatuses.Clear();
    }

    public void AddStatus(IStatus status) {
        if (statuses.Contains(status)) return;
        statuses.Add(status);
        status.OnStatusEnter(this.gameObject);
    }

    public void RemoveStatus(IStatus status) {
        if (status == null) return;
        status.OnStatusExit(this.gameObject);
        statuses.Remove(status);
    }

    public void InterruptStatus(IStatus status) {
        if (status == null) return;
        status.OnStatusInterrupt(this.gameObject);
        statuses.Remove(status);
    }

    public void InterruptAll() {
        foreach (IStatus status in statuses) {
            status.OnStatusInterrupt(this.gameObject);
        }
        foreach (IStatus status in statuses) {
            if (!status.IsPermanent) toRemoveStatuses.Add(status);
        }
        foreach (IStatus status in toRemoveStatuses) {
            statuses.Remove(status);
        }
    }

    //Event Listener Functions

    public void OnDeath(GameObject entity) {
        if (entity.Equals(this.gameObject)) {
            InterruptAll();
        }
    }
}
