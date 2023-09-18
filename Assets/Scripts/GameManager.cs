using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    public void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        //If using WebGL, disable the exit button since browser would just sit on the page anyway.
        exitButton.gameObject.SetActive(false);
#endif
    }

    public void OnExitButtonClick()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
