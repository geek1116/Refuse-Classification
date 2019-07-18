// 导入配置 居右

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    public Hashtable garbageData = new Hashtable(); // value: GarbageData
    public Hashtable map = new Hashtable(); // value: Map
    
    private void Awake() 
    {
        garbageData.Add(1, new GarbageData(1, "Recyclable", 0, 1));
        garbageData.Add(2, new GarbageData(2, "Dry", 1, 2));
        garbageData.Add(3, new GarbageData(3, "Wet", 2, 3));
        garbageData.Add(4, new GarbageData(4, "Pernicious", 3, 4));
        garbageData.Add(5, new GarbageData(5, "Mysterious", 4, 5));
    }
}
