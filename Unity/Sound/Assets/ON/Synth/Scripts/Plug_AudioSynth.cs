﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageTools.Core;

namespace ON.synth
{
    public class Plug_AudioSynth : MonoBehaviour
    {

        [System.Serializable]
        public struct Oscillators{
            public Oscillator volumeOscillator;
            public Oscillator frequencyOscillator;
            public Oscillator panOscillator;
        }

        [System.Serializable]
        public struct tone{
            public float volume;
            public float frequency;
            public float pan;
            public float counter {get;set;}
            public float output {get;set;}
            public float time {get;set;}
            public float prevVolume {get;set;}
            public float prevFrequency {get;set;}
            public float prevPan {get;set;}
            public Oscillators oscillators;
            public bool volumeOscillateUpdate {get;set;}
            public bool frequencyOscillateUpdate {get;set;}
            public bool panOscillateUpdate {get;set;}
        }
        [Tooltip("X: amplitude, Y: frequency")]
        
        public tone[] tones;

        float sampleRate;

        void Start()
        {
            sampleRate = AudioSettings.outputSampleRate;
        }

        void OnAudioFilterRead (float[] data, int channels) {
            for (int i = 0; i < tones.Length; i++)
            {
                
                tones[i].volumeOscillateUpdate = false;
                tones[i].frequencyOscillateUpdate = false;
                tones[i].panOscillateUpdate = false;
            }
            
            for (int i = 0; i < data.Length; i += channels) {
                float fraction = ((float)i/data.Length);
               
                float[] multiVal = MultiTone((float)1/(float)sampleRate, fraction);
                data[i] = multiVal[0];
                if(channels==2)
                    data[i+1] =  multiVal[1];
            }

            for (int i = 0; i < tones.Length; i++)
            {
                tones[i].prevVolume = tones[i].volume;
                tones[i].prevFrequency = tones[i].frequency;
                tones[i].prevPan = tones[i].pan;
                tones[i].volumeOscillateUpdate = true;
                tones[i].frequencyOscillateUpdate = true;
                tones[i].panOscillateUpdate = true;
            
            }

        }

        void Update(){
            for (int i = 0; i < tones.Length; i++)
            {
                
                if(tones[i].oscillators.volumeOscillator!=null && tones[i].volumeOscillateUpdate)
                    tones[i].volume = Synth_Util.GetOscValue(tones[i].oscillators.volumeOscillator);
                if(tones[i].oscillators.frequencyOscillator!=null && tones[i].frequencyOscillateUpdate)
                    tones[i].frequency = Synth_Util.GetOscValue(tones[i].oscillators.frequencyOscillator);
                if(tones[i].oscillators.panOscillator!=null && tones[i].panOscillateUpdate)
                    tones[i].pan = Synth_Util.GetOscValue(tones[i].oscillators.panOscillator);
                
            }
        }

        float[] MultiTone(float t, float fraction){

            float[] value = new float[]{0,0};

            for (int i = 0; i < tones.Length; i++)
            {
                float pan = Mathf.Lerp(tones[i].prevPan,tones[i].pan,fraction);
                float volume = Mathf.Lerp(tones[i].prevVolume,tones[i].volume,fraction);
                float frequency = Mathf.Lerp(tones[i].prevFrequency,tones[i].frequency,fraction);

                tones[i].counter += t*frequency*Mathf.PI*2;
                tones[i].counter = tones[i].counter % (Mathf.PI*2);
                tones[i].output = volume * (Mathf.Sin( tones[i].counter ));
                
                value[0] += tones[i].output * Mathf.Clamp(Mathf.Abs(1-pan),0,1);
                value[1] += tones[i].output * Mathf.Clamp((pan+1),0,1);
            }

            if(tones.Length==0){
                value[0] = 1;
                value[1] = 1;
            }

            return value;
            
        }
    }
}