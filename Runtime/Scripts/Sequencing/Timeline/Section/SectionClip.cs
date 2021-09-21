// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 03/01/2021 20:38:59 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Hawksbill.Sequencing
{
    ///<summary>Put text here to describe the Class</summary>
    public class SectionClip : PlayableAsset
    {
        //public new string name;
        public Mode mode = Mode.ForwardOnce;
        [Range (0.01f, 5)] public float speed = 1;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) =>
            ScriptPlayable<SectionBehaviour>.Create (graph);

        /// <summary>Gets the position on the timeline</summary>
        /// <returns>True when animation is complete</returns>
        public bool getTimelinePosition(TimelineClip clip, float time, out float position)
        {
            float start = (float) clip.start, end = (float) clip.end, range = Mathf.Abs (end - start);
            float f = (time * speed) / range;

            switch (mode)
            {
                default:
                case SectionClip.Mode.ForwardOnce:
                    position = start + Mathf.Clamp01 (f) * range;
                    return f >= 1;
                case SectionClip.Mode.ForwardLoop:
                    position = start + ((f % 1) * range);
                    return false;

                case SectionClip.Mode.ReverseOnce:
                    position = end - Mathf.Clamp01 (f) * range;
                    return f >= 1;
                case SectionClip.Mode.ReverseLoop:
                    position = end - ((f % 1) * range);
                    return false;

                case SectionClip.Mode.PingPongOnce:
                    position = start + Mathf.PingPong (Mathf.Clamp (f, 0, 2), 1) * range;
                    return f >= 2;
                case SectionClip.Mode.PingPongLoop:
                    position = start + Mathf.PingPong (f % 2, 1) * range;
                    return false;
            }
        }
        public enum Mode { ForwardOnce, ForwardLoop, ReverseOnce, ReverseLoop, PingPongOnce, PingPongLoop }
    }
}