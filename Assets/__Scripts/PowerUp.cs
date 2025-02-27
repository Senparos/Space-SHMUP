using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(BoundsCheck))]
public class PowerUp : MonoBehaviour{
    [Header("Inscribed")]
    [Tooltip("x holds a min val and y a maxc val for a randomrang call")]

    public Vector2 rotMinMax = new Vector2(15, 90);
    [Tooltip("x holds a min val and y a msame as above")]
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 10;
    public float fadeTime = 4;

    [Header("Dynamic")]
    public eWeaponType _type; //changed from type in the book
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Material cubeMat;


    // Start is called before the first frame update
    void Awake()
    {
        cube = transform.GetChild(0).gameObject;

        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeMat = GetComponent<Renderer>().material;

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax[0], rotMinMax[1]),
                                   Random.Range(rotMinMax[0], rotMinMax[1]),
                                   Random.Range(rotMinMax[0], rotMinMax[1]));

        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if(u>=1)
        {
            Destroy(this.gameObject);
            return;
        }

        if(u>0)
        {
            Color c = cubeMat.color;
            c.a = 1f - u;
            cubeMat.color = c;

            c = letter.color;
            c.a = 1f - (u * .5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }

    public eWeaponType type { get { return _type; } set { SetType(value); } }

    public void SetType(eWeaponType wt)
    {
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION(wt);
        cubeMat.color = def.powerUpColor; 
        letter.text = def.letter;
        _type = wt;  /// changed from _type

    }

    public void AbsorbedBy(GameObject target)
    {

        Destroy ( this.gameObject );

    }



































}
