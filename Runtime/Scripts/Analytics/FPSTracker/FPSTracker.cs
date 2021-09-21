// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawksbill
{
    public class FPSTracker : SingletonMonoBehaviour<FPSTracker>
    {
        const int SAMPLES = 200;
        [Range (0.1f, 1)] public float updateSpeed = 0.1f;
        //[Range (1, 200)] public int targetFrameRate = 120;
        [Range (0.1f, 3f)] public float fontScale = 1;
        public Color textColor = new Color (1f, 1f, 1f, 1.0f);
        public bool showLogs = true;

        [Header ("Output:")]
        public int frameIndex;
        public float fps, totalTime, frameTime, memory;
        string text = "...";

        static System.Diagnostics.Stopwatch stopWatch;
        public static bool isEnabled { get { return instance && instance.enabled; } }

        long[] timestamps;
        long timestamp;

        protected override void Awake()
        {
            base.Awake ();
            (stopWatch = new System.Diagnostics.Stopwatch ()).Start ();
            timestamps = new long[SAMPLES];
        }

        void Start() => timestamp = stopWatch.ElapsedMilliseconds + (long) (updateSpeed * 1000);

        void Update()
        {
            timestamps[(++frameIndex) % SAMPLES] = stopWatch.ElapsedTicks;
            if (stopWatch.ElapsedMilliseconds > timestamp && frameIndex >= SAMPLES)
            {
                timestamp += (long) (updateSpeed * 1000);
                memory = (float) System.GC.GetTotalMemory (false) / (1024 * 1024);
                totalTime = Time.time;
                frameTime = TickToMilliseconds (stopWatch.ElapsedTicks - timestamps[(frameIndex + 1) % SAMPLES]) / SAMPLES;
                fps = 1000f / frameTime;
                text = string.Format ("{0:00.0} ms ({1:0} fps) {2:t} {3:0}mb", frameTime, fps, TimeSpan.FromSeconds ((int) totalTime), memory.ToString ("N2"));
            }
        }


        public static float TickToMilliseconds(long ticks) => (float) ticks * 1000 / System.Diagnostics.Stopwatch.Frequency;
        public static float TickToSeconds(long ticks) => (float) ticks / System.Diagnostics.Stopwatch.Frequency;

#if (DEVELOPMENT_BUILD || UNITY_EDITOR)

        void OnValidate() => updateStyle ();

        GUIStyle style, startStyle;
        void OnGUI()
        {
            if (!Application.isPlaying) return;
            if (style == null)
            {
                style = new GUIStyle (GUI.skin.label);
                style.alignment = TextAnchor.MiddleLeft;
                style.padding = new RectOffset (4, 4, 0, 0);
                style.fontSize = 16;
                startStyle = new GUIStyle (style);
                updateStyle ();
            }

            Vector2 screen = setScaleMatrix (600);
            GUILayoutOption minWidth = GUILayout.MinWidth (screen.x);

            GUILayout.Label (text, style, minWidth);
            drawLogs (minWidth);
        }


        Vector2 setScaleMatrix(float width)
        {
            GUI.matrix = Matrix4x4.Scale (Vector3.one * (Screen.width / width));
            return new Vector2 (width, Screen.height / width);
        }

        void updateStyle()
        {
            if (style == null) return;
            style.normal.textColor = textColor;
            style.fontSize = (int) ((float) startStyle.fontSize * fontScale);
        }

#endif

        static Dictionary<string, object> Logs = new Dictionary<string, object> ();
        public static void Log(string name, object value = null)
        {
            //print (name + ": " + value);
            if (!instance) return;
            if (Logs == null) Logs = new Dictionary<string, object> ();
            Logs[name] = value;
        }

        public void drawLogs(GUILayoutOption minWidth)
        {
#if (DEVELOPMENT_BUILD || UNITY_EDITOR)
            if (showLogs)
                Logs.ForAll (p => GUILayout.Label (p.Value != null ? p.Key + ": " + p.Value : p.Key, style, minWidth));
#endif
        }
    }
}
