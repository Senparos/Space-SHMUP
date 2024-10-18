using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class BlinkColorOnHit : MonoBehaviour{
    private static float blinkDuration = .1f;
    private static Color blickColor = Color.red;

    [Header("Dynamic")]
    public bool showingColor = false;
    public float blinkCompleteTime;
    public bool ignoreOnCollisionEnter = false;

    private Material[] materials;
    private Color[] origionalColors;
    private BoundsCheck bndCheck;


    
    void Awake()
    {
        bndCheck = GetComponentInParent<BoundsCheck>();
        //get mats and colors for this game object and children
        materials = Utils.GetAllMaterials(gameObject);
        origionalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            origionalColors[i] = materials[i].color;
        }
    }

     void Update()
    {
        if (showingColor && Time.time > blinkCompleteTime) RevertColors();

    }

    private void OnCollisionEnter(Collision coll)
    {
        if (ignoreOnCollisionEnter) return;

        ProjectileHero p = coll.gameObject.GetComponent<ProjectileHero>(); 
        if (p != null)
        {
            if(bndCheck != null && !bndCheck.isOnScreen)
            {
                return;
            }
            SetColors();
        }
    }

    public void SetColors()
    {
        foreach(Material m in materials) {
            m.color = blickColor;
        }
        showingColor = true;
        blinkCompleteTime = Time.time + blinkDuration;
    }

    void RevertColors()
    {
        for (int i = 0;i < materials.Length;i++)
        {
            materials[i].color = origionalColors[i];
        }
        showingColor = false;
    }
















}
