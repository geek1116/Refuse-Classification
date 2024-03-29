﻿// 导入配置 居右
// Config构造函数读取配置
// Map GetMapConfig(int); 获取该关卡配置

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;
using UnityEditor;

public class Config
{
    private Map map = null;

    private Dictionary<int, GarbageData> garbageData = new Dictionary<int, GarbageData>(); // 垃圾数据
    private List<int> garbageDataCodes = new List<int>();
    private Dictionary<int, Sprite> image = new Dictionary<int, Sprite>();// 美术资源 垃圾code-object

    // private static string resourcesPath = "Assets/Resources/";
    private static string levelMapConfigPath = "LevelMapConfig/";
    private static string garbageConfigPath = "GarbageConfig/";
    private static string backgroundPath = "Sprites/Background/";
    private static string conveyorPath = "Sprites/Conveyor/";

    private static string[] garbageImagePath = {"", "Sprites/Recyclable/", "Sprites/Dry/", "Sprites/Wet/", "Sprites/Pernicious/", "Sprites/Mixed/", "Sprites/Mysterious/"};

    private string trashCanImagePath = "Sprites/TrashCan/";

    private string[] trashCanImageName = {"", "Recyclable", "Dry", "Wet", "Harmful", "Marvel"};


    private Dictionary<int, Sprite> trashCanImage = new Dictionary<int, Sprite>();// 垃圾桶图片资源

    
    public Config() 
    {
        //读取GarbageData配置
        string path = garbageConfigPath + "GarbageConfig";

        string res = Resources.Load(path).ToString();
        string[] lines = res.Split('\n');

        for(int i = 1; i < lines.Length; i++)
        {
            string[] attribute = lines[i].Split(',');
            int code = int.Parse(attribute[0]);
            string name = attribute[1];
            int type = int.Parse(attribute[2]);
            string imageUrl = garbageImagePath[type] + attribute[3];
            string splitCode = attribute[4];
            int buff = int.Parse(attribute[5]);

            garbageData.Add(code, new GarbageData(code, name, type, imageUrl,splitCode,buff));
            garbageDataCodes.Add(code);
            image.Add(code, Resources.Load<Sprite>(imageUrl));
        }

        for(int i=1;i<=5;i++)
        {
            trashCanImage.Add(i, Resources.Load<Sprite>(trashCanImagePath + trashCanImageName[i]));
        }

    }

    public Sprite GetTrashCanImage(int code)
    {
        return trashCanImage[code];
    }

    public GarbageData GetGarbageData(int code)
    {
        return garbageData[code];
    }

    public int GetGarbageDataCount()
    {
        return garbageData.Count;
    }

    public int GetRandomCode()
    {
        int seed = Random.Range(0, GameData.config.GetGarbageDataCount());
        return garbageDataCodes[seed];
    }

    public Sprite GetImage(int code)
    {
        return image[code];
    }

    public Map GetMapConfig(int level)
    {
        map = new Map();

        int star;
        List<int> carType = new List<int>();
        List<int> garbageCodes = new List<int>();
        List<Vector3> arrPath = new List<Vector3>();
        List<int> count = new List<int>();
        int rewardGold;
        List<Sprite> backgroundImage = new List<Sprite>();
        bool portal = false;
        bool pipe = false;
        bool blowtorch = false;
        Sprite conveyorImage;

        string mapFileName = "LevelMap" + level.ToString();
        string path = levelMapConfigPath + mapFileName;

        string res = Resources.Load(path).ToString();
        string[] lines = res.Split('\n');

        string line;
        int index = 0;
        line = lines[index++]; //0
        string[] starStr = line.Split(',');
        star = int.Parse(starStr[0]);

        line = lines[index++]; //1
        string[] carTypeStrs = line.Split(',');
        foreach (string carTypeStr in carTypeStrs)
        {
            if (string.IsNullOrEmpty(carTypeStr)) break;
            carType.Add(int.Parse(carTypeStr));
        }

        line = lines[index++]; //2
        string[] garbageCodeRanges = line.Split(',');
        foreach (string garbageRange in garbageCodeRanges)
        {
            if (string.IsNullOrEmpty(garbageRange)) break;
            string[] code = garbageRange.Split('|');
            for (int i = int.Parse(code[0]); i <= int.Parse(code[1]); i++)
            {
                garbageCodes.Add(i);
            }
        }

        line = lines[index++]; //3
        string[] points = line.Split(',');
        foreach (string point in points)
        {
            if (string.IsNullOrEmpty(point)) break;
            string[] pos = point.Split('|');
            arrPath.Add(new Vector3(float.Parse(pos[0]), float.Parse(pos[1])));
        }

        line = lines[index++]; //4
        count.Add(0);
        string[] countStrs = line.Split(',');
        foreach (string countStr in countStrs)
        {
            count.Add(int.Parse(countStr));
        }

        line = lines[index++]; //5
        string[] rewardGoldStr = line.Split(',');
        rewardGold = int.Parse(rewardGoldStr[0]);

        line = lines[index++]; //6
        string[] backgroundName = line.Split(',');
        string backgroundUrl = backgroundPath + backgroundName[0];
        UnityEngine.Object[] images = Resources.LoadAll(backgroundUrl, typeof(Sprite));
        for (int i = 0; i < images.Length; i++)
        {
            Sprite image = Resources.Load<Sprite>($"{backgroundUrl}/{i+1}");
            backgroundImage.Add(image);
        }

        line = lines[index++]; //7
        if (line.Contains("yes")) portal = true;

        line = lines[index++]; //8
        if (line.Contains("yes")) pipe = true;

        line = lines[index++]; //9
        if (line.Contains("yes")) blowtorch = true;

        line = lines[index++]; //10
        string[] conveyorName = line.Split(',');
        string conveyorUrl = conveyorPath + conveyorName[0];
        conveyorImage = Resources.Load<Sprite>(conveyorUrl);
        Debug.Log(conveyorImage);

        map.SetMap(star, carType, garbageCodes, arrPath, count, rewardGold, backgroundImage, portal, pipe, blowtorch, conveyorImage);

        return map;
    }

    public Map GetMap()
    {
        return map;
    }
}
