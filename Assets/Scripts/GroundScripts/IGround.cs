using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGround
{
    void IgnoreCollision(Collider2D fighter, bool ignore);
}
