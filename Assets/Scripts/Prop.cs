using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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