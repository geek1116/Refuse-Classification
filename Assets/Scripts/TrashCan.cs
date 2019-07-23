using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private string garbageTag = "Garbage";

    private int type;

    private LevelManager levelManager;

    public void SetType(int type)
    {
        this.type = type;
    }

    void OnEnable()
    {
        levelManager = GameObject.Find("Level").GetComponent<LevelManager>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(garbageTag))
        {
            Garbage garbage = collision.gameObject.GetComponent<Garbage>();
            garbage.SetGarbagePosStatus(true);
            // mysterious can might be use the garbage for buff effect
            if (type == (int)GarbageData.GarbageType.Mysterious)
            {
                if(garbage.type == type)
                {
                    LevelManager.instance.OnBuff(garbage);
                    GameData.playerData.AddHandbook(garbage.garbageData.code);// 分类成功后添加到图鉴
                }
                else
                {
                    Debug.Log(garbage.garbageData.name.ToString() + " is not a mysterious garbage.");
                }
            }
            else // other trash can just compare type and destroy that garbage
            {
                if (garbage.type != type)
                {
                    LevelManager.instance.gameObject.GetComponent<LevelInit>().SubStar();
                }
                else
                {
                    GameData.playerData.AddHandbook(garbage.garbageData.code);// 分类成功后添加到图鉴
                }
                levelManager.GetComponent<LevelManager>().RemoveGarbage(collision.gameObject);
            }
        }
    }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag(garbageTag))
    //     {
    //         collision.gameObject.GetComponent<Garbage>().SetGarbagePosStatus(false);
    //         Debug.Log("Exit Garbage Can!");
    //     }
    // }
}
