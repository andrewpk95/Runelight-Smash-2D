using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxManager : MonoBehaviour
{
    public List<IHurtbox> hurtboxes;
    public int layer;

    private Color defaultColor = new Color(1, 1, 1, 0);
    private const string SHADER_COLOR_NAME = "_Color";
    
    // Start is called before the first frame update
    void Start()
    {
        hurtboxes = new List<IHurtbox>();
        IHurtbox[] list = GetComponentsInChildren<IHurtbox>();
        foreach (IHurtbox element in list) {
            hurtboxes.Add(element);
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

    public void SetHurtboxLayer(int layerMask) {
        foreach (IHurtbox hurtbox in hurtboxes) {
            hurtbox.SetLayer(layerMask);
        }
    }

    public void ChangeSpriteColor(Color color) {
        foreach (IHurtbox hurtbox in hurtboxes) {
            hurtbox.ChangeSpriteColor(color);
        }
    }

    public void ResetSpriteColor() {
        ChangeSpriteColor(defaultColor);
    }

    public void StartFlashing(Color color1, Color color2, float tick) {
        StartCoroutine(FlashRoutine(color1, color2, tick));
    }

    public void StopFlashing() {
        StopAllCoroutines();
        ResetSpriteColor();
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
