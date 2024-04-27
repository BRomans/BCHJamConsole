using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gtec.Chain.Common.Templates.Utilities;

public class DataStreamer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DataCallback(float [,] data)
    {
        Debug.Log("Data received: " + data.ToString());
    }
}
