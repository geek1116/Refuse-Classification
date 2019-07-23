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

    public static readonly string[] typeTitle = { "", "可回收垃圾", "干垃圾", "湿垃圾", "有害垃圾", "混合垃圾", "神秘垃圾" };

    public enum Buff
    {
        BadBoy = 1, GreenTeaGirl = 2, AcademicTrash = 3, KeyboardMan = 4
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

    public List<int> OnSplitCode()
    {
        List<int> ret = new List<int>();
        string[] splitCodeStr = splitCode.Split('|');
        foreach (string str in splitCodeStr)
        {
            ret.Add(int.Parse(str));
        }
        return ret;
    }

    public override string ToString()
    {
        return string.Format("{0}属于{1}", name, typeTitle[type]);
    }
}