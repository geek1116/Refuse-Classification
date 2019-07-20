// 地图的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private int star = 0;
    private List<int> carType = new List<int>();
    private List<Vector3> arrPath = new List<Vector3>();
    private List<int> garbageCodes = new List<int>();
    private List<int> count =  new List<int>(); //0-carType对应常规垃圾的数量 1-carType不对应常规垃圾的树立 2-混合垃圾数量 3-神秘垃圾数量
    private int rewardGold = 0;

    public void SetMap(int _star, List<int> _carType, List<int> _garbageCodes, List<Vector3> _arrPath, List<int> _count, int _rewardGold)
    {
        star = _star;
        carType = _carType;
        garbageCodes = _garbageCodes;
        arrPath = _arrPath;
        count = _count;
        rewardGold = _rewardGold;
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

}


