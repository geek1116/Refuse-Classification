using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseProp
{
    public readonly float cd;
    public readonly int price;
    public bool isActive;

    public BaseProp(float _cd, int _price)
    {
        cd = _cd;
        price = _price;
        isActive = true;
    }

    public abstract void Trigger();
}

public class SlowDownProp : BaseProp
{
    public float duration = 5.0f;
    public float speed = 1.0f;

    public SlowDownProp(float _cd, int _price) : base(_cd, _price) { }

    public override void Trigger()
    {
        LevelManager.instance.OnSlowDown(duration, speed);
    }
}

public class RemindProp : BaseProp
{
    public RemindProp(float _cd, int _price) : base(_cd, _price) { }

    public override void Trigger()
    {
        LevelManager.instance.OnRemind();
    }
}
public class EliminateProp : BaseProp
{
    public EliminateProp(float _cd, int _price) : base(_cd, _price) { }

    public override void Trigger()
    {
        LevelManager.instance.OnEliminate();
    }
}

public class Prop : MonoBehaviour
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

}

