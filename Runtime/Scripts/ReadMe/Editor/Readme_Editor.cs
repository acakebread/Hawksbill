// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 31/08/2021 08:39:12 by seantcooper
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>

    [CustomEditor (typeof (Readme)), InitializeOnLoad]
    public class Readme_Editor : Editor
    {
        const float LineSpace = 10;
        Readme readme => target as Readme;

        static Readme_Editor()
        {
            EditorApplication.delayCall += SelectAutomatically;
        }

        static void SelectAutomatically()
        {
            // var readme = AssetDatabase.FindAssets ("t:Readme").
            //     Select (a => AssetDatabase.LoadAssetAtPath<Readme> (AssetDatabase.GUIDToAssetPath (a))).
            //     OrderBy (a => -a.priority).FirstOrDefault ();

            // if (readme)
            // {
            //     Debug.Log ("Show readme");
            //     Selection.activeObject = readme;
            // }
            // Debug.Log ("Find " + String.Join (",", AssetDatabase.FindAssets ("t:Readme")));
        }

        protected override void OnHeaderGUI()
        {
            GUILayout.Label (readme.image, GUILayout.Width (Screen.width), GUILayout.Height (Screen.width * readme.image.height / readme.image.width));
            GUILayout.Space (LineSpace);
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label (readme.title, Styles.Title);
            GUILayout.Space (LineSpace);
            readme.sections.ForAll (s => drawSection (s));
        }

        void drawSection(Readme.Section section)
        {
            drawLabel (section.header, Styles.Header);
            drawLabel (section.body, Styles.Body);
            drawLinkButton (section.link, Styles.Link);
            GUILayout.Space (LineSpace);
        }

        void drawLabel(string text, GUIStyle style)
        {
            if (!string.IsNullOrEmpty (text)) GUILayout.Label (text, style);
        }

        void drawLinkButton(string link, GUIStyle style)
        {
            if (!string.IsNullOrEmpty (link) && GUILayout.Button (link, Styles.Link))
                Application.OpenURL (link);
        }

        // Styles
        static class Styles
        {
            static Styles()
            {
                Body = new GUIStyle (EditorStyles.label)
                {
                    wordWrap = true,
                    fontSize = 14,
                    richText = true,
                };
                Title = new GUIStyle (EditorStyles.label)
                {
                    normal = { textColor = new Color32 (255, 255, 255, 255) },
                    wordWrap = true,
                    fontSize = 26,
                    richText = true,

                };
                Header = new GUIStyle (EditorStyles.label)
                {
                    wordWrap = true,
                    fontStyle = FontStyle.Bold,
                    fontSize = 18,
                    richText = true,
                };
                Link = new GUIStyle (EditorStyles.label)
                {
                    wordWrap = false,
                    stretchWidth = false,
                    normal = { textColor = new Color32 (255, 160, 0, 255) },
                    fontSize = 14,
                    richText = true,
                };
            }
            public static GUIStyle Title;
            public static GUIStyle Header;
            public static GUIStyle Body;
            public static GUIStyle Link;
        }
    }
}
