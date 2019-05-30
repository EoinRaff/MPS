using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
    [SerializeField]
    private float _density;

    public float Density { get => _density; set => _density = value; }
}
