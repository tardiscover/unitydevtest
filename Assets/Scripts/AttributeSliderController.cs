using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Used for each AttributeSlider.  Set public properties in the inspector to determine title and attribute controlled.
/// (Attributes are collections of blend shapes that share a suffix.  They are initialized by CharacterInfoController.cs.)
/// </summary>
public class AttributeSliderController : MonoBehaviour
{
    public string sliderTitle;
    public string blendShapeInfoAttribute;

    [Range(0.0f, 100.0f)]
    public float defaultValue;

    private List<BlendShapeInfo> blendShapeInfoListForAttribute;

    private CharacterInfoController characterInfoController;    //This could be public if you ever have another character you want to use

    private TextMeshProUGUI textAttributeTitle;

    public BlendShapeSource[] blendShapeSources;

    private Slider sliderComponent;

    private void Awake()
    {
        InitializeCharacterInfoController();
        InitializeSlider();
        InitializeText();
    }

    private void Start()
    {
        LinkToAttribute();
    }

    /// <summary>
    /// If characterInfoController isn't already specified, set it to the default (found by key).
    /// </summary>
    private void InitializeCharacterInfoController()
    {
        if (characterInfoController == null)
        {
            GameObject character = GameObject.FindGameObjectWithTag("DefaultCharacter");
            if (character != null)
            {
                characterInfoController = character.GetComponent<CharacterInfoController>();
            }
        }
    }

    private void InitializeSlider()
    {
        sliderComponent = GetComponentInChildren<Slider>();
        sliderComponent.SetValueWithoutNotify(defaultValue);    //Don't just set sliderComponent.value or you'll trigger OnSliderValueChanged() too soon.
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
