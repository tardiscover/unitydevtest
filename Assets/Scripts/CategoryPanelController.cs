using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CategoryPanelController : MonoBehaviour
{
    public string categoryName;
    public bool isExpanded = true;
    public TextMeshProUGUI toggleExpandText;
    public TextMeshProUGUI categoryText;
    public GameObject categoryContent;

    private void Awake()
    {
        if (string.IsNullOrEmpty(categoryName) == false)
        {
            categoryText.text = categoryName;
        }
    }

    public void OnToggleExpandButtonClick()
    {
        if (isExpanded)
        {
            //collapse
            toggleExpandText.text = "+";
            categoryContent.SetActive(false);
            isExpanded = false;
        }
        else
        {
            //expand
            toggleExpandText.text = "-";
            categoryContent.SetActive(true);
            isExpanded = true;
        }
    }
}
