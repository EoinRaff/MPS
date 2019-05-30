using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;

    public enum Shape { Sphere, Rectangle}
    public Shape shape;

    public float Density { get; private set; }
    public float Volume { get; private set; }
    public float Mass { get; private set; }

    public float DisplacedFluidVolume { get; private set; }

    Vector3 buoyantForce;

    public WaterVolume fluid;
    private bool submerged;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        MassVolumeDensity();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            submerged = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            if (transform.position.y + sphereCollider.radius <= 0)
            {
                submerged = true;
            }
            else
            {
                submerged = false;
            }
        }
    }

    void FixedUpdate()
    {
        Mass = rb.mass;

        float depth =  Depth();

        DisplacedFluidVolume = SubmergedVolume(depth);

        buoyantForce = Physics.gravity * -1 * (DisplacedFluidVolume * fluid.Density);
      

        rb.velocity += (Physics.gravity * Mass * Time.fixedDeltaTime) + (buoyantForce * Time.fixedDeltaTime);

        //DebugValues();
    }

    private void DebugValues()
    {
        Debug.Log("Mass: " + Mass + ", Displaced Fluid Mass: " + fluid.Density * SubmergedVolume(Depth()));
    }

    private float Depth()
    {
        if (!submerged)
            return 0;

        Vector3 radiusY = new Vector3(0, sphereCollider.radius, 0);
        Vector3 bottomPoint = transform.position - radiusY;
        return Mathf.Abs(bottomPoint.y);
    }

    private void MassVolumeDensity()
    {
        Mass = rb.mass;
        if (shape == Shape.Sphere)
        {
            Volume = SphereVolume(sphereCollider.radius);
        }
        Density = Mass / Volume; //not used but good to have
    }

    private float SphereVolume(float radius)
    {
        return 4f / 3f * Mathf.PI * Mathf.Pow(radius, 3);
    }

    private float SubmergedVolume(float depth)
    {
        if (depth >= 2 * sphereCollider.radius)
            return Volume;

        float _3r2 = 3 * Mathf.Pow(sphereCollider.radius, 2);
        float _h2 = Mathf.Pow(depth, 2);

        return Volume * (Mathf.PI * depth * (_3r2 + _h2) / 6);
    }
}
