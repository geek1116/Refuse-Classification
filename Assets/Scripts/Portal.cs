using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public float cd;

    public GameObject inDoor;
    public GameObject outDoor;

    private float timer = 0.0f;
    private bool trigger;
    private int targetPathIndex = 4;

    public void SetTargetIndex(int pathIndex)
    {
        targetPathIndex = pathIndex;
    }

    private void Awake()
    {
        GetComponent<BoxCollider2D>().offset = inDoor.transform.localPosition;
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer < 0.0f)
        {
            trigger = true;
            timer = cd;
            inDoor.GetComponent<Animator>().SetTrigger("Open");
            outDoor.GetComponent<Animator>().SetTrigger("Open");
            Invoke("SetTrigger", 0.5f);
        }
    }

    private void SetTrigger()
    {
        trigger = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (trigger)
        {
            trigger = false;
            if (collision.CompareTag("Garbage"))
            {
                LevelManager.instance.OnPortalTrigger(collision.gameObject, outDoor.transform.position, targetPathIndex);
            }

        }
    }
}
