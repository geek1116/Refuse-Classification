// 地图的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private int star;
    private List<int> carType = new List<int>();
    private List<Vector3> arrPath = new List<Vector3>();
    private List<GarbageData> garbageDatas = new List<GarbageData>();
    private List<int> count =  new List<int>(); //0-carType对应常规垃圾的数量 1-carType不对应常规垃圾的树立 2-混合垃圾数量 3-神秘垃圾数量

    public List<Vector3> GetArrPath()
    {
        return arrPath;
    }

    public List<GarbageData> GetGarbageDatas()
    {
        return garbageDatas;
    }

    public void SetMap(int _star, List<int> _carType, List<GarbageData> _garbageDatas, List<Vector3> _arrPath, List<int> _count)
    {
        star = _star;
        carType = _carType;
        garbageDatas = _garbageDatas;
        arrPath = _arrPath;
        count = _count;
    }

    public int GetStar()
    {
        return star;
    }

    public List<int> GetCarType()
    {
        return carType;
    }
}


