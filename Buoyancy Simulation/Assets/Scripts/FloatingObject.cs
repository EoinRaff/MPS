using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private AudioSource audioSource;

    public enum Shape { Sphere, Rectangle}
    public Shape shape;

    public float Density { get; private set; }
    public float Volume { get; private set; }
    public float Mass { get; private set; }

    public float DisplacedFluidVolume { get; private set; }

    Vector3 buoyantForce;

    public float Buoyancy { get; private set; }

    public WaterVolume fluid;
    private bool submerged;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();

        Mass = rb.mass;

        if (shape == Shape.Sphere)
            Volume = SphereVolume(sphereCollider.radius);

        Density = CalculateDensity(Mass);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            submerged = true;
        }
        if (other.tag == "Floor")
        {
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            if (transform.position.y + sphereCollider.radius <= 0)
            {
                submerged = true;
                audioSource.Play();
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
        Density = CalculateDensity(Mass);
        float depth =  CalculateDepth();

        DisplacedFluidVolume = SubmergedVolume(depth);

        buoyantForce = Physics.gravity * -1 * (DisplacedFluidVolume * fluid.Density);
        Buoyancy = buoyantForce.magnitude;

        rb.velocity += (Physics.gravity * Mass * Time.fixedDeltaTime) + (buoyantForce * Time.fixedDeltaTime);

    }

    private void DebugValues()
    {
        Debug.Log("Mass: " + Mass + ", Displaced Fluid Mass: " + fluid.Density * SubmergedVolume(CalculateDepth()));
    }

    private float CalculateDepth()
    {
        if (!submerged)
            return 0;

        Vector3 radiusY = new Vector3(0, sphereCollider.radius, 0);
        Vector3 bottomPoint = transform.position - radiusY;
        return Mathf.Abs(bottomPoint.y);
    }

    private float CalculateDensity(float mass)
    {
        return mass / Volume;
    }

    private float SphereVolume(float radius)
    {
        return 4f / 3f * Mathf.PI * Mathf.Pow(radius, 3);
    }

    private float SubmergedVolume(float depth)
    {
        if (depth >= 2 * sphereCollider.radius)
            return Volume;

        float t1 = 3 * sphereCollider.radius - depth;// * Mathf.Pow(depth, 3);
        float t2 = Mathf.Pow(depth, 2);

        return Mathf.PI * t2 * (t1) / 3;
    }
}
