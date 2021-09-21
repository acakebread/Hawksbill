// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 02/02/2021 19:54:01 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;

namespace Wonder14.Audio
{
    ///<summary>Put text here to describe the Class</summary>
    public class AudioSnapshotTransition : MonoBehaviour
    {
        public AudioMixerSnapshot snapshot;
        public bool useLastSnapshot;

        [Range (0, 5)] public float timeToReach = 1;

        //static List<AudioMixerSnapshot> snapshots = new List<AudioMixerSnapshot> ();

        static AudioMixerSnapshot lastSnapshot;

        void Start()
        {
            startSnapshot (snapshot); //useLastSnapshot && lastSnapshot ? lastSnapshot : snapshot);
        }

        void startSnapshot(AudioMixerSnapshot snapshot)
        {
            if (snapshot)
            {
                //snapshots.Add (snapshot);
                snapshot.TransitionTo (timeToReach);
                Debug.Log ("<color=#00ffbfff>" + "AudioSnapshotTransition (" + snapshot.name + ") on " + name + " in " + timeToReach + "!" + "</color>");
                return;
            }
            Debug.LogError ("<color=#00ffbfff>" + "Missing Snapshot in AudioSnapshotTransition on " + name + "!" + "</color>");

            // if (useLastSnapshot)
            // {
            //     if (snapshots.Count > 1)
            //     {
            //         snapshots.RemoveAt (snapshots.Count - 1);
            //         snapshot = snapshots.LastOrDefault ();

            //         if (snapshot)
            //         {
            //             snapshots.LastOrDefault ().TransitionTo (timeToReach);
            //             Debug.Log ("<color=#00ffbfff>" + "AudioSnapshotTransition (" + snapshot.name + ") on " + name + " in " + timeToReach + "!" + "</color>");
            //             return;
            //         }
            //         else Debug.LogError ("<color=#00ffbfff>" + "Missing return state for AudioSnapshotTransition on " + name + "!" + "</color>");
            //     }
            //     else Debug.LogError ("<color=#00ffbfff>" + "Missing return state for AudioSnapshotTransition on " + name + "!" + "</color>");
            // }
            // else Debug.LogError ("<color=#00ffbfff>" + "Missing Snapshot in AudioSnapshotTransition on " + name + "!" + "</color>");
        }
    }
}