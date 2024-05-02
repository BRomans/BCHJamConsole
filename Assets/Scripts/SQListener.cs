using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Gtec.Chain.Common.Nodes.InputNodes.ChannelQuality;

public class SQListener : MonoBehaviour
{
    public UnityEvent qualityThresholdReached = new UnityEvent();
    public UnityEvent qualityThresholdNotReached = new UnityEvent();

    public int QualityAvg { get => qualitySum;}

    public int qualityThreshold = 5;

    public int _numberOfChannels = 8;
    private int[] channelQualities;

    private int qualitySum;
    // Start is called before the first frame update
    void Start()
    {
        channelQualities = new int[_numberOfChannels];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnReceiveSQ(List<ChannelStates> channelStates)
    {
        for (int i = 0; i < channelStates.Count; i++)
        {
        
            if (channelStates[i] == ChannelStates.BadFloating || channelStates[i] == ChannelStates.BadGrounded)
            {
                channelQualities[i] = 0;
            }
            else
            {
                channelQualities[i] = 1;
            }
        }
        if (CheckQualityThreshold())
        {
            qualityThresholdReached.Invoke();
        }
        else
        {
            qualityThresholdNotReached.Invoke();
        }
        Debug.Log("Quality Average: " + qualitySum);
    }

    private bool CheckQualityThreshold()
    {
        qualitySum = 0;
        for (int i = 0; i < channelQualities.Length; i++)
        {
            qualitySum += channelQualities[i];
        }
        if (qualitySum >= qualityThreshold)
        {
            return true;
        }
        return false;
    }
}
