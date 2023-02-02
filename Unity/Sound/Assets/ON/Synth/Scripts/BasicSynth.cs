using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSynth : MonoBehaviour
{
    [Range(20,1000)]  //Creates a slider in the inspector
    public float frequency1 = 300;
 
    [Range(20,1000)]  //Creates a slider in the inspector
    public float frequency2 = 310;
 
    public float sampleRate = 44100;
 
    AudioSource audioSource;
    float timeIndex = 0;
 
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Play(); //avoids audiosource from starting to play automatically
    }
   
   
    void OnAudioFilterRead(float[] data, int channels)
    {
        for(int i = 0; i < data.Length; i+= channels)
        {          
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);
           
            if(channels == 2)
                data[i+1] = CreateSine(timeIndex, frequency2, sampleRate);
           
            timeIndex++;

            timeIndex = timeIndex % ((sampleRate));
           
        }
    }
   
    //Creates a sinewave
    public float CreateSine(float timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }
}