// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 02/02/2021 17:39:30 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Wonder14.Audio
{
    ///<summary>Put text here to describe the Class</summary>
    public class AudioHooks : MonoBehaviour
    {
        public string hookOnStart;
        public Hook[] hooks;

        void Start()
        {
            if (!String.IsNullOrEmpty (hookOnStart)) play (hookOnStart);
        }

        public Hook contains(string name) => find (name);
        public Hook find(string name) => hooks.FirstOrDefault (h => h.name.ToLower () == name.ToLower ());
        public void play(string name)
        {
            Pretty.Log (Pretty.Colors.Audio, this.name + ": Play '" + name + "'");
            find (name)?.instantiate (transform);
        }
        public void stop(string name)
        {
            Pretty.Log (Pretty.Colors.Audio, this.name + ": Stop '" + name + "'");
            find (name)?.destroy ();
        }
        public void stopAll()
        {
            Pretty.Log (Pretty.Colors.Audio, this.name + ": StopAll");
            gameObject.children ().ForAll (g => Destroy (g));
        }
        public void pause(string name)
        {
            Pretty.Log (Pretty.Colors.Audio, this.name + ": Pause '" + name + "'");
            find (name)?.instance.GetComponentsInChildren<AudioSource> ().ForAll (s => s.Pause ());
        }
        public void resume(string name)
        {
            Pretty.Log (Pretty.Colors.Audio, this.name + ": Resume '" + name + "'");
            find (name)?.instance.GetComponentsInChildren<AudioSource> ().ForAll (s => s.UnPause ());
        }

        [Serializable]
        public class Hook
        {
            public string name;
            public AudioNode prefab;
            [HideInInspector] public AudioNode instance;

            public AudioNode instantiate(Transform transform) => prefab ? instance = prefab.instantiateR (transform) : null;
            public void destroy() => Destroy (instance);

            public static implicit operator bool(Hook empty) => empty != null;
        }

        static IEnumerable<AudioHooks> Find(Transform transform, string name) =>
            transform.GetComponentsInChildren<AudioHooks> ().Where (h => h.contains (name));

        public static void Play(Transform transform, string name) => Find (transform, name).ForAll (h => h.play (name));
        public static void Stop(Transform transform, string name) => Find (transform, name).ForAll (h => h.stop (name));
    }
}