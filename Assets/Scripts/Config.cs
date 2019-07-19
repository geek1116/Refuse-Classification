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
            int carType = int.Parse(attribute[2]);
            string imageUrl = attribute[3];
            string splitCode = attribute[4];

            garbageData.Add(code, new GarbageData(code, name, carType, imageUrl,splitCode));
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
        int star, carType;
        List<GarbageData> garbageDatas = new List<GarbageData>();
        List<Vector3> arrPath = new List<Vector3>();

        string mapFileName = "LevelMap" + level.ToString() + ".csv";
        string path = levelMapConfigPath + mapFileName;

        StreamReader sr = File.OpenText(path);
        string line;
        
        line = sr.ReadLine();
        string[] starStr = line.Split(',');
        star = int.Parse(starStr[0]);

        line = sr.ReadLine();
        string[] carTypeStr = line.Split(',');
        carType = int.Parse(carTypeStr[0]);


        line = sr.ReadLine();
        string[] garbageCodes = line.Split(',');
        foreach (string garbageCode in garbageCodes)
        {
            if(string.IsNullOrEmpty(garbageCode)) break;
            string[] code = garbageCode.Split('|');
            for(int i = int.Parse(code[0]); i <= int.Parse(code[1]); i++)
            {
                garbageDatas.Add(garbageData[i]);
            }
        }
        
        line = sr.ReadLine();
        string[] points = line.Split(',');
        foreach (string point in points)
        {
            if(string.IsNullOrEmpty(point)) break;
            string[] pos = point.Split('|');
            arrPath.Add(new Vector3(float.Parse(pos[0]), float.Parse(pos[1])));
        }

        map.SetMap(star, carType, garbageDatas, arrPath);

        return map;
    }
}
