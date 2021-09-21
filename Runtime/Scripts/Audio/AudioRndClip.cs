// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 03/02/2021 09:23:43 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Wonder14.Audio
{
    ///<summary>Put text here to describe the Class</summary>
    public class AudioRndClip : MonoBehaviour
    {
        public AudioSource source;
        public AudioClip[] clips;

        void Awake()
        {
            if (!source || clips.Length == 0)
            {
                Debug.LogError ("<color=#00ffbfff>AudioRndClip source is null or audio clips is empty!</color>");
                return;
            }
            source.clip = clips[(int) (UnityEngine.Random.value * clips.Length)];
            //source.Play ();
            //print (source.clip);
        }
    }
}