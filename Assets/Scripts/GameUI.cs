using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [Header("Props Button Refs:")]
    public Button slowDownBtn;
    public Button remindBtn;
    public Button eliminateBtn;

    [Header("Props CD and PRICE:")]
    public float slowDownCD;
    public int slowDownPrice;

    public float remindCD;
    public int remindPrice;

    public float eliminateCD;
    public int eliminatePrice;

    [Header("Top UI Refs:")]
    public Text goldText;

    public Text starText;

    private BaseProp slowDownProp;
    private BaseProp remindProp;
    private BaseProp eliminateProp;

    private void Awake()
    {
        slowDownProp = new SlowDownProp(slowDownCD, slowDownPrice);
        slowDownBtn.GetComponent<PropButton>().SetProp(slowDownProp);

        remindProp = new RemindProp(remindCD, remindPrice);
        remindBtn.GetComponent<PropButton>().SetProp(remindProp);

        eliminateProp = new EliminateProp(eliminateCD, eliminatePrice);
        eliminateBtn.GetComponent<PropButton>().SetProp(eliminateProp);


    }

    public void Update()
    {
        goldText.text = string.Format("${0}", GameData.playerData.GetGold());
        string temp = "";
        int starCount = GameObject.Find("Level").GetComponent<LevelInit>().GetGamingStar();
        for(int i=0;i<starCount;i++) temp += "★";
        starText.text = temp;
    }

}

