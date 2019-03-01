using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    void OnStatusEnter(GameObject entity);

    void OnStatusStay(GameObject entity);

    void OnStatusExit(GameObject entity);
}
