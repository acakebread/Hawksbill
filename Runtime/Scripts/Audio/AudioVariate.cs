// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 08/02/2021 16:12:44 by seantcooper
using UnityEngine;
using Hawksbill;
using System;

namespace Wonder14.Audio
{
    public class AudioVariate : MonoBehaviour
    {
        public AudioSource[] sources;
        public Randomize randomize;

        void Start() => sources.ForAll (s => randomize.apply (s));
    }

    [Serializable]
    public class Randomize
    {
        [Range (0, 1)] public float volume = 0;
        [Range (0, 1)] public float pitch = 0;
        [Range (0, 1)] public float pan = 0;

        static float rnd => UnityEngine.Random.value;
        static float rndSpread(float v) => v == 0 ? 0 : rnd * v * 2 - v;

        public void apply(AudioSource source)
        {
            if (!source) return;
            source.volume += rndSpread (volume);
            source.pitch += rndSpread (pitch);
            source.panStereo += rndSpread (pan);
        }
    }
}