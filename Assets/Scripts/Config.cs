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

    private static string levelMapConfigPath = "LevelMapConfig/";
    private static string garbageConfigPath = "GarbageConfig/";
    
    public Config() 
    {
        //读取GarbageData配置
        garbageData = new Dictionary<int, GarbageData>();
        string path = garbageConfigPath + "GarbageConfig";

        string res = Resources.Load(path).ToString();
        string[] lines = res.Split('\n');

        for(int i = 1; i < lines.Length; i++)
        {
            string[] attribute = lines[i].Split(',');
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

        int star;
        List<int> carType = new List<int>();
        List<GarbageData> garbageDatas = new List<GarbageData>();
        List<Vector3> arrPath = new List<Vector3>();
        List<int> count = new List<int>();

        string mapFileName = "LevelMap" + level.ToString();
        string path = levelMapConfigPath + mapFileName;

        string res = Resources.Load(path).ToString();
        string[] lines = res.Split('\n');
        
        string line;
        line = lines[0];
        string[] starStr = line.Split(',');
        star = int.Parse(starStr[0]);

        line = lines[1];
        string[] carTypeStrs = line.Split(',');
        foreach (string carTypeStr in carTypeStrs)
        {
            carType.Add(int.Parse(carTypeStr));
        }
    

        line = lines[2];
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
        
        line = lines[3];
        string[] points = line.Split(',');
        foreach (string point in points)
        {
            if(string.IsNullOrEmpty(point)) break;
            string[] pos = point.Split('|');
            arrPath.Add(new Vector3(float.Parse(pos[0]), float.Parse(pos[1])));
        }

        line = lines[4];
        string[] countStrs = line.Split(',');
        foreach (string countStr in countStrs)
        {
            count.Add(int.Parse(countStr));
        }

        map.SetMap(star, carType, garbageDatas, arrPath, count);

        return map;
    }
}
