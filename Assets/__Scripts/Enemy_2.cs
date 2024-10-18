using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy{

    [Header("Enemy_2 Inscribed Fields")]
    public float lifeTime = 10;

    // enemy 2 uses sine wave to modifyu the interpol
    [Tooltip("Determins how much the Sine wave will ease the interpolation")]
    public float sinEccentricity = .6f;
    public AnimationCurve rotCurve;


    [Header("Enemy_2 Private Fields")]
    [SerializeField] private float birthTime;
    [SerializeField] private Vector3 p0, p1;

    private Quaternion baseRotation;

    private void Start()
    {
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //Pick any point on the right side of the screen
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        // possible swap sides
        if(Random.value > .5f)
        {
            p0.x *= -1;
            p0.y *= -1;
        }

        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //Use the animationcurve to se the rotation about y
        float shipRot = rotCurve.Evaluate(u) * 360;
        transform.rotation = baseRotation * Quaternion.Euler(-shipRot, 0, 0);


        //Adjusts u by adding a U curve based on the sine wave
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

        pos = (1-u) * p0 + u * p1;

    }





















}
