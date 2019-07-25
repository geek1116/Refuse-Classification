using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Page Button:")]
    public Button playButton;
    public Button quitButton;
    public Button bookButton;

    public GameObject MainPanel;

    public GameObject BookPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(() => MenuController.instance.ShowLevelMenu());
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

    public void OnClickBook()
    {
        MainPanel.SetActive(false);
        BookPanel.SetActive(true);
    }

    public void OnClickQuit()
    {
        // Debug.Log("OnQuit");
        // quitView.SetActive(true);
        Application.Quit();
    }

    private void ResetView()
    {
        BookPanel.SetActive(false);
    }
}
