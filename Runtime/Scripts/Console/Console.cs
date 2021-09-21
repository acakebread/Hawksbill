// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;

// namespace Hawksbill
// {
//     public class Console : MonoBehaviour
//     {
//         UIDocument document => GetComponent<UIDocument> ();
//         List<string> items;
//         T getItem<T>(string name) where T : VisualElement => document.rootVisualElement.Q (name) as T;
//         ListView output => getItem<ListView> ("output");
//         TextField input => getItem<TextField> ("input");

//         void OnEnable()
//         {
//             items = new List<string> () { "hello", "world" };
//             output.itemsSource = items;
//             output.makeItem = makeItem;
//             output.bindItem = (e, i) => (e as Label).text = items[i];
//             output.itemHeight = 16;
//             output.MarkDirtyRepaint ();

//             //output.selectionType = SelectionType.Multiple;
//             // output.onItemChosen += obj => Debug.Log (obj);
//             // output.onSelectionChanged += objects => Debug.Log (objects);

//             input.RegisterCallback<KeyUpEvent> (onKeyUp);
//         }

//         void onKeyUp(KeyUpEvent e)
//         {
//             if (e.keyCode == KeyCode.Return || e.keyCode == (KeyCode) 10)
//             {
//                 addItem (input.value);
//                 input.value = "";
//             }
//         }

//         VisualElement makeItem()
//         {
//             return new Label { style = { color = Color.white, unityTextAlign = TextAnchor.MiddleLeft } };
//         }

//         void addItem(string item)
//         {
//             items.Add (item);
//             output.Refresh ();
//         }
//     }
// }