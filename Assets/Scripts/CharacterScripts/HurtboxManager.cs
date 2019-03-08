using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxManager : MonoBehaviour
{
    public List<GameObject> hurtboxes;
    public List<Material> spriteMaterials;
    public int layer;

    private const string SHADER_COLOR_NAME = "_Color";
    
    // Start is called before the first frame update
    void Start()
    {
        hurtboxes = new List<GameObject>();
        spriteMaterials = new List<Material>();
        Transform[] list = GetComponentsInChildren<Transform>();
        foreach (Transform trans in list) {
            if (trans.gameObject.tag == "Hurtbox") {
                hurtboxes.Add(trans.gameObject);
                spriteMaterials.Add(trans.gameObject.GetComponent<SpriteRenderer>().material);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetHurtboxLayer(int layerMask) {
        foreach (GameObject hurtbox in hurtboxes) {
            hurtbox.layer = layerMask;
        }
    }

    public void ChangeSpriteColor(Color color) {
        foreach (Material material in spriteMaterials) {
            material.SetColor(SHADER_COLOR_NAME, color);
        }
    }

    public void StartFlashing(Color color1, Color color2, float tick) {
        StartCoroutine(FlashRoutine(color1, color2, tick));
    }

    public void StopFlashing() {
        StopAllCoroutines();
        ChangeSpriteColor(new Color(1, 1, 1, 0));
    }

    IEnumerator FlashRoutine(Color color1, Color color2, float tick) {
        while (true) {
            ChangeSpriteColor(color1);
            yield return new WaitForSeconds(tick);
            ChangeSpriteColor(color2);
            yield return new WaitForSeconds(tick);
        }
    }
}
