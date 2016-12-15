﻿using UnityEngine;
using System.Collections;
using System;

public class AudioGen : MonoBehaviour {

    public double frequency = 440;
    public double gain = 0.05;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000;

    private double[] tones = { 440.0, 1046.5, 3729.3, 1046.5 };
    private int tone_index;
    private float last_time;

    void Start()
    {
    }

    void OnAudioFilterRead(float [] data, int channels)
    {
        increment = frequency * 2 * Math.PI / sampling_frequency;
        for (int i = 0; i < data.Length; i = i+channels)
        {
            phase = phase + increment;

            data[i] = (float)(gain * Math.Sin(phase));

            if (channels == 2)
                data[i + 1] = data[i];

            if (phase > 2 * Math.PI)
                phase = 0;   
                     
        }

        
    }

    void Update()
    {
        
    }
}
