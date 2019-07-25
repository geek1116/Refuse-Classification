using System.Collections;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    // node of Game Scene
    [Header("Don't change variable: ")]
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject level;

    private AudioSource audioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        mainMenu.SetActive(true);
        levelMenu.SetActive(false);
        level.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ShowLevelMenu();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShowLevel();
        }
    }

    public void ShowMainMenu()
    {
        levelMenu.SetActive(false);
        level.SetActive(false);

        mainMenu.SetActive(true);
        if(!audioSource.isPlaying) audioSource.Play();
    }

    public void ShowLevelMenu()
    {
        mainMenu.SetActive(false);
        level.SetActive(false);

        levelMenu.SetActive(true);
        if(!audioSource.isPlaying) audioSource.Play();
    }

    public void ShowLevel()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(false);

        level.SetActive(true);
        audioSource.Stop();
    }

    public void Restart()
    {
        level.SetActive(false);
        level.SetActive(true);
    }
}
