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

    public WaterVolume fluid;

    private bool submerged;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        MassVolumeDensity();
        Debug.Log(string.Format("Mass: {0}, Volume: {1}, Density {2}", Mass, Volume, Density));
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
        float depth =  Depth();

        float displacedVolume = SubmergedVolume(depth);

        Vector3 buoyantForce = Physics.gravity * -1 * (displacedVolume * fluid.density);
        rb.velocity += buoyantForce * Time.fixedDeltaTime;
//        rb.AddForce(buoyantForce, ForceMode.Acceleration);
        //rb.AddForceAtPosition()//geometric center of submerged area
//        Debug.Log(string.Format("Volume: {0}, Submerged Volume: {1}", Volume, displacedVolume));

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
            Density = SphereDensity(sphereCollider.radius);
        }
        Volume = Mass / Density;
    }

    private float SphereDensity(float radius)
    {
        return (4 / 3) * Mathf.PI * Mathf.Pow(radius, 3);
    }

    private float SubmergedVolume(float depth)
    {
        if (depth >= 2 * sphereCollider.radius)
            return Volume;

        float _3r2 = 3 * Mathf.Pow(sphereCollider.radius, 2);
        float _h2 = Mathf.Pow(depth, 2);

        return Volume * ((Mathf.PI * depth * (_3r2 + _h2)) / 6);
    }
}
