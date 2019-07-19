using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    [Header("Fixed node: ")]
    public GameObject garbagePrefab;
    public GameObject background;
    public GameObject conveyor;


    public static float speed = 1f;
    [Header("Level Config")]
    public int garbageCount = 5;
    public float intervalTime = 3.0f;

    private Map map;

    public static List<Vector3> arrPath = new List<Vector3>();
    private float curTime = 0.0f, preTime = -3.0f;
    private int curGarbageCount = 0;

    void Awake() {
        map = GameData.config.GetMapConfig(1);
        arrPath = map.GetArrPath();
    }

    // Update is called once per frame
    void Update()
    {
        GenerateGarbage();
    }

    void GenerateGarbage()
    {
        curTime = Time.time;
        if(curGarbageCount < garbageCount && curTime - preTime >= intervalTime)
        {
            int index = Random.Range(0, garbageCount);
            GameObject garbage = Instantiate(garbagePrefab, arrPath[0], Quaternion.identity);
            garbage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(map.GetGarbageDatas()[index].imageUrl);
            preTime = curTime;
            curGarbageCount++;
        }
    }

}
