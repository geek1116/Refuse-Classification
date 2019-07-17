using UnityEngine;
using System.Collections;

public class Level: MonoBehaviour
{

    private void OnEnable()
    {
        Debug.Log("Level Enable!");
    }

    private void OnDisable()
    {
        Debug.Log("Level Disable!");
    }

}
