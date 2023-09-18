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

    //The default value of the slider if you don't want to start at 0.  (Such as for Slider - Fem)
    [Range(0.0f, 100.0f)]
    public float defaultValue;

    //This is a list of blendShapeInfo for all attributes bound to THIS AttributeSlider based on the info in THIS slider's attributeSliderBindings property
    private List<BlendShapeInfo> blendShapeInfoListForAttributes;

    //This could be public if you ever have another character you want to use
    private CharacterInfoController characterInfoController;

    private TextMeshProUGUI textAttributeTitle;

    //The Attribute Slider Bindings visible in and assignable from the Inspector
    //(until populated from some other datasource in future, like JSON, DB, or webservice).
    public AttributeSliderBinding[] attributeSliderBindings;

    //The same as the above, but stored in a sorted dictionary by attribute for quick and easy reference.
    private SortedDictionary<string, AttributeSliderBinding> attributeSliderBindingSortedDictionary = new();


    private Slider sliderComponent;

    private void Awake()
    {
        InitializeCharacterInfoController();
    }

    private void Start()
    {
        BindAttributesToList();
        InitializeSlider();
        InitializeText();
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

    /// <summary>
    /// Initialize the slider
    /// </summary>
    private void InitializeSlider()
    {
        sliderComponent = GetComponentInChildren<Slider>();
        //!!!sliderComponent.SetValueWithoutNotify(defaultValue);    //Don't just set sliderComponent.value or you'll trigger OnSliderValueChanged() too soon.
        sliderComponent.value = defaultValue;
    }

    private void InitializeText()
    {
        textAttributeTitle = gameObject.GetComponentInChildren<TextMeshProUGUI>();  //Note! Currently assumes only 1 TextMeshProUGUI exists in children
        if (!string.IsNullOrEmpty(sliderTitle))
        {
            textAttributeTitle.text = sliderTitle;
        }
    }

    /// <summary>
    /// Populate blendShapeInfoListForAttributes (via appending) with all of the blend shape bindings for ONE attribute.
    /// </summary>
    /// <param name="attributeToBind"></param>
    private void AppendAttributeBindingToList(string attributeToBind)
    {
        blendShapeInfoListForAttributes.AddRange(characterInfoController.GetBlendShapesForAttribute(attributeToBind));
    }

    /// <summary>
    /// Populate blendShapeInfoListForAttributes with all of the blend shape bings bindings for ALL attributes bound to this slider.
    /// Also populates attributeSliderBindingSortedDictionary so we can efficiently access the other properties of a binding besides the attribute name;
    /// </summary>
    private void BindAttributesToList()  //!!!
    {
        blendShapeInfoListForAttributes = new();
        foreach (AttributeSliderBinding attributeSliderBinding in attributeSliderBindings)
        {
            AppendAttributeBindingToList(attributeSliderBinding.attribute);
            attributeSliderBindingSortedDictionary.Add(attributeSliderBinding.attribute, attributeSliderBinding);
        }
    }

    /// <summary>
    /// Handle a change in slider value by adjusting all blend shapes bound to this slider.
    /// </summary>
    public void OnSliderValueChanged()
    {
        AttributeSliderBinding currentattributeSliderBinding;
        float reverseEffectadjustment = sliderComponent.maxValue - sliderComponent.minValue;  //precalc in case you want the opposite effect from the slider

        //Adjust all blend shapes with the attribute
        foreach (BlendShapeInfo blendShapeInfo in blendShapeInfoListForAttributes)
        {
            currentattributeSliderBinding = attributeSliderBindingSortedDictionary[blendShapeInfo.attribute];
            if (currentattributeSliderBinding.reverseSliderEffect)
            {
                blendShapeInfo.skinnedMeshRenderer.SetBlendShapeWeight(blendShapeInfo.blendShapeIndex, (reverseEffectadjustment - sliderComponent.value));
            }
            else
            {
                blendShapeInfo.skinnedMeshRenderer.SetBlendShapeWeight(blendShapeInfo.blendShapeIndex, sliderComponent.value);
            }
        }
    }
}
