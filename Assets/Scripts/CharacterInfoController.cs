using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoController : MonoBehaviour
{
    void Start()
    {
        CategorizeBlendShapes();
    }

    /// <summary>
    /// Categorize BlendShapes from all body parts.  //!!!(In progress)
    /// </summary>
    void CategorizeBlendShapes()
    {
        int blendShapeCount;
        BlendShapeInfo blendShapeInfo;

        SkinnedMeshRenderer[] allSkinnedMeshRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        Debug.Log($"allSkinnedMeshRenders.Length = {allSkinnedMeshRenders.Length}");    //!!!
        foreach(SkinnedMeshRenderer skinnedMeshRender in allSkinnedMeshRenders)
        {
            blendShapeCount = skinnedMeshRender.sharedMesh.blendShapeCount;
            if (blendShapeCount > 0)
            {
                for (int index = 0; index < blendShapeCount; index++)
                {
                    blendShapeInfo = new BlendShapeInfo(skinnedMeshRender, index);
                    Debug.Log($"CategorizeBlendShapes blendShapeInfo: {blendShapeInfo.bodyPart}, {blendShapeInfo.blendShapeIndex}, {blendShapeInfo.blendShapeName}, {blendShapeInfo.attribute}");    //!!!!!!!!!
                }
            }
        }
    }
}
