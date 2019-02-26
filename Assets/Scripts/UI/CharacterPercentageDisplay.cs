using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterPercentageDisplay : MonoBehaviour
{
    public TextMeshPro textMesh;
    public IDamageable damageable;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        damageable = GetComponentInParent<IDamageable>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = damageable.GetDamage().ToString() + "%";
    }
}
