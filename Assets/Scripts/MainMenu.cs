using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => MenuController.instance.ShowLevelMenu());
    }

    private void OnEnable()
    {
        Debug.Log("Main Menu Enable!");
    }

    private void OnDisable()
    {
        Debug.Log("Main Menu Disable!");
    }

    
}
