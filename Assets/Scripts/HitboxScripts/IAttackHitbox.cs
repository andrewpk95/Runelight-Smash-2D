using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHitbox : IHitbox
{
    IHitboxStat Stats {get; set;}
}
