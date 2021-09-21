// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 09/08/2021 11:20:53 by seantcooper
// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 07/08/2021 10:17:43 by seantcooper
// using UnityEngine;
// using UnityEditor;
// using Hawksbill;
// using Hawksbill.Geometry;
// using static Hawksbill.GenericItemDesc;
// using static Hawksbill.GenericItemDesc.Field;
// using System.Linq;

// namespace Hawksbill
// {

//     [CanEditMultipleObjects, CustomPropertyDrawer (typeof (GenericItemQuery))]
//     public class GenericItemQuery_PropertyDrawer : PropertyDrawer
//     {
//         const float Padding = 2;
//         static Color DefaultColor = new Color (0, 0, 0, 0.1f);
//         static Color SelectedColor = new Color (0, 0.3f, 1, 0.1f);

//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             GenericItemQuery query = property.getTarget () as GenericItemQuery;

//             var desc = property.FindPropertyRelative ("description");

//             Rect b = new Rect (0, position.y, Screen.width, GetPropertyHeight (property, label)).inflate (Padding);
//             EditorGUI.DrawRect (b, new Color (0, 0, 0, 0.1f));

//             Rect line = new Rect (position) { height = EditorGUIUtility.singleLineHeight, y = position.y + Padding };

//             EditorGUI.PropertyField (line, property.FindPropertyRelative (nameof (query.description)));
//             line.y += line.height + EditorGUIUtility.standardVerticalSpacing;

//             EditorGUI.PropertyField (line, property.FindPropertyRelative (nameof (query.container)));
//             line.y += line.height + EditorGUIUtility.standardVerticalSpacing;

//             if (!(query.description && query.container)) return;

//             var fields = query.description.fields;
//             var items = query.items;

//             for (int i = 0; i < fields.Length; i++)
//             {
//                 // var content = new GUIContent (name.stringValue);
//                 // EditorGUI.BeginProperty (line, content, xvalue);
//                 // EditorGUI.BeginChangeCheck ();

//                 var field = fields[i];
//                 var options = query.items.getDistinctValues (field.name).Select (v => v.ToString ()).ToArray ();
//                 EditorGUI.Popup (line, field.name, -1, options);



//                 // switch ((Value.Type) type.intValue)
//                 // {
//                 //     case Value.Type.Boolean:
//                 //         newValue = EditorGUI.Toggle (line, content, value);
//                 //         break;

//                 //     case Value.Type.Int:
//                 //         if ((int) range.min == (int) range.max) newValue = EditorGUI.DelayedIntField (line, content, value);
//                 //         else newValue = EditorGUI.IntSlider (line, content.text, value, (int) range.min, (int) range.max);
//                 //         break;

//                 //     case Value.Type.Float:
//                 //         if (range.min == range.max) newValue = EditorGUI.DelayedFloatField (line, content, value);
//                 //         else newValue = EditorGUI.Slider (line, content.text, value, range.min, range.max);
//                 //         break;

//                 //     default:
//                 //     case Value.Type.String: newValue = EditorGUI.TextField (line, content, value); break;
//                 // }
//                 //                if (EditorGUI.EndChangeCheck ()) xvalue.FindPropertyRelative ("raw").stringValue = newValue;
//                 //EditorGUI.EndProperty ();
//                 line.y += line.height + EditorGUIUtility.standardVerticalSpacing;
//             }
//         }





//         // for (int i = 0; i < count; i++)
//         // {
//         //     var element = values.GetArrayElementAtIndex (i);
//         //     var field = element.FindPropertyRelative ("field");
//         //     if (field == null) continue;

//         //     var xvalue = element.FindPropertyRelative ("value");
//         //     var name = field.FindPropertyRelative ("name");
//         //     var type = field.FindPropertyRelative ("type");

//         //     var rangep = field.FindPropertyRelative ("range");
//         //     MinMaxFloat range = new MinMaxFloat (rangep.FindPropertyRelative ("min").floatValue, rangep.FindPropertyRelative ("max").floatValue);
//         //     Field.Value newValue, value = (Field.Value) xvalue.getTarget ();

//         //     var content = new GUIContent (name.stringValue);
//         //     EditorGUI.BeginProperty (line, content, xvalue);
//         //     EditorGUI.BeginChangeCheck ();
//         //     switch ((Value.Type) type.intValue)
//         //     {
//         //         case Value.Type.Boolean:
//         //             newValue = EditorGUI.Toggle (line, content, value);
//         //             break;

//         //         case Value.Type.Int:
//         //             if ((int) range.min == (int) range.max) newValue = EditorGUI.DelayedIntField (line, content, value);
//         //             else newValue = EditorGUI.IntSlider (line, content.text, value, (int) range.min, (int) range.max);
//         //             break;

//         //         case Value.Type.Float:
//         //             if (range.min == range.max) newValue = EditorGUI.DelayedFloatField (line, content, value);
//         //             else newValue = EditorGUI.Slider (line, content.text, value, range.min, range.max);
//         //             break;

//         //         default:
//         //         case Value.Type.String: newValue = EditorGUI.TextField (line, content, value); break;
//         //     }
//         //     if (EditorGUI.EndChangeCheck ()) xvalue.FindPropertyRelative ("raw").stringValue = newValue;
//         //     EditorGUI.EndProperty ();
//         //     line.y += line.height + EditorGUIUtility.standardVerticalSpacing;
//         // }


//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             GenericItemQuery query = property.getTarget () as GenericItemQuery;
//             return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) *
//                 (query.description.fields.Length + 2) + Padding * 2;
//         }
//     }
// }