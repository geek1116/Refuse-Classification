using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{

    public GameObject MainPanel;

    public Button recyclableBtn;

    public Button harmfulBtn;

    public Button dryBtn;

    public Button wetBtn;

    public Button backButton;

    public GameObject prefab;

    void Awake()
    {
        recyclableBtn.onClick.AddListener(() => Recyclable());
        harmfulBtn.onClick.AddListener(() => Harmful());
        dryBtn.onClick.AddListener(() => Dry());
        wetBtn.onClick.AddListener(() => Wet());
        backButton.onClick.AddListener(() => BackView());
    }

    void OnEnable()
    {
    }

    void Recyclable()
    {
        GenerateBookItemByType(1);
    }

    void Harmful()
    {
        GenerateBookItemByType(2);
    }

    void Dry()
    {
        GenerateBookItemByType(3);
    }

    void Wet()
    {
        GenerateBookItemByType(4);
    }

    void GenerateBookItemByType(int type)
    {
        int t = 0;
        float x = -350f, y = 450f;
        GarbageData temp;
        List<int> garbageCode = GameData.playerData.GetHandbook();
        foreach(int item in garbageCode)
        {
            temp = GameData.config.GetGarbageData(item);
            if(temp.type == type)
            {
                GameObject Item = Instantiate(prefab, new Vector3(x,y,0), Quaternion.identity);
                Debug.Log(x + " " + y);
                Item.transform.Find("Name").gameObject.GetComponent<Text>().text = temp.name;
                Item.transform.Find("Image").gameObject.GetComponent<Image>().sprite = GameData.config.GetImage(item);
                Item.transform.SetParent(gameObject.transform);
                Item.GetComponent<RectTransform>().anchoredPosition = new Vector2(x,y);
                Item.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                t++;
                if(t%3 == 0)
                {
                    x = -350;
                    y -= 250;
                }
                else
                {
                    x += 350;
                }
            }
        }
    }

    void BackView()
    {
        gameObject.SetActive(false);
        MainPanel.SetActive(true);
        //MenuController.instance.ShowMainMenu();
    }

}
