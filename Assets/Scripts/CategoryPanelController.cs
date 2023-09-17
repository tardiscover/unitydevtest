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
            //!!!toggleExpandText.text = "+";
            //!!!toggleExpandText.text = "\U0000f107";  //angle-down in font awesome
            toggleExpandText.text = "\U0000002b";       //plus in font awesome
            categoryContent.SetActive(false);
            isExpanded = false;
        }
        else
        {
            //expand
            //!!!toggleExpandText.text = "-";
            //!!!toggleExpandText.text = "\U0000f106";  //angle-up in font awesome
            toggleExpandText.text = "\U0000f068";       //minus in font awesome
            categoryContent.SetActive(true);
            isExpanded = true;
        }
    }
}
