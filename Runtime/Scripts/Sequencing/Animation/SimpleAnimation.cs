// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/05/2021 14:25:37 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill.Sequencing
{
    public class SimpleAnimation : MonoBehaviour
    {
        public GameObject prefab;
        public AnimationClip clip;
        public float time;

        void Start()
        {
            
        }

        void Update()
        {
            if (!clip) return;
            if (Application.isPlaying) time = Time.time;
            clip.SampleAnimation (gameObject, time);
        }
    }
}