using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxContainer : MonoBehaviour
{
    Dictionary<string, IHitbox> hitboxes;
    List<IHitbox> activatedHitboxes;

    // Start is called before the first frame update
    void Start()
    {
        hitboxes = new Dictionary<string, IHitbox>();
        IHitbox[] list = GetComponentsInChildren<IHitbox>();
        foreach (IHitbox hitbox in list) {
            hitboxes.Add(hitbox.GetName(), hitbox);
            //Debug.Log(hitbox.GetName());
        }
        activatedHitboxes = new List<IHitbox>();
    }

    public List<IHitbox> GetActivatedHitboxes() {
        return activatedHitboxes;
    }

    public void ActivateHitbox(string hitboxName) {
        if (hitboxes.Count <= 0) {
            return;
        }
        //Debug.Log("Activating " + hitboxGroupName);
        activatedHitboxes.Add(hitboxes[hitboxName]);
        hitboxes[hitboxName].Enable();
    }

    public void DeactivateHitbox(string hitboxName) {
        if (hitboxes.Count <= 0) {
            return;
        }
        activatedHitboxes.Remove(hitboxes[hitboxName]);
        hitboxes[hitboxName].Reset();
    }

    public void DeactivateAllHitbox() {
        foreach (IHitbox hitbox in activatedHitboxes) {
            hitbox.Reset();
        }
        activatedHitboxes.Clear();
    }
}
