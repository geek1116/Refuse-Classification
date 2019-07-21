﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInit : MonoBehaviour
{
    public GameObject trashcanPrefab;

    public GameObject Defeat;

    public GameObject Success;

    public Button Exit;

    public Button TryAgain;

    public Button Confirm;

    public Button Next;

    private Map map;

    //垃圾桶初始位置
    private float trashcanX = -4.5f;

    private float trashcanY = -7.5f;

    private float interval = 3f; // 垃圾桶间隔

    private int GamingStar = 5; // 游戏中的星星数

    private List<GameObject> trashcans = new List<GameObject>();

    void Awake()
    {
        Exit.onClick.AddListener(() => MenuController.instance.ShowLevelMenu());
        TryAgain.onClick.AddListener(() => MenuController.instance.ShowLevel());
    }

    void OnEnable()
    {
        Defeat.SetActive(false);
        Success.SetActive(false);

        float x = trashcanX, y = trashcanY;
        map = GameData.config.GetMap();
        if(map == null) map = GameData.config.GetMapConfig(1);
        GamingStar = map.GetStar();
        
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
        return GamingStar;
    }

    public void SubStar()
    {
        GamingStar--;
        if(GamingStar <= 0)
        {
            //DestoryAllTrashCan();
            GameObject level = GameObject.Find("Level");
            level.GetComponent<LevelManager>().ClearGarbages();
            Defeat.SetActive(true);
            //level.SetActive(false);
        }
    }

    public void DestoryAllTrashCan()
    {
        foreach (GameObject item in trashcans)
        {
            Destroy(item);
        }
    }

    void OnDisable()
    {
        DestoryAllTrashCan();
    }

}
