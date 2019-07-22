// 垃圾的类型定义 居右

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageData
{
    public int code;
    public string name;
    public int type;
    public string imageUrl;
    public string splitCode;
    public int buff;

    public enum GarbageType
    {
        Recyclable = 1, Dry = 2, Wet = 3, Pernicious = 4, Mixed = 5, Mysterious = 6
    }

    public static readonly string[] typeTitle = { "", "可回收垃圾", "干垃圾", "湿垃圾", "有害垃圾", "混合垃圾", "神秘垃圾" };

    public enum Buff
    {
        BadBoy = 1, GreenTeaGirl = 2, AcademicTrash = 3, KeyboardMan = 4
    }

    public GarbageData(int _code, string _name, int _type, string _imageUrl, string _splitCode, int _buff)
    {
        code = _code;
        name = _name;
        type = _type;
        imageUrl = _imageUrl;
        splitCode = _splitCode;
        buff = _buff;
    }

    public List<int> OnSplitCode()
    {
        List<int> ret = new List<int>();
        string[] splitCodeStr = splitCode.Split('|');
        foreach (string str in splitCodeStr)
        {
            ret.Add(int.Parse(str));
        }
        return ret;
    }

    public override string ToString()
    {
        return string.Format("{0}属于{1}", name, typeTitle[type]);
    }
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Garbage : MonoBehaviour
{
    public GarbageData garbageData;
    public int type;
    public int buff;
    
    public void Reset(GarbageData _garbageData)
    {
        garbageData = _garbageData;
        type = _garbageData.type;
        buff = _garbageData.buff;
        GetComponent<SpriteRenderer>().sprite = GameData.config.GetImage(garbageData.code);

    }

    private List<Vector3> arrPath;
    private int curIndex = 0;
    private int arrCount = 0;
    private float speed;

    // touch offset allows ball not to shake when it starts moving
    float deltaX, deltaY;
    private Vector2 logicPos;
    private Rigidbody2D rb;

    // ball movement not allowed if you touches not the ball at the first time
    private bool isDragingMove = false;

    private bool isInTrashCan = false;
    public void SetGarbagePosStatus(bool isInTrashCan)
    {
        this.isInTrashCan = isInTrashCan;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public void Remind()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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

    void Start()
    {
        arrPath = LevelManager.instance.arrPath;
        speed = LevelManager.instance.speed;
        arrCount = arrPath.Count;
        logicPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // logically move
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

                    // if (isInTrashCan)
                    // {
                    //     ThrowGarbage();
                    //     return;
                    // }
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
        JudgeCollectType();
        LevelManager.instance.OnGarbageArrailCar(gameObject);
    }

    private void JudgeCollectType()
    {
        Map map = GameData.config.GetMap();
        List<int> carType = map.GetCarType();
        bool flag = true;
        foreach (int item in carType) if(item == type) flag = false;
        if(flag) LevelManager.instance.gameObject.GetComponent<LevelInit>().SubStar();
    }

    private void ThrowGarbage()
    {
        Destroy(gameObject);
    }

}
