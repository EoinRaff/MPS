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

    // Update is called once per frame
    void Update()
    {
        FluidDensity.text = string.Format ("Fluid Density: {0:0.00}", FluidDensitySlider.value);
        fluid.Density = FluidDensitySlider.value;

        DuckWeight.text = string.Format("Duck Mass: {0:0.00}", DuckWeightSlider.value);
        duckRB.mass = DuckWeightSlider.value;

        DisplacedFluid.text = string.Format("Displaced Fluid Mass: {0:0.00}", duck.DisplacedFluidVolume * FluidDensitySlider.value);

        DuckDensity.text = string.Format("Duck Density: {0:0.00}", duck.Density);
    }
}
