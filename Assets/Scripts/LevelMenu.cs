using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button levelOneButton;

    private void Awake()
    {
        levelOneButton.onClick.AddListener(() => MenuController.instance.ShowLevel());
    }

    private void OnEnable()
    {
        Debug.Log("Level Menu Enable!");
    }

    private void OnDisable()
    {
        Debug.Log("Level Menu Disable!");
    }
}
