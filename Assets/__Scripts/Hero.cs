using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero      S {  get; private set; }//singleton property //a

    [Header("Inscribed")]

    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;

    [Header("Dynamic")]     [Range(0, 4)]    [SerializeField]

    private float _shieldLevel = 1;
    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;

    public delegate void WeaponFireDelagate();

    public event WeaponFireDelagate fireEvent;


    private void Awake()
    {
        if (S == null)
        {
            S = this;// set the singleton on ly if its null
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attemted to assign second Hero.S!");
        }
        //fireEvent += TempFire;

        // reset the weapons to start heero with 1 blaster
        ClearWeapons();
        weapons[0].SetType(eWeaponType.blaster);
    }


    // Update is called once per frame
    void Update()
    {
        //pull in info from input cl,ass
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        //Change transform.pos based on azed
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;//
        pos.y += vAxis * speed * Time.deltaTime; //
        transform.position = pos;

        // totate the shit to make it feel dynamic
        transform.rotation = Quaternion.Euler(vAxis * pitchMult, hAxis * rollMult, 0);

        //allow shit to fire
        if (Input.GetAxis("Jump")== 1 && fireEvent != null)
        {
            fireEvent();
        }

    }

//    void TempFire()
//    {
//        GameObject projGo=Instantiate<GameObject>(projectilePrefab);
//        projGo.transform.position = transform.position;
//        Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
//        
//        ProjectileHero proj = projGo.GetComponent<ProjectileHero>();
//        proj.type = eWeaponType.blaster;
//        float tSpeed = Main.GET_WEAPON_DEFINITION(proj.type).velocity;
//        rigidB.velocity = Vector3.up * tSpeed;
//
    //}

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        PowerUp pUp = go.GetComponent<PowerUp>();

        if (enemy != null) // if the sheild was triggered by and enemy dec the level of sheiild by 1
        {
            shieldLevel--;
            Destroy(go);
        }
        else if(pUp != null)
        {
            AbsorbPowerUp(pUp);
        }
        else
        {
            Debug.LogWarning("Sheild trigger hit by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(PowerUp pUp)
    {
        Debug.Log("Absorbed PowerUp; "+ pUp.type);
        switch (pUp.type)
        {
            case eWeaponType.sheild:
                shieldLevel++;
                break;

            default:
                if(pUp.type == weapons[0].type)
                {
                    Weapon weap = GetEmptyWeaponSlot();
                    if(weap != null)
                    {
                        weap.SetType(pUp.type); 
                    }
                } else
                {
                    ClearWeapons();
                    weapons[0].SetType(pUp.type);
                }
                break;
        }
        pUp.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel
    {

        get { return (_shieldLevel); }
        private set
        {
            _shieldLevel = Mathf.Min(value, 4);
            //if the shidl is goign to be set to less that 0 
            if (value < 0)
            {
                Destroy(this.gameObject); //destroys the hero if you run out of shields
                Main.HERO_DIED();   
            }
        }
    }
    Weapon GetEmptyWeaponSlot()
    {
        for(int i=0; i < weapons.Length; i++)
        {
            if (weapons[i].type == eWeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }


   void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(eWeaponType.none);
        }
    }











}
