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
    private List<int> levelStar;
    private List<int> handbook;
    
    private string fileName = Application.persistentDataPath + "/playerData.txt";

    public void WriteData()
    {
        if (this == null) return;
        SortHandbook();
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
    }

    public PlayerData()
    {
        gold = 50;
        levelStar = new List<int>();
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
        int level = levelStar.Count;
        if(level + 1 < _level || _level < 1 )
        {
            Debug.Log("PlayerData_SetLevelStat: _level is invalid!");
            return false;
        }
        if(level < _level) levelStar.Add(_star);
        else 
        {
            if(levelStar[_level - 1] >= _star ) 
            {
                Debug.Log("PlayerData_SetLevelStat: _star is invalid!");
                return false;
            }
            levelStar[_level - 1] = _star;
        }
        //WriteData();
        return true;
    }

    public int GetLevelStar(int _level)
    {
        if(levelStar.Count < _level ||  _level < 1) Debug.Log("PlayerData_GetLevelStat: _level is invalid!");
        return levelStar[_level - 1];
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

    public List<int> GetHandbook()
    {
        return handbook;
    }

    public void SortHandbook()
    {
        handbook.Sort();
    }
}

[System.Serializable]
public class SaveData{
    public int gold;
    public List<int> levelStar;
    public List<int> handbook;
}