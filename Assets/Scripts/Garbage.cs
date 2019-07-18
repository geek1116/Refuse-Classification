using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageData
{
    public int code;
    public string name;
    public int type;
    public int imageId;
    Garbage(int _code, string _name, int _type, int _imageId){
        code = _code;
        name = _name;
        type = _type;
        imageId = _imageId;
    }
}

public class Garbage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
