using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInit : MonoBehaviour
{
    public GameObject trashcanPrefab;

    public Text starText;

    private Map map;

    //垃圾桶初始位置
    private float trashcanX = -4.5f;

    private float trashcanY = -7.5f;

    private float interval = 3f; // 垃圾桶间隔

    private int GameingStar = 5; // 游戏中的星星数

    private List<GameObject> trashcans = new List<GameObject>();

    void OnEnable()
    {        
        float x = trashcanX, y = trashcanY;
        map = GameData.config.GetMap();
        if(map == null) map = GameData.config.GetMapConfig(1);
        GameingStar = map.GetStar();
        Debug.Log(GameingStar);
        
        List<int> carType = map.GetCarType();
        int[] temp = {1,1,1,1,1};
        foreach (int item in carType) if(1<=item && item<=4) temp[item]=0;

        Config config = GameData.config;
        for(int i=1;i<=4;i++)
        {
            if(temp[i]==1)
            {
                GameObject trashcan = Instantiate(trashcanPrefab, new Vector3(x,y), Quaternion.identity);
                trashcan.GetComponent<SpriteRenderer>().sprite = config.GetTrashCanImage(i);
                trashcan.GetComponent<TrashCan>().SetType(i);
                x += interval;
                trashcans.Add(trashcan);
            }
        }
    }

    public int GetGamingStar()
    {
        return GameingStar;
    }

    public void SubStar()
    {
        GameingStar--;
    }

    void OnDisable()
    {
        foreach (GameObject item in trashcans)
        {
            Destroy(item);
        }
    }

}
