using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for display and setting via inspector.  A list of these (often just one) is used to connect an Attribute slider to the attributes that it should adjust.
/// </summary>
[System.Serializable]
public class AttributeSliderBinding
{
    public string attribute;    //The name of the attribute, which will be found in one or more BlendShapeInfo objects.

    public bool reverseSliderEffect;    
            //If true, reverse the sign of the value (and adjust the min and max of the slider as necessary).
            //If both negative and positive attributes are assigned to the same slider, the first half (-100 to 0) will adust
            //the inverted attribute(s), and the second half will adjust the standard one(s).
}