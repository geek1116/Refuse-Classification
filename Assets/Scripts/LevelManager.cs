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


    public static float speed = 2f;
    [Header("Level Config")]
    public float intervalTime = 6.0f;
    public int garbageCount = 5;


    private Map map;

    public static List<Vector3> arrPath = new List<Vector3>();
    private float timer = 0.0f;

    void Awake() {
        map = GameData.config.GetMapConfig(1);
        arrPath = map.GetArrPath();
        garbageCount = map.GetGarbageDatas().Count;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateGarbage();
    }

    void GenerateGarbage()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            int index = Random.Range(0, garbageCount);
            GameObject garbage = Instantiate(garbagePrefab, arrPath[0], Quaternion.identity);
            garbage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(map.GetGarbageDatas()[index].imageUrl);
            timer = intervalTime;
        }
    }

}
