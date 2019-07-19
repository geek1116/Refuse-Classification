using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageGuideView : MonoBehaviour
{
    [Header("Button References:")]
    public Button returnButton;
    public Button recyclableButton;
    public Button dryButton;
    public Button wetButton;
    public Button perniciousButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(OnClickReturn);
        recyclableButton.onClick.AddListener(OnClickRecyclable);
        dryButton.onClick.AddListener(OnClickDry);
        wetButton.onClick.AddListener(OnClickWet);
        perniciousButton.onClick.AddListener(OnClickPernicious);
    }

    private void OnClickReturn()
    {
        Debug.Log("OnReturn");
        gameObject.SetActive(false);
    }

    private void OnClickRecyclable()
    {
        Debug.Log("On Recyclable");
    }

    private void OnClickDry()
    {
        Debug.Log("On Dry");
    }

    private void OnClickWet()
    {
        Debug.Log("On Wet");
    }

    private void OnClickPernicious()
    {
        Debug.Log("On Pernicious");
    }
}
