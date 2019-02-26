using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatform
{
    void IgnorePlatformCollision(Collider2D fighter, bool ignore);

    void IgnoreEdgeCollision(Collider2D fighter, bool ignore);

    void IgnoreAllCollision(Collider2D fighter, bool ignore);
}
