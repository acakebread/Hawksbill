// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 10/12/2020 17:40:08 by seancooper
using UnityEngine;
using Hawksbill;
using UnityEngine.Playables;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.Timeline;
using System.Collections.Generic;
using Unity.Mathematics;
using static UnityEngine.Timeline.TimelineAsset;

namespace Hawksbill.Sequencing
{
    [RequireComponent (typeof (PlayableDirector))]
    public class SectionController : MonoBehaviour
    {
        public PlayableDirector playableDirector => GetComponent<PlayableDirector> ();
        public TimelineAsset playableAsset => playableDirector ? (TimelineAsset) playableDirector.playableAsset : null;
        public bool hasPlayableDirector => playableAsset;

        public IEnumerable<TrackAsset> rootTracks => hasPlayableDirector ? playableAsset.GetRootTracks () : Enumerable.Empty<TrackAsset> ();
        public IEnumerable<SectionTrack> tracks => rootTracks.Select (t => t as SectionTrack).Where (t => t);
        public IEnumerable<TimelineClip> clips => tracks.SelectMany (t => t.GetClips ().Where (c => c.asset as SectionClip));

        public TimelineClip this[string name] => clips.FirstOrDefault (c => c.displayName == name);

        public bool resetTimeOnStart = true;
        [ReadOnly] public string currentlyPlaying;
        [ReadOnly] public string lastPlayed;

        void OnValidate()
        {
            if (playableDirector.playOnAwake != false) playableDirector.playOnAwake = false;
            if (playableDirector.timeUpdateMode != DirectorUpdateMode.Manual) playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
            if (playableDirector.extrapolationMode != DirectorWrapMode.None) playableDirector.extrapolationMode = DirectorWrapMode.None;
        }

        void Start()
        {
            if (resetTimeOnStart) setTime (0);
        }

        public void pause() => playableDirector.Pause ();
        public void resume() => playableDirector.Resume ();
        public void destroy() => Destroy (gameObject);

        public void play(string name) => play (name, null);
        public bool play(string name, Action complete)
        {
            if (!this.enabled) return false;
            var clip = this[name];
            if (clip == null)
            {
                Pretty.LogWarning (Pretty.Colors.Anim, "Missing Section '" + name + "'");
                return false;
            }
            play (clip, complete);
            return true;
        }

        void play(TimelineClip clip, Action complete = null)
        {
            StopAllCoroutines ();
            IEnumerator _play()
            {
                currentlyPlaying = clip.displayName;
                float startTime = Time.time;
                while (enabled)
                {
                    bool finished = (clip.asset as SectionClip).getTimelinePosition (clip, Time.time - startTime, out float position);
                    setTime (position);
                    if (finished) break;
                    yield return null;
                }
                complete?.Invoke ();
                lastPlayed = currentlyPlaying;
                currentlyPlaying = "";
            }
            StartCoroutine (_play ());
        }

        void setTime(double time)
        {
            playableDirector.time = time;
            playableDirector.DeferredEvaluate ();
        }
    }
}