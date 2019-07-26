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

    public AudioClip[] audioClips;

    public AudioClip MainPage;

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

        if(audioSource.clip != MainPage)
        {
            audioSource.clip = MainPage;
            audioSource.Stop();
        }
        if(!audioSource.isPlaying) audioSource.Play();
    }

    public void ShowLevel()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(false);

        level.SetActive(true);
    }

    public void ChangeAudio()
    {
        audioSource.Stop();
        audioSource.clip = audioClips[GameData.level-1];
        audioSource.Play();
    }

    public void Restart()
    {
        level.SetActive(false);
        level.SetActive(true);
    }
}
