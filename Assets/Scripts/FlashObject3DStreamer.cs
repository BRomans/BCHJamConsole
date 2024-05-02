using UnityEngine;
using System;
using Gtec.UnityInterface;
using UnityEngine.Events;

public class FlashObject3DStreamer : FlashObject3D
{
    public bool stateOnOff = true;

    public bool released = false;

    public GameObject oscReceivers;

    public GameObject textOn;

    public GameObject textOff;

    public FlashObject3DStreamer(int classId) : base(classId)
    {
         _classId = classId;
    }

    public void SetOnOff()
    {
        if(released)
        {
            if(oscReceivers != null)
                oscReceivers.SetActive(stateOnOff);
            if(textOn != null)
                textOn.SetActive(stateOnOff);
            if(textOff != null)
                textOff.SetActive(!stateOnOff);
            stateOnOff = !stateOnOff;
            released = false;
        }
    }

    public void SetReleased()
    {
        released = true;
    }
}