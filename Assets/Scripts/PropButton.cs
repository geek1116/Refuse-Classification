using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropButton : MonoBehaviour
{
    private BaseProp prop;

    public void SetProp(BaseProp _prop)
    {
        prop = _prop;
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (prop.isActive)
        {
            if (GameData.playerData.CostGold(prop.price))
            {
                prop.Trigger();
                prop.isActive = false;
                Invoke("ActiveProp", prop.cd);
            }
        }
    }

    private void ActiveProp()
    {
        prop.isActive = true;
    }

}
