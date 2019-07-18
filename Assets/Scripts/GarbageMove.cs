using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageMove : MonoBehaviour
{
    private List<Vector3> arrPath = LevelManager.arrPath;
    private int curIndex = 0;
    private int arrCount = 0;
    private float speed = LevelManager.speed;
    void Start()
    {
        arrCount = arrPath.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if(curIndex < arrCount)
        {
            if(transform.position != arrPath[curIndex])
            {
                Vector2 temp = Vector2.MoveTowards(transform.position, arrPath[curIndex], speed*Time.deltaTime);
                GetComponent<Rigidbody2D>().MovePosition(temp);
            }
            else
            {
                curIndex++;
            }
        }
    }
}
