using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSliderController : MonoBehaviour
{
    public BlendShapeSource[] blendShapeSources;

    private Slider sliderComponent;

    private void Awake()
    {
        sliderComponent = GetComponentInChildren<Slider>();
    }

    public void OnSliderValueChanged()
    {
        Debug.Log($"OnSliderValueChanged {sliderComponent.value}");
    }
}
