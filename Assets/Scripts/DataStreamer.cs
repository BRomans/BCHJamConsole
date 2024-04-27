using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gtec.Chain.Common.Templates.Utilities;

public class DataStreamer : MonoBehaviour
{
    public int _classId;

    public int ClassId
    {
        get { return _classId; }
        set { _classId = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int DataCallback(float [,] data)
    {
        //Debug.Log("Data received: " + data.ToString());
        return _classId;
    }

}
