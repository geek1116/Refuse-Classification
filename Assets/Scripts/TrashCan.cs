using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrashCan : MonoBehaviour
{
    private LevelManager levelManager;
    private string garbageTag = "Garbage";

    private int type;

    public void SetType(int type)
    {
        this.type = type;
    }

    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(garbageTag))
        {
            bool isMatch = false;
            Garbage garbage = collision.gameObject.GetComponent<Garbage>();
            // mysterious can might be use the garbage for buff effect
            if (type == (int)GarbageData.GarbageType.Mysterious)
            {
                if(garbage.type == type)
                {
                    isMatch = true;
                    levelManager.OnBuff(garbage);
                    GameData.playerData.AddHandbook(garbage.garbageData.code);// 分类成功后添加到图鉴
                }
                else
                {
                    Debug.Log(garbage.garbageData.name.ToString() + " is not a mysterious garbage.");
                }
            }
            else // normal trash can just compare type and destroy that garbage
            {
                if (garbage.type == type)
                {
                    isMatch = true;
                    GameData.playerData.AddHandbook(garbage.garbageData.code);// 分类成功后添加到图鉴
                    levelManager.GetComponent<LevelManager>().RemoveGarbage(collision.gameObject);
                }
                else
                {
                    levelManager.GetComponent<LevelInit>().SubStar();
                    levelManager.RemoveButNotDestory(garbage.gameObject);
                    SpitGarbage(garbage);
                }
            }
            if (isMatch)
            {
                levelManager.OnCollectingGarbage(isMatch);
            }
        }
    }

    private void SpitGarbage(Garbage garbage)
    {
        garbage.SpitByTrashCan(transform);
    }

}
