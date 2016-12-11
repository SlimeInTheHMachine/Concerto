using UnityEngine;
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
        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        }
    }
}
