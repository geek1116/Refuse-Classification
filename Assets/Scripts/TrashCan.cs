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

            if(type == garbage.type)
            {
                isMatch = true;
                if(type == (int)GarbageData.GarbageType.Mysterious)
                {
                    levelManager.OnBuff(garbage);
                }
                else
                {
                    levelManager.RemoveGarbage(garbage.gameObject);
                }
            }
            else
            {
                //if (type == (int)GarbageData.GarbageType.Mysterious)
                //{
                //    Debug.Log(garbage.garbageData.name.ToString() + " is not a mysterious garbage.");
                //}
                //else
                //{
                    levelManager.GetComponent<LevelInit>().SubStar();
                //}
                levelManager.ThrowGarbage(garbage.gameObject);
                SpitGarbage(garbage);
            }
            levelManager.OnCollectingGarbage(isMatch);
        }
    }

    private void SpitGarbage(Garbage garbage)
    {
        garbage.SpitByTrashCan(transform);
    }

}
