// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 13/03/2021 16:30:26 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using UnityEditor;

namespace Hawksbill
{
#if UNITY_EDITOR
    ///<summary>Put text here to describe the Class</summary>
    public static class GUIHelpers
    {
        public class WideMode : IDisposable
        {
            bool _widemode;
            public WideMode(bool widemode)
            {
                _widemode = EditorGUIUtility.wideMode;
                EditorGUIUtility.wideMode = widemode;
            }
            public void Dispose() => EditorGUIUtility.wideMode = _widemode;
        }

        public class Enable : IDisposable
        {
            bool enabled;
            public Enable(bool enabled)
            {
                this.enabled = GUI.enabled;
                GUI.enabled = enabled;
            }
            public void Dispose() => GUI.enabled = enabled;
        }

        public class ForeColor : IDisposable
        {
            Color color;
            public ForeColor(Color color)
            {
                this.color = GUI.color;
                GUI.color = color;
            }
            public void Dispose() => GUI.color = color;
        }

        public class BackColor : IDisposable
        {
            Color color;
            public BackColor(Color color)
            {
                this.color = GUI.color;
                GUI.backgroundColor = color;
            }
            public void Dispose() => GUI.backgroundColor = color;
        }

        public class Indent : IDisposable
        {
            int indent;
            public Indent(int indent)
            {
                this.indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = indent;
            }
            public void Dispose() => EditorGUI.indentLevel = indent;
        }
    }
#endif
}