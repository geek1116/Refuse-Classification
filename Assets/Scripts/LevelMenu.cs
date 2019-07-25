using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button levelOneButton;

    public Button levelTwoeButton;

    public Button levelThreeButton;

    public Button levelFourButton;

    public Button backButton;

    public GameObject dialog;

    void OnEnable()
    {
        dialog.SetActive(false);
    }

    private void Awake()
    {
        levelOneButton.onClick.AddListener(() => ClickLevel(1));
        levelTwoeButton.onClick.AddListener(() => ClickLevel(2));
        levelThreeButton.onClick.AddListener(() => ClickLevel(3));
        levelFourButton.onClick.AddListener(() => ClickLevel(4));
        backButton.onClick.AddListener(() => MenuController.instance.ShowMainMenu());
    }
    
    void ClickLevel(int level)
    {
        if(GameData.playerData.GetLevelCount()+1 >= level)
        {
            GameData.level = level;
            MenuController.instance.ShowLevel();
        }
        else
        {
        }
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
