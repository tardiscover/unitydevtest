using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to easily access and categorize the parts and parents of a blendShape.
/// </summary>
public class BlendShapeInfo
{
    public SkinnedMeshRenderer skinnedMeshRenderer; //A SkinnedMeshRenderer on a child GameObject of the Character
    public string blendShapeName;   //A blend shape name within that SkinnedMeshRenderer
    public int blendShapeIndex;     //The blend shape index within that SkinnedMeshRenderer
    public string attribute;        //The "attribute" of the blend shape, based on the suffix of the name ignoring the body part.  Several body parts may have the same attribute, such as "body_muscular_heavy".
    public string bodyPart;         //The body part (parent object) of the blend shape.  A body part may have several blend shapes, each with a different attribute.
    public bool isComplete;         //A flag for convenience to represent that a BlendShapeInfo has all of its parts (particularly an attribute).  Stored for convenience and code clarity..

    /// <summary>
    /// Used to store data about a BlendShape, including its skinnedMeshRenderer, name, index, attribute, and bodyPart.
    /// 
    /// Constructor is passed its skinnedMeshRenderer and index.
    /// </summary>
    /// <param name="initSkinnedMeshRenderer"></param>
    /// <param name="initBlendShapeIndex"></param>
    public BlendShapeInfo(SkinnedMeshRenderer initSkinnedMeshRenderer, int initBlendShapeIndex)
    {
        skinnedMeshRenderer = initSkinnedMeshRenderer;

        if (skinnedMeshRenderer == null) {
            Debug.LogError("New BlendShapeInfo has null skinnedMeshRenderer!");
            blendShapeIndex = -1;
        }
        else if (initBlendShapeIndex >= skinnedMeshRenderer.sharedMesh.blendShapeCount)
        {
            Debug.LogError($"New BlendShapeInfo specified index of {initBlendShapeIndex} when blendShapeCount = {skinnedMeshRenderer.sharedMesh.blendShapeCount}!");
            blendShapeIndex = -1;
        }
        else
        {
            blendShapeIndex = initBlendShapeIndex;
            blendShapeName = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(blendShapeIndex);
            bodyPart = skinnedMeshRenderer.gameObject.name;
            attribute = CalcAttribute();
            isComplete = !string.IsNullOrEmpty(attribute);
        }
    }

    /// <summary>
    /// Calculates and return the attribute value (if any) based on the blendShapeName (which must already be populated) and other factors.
    /// </summary>
    /// <returns></returns>
    private string CalcAttribute()
    {
        string returnValue = "";    //Until/unless calculated otherwise

        string parentName = skinnedMeshRenderer.gameObject.name;
        //Debug.Log($"parentName = '{parentName}', expectedPrefix = '{expectedPrefix}'"); //!!!

        //!!! NOTE: The following shows that there is inconsistent BlendShape naming for eyelash_L, eyelash_R, teeth_canine_bot_L, and teeth_canine_bot_R.
        //string expectedPrefix = "bs_" + parentName + ".";
        //if (!StrFunc.StartsWith(blendShapeName, expectedPrefix))
        //{
        //    Debug.LogError($"BlendShape '{blendShapeName}' does NOT start with '{expectedPrefix}' as expected!");   //!!!
        //}

        int periodPosition = blendShapeName.IndexOf(".", System.StringComparison.Ordinal);
        if (periodPosition < 0)
        {
            Debug.LogError($"BlendShape '{blendShapeName}' does NOT contain a period as expected!");   //!!!
        }
        else
        {
            int parentNamePositionAfterPeriod = blendShapeName.IndexOf(parentName + "_", periodPosition + 1, System.StringComparison.Ordinal);   //!!! Do I want this Case Sensitive?
            if (parentNamePositionAfterPeriod != periodPosition + 1)
            {
                //Debug.LogError($"BlendShape '{blendShapeName}' does NOT contain '{parentName}_' immediately after the period as expected!");   //!!!
                //!!! 'bs_canine_bot_L.teeth_canine_bot_L' and 'bs_canine_bot_R.teeth_canine_bot_R' have nonstandard naming
            }
            else
            {
                returnValue = blendShapeName.Substring(periodPosition + parentName.Length + 2);       //String after the period and the parentName and the "_".
            }
        }

        return returnValue;
    }
}
