// 垃圾的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageData
{
    public int code;
    public string name;
    public int type;
    public int imageId;

    public enum GarbageType
    {
        Recyclable, Dry, Wet, Pernicious, Mysterious
    }

    GarbageData(int _code, string _name, int _type, int _imageId)
    {
        code = _code;
        name = _name;
        type = _type;
        imageId = _imageId;
    }
}

public class Garbage : MonoBehaviour
{
    public GarbageData garbageData;
    public int speed;
    public int buff;

    Garbage(GarbageData _garbageData, int _speed, int _buff)
    {
        garbageData = _garbageData;
        speed = _speed;
        buff = _buff;
    }

}
