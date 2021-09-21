// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hawksbill
{
    public static class SerializedPropertyEx
    {
        public static string getToolTip(this SerializedProperty prop)
        {
            FieldInfo info = prop.getFieldInfo ();
            if (info != null)
            {
                TooltipAttribute tooltip = (TooltipAttribute) info.GetCustomAttribute (typeof (TooltipAttribute));
                if (tooltip != null)
                    return tooltip.tooltip;
            }
            return null;
        }

        public static FieldInfo getFieldInfo(this SerializedProperty prop) => prop.getFieldInfo (out Type type);
        public static FieldInfo getFieldInfo(this SerializedProperty prop, out Type type)
        {
            const string arrayData = @"\.Array\.data\[[0-9]+\]";
            var path = prop.propertyPath;
            var lookingForArrayElement = Regex.IsMatch (path, arrayData + "$");
            path = Regex.Replace (path, arrayData, ".___ArrayElement___");

            FieldInfo fieldInfo = null;
            type = prop.serializedObject.targetObject.GetType ();
            string[] parts = path.Split ('.');
            for (int i = 0; i < parts.Length; i++)
            {
                string member = parts[i];
                FieldInfo foundField = null;
                for (Type currentType = type; foundField == null && currentType != null; currentType = currentType.BaseType)
                    foundField = currentType.GetField (member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (foundField == null)
                {
                    type = null;
                    return null;
                }

                fieldInfo = foundField;
                type = fieldInfo.FieldType;
                if (i < parts.Length - 1 && parts[i + 1] == "___ArrayElement___" && type.IsArrayOrList ())
                {
                    i++;
                    type = type.GetArrayOrListElementType ();
                }
            }

            if (lookingForArrayElement && type != null && type.IsArrayOrList ())
                type = type.GetArrayOrListElementType ();
            return fieldInfo;
        }

        static Type GetArrayOrListElementType(this Type listType)
        {
            if (listType.IsArray) return listType.GetElementType ();
            else if (listType.IsGenericType && listType.GetGenericTypeDefinition () == typeof (List<>)) return listType.GetGenericArguments ()[0];
            return null;
        }

        static bool IsArrayOrList(this Type listType)
        {
            if (listType.IsArray) return true;
            else if (listType.IsGenericType && listType.GetGenericTypeDefinition () == typeof (List<>)) return true;
            return false;
        }
    }
}
