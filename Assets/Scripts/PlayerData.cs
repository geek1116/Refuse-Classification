// 玩家数据 居右
// void AddGold(int); 增加金币
// bool CostGold(int); 消费
// bool SetLevelStar(int,int); 设置关卡通过星级
// int GetLevelStar(int); 获取该关卡星级

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private int gold;
    private List<int> levelStar;
    
    public PlayerData()
    {
        gold = 50;
        levelStar = new List<int>();
    }

    public int GetGold() { return gold; }

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
            if(levelStar[_level-1] >= _star ) 
            {
                Debug.Log("PlayerData_SetLevelStat: _star is invalid!");
                return false;
            }
            levelStar[_level-1] = _star;
        }
        return true;
    }

    public int GetLevelStar(int _level)
    {
        if(levelStar.Count < _level ||  _level < 1) Debug.Log("PlayerData_GetLevelStat: _level is invalid!");
        return levelStar[_level-1];
    }
}