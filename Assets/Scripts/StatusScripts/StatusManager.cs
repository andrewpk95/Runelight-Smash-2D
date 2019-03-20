using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : FreezeBehaviour
{
    List<IStatus> statuses;
    List<IStatus> toRemoveStatuses;
    
    // Start is called before the first frame update
    void Awake()
    {
        statuses = new List<IStatus>();
        toRemoveStatuses = new List<IStatus>();
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
}
