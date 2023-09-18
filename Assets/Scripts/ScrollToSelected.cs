using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

/// <summary>
/// Adapted from 
/// https://gist.github.com/marcelschmidt1337/14a1528f98307d3d826f522196e9817f
/// so that when you navigate to controls outside the viewport of the scrollview, 
/// the selected control will scroll into view.
/// 
/// Was working, but broke when I added intermediate ancestors with the collapsing Category panels.
/// Next on my list of things to address (unless given something else as a priority).
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class ScrollToSelected : MonoBehaviour
{

    public float scrollSpeed = 10f;
    public VerticalLayoutGroup verticalLayoutGroup;

    ScrollRect m_ScrollRect;
    RectTransform m_RectTransform;
    RectTransform m_ContentRectTransform;
    RectTransform m_SelectedRectTransform;

    private GameObject selected;    //!!!
    private GameObject previouslySelected;    //!!!

    void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
        m_RectTransform = GetComponent<RectTransform>();
        m_ContentRectTransform = m_ScrollRect.content;
    }

    void Update()
    {
        selected = EventSystem.current.currentSelectedGameObject;   //!!! Newer way?
        //if (selected != null && selected != previouslySelected)   //Can't do this because of Lerp
        if (selected != null)
        {
            if (selected != previouslySelected)
            {
                previouslySelected = selected;
                //Debug.Log(selected.transform.parent.gameObject.name);  //!!!!!!!!!!!!!
            }
            UpdateScrollToSelected(selected.transform.parent.gameObject);    //!!!
        }
    }

    public void UpdateScrollToSelected(GameObject selected)
    {

        // grab the current selected from the eventsystem
        //!!!GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null)
        {
            return;
        }
        if (selected.transform.parent != m_ContentRectTransform.transform)  //!!!!!!! This needs changing now that there is nested content  from collapsing Category Panels.
        {
            return;
        }
        Debug.Log($"UpdateScrollToSelected, {selected.name}");   //!!!!!!!!!!!

        m_SelectedRectTransform = selected.GetComponent<RectTransform>();

        // math stuff
        //!!!Vector3 selectedDifference = m_RectTransform.localPosition - m_SelectedRectTransform.localPosition;
        Debug.Log($"m_RectTransform.localPosition = {m_RectTransform.localPosition}"); //!!!!!!!!!!!!!!!!!!!
        Debug.Log($"m_SelectedRectTransform.localPosition = {m_SelectedRectTransform.localPosition}"); //!!!!!!!!!!!!!!!!!!!
        Debug.Log($"transform.InverseTransformPoint(m_SelectedRectTransform.position) = {transform.InverseTransformPoint(m_SelectedRectTransform.position)}"); //!!!!!!!!!!!!!!!!!!!
        Vector3 selectedDifference = m_RectTransform.localPosition - m_SelectedRectTransform.localPosition;
        float contentHeightDifference = (m_ContentRectTransform.rect.height - m_RectTransform.rect.height);

        float selectedPosition = (m_ContentRectTransform.rect.height - selectedDifference.y);
        float currentScrollRectPosition = m_ScrollRect.normalizedPosition.y * contentHeightDifference;
        float above = currentScrollRectPosition - (m_SelectedRectTransform.rect.height / 2) + m_RectTransform.rect.height;
        float below = currentScrollRectPosition + (m_SelectedRectTransform.rect.height / 2);

        // check if selected is out of bounds
        if (selectedPosition > above)
        {
            float step = selectedPosition - above;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            //Debug.Log($"padding.top = {verticalLayoutGroup.padding.top}");    //!!!
        }
        else if (selectedPosition < below)
        {
            float step = selectedPosition - below;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            //Debug.Log($"padding.top = {verticalLayoutGroup.padding.bottom}");    //!!!
        }
    }
}
