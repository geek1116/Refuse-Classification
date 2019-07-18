// 导入配置 居右

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public Hashtable garbageData = new Hashtable(); // value: GarbageData
    public Hashtable map = new Hashtable(); // value: Map
    
    private void Awake() 
    {
        garbageData.Add(1, garbage(1, "abc", (int)GarbageData.GarbageType.Dry, 123))
    }
}
