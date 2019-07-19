// 地图的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private List<Vector3> arrPath = new List<Vector3>();
    private List<GarbageData> garbageDatas = new List<GarbageData>();
    private int star;

    public void AddArrPath(Vector3 vector)
    {
        arrPath.Add(vector);
    }

    public List<Vector3> GetArrPath()
    {
        return arrPath;
    }

     public void AddGarbageDatas(GarbageData garbageData)
    {
        garbageDatas.Add(garbageData);
    }

    public List<GarbageData> GetGarbageDatas()
    {
        return garbageDatas;
    }

    public void SetStar(int _star)
    {
        star = _star;
    }

    public int GetStar()
    {
        return star;
    }
}
