// 地图的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe
{
    public Vector3 point = new Vector3();
    public float angle;
    public Pipe(Vector3 _point, float _angle)
    {
        point = _point;
        angle = _angle;
    }
}

public class Map
{
    private int star = 0;
    private List<int> carType = new List<int>();
    private List<Vector3> arrPath = new List<Vector3>();
    private List<int> garbageCodes = new List<int>();
    private List<int> count =  new List<int>();
    private int rewardGold;
    private string backgroundUrl;
    private List<Vector3> portalPoint = new List<Vector3>();
    private List<Pipe> pipe = new List<Pipe>();
    private List<Vector3> blowtorch = new List<Vector3>();

    public void SetMap(int _star, List<int> _carType, List<int> _garbageCodes, List<Vector3> _arrPath, List<int> _count, 
                       int _rewardGold, string _backgroundUrl, List<Vector3> _portalPoint, List<Pipe> _pipe, List<Vector3> _blowtorch)
    {
        star = _star;
        carType = _carType;
        garbageCodes = _garbageCodes;
        arrPath = _arrPath;
        count = _count;
        rewardGold = _rewardGold;
        backgroundUrl = _backgroundUrl;
        portalPoint = _portalPoint;
        pipe = _pipe;
        blowtorch = _blowtorch;
    }

    public List<string> GetMapTitle()
    {
        List<string> mapTitle = new List<string>();
        foreach (int type in carType)
        {
            mapTitle.Add(GarbageData.typeTitle[type]);
        }
        return mapTitle;
    }

    public List<Vector3> GetArrPath()
    {
        return arrPath;
    }

    public List<int> GetGarbageCodes()
    {
        return garbageCodes;
    }


    public int GetStar()
    {
        return star;
    }

    public List<int> GetCarType()
    {
        return carType;
    }

    public List<int> GetCount()
    {
        return count;
    }

    public int GetRewardGold()
    {
        return rewardGold;
    }

    public string GetBackgroundUrl()
    {
        return backgroundUrl;
    }

    public bool ExistPortalPoint()
    {
        if (portalPoint.Count > 0) return true;
        return false;
    }

    public List<Vector3> GetPortalPoint()
    {
        return portalPoint;
    }

    public bool ExistPipe()
    {
        if (pipe.Count > 0) return true;
        return false;
    }

    public List<Pipe> GetPipe()
    {
        return pipe;
    }

    public bool ExitBlowtorch()
    {
        if (blowtorch.Count > 0) return true;
        return false;
    }

    public List<Vector3> GetBlowtorcht()
    {
        return blowtorch;
    }
}


