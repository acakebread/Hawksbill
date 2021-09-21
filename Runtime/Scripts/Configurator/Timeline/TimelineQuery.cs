// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 23/08/2021 18:28:05 by seantcooper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Hawksbill.Configurator
{
    ///<summary>Timeline Query</summary>
    [RequireComponent (typeof (PlayableDirector))]
    public class TimelineQuery : MonoBehaviour, IEnumerable<TimelineQuery.ClipInfo>
    {
        const double PartitionSize = 5;

        [Button] public bool query;

        PlayableDirector director => GetComponent<PlayableDirector> ();

        public IEnumerator<ClipInfo> GetEnumerator() => GetEnumerator ();
        IEnumerator IEnumerable.GetEnumerator() => QueryClips (director).GetEnumerator ();

        void OnValidate()
        {
            if (query)
            {
                query = false;
                foreach (var clip in QueryClips (director))
                    print (clip);
            }
        }

        ///<summary>Get all Clips on Timeline and nested timelines</summary>
        internal static IEnumerable<ClipInfo> QueryClips(PlayableDirector director, bool recursive = true, string path = "", double currentTime = 0)
        {
            if (!director) yield break;

            TimelineAsset asset = director.playableAsset as TimelineAsset;
            if (!asset) yield break;

            path += (path.Length == 0 ? "" : "/") + asset.name;

            foreach (TrackAsset track in asset.GetOutputTracks ())
            {
                foreach (TimelineClip clip in track.GetClips ())
                {
                    ClipInfo clipInfo = new ClipInfo (director, track, clip, path + "/" + track.name, currentTime + clip.start, clip.duration);
                    yield return clipInfo;

                    if (clipInfo.gameObject)
                    {
                        foreach (var child in clipInfo.children = QueryClips (clipInfo.gameObject.GetComponent<PlayableDirector> (), recursive, clipInfo.path, clipInfo.start).ToArray ())
                        {
                            child.parent = clipInfo;
                            if (recursive) yield return child;
                        }
                    }
                }
            }
        }

        public class ClipInfo
        {
            public readonly string path;
            public readonly double start, end;
            public readonly TimelineClip clip;
            public readonly TrackAsset track;
            public readonly PlayableDirector playableDirector;
            public readonly GameObject gameObject;
            public ClipInfo parent;
            public ClipInfo[] children;
            //
            public string name => clip.displayName;
            public UnityEngine.Object asset => clip.asset;
            public Type assetType => asset.GetType ();
            public SkipType skipType =>
                gameObject?.GetComponentInChildren<ITimelineSkip> ()?.skipType ?? SkipType.None;
            //
            public ClipInfo(PlayableDirector director, TrackAsset track, TimelineClip timelineClip, string path, double start, double duration)
            {
                this.playableDirector = director;
                this.track = track;
                this.clip = timelineClip;
                this.path = path;
                this.start = start;
                this.end = start + duration;

                ControlPlayableAsset playableAsset = asset as ControlPlayableAsset;
                if (playableAsset)
                {
                    if (!(gameObject = playableAsset.prefabGameObject))
                    {
                        ExposedReference<GameObject> source = (playableAsset as ControlPlayableAsset).sourceGameObject;
                        gameObject = director.GetReferenceValue (source.exposedName, out bool iv) as GameObject;
                    }
                }
            }
            //
            public override string ToString() => path + " [" + assetType + " " + name + " " + start + " " + end + "]";
            public static implicit operator bool(ClipInfo empty) => empty != null;
        }

        public class Collection : IEnumerable<TimelineQuery.ClipInfo>
        {
            IEnumerable<ClipInfo> clips;

            public Collection() => this.clips = Enumerable.Empty<TimelineQuery.ClipInfo> ();
            public Collection(IEnumerable<ClipInfo> clips) => this.clips = clips;

            // Query
            public Collection getSkippable() => new Collection (clips.Where (c => c.skipType == SkipType.Full));

            // IEnumerable<TimelineQuery.ClipInfo>
            public IEnumerator<TimelineQuery.ClipInfo> GetEnumerator() => clips.GetEnumerator ();
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator ();
        }
    }

    public static class PlayableDirector_Query
    {
        static int frameIndex;
        static Dictionary<PlayableDirector, TimelineQuery.Collection> cache = new Dictionary<PlayableDirector, TimelineQuery.Collection> ();

        public static TimelineQuery.Collection getClipInfos(this PlayableDirector director, bool recurive = true)
        {
            // Keep query for a frame
            if (frameIndex != Time.frameCount || cache.ContainsKey (director))
            {
                TimelineQuery.Collection collection = new TimelineQuery.Collection (TimelineQuery.QueryClips (director, recurive));
                frameIndex = Time.frameCount;
                cache[director] = collection;
            }
            return cache[director];
        }
    }
}