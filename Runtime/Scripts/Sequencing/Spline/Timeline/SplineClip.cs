// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 16/03/2021 20:00:14 by seantcooper
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.Timeline;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Timeline;
#endif

namespace Hawksbill.Sequencing
{
    ///<summary>SplineClip that is on the timeline</summary>
    public class SplineClip : PlayableAsset
    {
        public float startIndex, endIndex;
        [ReadOnly] public int pointCount;
        [ReadOnly] public SplineData data;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SplineBehaviour>.Create (graph);
            playable.GetBehaviour ().clip = this;
            return playable;
        }
        public float getIndex(float f) => startIndex + (endIndex - startIndex) * Mathf.Clamp01 (f);
    }

    ///<summary>SplineClip Behaviour</summary>
    public class SplineBehaviour : PlayableBehaviour
    {
        public string name;
        public SplineClip clip;
        static Dictionary<SplinePlayable, float> mixer = new Dictionary<SplinePlayable, float> ();

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SplinePlayable splinePlayable = playerData as SplinePlayable;
            if (splinePlayable == null) return;

            if (splinePlayable.data)
            {
                clip.pointCount = splinePlayable.data.nodes.Count;
                clip.data = splinePlayable.data;
            }

            var position = clip.getIndex ((float) (playable.GetTime () / playable.GetDuration ()));

            if (info.weight == 0) return;
            if (info.weight == 1) { splinePlayable.position = position; return; }
            if (mixer.ContainsKey (splinePlayable))
            {
                splinePlayable.position = Mathf.Lerp (mixer[splinePlayable], position, info.weight);
                mixer.Remove (splinePlayable);
            }
            else mixer[splinePlayable] = position;
        }
    }

#if UNITY_EDITOR
    [CustomTimelineEditor (typeof (SplineClip))]
    public class SplineClipEditor : ClipEditor
    {
        // assetOwner = SplineTrack
        // asset = SplineClip
        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            //  base.DrawBackground (clip, region);
            SplineClip asset = clip.asset as SplineClip;
            if (!asset.data || region.position.width <= 0) return;

            float d1 = (float) clip.duration, d2 = (float) (region.endTime - region.startTime);
            float width = region.position.width * d1 / d2;

            Rect rect = new Rect (((float) region.startTime / d1) * width - region.position.x,
                region.position.y, width, region.position.height);

            float range = asset.endIndex - asset.startIndex;
            bool drawText = Mathf.Abs (rect.width / range) > 16;

            float ix(float i) => rect.x + rect.width * ((i - asset.startIndex) / range);

            // block out unused time
            if (!asset.data.loop)
            {
                float x1 = ix (0), x2 = ix (asset.data.nodes.Count - 1);
                float min = Mathf.Min (x1, x2) - 1, max = Mathf.Max (x1, x2) + 2;
                if (min >= rect.xMin && min < rect.xMax)
                    EditorGUI.DrawRect (new Rect (rect.x, rect.y, min - rect.x, rect.height), block);
                if (max >= rect.xMin && max < rect.xMax)
                    EditorGUI.DrawRect (new Rect (max, rect.y, rect.width - max, rect.height), block);
            }

            // draw grid lines
            void drawIndex(int i)
            {
                if (!asset.data.loop && (i < 0 || i >= asset.data.nodes.Count)) return;
                int index = (int) asset.data.scope (i);

                Rect r = new Rect (ix (i), rect.y, 1, rect.height);
                EditorGUI.DrawRect (r, index == 0 ? hard : light);

                if (drawText)
                {
                    r.x++;
                    r.width = 100;
                    GUI.Label (r, index.ToString (), numberStyle);
                }
            }

            if (asset.startIndex < asset.endIndex)
                for (int i = Mathf.CeilToInt (asset.startIndex), end = Mathf.FloorToInt (asset.endIndex); i <= end; drawIndex (i), i++) ;
            else if (asset.startIndex > asset.endIndex)
                for (int i = Mathf.FloorToInt (asset.startIndex), end = Mathf.CeilToInt (asset.endIndex); i >= end; drawIndex (i), --i) ;
        }

        Color light = new Color (1, 1, 1, 0.1f), hard = new Color (1, 1, 1, 0.5f), block = new Color (0, 0, 0, 0.3f);

        GUIStyle _numberStyle;
        GUIStyle numberStyle
        {
            get
            {
                if (_numberStyle == null)
                {
                    _numberStyle = new GUIStyle (GUI.skin.label);
                    _numberStyle.fontSize = 6;
                    _numberStyle.alignment = TextAnchor.UpperLeft;
                }
                return _numberStyle;
            }
        }
    }
#endif
}