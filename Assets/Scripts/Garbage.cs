// 垃圾的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageData
{
    public int code;
    public string name;
    public int type;
    public string imageId;
    public string splitCode;

    public enum GarbageType
    {
        Recyclable, Dry, Wet, Pernicious, Mysterious,Mixed
    }

    public GarbageData(int _code, string _name, int _type, string _imageId, string _splitCode)
    {
        code = _code;
        name = _name;
        type = _type;
        imageId = _imageId;
        splitCode = _splitCode;
    }
}

public class Garbage
{
    public GarbageData garbageData;
    public int buff;
    //public static int speed;

    public Garbage(GarbageData _garbageData,int _buff)
    {
        garbageData = _garbageData;
        buff = _buff;
    }

}
