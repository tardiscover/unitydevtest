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
        Debug.Log($"OnSliderValueChanged {sliderComponent.value}"); //!!!

        int blendShapeIndex;

        foreach (BlendShapeSource blendShapeSource in blendShapeSources)
        {
            //Debug.Log($"OnSliderValueChanged - blendShapeCount = {blendShapeSource.skinnedMeshRenderer.sharedMesh.blendShapeCount}"); //!!!

            blendShapeIndex = blendShapeSource.skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeSource.blendShape);  //!!!

            if (blendShapeIndex >= 0)
            {
                blendShapeSource.skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, sliderComponent.value);
            }
            else
            {
                Debug.Log($"OnSliderValueChanged - No blendShapeIndex for {blendShapeSource.skinnedMeshRenderer.name}, {blendShapeSource.blendShape}"); //!!!
            }
        }
    }
}
