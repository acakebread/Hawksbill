// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 03/02/2021 09:23:43 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Linq;
using System.Globalization;
using System;

namespace Wonder14.Audio
{
    ///<summary>Put text here to describe the Class</summary>
    public class AudioTrigger : MonoBehaviour
    {
        public Action action;
        [ArrayItem (true)] public AudioNode[] nodes;

        void Awake()
        {
            if (action == Action.PlayOnAwake)
            {
                nodes.Where (n => n).ForAll (n => n.instantiateR (transform));
            }
        }

        public AudioNode instantiate(string name)
        {
            var prefab = nodes.FirstOrDefault (n => n.name.StartsWith (name, StringComparison.OrdinalIgnoreCase));
            if (prefab) return prefab.instantiateR (transform);
            else Debug.LogError ("<color=#00a078ff>AudioNode Node Found '" + name + "'</color>");
            return null;
        }

        public void stopAll()
        {
            GetComponentsInChildren<AudioNode> ().ForAll (a => Destroy (a.gameObject));
        }

        public enum Action
        {
            Manual = 0,
            PlayOnAwake = 1,
        }
    }
}