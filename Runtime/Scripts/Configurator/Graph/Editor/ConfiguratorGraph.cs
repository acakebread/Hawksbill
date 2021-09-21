// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 17/09/2021 16:18:06 by seantcooper
using System;
using System.Collections.Generic;
// using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using static UnityEditor.AssetPreview;
using static UnityEngine.GUILayout;
using System.Linq;

namespace Hawksbill.Configurator
{
    public class ConfiguratorGraphWindow : EditorWindow
    {
        [MenuItem ("Hawksbill/Configurator/Configurator Graph")]
        public static void ShowWindow()
        {
            var _window = EditorWindow.GetWindow (typeof (ConfiguratorGraphWindow), false, "Configurator Graph");
        }

        [SerializeField] Vector2 scrollPosition;

        void OnGUI()
        {
            var brain = FindObjectOfType<ConfiguratorBrain> ();
            if (!brain) return;


            scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
            drawColumn (brain.getPrimarySelectable ());
            EditorGUILayout.EndScrollView ();
        }

        static Dictionary<UnityEngine.Object, Hawksbill.Editor> cachedEditors = new Dictionary<UnityEngine.Object, Hawksbill.Editor> ();
        public static Hawksbill.Editor GetEditor(UnityEngine.Object target)
        {
            if (!cachedEditors.ContainsKey (target)) cachedEditors[target] = Editor.CreateEditor (target) as Hawksbill.Editor;
            return cachedEditors[target];
        }

        const float ColumnWidth = 200;
        void drawColumn(ConfiguratorSelectable selectable) => drawColumn (new ConfiguratorSelectable[] { selectable });
        void drawColumn(ConfiguratorSelectable[] selectables)
        {
            if (selectables.Length == 0) return;

            EditorGUIUtility.labelWidth = ColumnWidth / 3;
            EditorGUILayout.BeginVertical ("Box", MaxWidth (ColumnWidth), MinWidth (ColumnWidth));

            foreach (ConfiguratorSelectable selectable in selectables)
                drawSelectable (selectable);

            EditorGUILayout.EndVertical ();
        }

        void drawSelectable(ConfiguratorSelectable selectable)
        {
            drawObjectName (GetMiniThumbnail (selectable), selectable.name);

            EditorGUILayout.BeginHorizontal ("box");
            EditorGUILayout.BeginVertical ("box");
            using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
                selectable.GetComponents<ConfiguratorExtension> ().ForAll (drawExtension);
            EditorGUILayout.EndVertical ();

            Debug.Log ("selectable.immediateChildSelectables " + selectable.immediateChildSelectables.Count ());
            drawColumn (selectable.immediateChildSelectables.ToArray ());
            EditorGUILayout.EndHorizontal ();
        }

        void drawExtension(ConfiguratorExtension extension)
        {
            drawObjectName (GetMiniThumbnail (extension), extension.GetType ().Name.Replace ("Configurator", ""));
            // drawObjectName (extension);
            // var editor = GetEditor (extension);
            // if (EditorGUILayout.Foldout (false, editor.serializedObject.targetObject.GetType ().Name))
            // {
            //     using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
            //         editor.OnInspectorGUI (true);
            // }
        }

        void drawObjectName(Texture2D icon, string name)
        {
            EditorGUILayout.BeginHorizontal ();
            GUILayout.Label (icon, MaxHeight (EditorGUIUtility.singleLineHeight), MaxWidth (EditorGUIUtility.singleLineHeight));
            GUILayout.Label (name);
            EditorGUILayout.EndHorizontal ();
        }

        void drawObjectName(ConfiguratorObject obj)
        {
            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.ObjectField (obj, obj.GetType (), true);
            EditorGUILayout.EndHorizontal ();
        }

        //     static float h;
        //     static GUIStyle[] headerStyles, timesStyles, countersStyles;
        //     static GUIStyle normalStyle, numberStyle, greenStyle, amberStyle, redStyle;
        //     void createStyles()
        //     {
        //         normalStyle = new GUIStyle (GUI.skin.label);
        //         normalStyle.fontSize = 10;
        //         normalStyle.padding = new RectOffset (3, 3, 3, 3);
        //         normalStyle.margin = new RectOffset (2, 2, 2, 2);
        //         normalStyle.normal.background = MakeTex (new Color (1, 1, 1, 0.02f));

