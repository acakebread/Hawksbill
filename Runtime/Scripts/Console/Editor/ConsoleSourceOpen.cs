// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/03/2021 11:27:55 by seantcooper
// WORKS FOR VERSION UNITY 2020.2.6f1

using System;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public static class ConsoleSourceOpen
{
    [OnOpenAssetAttribute (0)]
    static bool OnOpenAsset(int instanceID, int lineNumber)
    {
        try
        {
            if (lineNumber < 0) return false;
            UnityEngine.Object o = EditorUtility.InstanceIDToObject (instanceID);
            if (o.name == "Pretty" && o.GetType ().Name == "MonoScript")
            {
                var nextLine = GetConsoleOutput ().
                    Split ('\n').
                    SkipWhile (l => !l.StartsWith ("hawksbill.pretty", true, null)).
                    Skip (1).FirstOrDefault ();

                string[] result = nextLine.Split (new string[] { " (at " }, StringSplitOptions.None).Last ().Split (':');
                var path = (Application.dataPath.Substring (0, Application.dataPath.LastIndexOf ("Assets")) + result[0]);
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (path, int.Parse (result[1].Split (')')[0]));
                return true;
            }
        }
        catch { }
        return false;
    }

    static string GetConsoleOutput()
    {
        Type consoleWindow = Assembly.GetAssembly (typeof (UnityEditor.EditorWindow)).GetType ("UnityEditor.ConsoleWindow");
        object instance = consoleWindow.GetField ("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic).GetValue (null);
        if ((object) UnityEditor.EditorWindow.focusedWindow == instance)
            return consoleWindow.GetField ("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic).GetValue (instance).ToString ();
        return null;
    }
}
