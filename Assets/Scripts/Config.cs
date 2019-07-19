// 导入配置 居右
// Config构造函数读取配置
// Map GetMapConfig(int); 获取该关卡配置

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Config
{
    private Dictionary<int, GarbageData> garbageData; // 垃圾数据

    private static string levelMapConfigPath = Application.dataPath + "/Data/LevelMapConfig/";
    private static string garbageConfigPath = Application.dataPath + "/Data/GarbageConfig/";
    
    public Config() 
    {
        //读取GarbageData配置
        garbageData = new Dictionary<int, GarbageData>();
        string path = garbageConfigPath + "GarbageConfig.csv";

        StreamReader sr = File.OpenText(path);
        sr.ReadLine();
        string line;
        while((line = sr.ReadLine()) != null)
        {
            string[] attribute = line.Split(',');

            int code = int.Parse(attribute[0]);
            string name = attribute[1];
            int type = int.Parse(attribute[2]);
            string imageUrl = attribute[3];
            string splitCode = attribute[4];

            garbageData.Add(code, new GarbageData(code, name, type, imageUrl,splitCode));
        }
    }

    public GarbageData GetGarbageData(int code)
    {
        return garbageData[code];
    }

    public int GetGarbageDataCount()
    {
        return garbageData.Count;
    }

    public Map GetMapConfig(int level)
    {
        Map map = new Map();

        string mapFileName = "LevelMap" + level.ToString() + ".csv";
        string path = levelMapConfigPath + mapFileName;

        StreamReader sr = File.OpenText(path);
        
        map.SetStar(int.Parse(sr.ReadLine()));

        string line;
        line = sr.ReadLine();
        string[] garbageCode = line.Split(',');
        foreach (string code in garbageCode)
        {
            map.AddGarbageDatas(garbageData[int.Parse(code)]);
        }
        

        while((line = sr.ReadLine()) != null)
        {
            string[] pos = line.Split(',');
            map.AddArrPath(new Vector3(float.Parse(pos[0]), float.Parse(pos[1])));
        }

        return map;
    }
}
