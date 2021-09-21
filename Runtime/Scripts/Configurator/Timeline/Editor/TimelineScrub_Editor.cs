// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 28/08/2021 10:05:19 by seantcooper
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Hawksbill.Configurator.TimelineScrub;
using static Hawksbill.Configurator.TimelineQuery;
using UnityEditor.Timeline;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Collections.Generic;
using System;
using System.Reflection;
using Unity.Mathematics;

namespace Hawksbill.Configurator
{
    [CustomEditor (typeof (TimelineScrub))]
    public class TimelineScrub_Editor : Editor
    {
        TimelineScrub scrub => target as TimelineScrub;
        Texture2D[] icons => ProjectSettings.Instance.media.playControls.textures;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI ();

            // input and change the time

            drawTime ();
            GUILayout.Space (4);
            drawControls ();
            GUILayout.Space (4);
            drawScubber ();
            repaintTimeline ();
        }

        void drawTime()
        {
            EditorGUI.BeginChangeCheck ();
            var text = EditorGUILayout.TextField ("Current Time", scrub.time.ToString ("N4"));
            if (EditorGUI.EndChangeCheck () && double.TryParse (text, out double time))
                scrub.time = time;
        }

        void drawControls()
        {
            State selected = (State) GUILayout.Toolbar ((int) scrub.currentState, icons, GUILayout.MaxHeight (EditorGUIUtility.singleLineHeight * 1.05f));
            if (selected != scrub.currentState)
            {
                scrub.currentState = selected;
                scrub.setState (scrub.currentState);
                Repaint ();
                repaintTimeline ();
            }
        }

        void drawScubber()
        {
            float Height1 = 10, Height2 = scrub.showChildren ? Height1 / 2 : 0;
            Dictionary<string, ClipInfo[]> clips = scrub.director.getClipInfos ().GroupBy (c => c.path).ToDictionary (g => g.Key, g => g.ToArray ());
            float height = clips.Values.Select (c => c.First ().parent ? Height2 : Height1).Sum ();

            GUILayout.Label ("", GUILayout.MinHeight (height));
            Rect rect = GUILayoutUtility.GetLastRect (), p = new Rect (rect);

            double duration = scrub.director.duration;
            float getX(double t) => (float) (t / duration) * rect.width;

            EditorGUI.DrawRect (rect, new Color (0, 0, 0, 0.25f));

            foreach (var kv in clips)
            {
                p.height = kv.Value.First ().parent ? Height2 : Height1;
                if (p.height == 0) continue;

                TrackAsset track = kv.Value.First ().track;

                Color color = getTrackColor (track);

                foreach (var clip in kv.Value)
                {
                    float x1 = Mathf.Floor (getX (clip.start) + p.x), x2 = Mathf.Floor (getX (clip.end) + p.x);
                    EditorGUI.DrawRect (new Rect (x1, p.y, (x2 - x1) - 1, p.height - 1), color);
                }
                p.y += p.height;
            }

            if (Event.current.type == EventType.MouseDown && (mouseDown = rect.Contains (Event.current.mousePosition)) ||
                (Event.current.type == EventType.MouseDrag && mouseDown))
                setMouse (rect);

            // time marker
            float x = (float) (scrub.time / duration) * rect.width + rect.x;
            EditorGUI.DrawRect (new Rect (rect) { x = x, width = 1 }, new Color (1, 1, 1, 1));

            // show children
            Vector2 buttonSize = new Vector2 (20, 8);
            if (GUI.Button (new Rect (rect.center.x - buttonSize.x / 2, rect.max.y, buttonSize.x, buttonSize.y), ""))
                scrub.showChildren = !scrub.showChildren;
            GUILayout.Space (buttonSize.y);
        }

        Color getTrackColor(TrackAsset track)
        {
            foreach (var c in track.GetType ().GetCustomAttributes<TrackColorAttribute> ().Select (t => t.color))
                return c;
            return new Color (1, 1, 1, 0.6f);
        }

        bool mouseDown;
        void setMouse(Rect rect)
        {
            scrub.time = Mathf.Clamp01 ((Event.current.mousePosition.x - rect.x) / rect.width) * scrub.director.duration;
            Repaint ();
            repaintTimeline ();
            scrub.setState (State.Stop);
        }

        void repaintTimeline()
        {
            var window = EditorWindow.GetWindow<TimelineEditorWindow> ("Timeline (Scrub)", false);
            if (window && window.hasFocus)
            {
                // Debug.Log ("Window " + window.hasFocus + " " + window.is);
                window.Repaint ();
            }
        }
    }
}