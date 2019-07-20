// 垃圾的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageData
{
    public int code;
    public string name;
    public int type;
    public string imageUrl;
    public string splitCode;
    public int buff;

    public enum GarbageType
    {
        Recyclable = 1, Dry = 2, Wet = 3, Pernicious = 4, Mixed = 5, Mysterious = 6
    }

    public GarbageData(int _code, string _name, int _type, string _imageUrl, string _splitCode, int _buff)
    {
        code = _code;
        name = _name;
        type = _type;
        imageUrl = _imageUrl;
        splitCode = _splitCode;
        buff = _buff;
    }
}

public class Garbage : MonoBehaviour
{
    public GarbageData garbageData;
    public int type;
    public int buff;
    //public static int speed;
    
    public void Set(GarbageData _garbageData)
    {
        garbageData = _garbageData;
        type = _garbageData.type;
        buff = _garbageData.buff;
    }

}
