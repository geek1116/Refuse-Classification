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
    private List<Sprite> backgroundImage;
    private bool portal;
    private bool pipe;
    private bool blowtorch;
    private Sprite conveyorImage;

    public void SetMap(int _star, List<int> _carType, List<int> _garbageCodes, List<Vector3> _arrPath, List<int> _count, 
                       int _rewardGold, List<Sprite> _backgroundImage, bool _portal, bool _pipe, bool _blowtorch, Sprite _conveyorImage)
    {
        star = _star;
        carType = _carType;
        garbageCodes = _garbageCodes;
        arrPath = _arrPath;
        count = _count;
        rewardGold = _rewardGold;
        backgroundImage = _backgroundImage;
        portal = _portal;
        pipe = _pipe;
        blowtorch = _blowtorch;
        conveyorImage = _conveyorImage;
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

    public List<Sprite> GetBackgroundImage()
    {
        return backgroundImage;
    }

    public bool ExistPortal()
    {
        return portal;
    }

    public bool ExistPipe()
    {
        return pipe;
    }

    public bool ExitBlowtorch()
    {
        return blowtorch;
    }

    public Sprite GetConveyorImage()
    {
        return conveyorImage;
    }
}


