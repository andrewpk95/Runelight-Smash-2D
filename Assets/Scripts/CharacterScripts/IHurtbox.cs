using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtbox
{
    void SetLayer(int layer);

    void ChangeSpriteColor(Color color);

    GameObject GetOwner();
}
