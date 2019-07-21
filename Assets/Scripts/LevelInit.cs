using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInit : MonoBehaviour
{
    public GameObject trashcanPrefab;

    private Map map;

    private float trashcanX = -4.5f;

    private float trashcanY = -7.5f;

    private float interval = 3f;

    private List<GameObject> trashcans = new List<GameObject>();

    void OnEnable()
    {
        Debug.Log("kkkkk!!!");
        map = GameData.config.GetMapConfig(1);
        List<int> carType = map.GetCarType();
        int[] temp = {1,1,1,1,1};
        foreach (int item in carType) if(1<=item && item<=4) temp[item]=0;

        Config config = GameData.config;
        for(int i=1;i<=4;i++)
        {
            if(temp[i]==1)
            {
                GameObject trashcan = Instantiate(trashcanPrefab, new Vector3(trashcanX,trashcanY), Quaternion.identity);
                trashcan.GetComponent<SpriteRenderer>().sprite = config.GetTrashCanImage(i);
                trashcan.GetComponent<TrashCan>().SetType(i);
                trashcanX += interval;
                trashcans.Add(trashcan);
            }
        }
    }

    void OnDisable()
    {
        foreach (GameObject item in trashcans)
        {
            Destroy(item);
        }
    }

}
