using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller for obtaining and handling BlendShapeInfo from the Character objects and models.
/// </summary>
public class CharacterInfoController : MonoBehaviour
{
    //A dictionary (based on attribute) of lists of BlendShapeInfo's sharing that attribute
    //Populated here, but accessed elsewhere for a specified attribute through GetBlendshapesForAttribute() below.
    private Dictionary<string, List<BlendShapeInfo>> attributeDictionary = new();      //Stores lists of BlendShapeInfo by Attribute


    void Awake()
    {
        CategorizeBlendShapes();
    }

    /// <summary>
    /// Categorize BlendShapes from all body parts.
    /// </summary>
    void CategorizeBlendShapes()
    {
        int blendShapeCount;
        BlendShapeInfo blendShapeInfo;

        SkinnedMeshRenderer[] allSkinnedMeshRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        
        foreach(SkinnedMeshRenderer skinnedMeshRender in allSkinnedMeshRenders)
        {
            blendShapeCount = skinnedMeshRender.sharedMesh.blendShapeCount;
            if (blendShapeCount > 0)
            {
                for (int index = 0; index < blendShapeCount; index++)
                {
                    blendShapeInfo = new BlendShapeInfo(skinnedMeshRender, index);
                    //Debug.Log($"CategorizeBlendShapes blendShapeInfo: {blendShapeInfo.bodyPart}, {blendShapeInfo.blendShapeIndex}, {blendShapeInfo.blendShapeName}, {blendShapeInfo.attribute}");    //!!!!!!!!!

                    if (blendShapeInfo.isComplete)
                    {
                        AddBlendShapeToListForAttribute(blendShapeInfo);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adds the supplied blendShapeInfo to the List in the attributeDictionary associated with its attribute value
    /// </summary>
    /// <param name="blendShapeInfo"></param>
    private void AddBlendShapeToListForAttribute(BlendShapeInfo blendShapeInfo)
    {
        if (attributeDictionary.ContainsKey(blendShapeInfo.attribute) == false)
        {
            attributeDictionary.Add(blendShapeInfo.attribute, new List<BlendShapeInfo>());
        }
        attributeDictionary[blendShapeInfo.attribute].Add(blendShapeInfo);
    }

    /// <summary>
    /// Gets the list of all BlendShapes with a given attribute.
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public List<BlendShapeInfo> GetBlendShapesForAttribute(string attribute)
    {
        if (attributeDictionary.ContainsKey(attribute))
        {
            return attributeDictionary[attribute];
        }
        else
        {
            return new List<BlendShapeInfo>();  //return empty list
        }
    }
}
