using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Hawksbill
{
    [UnityEditor.InitializeOnLoad]
    public class EditorSettings : Editor
    {
        static EditorSettings()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawMarkers;
        }
        static void DrawMarkers(int instanceID, Rect rect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject (instanceID) as GameObject;
            PrettyEditorAttribute attribute;

            if (gameObject && (attribute = gameObject.GetComponent<PrettyEditorAttribute> ()))
            {
                switch (attribute.type)
                {
                    case PrettyEditorAttribute.Type.Full:
                        const float Stub = 12;
                        rect.xMin -= Stub;
                        Rect row = rect, stub = rect, line = rect;
                        EditorGUI.DrawRect (row, attribute.color);
                        stub.xMax = stub.xMin + Stub - 2;
                        EditorGUI.DrawRect (stub, attribute.color);
                        line.yMin = line.yMax - 1;
                        EditorGUI.DrawRect (line, attribute.color);

                        if (attribute.error)
                            EditorGUI.DrawRect (new Rect (rect) { width = rect.height * 4 }, attribute.errorColor);
                        break;

                    case PrettyEditorAttribute.Type.Margin:
                        Rect r = new Rect (rect) { x = 32, width = 4, height = rect.height - 1 };
                        if (attribute.error)
                            EditorGUI.DrawRect (new Rect (r) { x = r.x + r.width + 1 }, attribute.errorColor);
                        EditorGUI.DrawRect (r, attribute.color);
                        break;
                }
            }
        }
    }
}
