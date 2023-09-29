using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Adapted from 
/// https://gist.github.com/marcelschmidt1337/14a1528f98307d3d826f522196e9817f
/// so that when you navigate to controls outside the viewport of the scrollview, 
/// the selected control will scroll into view.
/// 
/// Adjusted for multiple nested controls rather than assuming all siblings in the VerticalLayoutGroup.
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class ScrollToSelected : MonoBehaviour
{

    public float scrollSpeed = 10f;
    public VerticalLayoutGroup verticalLayoutGroup;

    public GameObject greenSquare;  //!!!!!!!
    public GameObject redSquare;  //!!!!!!!
    public GameObject purpleSquare;  //!!!!!!!
    public GameObject orangeSquare;  //!!!!!!!

    ScrollRect m_ScrollRect;
    RectTransform m_RectTransform;
    RectTransform m_ViewPortRectTransform;  //!!!
    RectTransform m_ContentRectTransform;
    RectTransform m_SelectedRectTransform;

    private GameObject selected;    //!!!
    private GameObject previouslySelected;    //!!!

    void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
        m_RectTransform = GetComponent<RectTransform>();
        m_ViewPortRectTransform = m_ScrollRect.viewport;
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
        Debug.Log($"UpdateScrollToSelected, {selected.name}");   //!!!!!!!!!!!
        //if (selected.transform.parent != m_ContentRectTransform.transform)  // This needed changing now that there is nested content from collapsing Category Panels.
        if (!selected.transform.IsChildOf(m_ContentRectTransform.transform))
        {
            return;
        }

        m_SelectedRectTransform = selected.GetComponent<RectTransform>();

        // math stuff
        //!!!Vector3 selectedDifference = m_RectTransform.localPosition - m_SelectedRectTransform.localPosition;
        Debug.Log($"m_RectTransform.localPosition = {m_RectTransform.localPosition}"); //!!!!!!!!!!!!!!!!!!!
        Debug.Log($"m_SelectedRectTransform.localPosition = {m_SelectedRectTransform.localPosition}"); //!!!!!!!!!!!!!!!!!!!
        Debug.Log($"m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position) = {m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position)}"); //!!!!!!!!!!!!!!!!!!!
        Debug.Log($"m_ContentRectTransform.rect.height) = {m_ContentRectTransform.rect.height}"); //!!!!!!!!!!!!!!!!!!!

        Debug.Log($"m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position) = {m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position)}");  //!!!!!!!!
        Debug.Log($"m_RectTransform.localPosition + m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position) = {m_RectTransform.localPosition + m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position)}");  //!!!!!!!!

        float selectedTopBorderYRelativeToContent;
        float selectedBottomBorderYRelativeToContent;
        GetSelectedTopAndBottomRelativeToContainer(m_SelectedRectTransform, m_ContentRectTransform, out selectedTopBorderYRelativeToContent, out selectedBottomBorderYRelativeToContent);    //!!!

        float selectedTopBorderYRelativeToViewPort;
        float selectedBottomBorderYRelativeToViewPort;
        GetSelectedTopAndBottomRelativeToContainer(m_SelectedRectTransform, m_ViewPortRectTransform, out selectedTopBorderYRelativeToViewPort, out selectedBottomBorderYRelativeToViewPort);    //!!!

        //Vector3 selectedDifference = m_RectTransform.localPosition - m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position);
        Vector3 selectedDifference = m_RectTransform.localPosition - m_ContentRectTransform.InverseTransformPoint(m_SelectedRectTransform.position);
        float contentHeightDifference = (m_ContentRectTransform.rect.height - m_RectTransform.rect.height);

        //float selectedPosition = (m_ContentRectTransform.rect.height - selectedDifference.y);
        //float currentScrollRectPosition = m_ScrollRect.normalizedPosition.y * contentHeightDifference;
        //float above = currentScrollRectPosition - (m_SelectedRectTransform.rect.height / 2) + m_RectTransform.rect.height;
        //float below = currentScrollRectPosition + (m_SelectedRectTransform.rect.height / 2);

        float selectedPosition = m_ContentRectTransform.rect.height - (m_RectTransform.localPosition.y - selectedBottomBorderYRelativeToContent);
        float currentScrollRectPosition = m_ScrollRect.normalizedPosition.y * contentHeightDifference;
        float currentAdjustedScrollRectPosition = (1.0f - m_ScrollRect.normalizedPosition.y) * contentHeightDifference;

        Debug.Log($"m_ScrollRect.verticalNormalizedPosition = {m_ScrollRect.verticalNormalizedPosition}");  //!!!!!!!!
        Debug.Log($"contentHeightDifference = {contentHeightDifference}");  //!!!!!!!!
        Debug.Log($"selectedTopBorderYRelativeToContent = {selectedTopBorderYRelativeToContent}, selectedBottomBorderYRelativeToContent = {selectedBottomBorderYRelativeToContent}, selectedTopBorderYRelativeToContent - selectedBottomBorderYRelativeToContent = {selectedTopBorderYRelativeToContent - selectedBottomBorderYRelativeToContent}, m_SelectedRectTransform.rect.height = {m_SelectedRectTransform.rect.height}");  //!!!!!!!!
        Debug.Log($"currentAdjustedScrollRectPosition = {currentAdjustedScrollRectPosition}");  //!!!!!!!!

        redSquare.transform.localPosition = m_RectTransform.localPosition + new Vector3(0, selectedTopBorderYRelativeToViewPort, 0);
        purpleSquare.transform.localPosition = m_RectTransform.localPosition + new Vector3(0, selectedBottomBorderYRelativeToViewPort, 0);

        Debug.Log($"redSquare.transform.localPosition = {redSquare.transform.localPosition}");  //!!!!!!!!

        float viewPortTopBorderYRelativeToViewPort;     //This will equal zero if the origin is top left
        float viewPortBottomBorderYRelativeToViewPort;  //This will equal the height of the ViewPort if the origin is top left
        GetSelectedTopAndBottomRelativeToContainer(m_ViewPortRectTransform, m_RectTransform, out viewPortTopBorderYRelativeToViewPort, out viewPortBottomBorderYRelativeToViewPort);

        greenSquare.transform.localPosition = m_RectTransform.localPosition + new Vector3(0, viewPortTopBorderYRelativeToViewPort, 0);
        orangeSquare.transform.localPosition = m_RectTransform.localPosition + new Vector3(0, viewPortBottomBorderYRelativeToViewPort, 0);

        //Debug.Log($"****m_RectTransform.localPosition.y - m_RectTransform.rect.height = {m_RectTransform.localPosition.y - m_RectTransform.rect.height}");  //!!!!!!!!
        //greenSquare.transform.localPosition = m_RectTransform.localPosition;
        //orangeSquare.transform.localPosition = m_RectTransform.localPosition - new Vector3(0, m_RectTransform.rect.height, 0);

        Debug.Log($"****purpleSquare.transform.localPosition.y, greenSquare.transform.localPosition.y = {purpleSquare.transform.localPosition.y}, {greenSquare.transform.localPosition.y}");  //!!!!!!!!

        //orangeSquare.transform.position = m_ViewPortRectTransform.position + new Vector3(0, m_ViewPortRectTransform.rect.height, 0);

        Debug.Log($"**** m_RectTransform = {m_RectTransform.rect.height},  m_ViewPortRectTransform.position = {m_ViewPortRectTransform.rect.height}");  //!!!!!!!!

        Debug.Log($"**** selectedBottomBorderYRelativeToViewPort = {selectedBottomBorderYRelativeToViewPort},  viewPortBottomBorderYRelativeToViewPort = {viewPortBottomBorderYRelativeToViewPort}");  //!!!!!!!!

        // check if selected is out of bounds
        if (selectedTopBorderYRelativeToViewPort > viewPortTopBorderYRelativeToViewPort)
        {
            Debug.Log("above"); //!!!!!!!!!
            float step = viewPortTopBorderYRelativeToViewPort - selectedTopBorderYRelativeToViewPort;
            Debug.Log($"**** step = {step}");  //!!!!!!!!

            //float currentScrollRectPosition = m_ScrollRect.normalizedPosition.y * contentHeightDifference;
            // float currentAdjustedScrollRectPosition = (1.0f - m_ScrollRect.normalizedPosition.y) * contentHeightDifference;
            //(1.0f - m_ScrollRect.normalizedPosition.y) = currentAdjustedScrollRectPosition / contentHeightDifference
            //- m_ScrollRect.normalizedPosition.y) = (currentAdjustedScrollRectPosition / contentHeightDifference) - 1.0f
            //    m_ScrollRect.normalizedPosition.y = 1.0f - (newY / contentHeightDifference)

            //float newY = currentScrollRectPosition + step;
            float newY = currentAdjustedScrollRectPosition + step;
            //float newNormalizedY = newY / contentHeightDifference;
            float newNormalizedY = 1.0f - (newY / contentHeightDifference);
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            //Debug.Log($"padding.selectedTopBorderYRelativeToContent = {verticalLayoutGroup.padding.selectedTopBorderYRelativeToContent}");    //!!!
        }
        else if (selectedBottomBorderYRelativeToViewPort < viewPortBottomBorderYRelativeToViewPort)
        {
            Debug.Log("below"); //!!!!!!!!!
            float step = viewPortBottomBorderYRelativeToViewPort - selectedBottomBorderYRelativeToViewPort;
            Debug.Log($"**** step = {step}");  //!!!!!!!!
            //float newY = currentScrollRectPosition + step;
            float newY = currentAdjustedScrollRectPosition + step;
            //float newNormalizedY = newY / contentHeightDifference;
            float newNormalizedY = 1.0f - (newY / contentHeightDifference);
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            //Debug.Log($"padding.selectedTopBorderYRelativeToContent = {verticalLayoutGroup.padding.bottom}");    //!!!
        }
    }

    void GetSelectedTopAndBottomRelativeToContainer(RectTransform selectedRectTransform, RectTransform containertRectTransform, out float top, out float bottom)
    {
        Vector3[] selectedWorldCorners = new Vector3[4];
        selectedRectTransform.GetWorldCorners(selectedWorldCorners);

        top = containertRectTransform.InverseTransformPoint(selectedWorldCorners[1]).y;      //y of top left corner of selectedRectTransform relative to contentRectTransform
        bottom = containertRectTransform.InverseTransformPoint(selectedWorldCorners[0]).y;   //y of bottom left corner of selectedRectTransform relative to contentRectTransform
    }
}
