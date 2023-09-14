using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for display and setting via inspector.  May be deprecated in future in favor of BlendShapeInfo.
/// </summary>
[System.Serializable]
public class BlendShapeSource
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public string blendShapeName;
}
