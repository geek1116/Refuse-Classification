using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    [Header("Fixed Node:")]
    public GameObject pipe;
    public GameObject spray;
    public GameObject protal;

    public void Init(bool havePipe, bool haveSpray, bool haveProtal)
    {
        pipe.SetActive(havePipe);
        spray.SetActive(haveSpray);
        protal.SetActive(haveProtal);
    }
}
