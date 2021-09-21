// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 02/02/2021 16:39:49 by seantcooper
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Hawksbill;
using System;

namespace Wonder14.Audio
{
    [AssetColor (1, 0, 0)]
    public class AudioNode : MonoBehaviour
    {
        const int PlayCountActive = 0;
        public bool destroyOnClipFinish = true;
        [Range (0, 5)] public float delayDestroy = 2;
        public bool moveToRoot = false;
        public int playCount = 0;

        IEnumerable<AudioSource> audioSources => GetComponentsInChildren<AudioSource> ().Where (s => s.clip);
        IEnumerable<string> clipNames => audioSources.Select (s => s.clip.name);

        AudioSnapshotTransition[] snapshotTransitions => GetComponentsInChildren<AudioSnapshotTransition> ();
        bool hasAudioSource => audioSources.Count () > 0;
        bool isLooping => audioSources.Any (s => s.loop);
        float audioLength => hasAudioSource ? audioSources.Select (s => s.clip.length).Max () : 0;
        float transitionLength => snapshotTransitions.Length > 0 ? snapshotTransitions.Select (t => t.timeToReach).Max () : 0;

        public void instantiate() => instantiate (null);
        public void instantiate(Transform parent)
        {
            if (parent == null) Debug.LogWarning ("<color=#00a078ff>AudioNode::instantiate parent == NULL '" + name + "'</color>");
            this.instantiateR (moveToRoot && parent ? parent.root : parent);
        }

        public AudioNode instantiateR(Transform parent)
        {
            if (!this.gameObject.activeSelf)
            {
                Debug.LogWarning ("<color=#00a078ff>AudioNode NOT ACTIVE '" + name + "'</color>");
                return null;
            }

            if (playCount != 0)
            {
                if (GetPlayedCount (key) > playCount) return null;
                IncrementPlayedCount (key);
            }
            AudioNode result = null;

            try
            {
                result = this.instantiate (parent, (instance) => instance.name = name);
            }
            catch (Exception e)
            {
                Debug.LogError ("[EXCEPTION] " + e);
            }
            return result;

            // var instance = Instantiate (this, parent);
            // instance.name = name;
            // return instance;
        }

        void OnDestroy()
        {
            print ("<color=#00a078ff>AudioNode Destroyed '" + name + "'</color>");
        }

        IEnumerator Start()
        {
            var clipNames = audioSources.Select (s => s.clip.name);
            print ("<color=#00ffbfff>AudioNode Start '" + name + "'</color> started @ " + transform.position
                + " " + String.Join (",", clipNames));

            // if (moveToRoot)
            // {
            //     transform.parent = transform.root;
            //     print ("<color=#00ffbfff>AudioNode Moved to Root '" + name + "'</color>");
            // }

            if (!destroyOnClipFinish || isLooping) yield break;
            var length = Mathf.Max (audioLength, transitionLength);
            if (length == 0) yield break;

            yield return new WaitForSeconds (length + delayDestroy);
            print ("<color=#00a078ff>AudioNode Stopped (after " + length + ") '" + name + "'</color>");
            Destroy (this.gameObject);
        }

        // static Dictionary<string, int> Played = new Dictionary<string, int> ();
        // static int GetPlayedCount(string key) => Played.ContainsKey (key) ? Played[key] : 0;
        // static int IncrementPlayedCount(string key) => Played[key] = GetPlayedCount (key) + 1;

        static int GetPlayedCount(string key) => Persistence.Player.get<int> (key, 0); // //PlayerPrefs.HasKey (key) ? PlayerPrefs.GetInt (key) : 0;
        static void IncrementPlayedCount(string key) => Persistence.Player.set<int> (key, GetPlayedCount (key) + PlayCountActive);
        string key => name + " " + String.Join ("_", clipNames);

    }
}