﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GarbageMove : MonoBehaviour
{
    private List<Vector3> arrPath = LevelManager.arrPath;
    private int curIndex = 0;
    private int arrCount = 0;
    private float speed = LevelManager.speed;

    // touch offset allows ball not to shake when it starts moving
    float deltaX, deltaY;
    private Vector2 logalPos;
    private Rigidbody2D rb;

    // ball movement not allowed if you touches not the ball at the first time
    private bool isDragingMove = false;

    private bool isInTrashCan = false;
    public void SetGarbagePosStatus(bool isInTrashCan)
    {
        this.isInTrashCan = isInTrashCan;
    }


    void Start()
    {
        arrCount = arrPath.Count;
        logalPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // logically move
        if(curIndex < arrCount)
        {
            if((logalPos - (Vector2)arrPath[curIndex]).magnitude > 0.1f)
            {
                Vector2 dir = (Vector2)arrPath[curIndex] - logalPos;
                logalPos += dir.normalized * (speed * Time.deltaTime);
            }
            else
            {
                curIndex++;
            }
        }
        else
        {
            ArrivalEndPoint();
            return;
        }

        // Initiating touch event
        // if touch event takes place
        if (Input.touchCount > 0)
        {
            // get touch position
            Touch touch = Input.GetTouch(0);
            // obtain touch position
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            // get touch to take a deal with
            switch (touch.phase)
            {
                // if you touches the screen
                case TouchPhase.Began:
                    // if you touch the ball
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        // get the offset between position you touches
                        // and the center of the game object
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;
                        // if touch begins within the ball collider
                        // then it is allowed to move
                        isDragingMove = true;
                        // restrict some rigidbody properties so it moves
                        // more  smoothly and correctly
                        // rb.velocity = new Vector2(0, 0);
                        // rb.gravityScale = 0;
                    }
                    break;
                // you move your finger
                case TouchPhase.Moved:
                    // if you touches the ball and movement is allowed then move
                    // OverlapPoint() consume to much if we call it each frame, especially when multiple objects overlap (also cause jamming)
                    if (/*GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos) && */isDragingMove)
                        rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));

                    if (isInTrashCan)
                    {
                        ThrowGarbage();
                        return;
                    }
                    break;
                // you release your finger
                case TouchPhase.Ended:
                    // restore initial parameters
                    // when touch is ended
                    isDragingMove = false;
                    rb.MovePosition(logalPos);

                    break;
            }
        }

        if (!isDragingMove)
        {
            rb.MovePosition(logalPos);
        }

     }

    private void ArrivalEndPoint()
    {
        Destroy(gameObject);
    }

    private void ThrowGarbage()
    {
        Destroy(gameObject);
    }
    
}
