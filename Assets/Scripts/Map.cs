// 地图的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private List<Vector3> arrPath = new List<Vector3>();
    private List<GarbageData> garbageDatas = new List<GarbageData>();
    private int star;
    private int carType;

    public List<Vector3> GetArrPath()
    {
        return arrPath;
    }

    public List<GarbageData> GetGarbageDatas()
    {
        return garbageDatas;
    }

    public void SetMap(int _star, int _carType, List<GarbageData> _garbageDatas, List<Vector3> _arrPath)
    {
        star = _star;
        carType = _carType;
        garbageDatas = _garbageDatas;
        arrPath = _arrPath;
    }

    public int GetStar()
    {
        return star;
    }

    public int GetCarType()
    {
        return carType;
    }
}


