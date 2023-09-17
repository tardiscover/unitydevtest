using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// !!!!!!!!!!!! Currently not really used.  Still looking for a way to detect a Select/Focus event on a slider so that 
/// ScrollToSelected.cs doesn't need an Update event.
/// </summary>
public class SelectableScollViewContentController : MonoBehaviour, ISelectHandler
{
    private GameObject parentScrollView;
    private ScrollToSelected scrollToSelected;

    //void Awake()
    //{
    //    parentScrollView = gameObject;
    //    while (parentScrollView != null && parentScrollView.tag != "ScrollView")
    //    {
    //        parentScrollView = parentScrollView.transform.parent.gameObject;
    //    }
    //    Debug.Log($"SelectableScollViewContentController {parentScrollView.name}");  //!!!!!
    //    scrollToSelected = parentScrollView.GetComponent<ScrollToSelected>();
    //}

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log($"{transform.parent.gameObject.name} selected!");    //!!!!!!!!!
        //scrollToSelected.UpdateScrollToSelected(transform.parent.gameObject);   //pass parent's GameObject because contained in prefab
    }
}
