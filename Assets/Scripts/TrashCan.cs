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
            Debug.Log("Enter Garbage Can!");
        if (collision.gameObject.CompareTag(garbageTag))
        {
            collision.gameObject.GetComponent<Garbage>().SetGarbagePosStatus(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(garbageTag))
        {
            collision.gameObject.GetComponent<Garbage>().SetGarbagePosStatus(false);
            Debug.Log("Exit Garbage Can!");
        }
    }
}
