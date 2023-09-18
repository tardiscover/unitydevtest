using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller for the rotation slider.
/// </summary>
public class RotationSliderController : MonoBehaviour
{
    public GameObject character;
    private Slider sliderComponent;

    private void Start()
    {
        sliderComponent = GetComponentInChildren<Slider>();
    }

    /// <summary>
    /// Handle a change in rotation slider by spinning the character on the Y axis.
    /// </summary>
    public void OnSliderValueChanged()
    {
        character.transform.rotation = Quaternion.Euler(0, -sliderComponent.value, 0);
    }
}
