// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 02/02/2021 18:03:06 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;
using System.Collections;

namespace Wonder14.Audio
{
    ///<summary>Put text here to describe the Class</summary>
    public class AudioSpot : MonoBehaviour
    {
        public AudioNode[] spots;
        public Randomize randomize;
        [Range (0, 30)] public float leadTime = 1;
        [Range (0.2f, 30)] public float minSpace = 2;
        [Range (0.2f, 30)] public float maxSpace = 5;

        static float rnd => UnityEngine.Random.value;
        static float rndSpread(float v) => v == 0 ? 0 : rnd * v * 2 - v;
        static float rndRange(float min, float max) => rnd * (max - min) + min;

        IEnumerator Start()
        {
            var spots = this.spots.Select (s => new Spot (s)).ToArray ();
            yield return new WaitForSeconds (leadTime);
            while (enabled)
            {
                var spot = spots.OrderBy (s => s.lastTime).Take (Mathf.Max (1, spots.Length - 2)).OrderBy (s => rnd).FirstOrDefault ();
                if (spot)
                {
                    spot.instantiate (randomize, transform);
                    yield return new WaitForSeconds (rndRange (minSpace, maxSpace) + spot.length);
                    // spot.destroy ();
                }
                else yield return new WaitForSeconds (rndRange (minSpace, maxSpace));
            }
        }

        public class Spot
        {
            public AudioNode prefab;
            public AudioNode instance;
            public float lastTime;
            public float length;

            public Spot(AudioNode prefab)
            {
                this.prefab = prefab;
                var sources = prefab.GetComponentsInChildren<AudioSource> ().Where (a => a.clip).ToArray ();
                if (sources.Length == 0)
                    Debug.LogError ("<color=#00ffbfff>Audio spot effect needs to contain at least one valid AudioSource!</color>");
                length = sources.Length == 0 ? 0 : sources.Select (a => a.clip.length).Max ();
            }

            public AudioSource[] sources => instance.GetComponentsInChildren<AudioSource> ();
            public AudioNode instantiate(Randomize randomize, Transform transform)
            {
                lastTime = Time.time;
                instance = Instantiate (prefab, transform);

                instance.name = prefab.name;
                foreach (var source in sources)
                {
                    source.volume += rndSpread (randomize.volume);
                    source.pitch += rndSpread (randomize.pitch);
                    source.panStereo += rndSpread (randomize.pan);
                }
                return instance;
            }
            public void destroy() { Destroy (instance.gameObject); instance = null; }
            public static implicit operator bool(Spot empty) => empty != null;
        }

        [Serializable]
        public class Randomize
        {
            [Range (0, 1)] public float volume = 0;
            [Range (0, 1)] public float pitch = 0;
            [Range (0, 1)] public float pan = 0;
        }
    }
}