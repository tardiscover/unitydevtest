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

    private List<BlendShapeInfo> blendShapeInfoListForAttributes;    //This is a list of blendShapeInfo for all attributes bound to THIS AttributeSlider based on the info in THIS slider's attributeSliderBindings property

    private CharacterInfoController characterInfoController;    //This could be public if you ever have another character you want to use

    private TextMeshProUGUI textAttributeTitle;

    public AttributeSliderBinding[] attributeSliderBindings;

    private Slider sliderComponent;

    private void Awake()
    {
        InitializeCharacterInfoController();
    }

    private void Start()
    {

        BindAttributesToList(ref blendShapeInfoListForAttributes);
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

    private void AppendAttributeBindingToList(ref List<BlendShapeInfo> blendShapeInfoList, string attributeToBind)
    {
        Debug.Log($"************{name} {attributeToBind}, {characterInfoController.GetBlendShapesForAttribute(attributeToBind).Count}");  //!!!!!!!!!!
        blendShapeInfoList.AddRange(characterInfoController.GetBlendShapesForAttribute(attributeToBind));
        //Debug.Log($"{name} {attributeToBind}, blendShapeInfoList.Count = {blendShapeInfoList.Count}"); //!!!!!!!!!!!!
    }

    private void BindAttributesToList(ref List<BlendShapeInfo> blendShapeInfoList)  //!!!
    {
        //Debug.Log($"BindAttributesToList {name}, blendShapeInfoList.Count = {blendShapeInfoList.Count}, attributeSliderBindings.Length = {attributeSliderBindings.Length}"); //!!!!!!!!!!!!
        blendShapeInfoList = new();
        foreach (AttributeSliderBinding attributeSliderBinding in attributeSliderBindings)
        {
            AppendAttributeBindingToList(ref blendShapeInfoList, attributeSliderBinding.attribute);
        }
        Debug.Log($"***BindAttributesToList {name}, blendShapeInfoList.Count = {blendShapeInfoList.Count}, attributeSliderBindings.Length = {attributeSliderBindings.Length}"); //!!!!!!!!!!!!
    }

    public void OnSliderValueChanged()
    {
        Debug.Log($"OnSliderValueChanged blendShapeInfoListForAttributes {blendShapeInfoListForAttributes.Count}");    //!!!!!!!!!!!!!!!
        //Adjust all blend shapes with the attribute
        foreach (BlendShapeInfo blendShapeInfo in blendShapeInfoListForAttributes)
        {
            blendShapeInfo.skinnedMeshRenderer.SetBlendShapeWeight(blendShapeInfo.blendShapeIndex, sliderComponent.value);
        }
    }
}
