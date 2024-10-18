using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float powerUpDropChance = 1f;

    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
        {
            Destroy(gameObject);
        }
        //if(!bndCheck.isOnScreen)
        //{
           /// if (pos.y < bndCheck.camHeight - bndCheck.radius) 
           // { 
            //    Destroy(gameObject);
           // }
       // }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        // Check for collisions with ProjectileHero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null)
        {
            if (bndCheck.isOnScreen)
            {
                // Get damage from weapon definition
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;

                // If health is depleted, destroy the enemy
                if (health <= 0)
                {
                    if(!calledShipDestroyed)
                    {
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    //destroy this enemy
                    Destroy(this.gameObject);
                }
            }

            // Destroy the projectile regardless
            Destroy(otherGO);
        }
        else
        {
            // If it’s not a ProjectileHero, print a debug message
            print("Enemy hit by non-projectile hero: " + otherGO.name);
        }
    }













}
