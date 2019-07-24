﻿// 垃圾的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Garbage : MonoBehaviour
{
    public GarbageData garbageData;
    public int type;
    public int buff;

    // cache 
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private LevelManager levelManager;

    private List<Vector3> arrPath;
    private int curIndex = 0;
    private int arrCount = 0;
    private float speed;

    // touch offset allows ball not to shake when it starts moving
    float deltaX, deltaY;
    private Vector2 logicPos;
    private bool isDragingMove = false;

    private bool isThrown = false;

    public void Reset(GarbageData _garbageData)
    {
        garbageData = _garbageData;
        type = _garbageData.type;
        buff = _garbageData.buff;
        sr.sprite = GameData.config.GetImage(garbageData.code);
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public void Remind()
    {
        sr.color = Color.red;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void MoveToLogicPos()
    {
        rb.MovePosition(logicPos);
    }

    public void MoveForward(float distance)
    {
        if (curIndex >= arrCount) return;

        float remainDistance = distance;
        while(remainDistance > 0.0f)
        {
            float logicToNextPoint = (logicPos - (Vector2)arrPath[curIndex]).magnitude;
            if(remainDistance > logicToNextPoint)
            {
                remainDistance -= logicToNextPoint;
                // Update logicPos when we havn't reach EndPoint.
                if(++curIndex < arrCount)
                {
                    logicPos = arrPath[curIndex];
                }
                else // but if reached EndPoint we just move it to EndPoint AND let Update() method handle the OutOfRange Index.
                {
                    rb.MovePosition(arrPath[arrCount - 1]);
                    return;
                }
            }
            else
            {
                break;
            }
        }
        Vector2 dir = (Vector2)arrPath[curIndex] - logicPos;
        logicPos += dir.normalized * remainDistance;
        rb.MovePosition(logicPos);
    }

    public void SpitByTrashCan(Transform transform)
    {
        SetRigibodyToThrow(new Vector2(4.0f, 8.0f));
    }

    public void RollAtEndPoint()
    {
        SetRigibodyToThrow(new Vector2(8.0f, 0.0f));
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        levelManager = LevelManager.instance;
        arrPath = levelManager.arrPath;
        speed = levelManager.speed;
        arrCount = arrPath.Count;
        logicPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            return;
        }

        if(IsLogicMoveLegal())
        {
            HandleInputMove();
        }

    }

    private bool IsLogicMoveLegal()
    {
        if (curIndex < arrCount)
        {
            if ((logicPos - (Vector2)arrPath[curIndex]).magnitude > 0.1f)
            {
                Vector2 dir = (Vector2)arrPath[curIndex] - logicPos;
                logicPos += dir.normalized * (speed * Time.deltaTime);
            }
            else
            {
                curIndex++;
            }
        }
        else
        {
            ArrivalEndPoint();
            return false;
        }
        return true;
    }

    private void HandleInputMove()
    {
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
                    }
                    break;
                // you move your finger
                case TouchPhase.Moved:
                    // if you touches the ball and movement is allowed then move
                    // OverlapPoint() consume to much if we call it each frame, especially when multiple objects overlap (also cause jamming)
                    if (/*GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos) && */isDragingMove)
                        rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                    break;
                // you release your finger
                case TouchPhase.Ended:
                    // restore initial parameters
                    // when touch is ended
                    isDragingMove = false;
                    rb.MovePosition(logicPos);

                    break;
            }
        }

        if (!isDragingMove)
        {
            rb.MovePosition(logicPos);
        }
    }

    private void ArrivalEndPoint()
    {
        Map map = GameData.config.GetMap();
        List<int> carTypes = map.GetCarType();
        bool isMatch = false;
        foreach(int cartype in carTypes)
        {
            if(cartype == type)
            {
                isMatch = true;
                break;
            }
        }
        if (!isMatch)
        {
            levelManager.GetComponent<LevelInit>().SubStar();
        }
        levelManager.OnGarbageArrailCar(gameObject, isMatch);
    }

    private void SetRigibodyToThrow(Vector2 throwVelocity)
    {
        isThrown = true;
        transform.localScale = new Vector3(0.15f, 0.15f, 0.0f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 0.5f;
        rb.gravityScale = 1.0f;
        rb.velocity = throwVelocity;
        GetComponent<BoxCollider2D>().isTrigger = false;
        rb.gameObject.layer = 9; // 9 = GameUI Layer
        sr.color = Color.gray;
    }

}
