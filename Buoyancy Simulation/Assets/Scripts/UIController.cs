using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public WaterVolume fluid;
    public Rigidbody duckRB;
    public FloatingObject duck;

    public TextMeshProUGUI FluidDensity;
    public Slider FluidDensitySlider;

    public TextMeshProUGUI DuckWeight;
    public Slider DuckWeightSlider;

    public TextMeshProUGUI DisplacedFluid;
    public TextMeshProUGUI DuckDensity;

    public TextMeshProUGUI Force;

    // Update is called once per frame
    void Update()
    {
        FluidDensity.text = string.Format ("Fluid Density: {0:0.##}g/cm^3", FluidDensitySlider.value);
        fluid.Density = FluidDensitySlider.value;

        DuckWeight.text = string.Format("Duck Mass: {0:0.##}g", DuckWeightSlider.value);
        duckRB.mass = DuckWeightSlider.value;

        DisplacedFluid.text = string.Format("Displaced Fluid Mass: {0:0.##}g", duck.DisplacedFluidVolume * FluidDensitySlider.value);

        DuckDensity.text = string.Format("Duck Density: {0:0.##}g/cm^3", duck.Density);

        Force.text = string.Format("Buoyancy: {0:0.##}N", duck.Buoyancy);
    }
}
