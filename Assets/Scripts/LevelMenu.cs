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

    public Button dialogButton;

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

        dialogButton.onClick.AddListener(() => dialog.SetActive(false));
    }
    
    void ClickLevel(int level)
    {
        int i = GameData.playerData.GetLevelCount();
        i += 1;
        if (i >= level)
        {
            GameData.level = level;
            GameData.config.GetMapConfig(level);
            MenuController.instance.ShowLevel();
        }
        else
        {
            dialog.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Debug.Log("Level Menu Disable!");
    }
}
