// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Hawksbill.Analytics
{
    public class ProfilerWindow : EditorWindow
    {
        static float time;
        const string SP = " ";
        const string MSF = "N2";
        const int COLUMN_WIDTH = 64;
        const float UPDATE_SPEED = 0.25f;

        void Awake()
        {
            UnityEngine.Debug.Log (this + "::Awake");
            time = Time.time;
        }

        [MenuItem ("Hawksbill/Window/Profiler Window")]
        public static void ShowWindow()
        {
            var _window = EditorWindow.GetWindow (typeof (ProfilerWindow), false, "Profiler");
        }
        static float h;
        static GUIStyle[] headerStyles, timesStyles, countersStyles;
        static GUIStyle normalStyle, numberStyle, greenStyle, amberStyle, redStyle;
        void createStyles()
        {
            normalStyle = new GUIStyle (GUI.skin.label);
            normalStyle.fontSize = 10;
            normalStyle.padding = new RectOffset (3, 3, 3, 3);
            normalStyle.margin = new RectOffset (2, 2, 2, 2);
            normalStyle.normal.background = MakeTex (new Color (1, 1, 1, 0.02f));

            numberStyle = new GUIStyle (normalStyle);
            numberStyle.alignment = TextAnchor.MiddleRight;

            greenStyle = new GUIStyle (numberStyle);
            amberStyle = new GUIStyle (numberStyle);
            amberStyle.normal.background = MakeTex (new Color (1f, 0.5f, 0, 0.5f));
            redStyle = new GUIStyle (numberStyle);
            redStyle.normal.background = MakeTex (new Color (1f, 0, 0, 0.5f));

            headerStyles = new GUIStyle[4] { normalStyle, normalStyle, normalStyle, normalStyle };
            timesStyles = new GUIStyle[4] { normalStyle, normalStyle, numberStyle, numberStyle };
            countersStyles = new GUIStyle[4] { normalStyle, normalStyle, numberStyle, numberStyle };

            h = normalStyle.margin.horizontal;
        }

        private Texture2D MakeTex(Color _color)
        {
            Texture2D result = new Texture2D (1, 1);
            result.SetPixel (0, 0, _color);
            result.Apply ();
            return result;
        }

        void Update()
        {
            // JobProfiler.updateFPS ();
            if (Time.time >= time)
            {
                // myString = (1.0f / Time.smoothDeltaTime).ToString ();
                Repaint ();
                time = Time.time + UPDATE_SPEED;
            }
        }

        void OnGUI()
        {
            if (headerStyles == null) createStyles ();
            drawTimers ();
        }

        void drawTimers()
        {
            float w = EditorGUIUtility.currentViewWidth;
            float w1 = w - COLUMN_WIDTH * 2;
            var _columns = new float[4] { 8 - h, (w1 - 8) - h, COLUMN_WIDTH - h, COLUMN_WIDTH - h };

            drawColumn (_columns, headerStyles, "", "System:", "time ms", "slow ms");
            drawLine (GUILayoutUtility.GetLastRect ());

            foreach (var _timer in Profiler.profiles.Values)
                drawTimerColumn (_columns, _timer);

            drawLine (GUILayoutUtility.GetLastRect ());
        }

        void drawLine(Rect r)
        {
            EditorGUI.DrawRect (new Rect (r.x + 8, r.yMax + 2, r.width - 8, 1), new Color (0, 0, 0, 0.9f));
            GUILayout.Space (4);
        }

        void drawTimerColumn(float[] _columns, Profiler.Profile profile)
        {
            string name = profile.description; //name;
            GUIStyle[] _styles = new GUIStyle[] { normalStyle, normalStyle, getStyle (profile.ticks), getStyle (profile.slowest), };
            drawColumn (_columns, _styles, "", name, formatTicks (profile.ticks) + SP, "(" + formatTicks (profile.slowest) + ")" + SP);
        }

        void drawColumn(float[] columns, GUIStyle[] styles, params string[] values)
        {
            GUILayout.BeginHorizontal ();
            for (var i = 0; i < values.Length; i++)
                GUILayout.Label (values[i], styles[i], GUILayout.Width (columns[i]));
            GUILayout.EndHorizontal ();
        }

        public static string formatTicks(long ticks)
        {
            return ticks < 0 ? "..." : Profiler.TickToMilliseconds (ticks).ToString (MSF);
        }

        public static GUIStyle getStyle(long ticks)
        {
            var s = Profiler.TickToSeconds (ticks);
            if (s < 0.002) return greenStyle;
            if (s < 0.005) return amberStyle;
            return redStyle;
        }
    }
}
