using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{

    public GameObject MainPanel;

    public Button backButton;

    void Awake()
    {
        backButton.onClick.AddListener(() => BackView());
    }

    void OnEnable()
    {    
    }

    private void BackView()
    {
        gameObject.SetActive(false);
        MainPanel.SetActive(true);
        //MenuController.instance.ShowMainMenu();
    }

}