        //         numberStyle = new GUIStyle (normalStyle);
        //         numberStyle.alignment = TextAnchor.MiddleRight;

        //         greenStyle = new GUIStyle (numberStyle);
        //         amberStyle = new GUIStyle (numberStyle);
        //         amberStyle.normal.background = MakeTex (new Color (1f, 0.5f, 0, 0.5f));
        //         redStyle = new GUIStyle (numberStyle);
        //         redStyle.normal.background = MakeTex (new Color (1f, 0, 0, 0.5f));

        //         headerStyles = new GUIStyle[4] { normalStyle, normalStyle, normalStyle, normalStyle };
        //         timesStyles = new GUIStyle[4] { normalStyle, normalStyle, numberStyle, numberStyle };
        //         countersStyles = new GUIStyle[4] { normalStyle, normalStyle, numberStyle, numberStyle };

        //         h = normalStyle.margin.horizontal;
        //     }

        //     private Texture2D MakeTex(Color _color)
        //     {
        //         Texture2D result = new Texture2D (1, 1);
        //         result.SetPixel (0, 0, _color);
        //         result.Apply ();
        //         return result;
        //     }

        //     void Update()
        //     {
        //         // JobProfiler.updateFPS ();
        //         if (Time.time >= time)
        //         {
        //             // myString = (1.0f / Time.smoothDeltaTime).ToString ();
        //             Repaint ();
        //             time = Time.time + UPDATE_SPEED;
        //         }
        //     }

        //     void OnGUI()
        //     {
        //         if (headerStyles == null) createStyles ();
        //         drawTimers ();
        //     }

        //     void drawTimers()
        //     {
        //         float w = EditorGUIUtility.currentViewWidth;
        //         float w1 = w - COLUMN_WIDTH * 2;
        //         var _columns = new float[4] { 8 - h, (w1 - 8) - h, COLUMN_WIDTH - h, COLUMN_WIDTH - h };

        //         drawColumn (_columns, headerStyles, "", "System:", "time ms", "slow ms");
        //         drawLine (GUILayoutUtility.GetLastRect ());

        //         foreach (var _timer in Profiler.profiles.Values)
        //             drawTimerColumn (_columns, _timer);

        //         drawLine (GUILayoutUtility.GetLastRect ());
        //     }

        //     void drawLine(Rect r)
        //     {
        //         EditorGUI.DrawRect (new Rect (r.x + 8, r.yMax + 2, r.width - 8, 1), new Color (0, 0, 0, 0.9f));
        //         GUILayout.Space (4);
        //     }

        //     void drawTimerColumn(float[] _columns, Profiler.Profile profile)
        //     {
        //         string name = profile.description; //name;
        //         GUIStyle[] _styles = new GUIStyle[] { normalStyle, normalStyle, getStyle (profile.ticks), getStyle (profile.slowest), };
        //         drawColumn (_columns, _styles, "", name, formatTicks (profile.ticks) + SP, "(" + formatTicks (profile.slowest) + ")" + SP);
        //     }

        //     void drawColumn(float[] columns, GUIStyle[] styles, params string[] values)
        //     {
        //         GUILayout.BeginHorizontal ();
        //         for (var i = 0; i < values.Length; i++)
        //             GUILayout.Label (values[i], styles[i], GUILayout.Width (columns[i]));
        //         GUILayout.EndHorizontal ();
        //     }

        //     public static string formatTicks(long ticks)
        //     {
        //         return ticks < 0 ? "..." : Profiler.TickToMilliseconds (ticks).ToString (MSF);
        //     }

        //     public static GUIStyle getStyle(long ticks)
        //     {
        //         var s = Profiler.TickToSeconds (ticks);
        //         if (s < 0.002) return greenStyle;
        //         if (s < 0.005) return amberStyle;
        //         return redStyle;
        //     }
    }
}
