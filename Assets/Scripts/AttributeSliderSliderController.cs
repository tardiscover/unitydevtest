using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttributeSliderSliderController : MonoBehaviour, ISelectHandler
{
    private GameObject parentScrollView;

    void Awake()
    {
        parentScrollView = gameObject;
        while (parentScrollView != null && parentScrollView.tag != "ScrollView")
        {
            parentScrollView = gameObject.transform.parent.gameObject;
        }
        Debug.Log($"AttributeSliderSliderController {parentScrollView.name}");  //!!!!!
    }

    public void OnSelect(BaseEventData eventData)
    {
        //!!!Debug.Log($"{transform.parent.gameObject.name} selected!");    //!!!!!!!!!
    }
}
