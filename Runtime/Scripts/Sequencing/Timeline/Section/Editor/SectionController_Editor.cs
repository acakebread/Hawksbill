// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 05/01/2021 08:57:03 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;
using UnityEngine.Timeline;

namespace Hawksbill.Sequencing
{
    ///<summary>Put text here to describe the Class</summary>
    [CustomEditor (typeof (SectionController))]
    public class SectionController_Editor : Editor
    {
        static bool sections;
        SectionController controller => target as SectionController;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector ();
            drawSections ();
        }

        void drawSections()
        {
            sections = EditorGUILayout.Foldout (sections, "Sections");
            if (sections)
            {
                using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
                    foreach (var clip in controller.clips)
                        EditorGUILayout.ObjectField (clip.displayName, clip.asset, typeof (SectionClip), true);
            }
        }
    }
}