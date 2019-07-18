using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private int gold;
    private List<int> levelStar;
    
    public PlayerData()
    {
        gold = 0;
        levelStar = new List<int>();
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

public class GlobalData
{
    public static PlayerData playerData = new PlayerData();
}
