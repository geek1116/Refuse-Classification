// 玩家数据 居右
// void AddGold(int); 增加金币
// bool CostGold(int); 消费
// bool SetLevelStar(int,int); 设置关卡通过星级
// int GetLevelStar(int); 获取该关卡星级

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerData
{
    private int gold;
    private Dictionary<int, int> levelStar;
    private List<int> handbook;
    
    private string fileName = Application.persistentDataPath + "/playerData.txt";

    public void WriteData()
    {
        if (this == null) return;
        
        SaveData saveData = new SaveData();
        saveData.gold = gold;
        saveData.levelStar = levelStar;
        saveData.handbook = handbook;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(fileName, FileMode.Create);
        bf.Serialize(fs, saveData);
        fs.Close();
    }
    
    public void ReadData()
    {
        if(!File.Exists(fileName)) return;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(fileName, FileMode.Open);
        SaveData saveData = (SaveData)bf.Deserialize(fs);
        fs.Close();

        gold = saveData.gold;
        levelStar = saveData.levelStar;
        handbook = saveData.handbook;
        handbook.Sort();
    }

    public PlayerData()
    {
        gold = 50;
        levelStar = new Dictionary<int, int>();
        handbook = new List<int>();
        ReadData();
    }

    public int GetGold() 
    { 
        return gold; 
    }

    public void AddGold(int _gold)
    {
        gold += _gold;
    }

    public bool CostGold(int _gold)
    {
        if(gold < _gold) return false;
        gold -= _gold;
        return true;
    }
    
    public bool SetLevelStar(int _level, int _star)
    {
        if(!levelStar.ContainsKey(_level))
        {
            levelStar.Add(_level, _star);
            return true;
        }

        if (levelStar[_level] >= _star) return false;
        levelStar[_level] = _star;
        return true;
    }

    public int GetLevelStar(int _level)
    {
        return levelStar[_level];
    }

    public int GetLevelCount()
    {
        return levelStar.Count;
    }

    public void AddHandbook(int _code)
    {
        if(handbook.Contains(_code)) return;
        handbook.Add(_code);
    }

    public void AddHandbook(List<int> _codes)
    {
        foreach (int code in _codes)
        {
            AddHandbook(code);
        }
        handbook.Sort();
    }

    public List<int> GetHandbook()
    {
        return handbook;
    }
}

[System.Serializable]
public class SaveData{
    public int gold;
    public Dictionary<int, int> levelStar;
    public List<int> handbook;
}