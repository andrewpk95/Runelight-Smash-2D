using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    bool EndStatus {get; set;}
    bool IsPermanent {get; set;}
    
    void OnStatusEnter(GameObject entity);

    void OnStatusStay(GameObject entity);

    void OnStatusExit(GameObject entity);

    void OnStatusInterrupt(GameObject entity);
}
