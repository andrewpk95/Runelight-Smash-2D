using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, IHurtbox
{
    public GameObject owner;
    ICharacter ownerCharacter;
    IDamageable ownerDamageable;
    public HurtboxManager manager;
    public Collider2D hurtbox;
    public Material material;

    private const string SHADER_COLOR_NAME = "_Color";
    
    // Start is called before the first frame update
    void Start()
    {
        owner = this.gameObject.transform.root.gameObject;
        manager = GetComponentInParent<HurtboxManager>();
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLayer(int layer) {
        this.gameObject.layer = layer;
    }

    public void ChangeSpriteColor(Color color) {
        material.SetColor(SHADER_COLOR_NAME, color);
    }

    public GameObject GetOwner() {
        return owner;
    }
}
