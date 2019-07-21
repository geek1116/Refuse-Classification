using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropButton : MonoBehaviour
{
    private BaseProp prop;
    private Button btn;
    private Image image;

    public void SetProp(BaseProp _prop)
    {
        prop = _prop;
    }

    void Start()
    {
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (prop.isActive)
        {
            if (GameData.playerData.CostGold(prop.price))
            {
                prop.Trigger();
                InactiveProp();
                Invoke("ActiveProp", prop.cd);
            }
        }
    }

    private void InactiveProp()
    {
        image.color = Color.gray;
        prop.isActive = false;
    }

    private void ActiveProp()
    {
        image.color = Color.white;
        prop.isActive = true;
    }

}
