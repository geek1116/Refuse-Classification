using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Page Button:")]
    public Button playButton;
    public Button shopButton;
    public Button garbageGuideButton;
    public Button quitButton;
    public Button bookButton;

    [Header("Pop View on Main Page:")]
    public GameObject shopView;
    public GameObject guideView;
    public GameObject quitView;

    private void Awake()
    {
        playButton.onClick.AddListener(() => MenuController.instance.ShowLevelMenu());
        //shopButton.onClick.AddListener(OnClickShop);
        //garbageGuideButton.onClick.AddListener(OnClickGuide);
        bookButton.onClick.AddListener(OnClickBook);
        quitButton.onClick.AddListener(OnClickQuit);
    }

    private void OnEnable()
    {
        Debug.Log("Main Menu Enable!");
        ResetView();
    }

    private void OnDisable()
    {
        Debug.Log("Main Menu Disable!");
        ResetView();
    }

    public void OnClickGuide()
    {
        Debug.Log("OnGarbageGuide");
        guideView.SetActive(true);
    }

    public void OnClickShop()
    {
        Debug.Log("OnShop");
        shopView.SetActive(true);
    }

    public void OnClickBook()
    {
    }

    public void OnClickQuit()
    {
        // Debug.Log("OnQuit");
        // quitView.SetActive(true);
        Application.Quit();
    }

    private void ResetView()
    {
        guideView.SetActive(false);
        shopView.SetActive(false);
        quitView.SetActive(false);
    }
}
