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


    public static float speed = 8f;//4
    [Header("Level Config")]
    private float intervalTime = 1f;//3

    private Map map;
    public static List<Vector3> arrPath;
    private Dictionary< int, List<int> > garbageCodes = new Dictionary< int, List<int> >();
    private List<int> count = new List<int>();
    private int countSum;
    private int[] print = {0, 0, 0, 0, 0, 0};

    private float timer = 0.0f;

    void Awake() {
        SetLevelConfig(1);
    }

    // Update is called once per frame
    void Update()
    {
        CalCountSum();
        if(countSum > 0) GenerateGarbage();
        else
        {
            foreach (int item in print)
            {
                Debug.Log(item.ToString());
            }
        } 
    }

    private void SetLevelConfig(int level)
    {
        map = GameData.config.GetMapConfig(level);
        arrPath = map.GetArrPath();
        foreach (int code in map.GetGarbageCodes())
        {
            int type = GameData.config.GetGarbageData(code).type;
            if(!garbageCodes.ContainsKey(type)) garbageCodes.Add(type, new List<int>());
            garbageCodes[type].Add(code);
        }
        count = map.GetCount();
        CalCountSum();
    }

    private void GenerateGarbage()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            int code = GetRandGarbageCode();
            GarbageData garbageData = GameData.config.GetGarbageData(code);
            GameObject garbage = Instantiate(garbagePrefab, arrPath[0], Quaternion.identity);
            garbage.GetComponent<SpriteRenderer>().sprite = GameData.config.GetImage(code);
            garbage.GetComponent<Garbage>().Set(garbageData);
            timer = intervalTime;

        }
    }

    private void CalCountSum()
    {
        countSum = 0;
        foreach (int countIndex in count)
        {
            countSum += countIndex;
        }
    }

    private int GetRandGarbageCode()
    {
        int randNum = Random.Range(0, countSum);
        int sum = 0;
        int typeIndex = 0;

        for(int i = 0; i < count.Count; i++)
        {
            sum += count[i];
            if(randNum < sum)
            {
                typeIndex = i + 1;
                break;
            }
        }

        int codeIndex = Random.Range(0, garbageCodes[typeIndex].Count);
        int code = garbageCodes[typeIndex][codeIndex];

        count[typeIndex - 1]--;
        print[typeIndex - 1]++; 
        if(count[typeIndex - 1] < 1)
        {
            garbageCodes.Remove(typeIndex);
        }

        return code;
    }

}
