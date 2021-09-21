using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Wonder14.Audio
{

    public class AudioMuteMixer : MonoBehaviour
    {
        public AudioMixerGroup mixer;
        public string exposedVolumeParameter;
        public void mute(bool state)
        {
            mixer.audioMixer.SetFloat (exposedVolumeParameter, state ? 0 : -80);
        }
    }
}
