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
    private Hashtable garbageData; // value: GarbageData
    private List<string> mapFileName;
    
    public Config() 
    {
        //读取GarbageData配置
        garbageData = new Hashtable();
        garbageData.Add(1, new GarbageData(1, "Recyclable", 0, 1));
        garbageData.Add(2, new GarbageData(2, "Dry", 1, 2));
        garbageData.Add(3, new GarbageData(3, "Wet", 2, 3));
        garbageData.Add(4, new GarbageData(4, "Pernicious", 3, 4));
        garbageData.Add(5, new GarbageData(5, "Mysterious", 4, 5));

        //读取Map地图相对本文名字
        mapFileName = new List<string>();
    }

    public Map GetMapConfig(int level)
    {
        Map map = new Map();
        string path = Application.dataPath + mapFileName[level-1];

        StreamReader sr = File.OpenText(path);
        string line;
        sr.ReadLine();
        while((line = sr.ReadLine()) != null)
        {
            string[] pos = line.Split(',');
            map.arrPath.Add(new Vector3(float.Parse(pos[0]), float.Parse(pos[1])));
        }
        return map;
    }
}
