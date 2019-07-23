using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private string garbageTag = "Garbage";

    private int type;

    public void SetType(int type)
    {
        this.type = type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(garbageTag))
        {
            Garbage garbage = collision.gameObject.GetComponent<Garbage>();
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
            else // normal trash can just compare type and destroy that garbage
            {
                if (garbage.type != type)
                {
                    LevelManager.instance.GetComponent<LevelInit>().SubStar();
                    LevelManager.instance.RemoveButNotDestory(garbage.gameObject);
                    SpitGarbage(garbage);
                }
                else
                {
                    GameData.playerData.AddHandbook(garbage.garbageData.code);// 分类成功后添加到图鉴
                    LevelManager.instance.GetComponent<LevelManager>().RemoveGarbage(collision.gameObject);
                }
            }
        }
    }

    private void SpitGarbage(Garbage garbage)
    {
        garbage.OnSpitByTrashCan(transform);
    }

}
