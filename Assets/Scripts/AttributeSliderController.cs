using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttributeSliderController : MonoBehaviour
{
    public string sliderTitle;
    public string blendShapeInfoAttribute;
    private List<BlendShapeInfo> blendShapeInfoListForAttribute;

    public CharacterInfoController characterInfoController;

    private TextMeshProUGUI textAttributeTitle;

    public BlendShapeSource[] blendShapeSources;

    private Slider sliderComponent;

    private void Awake()
    {
        sliderComponent = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        InitializeText();
        LinkToAttribute();
    }

    private void InitializeText()
    {
        textAttributeTitle = gameObject.GetComponentInChildren<TextMeshProUGUI>();  //Note! Currently assumes only 1 TextMeshProUGUI exists in children
        if (!string.IsNullOrEmpty(sliderTitle))
        {
            textAttributeTitle.text = sliderTitle;
        }
    }
    private void LinkToAttribute()
    {
        blendShapeInfoListForAttribute = characterInfoController.GetBlendShapesForAttribute(blendShapeInfoAttribute);
    }

    public void OnSliderValueChanged()
    {
        //Adjust all blend shapes with the attribute
        foreach (BlendShapeInfo blendShapeInfo in blendShapeInfoListForAttribute)
        {
            blendShapeInfo.skinnedMeshRenderer.SetBlendShapeWeight(blendShapeInfo.blendShapeIndex, sliderComponent.value);
        }

        //ALSO adjust any specific blend shapes added in the inspector that might not include the attribute
        int blendShapeIndex;
        foreach (BlendShapeSource blendShapeSource in blendShapeSources)
        {
            if (!string.IsNullOrEmpty(blendShapeSource.blendShapeName))
            {
                blendShapeIndex = blendShapeSource.skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeSource.blendShapeName);  //!!!

                if (blendShapeIndex >= 0)
                {
                    blendShapeSource.skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, sliderComponent.value);
                }
                else
                {
                    Debug.Log($"OnSliderValueChanged - No blendShapeIndex for {blendShapeSource.skinnedMeshRenderer.name}, {blendShapeSource.blendShapeName}"); //!!!
                }
            }
        }
    }
}
