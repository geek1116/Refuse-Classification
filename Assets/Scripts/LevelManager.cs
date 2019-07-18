using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public static float speed = 1f;
    public int garbageCount = 5;
    public float intervalTime = 3.0f;
    public static List<Vector3> arrPath = new List<Vector3>();
    private Vector3 startPos = new Vector3(-2f, 3.2f, 0f);
    private string[] garbageName = {"banana", "battery", "fish_bones", "junk_food", "water_bottle"};
    private float curTime = 0.0f, preTime = -3.0f;
    private int curGarbageCount = 0;

    void OnEnable() {
        string file_url = Application.dataPath + "/data/path/path1.csv";
        StreamReader tx = File.OpenText(file_url);
        
        string line;
        tx.ReadLine();
        while((line = tx.ReadLine()) != null)
        {
            string[] pos = line.Split(',');
            arrPath.Add(new Vector3(float.Parse(pos[0]), float.Parse(pos[1])));
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        curTime = Time.time;
        if(curGarbageCount < garbageCount && curTime - preTime >= intervalTime)
        {
            int index = Random.Range(0, garbageCount);
            GameObject garbage = Resources.Load<GameObject>("Prefabs/" + garbageName[index]);
            GameObject.Instantiate(garbage, startPos, Quaternion.identity);
            preTime = curTime;
            curGarbageCount++;
        }
    }
}
