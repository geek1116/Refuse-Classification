using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInit : MonoBehaviour
{
    public GameObject trashcanPrefab;

    public GameObject Defeat;

    public GameObject Success;

    public Button SetButton;

    public GameObject Set;

    public Button Exit;

    public Button TryAgain;

    public Button Confirm;

    public Button Next;

    public Button SetBack;

    public Button SetExit;

    private Map map;

    //垃圾桶初始位置
    private float trashcanX = -4.5f;

    private float trashcanY = -8f;

    private float interval = 3f; // 垃圾桶间隔

    private int GamingStar = 5; // 游戏中的星星数

    private List<GameObject> trashcans = new List<GameObject>();

    private GameObject level;

    void Awake()
    {
        Exit.onClick.AddListener(() => MenuController.instance.ShowLevelMenu());
        TryAgain.onClick.AddListener(() => MenuController.instance.Restart());
        Confirm.onClick.AddListener(() => MenuController.instance.ShowLevelMenu());
        //Next.onClick.AddListener(() => MenuController.instance.ShowLevelMenu());

        SetButton.onClick.AddListener(() => ShowSet());
        SetBack.onClick.AddListener(() => BackGame());
        SetExit.onClick.AddListener(() => ExitGame());
    }

    void OnEnable()
    {
        level = GameObject.Find("Level");
        Defeat.SetActive(false);
        Success.SetActive(false);
        Set.SetActive(false);

        float x = trashcanX, y = trashcanY;
        map = GameData.config.GetMap();
        if(map == null) map = GameData.config.GetMapConfig(GameData.level);
        GamingStar = map.GetStar();
        
        List<int> carTypes = map.GetCarType();
        int[] temp = { 0,1,1,1,1,1};
        foreach (int carType in carTypes) if(1<=carType && carType<=6) temp[carType]=0;

        Config config = GameData.config;
        for(int i=1;i<=5;i++)
        {
            if(temp[i]==1)
            {
                GameObject trashcan = Instantiate(trashcanPrefab, new Vector3(x,y), Quaternion.identity);
                trashcan.GetComponent<SpriteRenderer>().sprite = config.GetTrashCanImage(i);
                if (i == 5) i = 6;         // magic!!!! because mixed Garbage does not have trash can!
                trashcan.GetComponent<TrashCan>().SetType(i);
                x += interval;
                trashcans.Add(trashcan);
            }
        }
    }

    public int GetGamingStar()
    {
        return GamingStar;
    }

    public void SubStar()
    {
        GamingStar--;
        if(GamingStar <= 0)
        {
            if(level.GetComponent<LevelManager>().HadUsedProp()) GameData.playerData.WriteData(); // 若使用过道具则金币有变化, 则保存数据

            //DestoryAllTrashCan();
            level.GetComponent<LevelManager>().ClearGarbages();
            Defeat.SetActive(true);
            level.GetComponent<LevelManager>().SetNeedGenerateGarbage(false);
            //level.SetActive(false);
        }
    }

    public void HasSuccess()
    {
        level.GetComponent<LevelManager>().ClearGarbages();
        Success.SetActive(true);
        level.GetComponent<LevelManager>().SetNeedGenerateGarbage(false);
    }

    public void DestoryAllTrashCan()
    {
        foreach (GameObject item in trashcans)
        {
            Destroy(item);
        }
    }

    void ShowSet()
    {
        Time.timeScale = 0f;
        Set.SetActive(true);
    }

    void BackGame()
    {
        Set.SetActive(false);
        Time.timeScale = 1f;
    }

    void ExitGame()
    {
        // clear relevant data of this level
        if(level.GetComponent<LevelManager>().HadUsedProp()) GameData.playerData.WriteData(); // 若使用过道具则金币有变化, 则保存数据
        level.GetComponent<LevelManager>().ClearGarbages();
        level.GetComponent<LevelManager>().SetNeedGenerateGarbage(false);
        MenuController.instance.ShowLevelMenu();

        Time.timeScale = 1f;
    }

    void OnDisable()
    {
        DestoryAllTrashCan();
    }

}
