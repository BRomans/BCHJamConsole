using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Accord.Math;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;

public class DataStreamer : MonoBehaviour
{
    public int _classId;

    [SerializeField]
    private int _sampleRate = 250;

    [SerializeField]
    private int _numChannels = 8;

    [SerializeField]
    private int _slidingWindow = 100;

    private int bufferLength = 0;
    private int slidingWindowBuffer = 0;
    public bool logData = false;
    float[,] eegData; // 8 channels, 1000 samples
    // Define frequency bands
    private const double AlphaMin = 8.0; // Hz
    private const double AlphaMax = 13.0; // Hz
    private const double BetaMin = 13.0; // Hz
    private const double BetaMax = 30.0; // Hz

    // Define normalization constants
    private float alphaMax = 5000.0f; // Adjust this based on your maximum alpha power value
    private float betaMax = 5000.0f;

    private float averageAlphaPower = 0.0f;
    private float averageBetaPower = 0.0f;

    private string filename = "";

    public int ClassId
    {
        get { return _classId; }
        set { _classId = value; }
    }

    public float AverageAlphaPower
    {
        get { return averageAlphaPower; }
    }

    public float AverageBetaPower
    {
        get { return averageBetaPower; }
    }

    // Start is called before the first frame update
    void Start()
    {
        eegData = new float[_sampleRate, _numChannels];
        //create filename based on timestamp
        filename = "power_bands_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        // Write header to CSV file
        if (logData)
            LogToCSV(filename, "Alpha Power, Beta Power, Alpha/Beta Ratio");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DataCallback(float[,] data)
    {

        if (bufferLength == 250 && slidingWindowBuffer == _slidingWindow)
        {
            AnalyzeEEG(eegData);
            if (logData)
                LogToCSV(filename, eegData.ToString());
        }
        if (bufferLength < _sampleRate)
        {
            // push sample into eegData
            for (int i = 0; i < _numChannels; i++)
            {
                eegData[bufferLength, i] = data[0, i];
            }
            bufferLength++;
        } else
        {
            // shift the data
            for (int i = 0; i < _sampleRate - 1; i++)
            {
                for (int j = 0; j < _numChannels; j++)
                {
                    eegData[i, j] = eegData[i + 1, j];
                }
            }
            // push sample into eegData
            for (int i = 0; i < _numChannels; i++)
            {
                eegData[_sampleRate - 1, i] = data[0, i];
            }
        }
        if (slidingWindowBuffer < _slidingWindow)
        {
            slidingWindowBuffer++;
        }
        else
        {
            slidingWindowBuffer = 0;
        }

    }


    public void AnalyzeEEG(float[,] eegData)
    {
        // Assume eegData is a 2D array with dimensions [channels, samples]

        int numChannels = eegData.GetLength(1);
        int numSamples = eegData.GetLength(0);

        float[] alphaPowers = new float[numChannels];
        float[] betaPowers = new float[numChannels];
        float[] alphaBetaRatios = new float[numChannels];

        // Apply Fourier Transform to each channel
        for (int i = 0; i < numChannels; i++)
        {
            float[] channelData = eegData.GetColumn(i);

            // Convert double[] to Complex[]
            Complex32[] fft = channelData.Apply(x => new Complex32(x, 0));

            // Compute fft
            Fourier.Forward(fft);

            // Calculate Power Spectral Density (PSD)
            double[] psd = fft.Apply(x => x.MagnitudeSquared());

            // Sum PSD within frequency bands
            double alphaPower = SumPSDWithinFrequencyRange(psd, AlphaMin, AlphaMax);
            double betaPower = SumPSDWithinFrequencyRange(psd, BetaMin, BetaMax);
            // Compute inverse fft
            Fourier.Inverse(fft);

            // Round the values to two decimal places
            alphaPower = Math.Round(alphaPower, 2);
            betaPower = Math.Round(betaPower, 2);

            // Calculate Alpha/Beta ratio
            alphaPower = Mathf.Clamp((float)alphaPower, 0, alphaMax);
            betaPower = Mathf.Clamp((float)betaPower, 0, betaMax);

            // Normalize the values between 0 and 1
            alphaPower /= alphaMax;
            betaPower /= betaMax;

            // Calculate the Alpha/Beta ratio
            double alphaBetaRatio = alphaPower / betaPower;

            alphaPowers[i] = (float)alphaPower;
            betaPowers[i] = (float)betaPower;
            alphaBetaRatios[i] = (float)alphaBetaRatio;

            
            //Debug.Log($"Channel {i + 1}: Alpha Power = {alphaPower}, Beta Power = {betaPower}");
        }
        
        averageAlphaPower = Mean(alphaPowers);
        averageBetaPower = Mean(betaPowers);
        Debug.Log("Alpha Power: " + averageAlphaPower + " Beta Power: " + averageBetaPower + " Alpha/Beta Ratio: " + Mean(alphaBetaRatios));
    }

    private float Mean(float[] values)
    {
        float sum = 0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }

        return sum / values.Length;
    }

    private double SumPSDWithinFrequencyRange(double[] psd, double minFreq, double maxFreq)
    {
        int numBins = psd.Length;
        double binWidth = 1.0; // Assuming uniform frequency bins
        int minIndex = (int)Math.Round(minFreq / binWidth);
        int maxIndex = (int)Math.Round(maxFreq / binWidth);

        double sum = 0;
        for (int i = minIndex; i <= maxIndex && i < numBins; i++)
        {
            sum += psd[i];
        }

        return sum;
    }

    private void LogToCSV(string filename, string data)
    {
        System.IO.File.AppendAllText(filename, data + Environment.NewLine);
    }
}


