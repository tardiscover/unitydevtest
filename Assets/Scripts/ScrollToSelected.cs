using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

/// <summary>
/// https://gist.github.com/marcelschmidt1337/14a1528f98307d3d826f522196e9817f
/// 
/// Currently not used because currentSelectedGameObject not working for  need to find when 
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
        //Debug.Log(selected.name);   //!!!!!!!!!!!

        if (selected == null)
        {
            return;
        }
        if (selected.transform.parent != m_ContentRectTransform.transform)
        {
            return;
        }

        m_SelectedRectTransform = selected.GetComponent<RectTransform>();

        // math stuff
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
