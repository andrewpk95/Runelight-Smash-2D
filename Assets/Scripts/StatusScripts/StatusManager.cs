using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    List<IStatus> statuses;
    
    // Start is called before the first frame update
    void Awake()
    {
        statuses = new List<IStatus>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(IStatus status in statuses) {
            status.OnStatusStay(this.gameObject);
        }
    }

    public void AddStatus(IStatus status) {
        statuses.Add(status);
        status.OnStatusEnter(this.gameObject);
    }

    public void RemoveStatus(IStatus status) {
        status.OnStatusExit(this.gameObject);
        statuses.Remove(status);
    }
}
