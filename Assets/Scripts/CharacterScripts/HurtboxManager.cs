using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxManager : MonoBehaviour
{
    public List<GameObject> hurtboxes;
    public List<SpriteRenderer> sprites;
    public int layer;

    public Color color;
    bool flash;
    
    // Start is called before the first frame update
    void Start()
    {
        hurtboxes = new List<GameObject>();
        sprites = new List<SpriteRenderer>();
        Transform[] list = GetComponentsInChildren<Transform>();
        foreach (Transform trans in list) {
            if (trans.gameObject.tag == "Hurtbox") {
                hurtboxes.Add(trans.gameObject);
                sprites.Add(trans.gameObject.GetComponent<SpriteRenderer>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFlashingAnimation();
    }

    void UpdateFlashingAnimation() {
        if (flash) {
            ChangeSpriteColor(color);
            flash = false;
        }
        else {
            ChangeSpriteColor(Color.white);
            flash = true;
        }
    }

    public void SetIntangible(bool intangible) {
        if (intangible) {
            if (layer == LayerMask.NameToLayer("IntangibleHurtbox")) return;
            SetHurtboxLayer(LayerMask.NameToLayer("IntangibleHurtbox"));
        }
        else {
            if (layer == LayerMask.NameToLayer("Hurtbox")) return;
            SetHurtboxLayer(LayerMask.NameToLayer("Hurtbox"));
        }
    }

    void SetHurtboxLayer(int layerMask) {
        foreach (GameObject hurtbox in hurtboxes) {
            hurtbox.layer = layerMask;
        }
    }

    public void Flash(Color color) {
        
    }

    void ChangeSpriteColor(Color color) {
        foreach (SpriteRenderer sprite in sprites) {
            sprite.color = color;
        }
    }
}
