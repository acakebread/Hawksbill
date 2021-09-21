// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 05/09/2021 18:27:50 by seantcooper
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;

// namespace Hawksbill.Configurator
// {
//     public static class Find
//     {
//         internal static IEnumerable<ConfiguratorSelectable> Selectables(bool includeInactive = false) =>
//             GameObject.FindObjectsOfType<ConfiguratorSelectable> (includeInactive);

//         internal static IEnumerable<ConfiguratorSelectable> ParentSelectables(ConfiguratorObject obj, bool includeInactive = false) =>
//             obj.GetComponentsInParent<ConfiguratorSelectable> (includeInactive).Where (s => s.transform != obj.transform);

//         internal static IEnumerable<ConfiguratorSelectable> ChildrenSelectables(ConfiguratorObject obj, bool includeInactive = false) =>
//             obj.GetComponentsInChildren<ConfiguratorSelectable> (includeInactive).Where (s => s.transform != obj.transform);
//     }

//     public static class Msg
//     {
//         ///<summary>Invoke all components in the Scene of type T</summary>
//         public static bool Scene<T>(Action<T> action, Phase phase = Phase.RuntimeAndEditor) =>
//             IsPhase (phase) ? InvokeAction (SceneManager.GetActiveScene ().GetRootGameObjects ().SelectMany (GetBehaviours<T>), action) : false;

//         public static bool Selectables<T>(Action<T> action, Phase phase = Phase.RuntimeAndEditor) =>
//             IsPhase (phase) ? InvokeAction (Find.Selectables ().Select (s => s.gameObject).SelectMany (GetBehaviours<T>), action) : false;

//         ///<summary>Invoke all components in the GameObject of type T</summary>
//         public static bool Object<T>(ConfiguratorObject obj, Action<T> action, Phase phase = Phase.RuntimeAndEditor) =>
//             IsPhase (phase) ? InvokeAction (GetBehaviours<T> (obj.gameObject), action) : false;

//         ///<summary>Invoke all components in the GameObject of type T</summary>
//         public static bool ParentSelectables<T>(ConfiguratorObject obj, Action<T> action, Phase phase = Phase.RuntimeAndEditor) =>
//             IsPhase (phase) ? InvokeAction (Find.ParentSelectables (obj).SelectMany (s => GetBehaviours<T> (s.gameObject)), action) : false;

//         ///<summary>Invoke all components in the GameObject of type T</summary>
//         public static bool ChildrenSelectables<T>(ConfiguratorObject obj, Action<T> action, Phase phase = Phase.RuntimeAndEditor) =>
//             IsPhase (phase) ? InvokeAction (Find.ChildrenSelectables (obj).SelectMany (s => GetBehaviours<T> (s.gameObject)), action) : false;

//         public static class Runtime
//         {
//             public static bool Scene<T>(Action<T> action) => Msg.Scene (action, Phase.Runtime);
//             public static bool Selectables<T>(ConfiguratorObject obj, Action<T> action) => Msg.Selectables (action, Phase.Runtime);
//             public static bool Object<T>(ConfiguratorObject obj, Action<T> action) => Msg.Object (obj, action, Phase.Runtime);
//             public static bool ParentSelectables<T>(ConfiguratorObject obj, Action<T> action) => Msg.ParentSelectables (obj, action, Phase.Runtime);
//             public static bool ChildrenSelectables<T>(ConfiguratorObject obj, Action<T> action) => Msg.ChildrenSelectables (obj, action, Phase.Runtime);
//         }

//         public static class Editor
//         {
//             public static bool Scene<T>(Action<T> action) => Msg.Scene (action, Phase.Editor);
//             public static bool Selectables<T>(Action<T> action) => Msg.Selectables (action, Phase.Editor);
//             public static bool Object<T>(ConfiguratorObject obj, Action<T> action) => Msg.Object (obj, action, Phase.Editor);
//             public static bool ParentSelectables<T>(ConfiguratorObject obj, Action<T> action) => Msg.ParentSelectables (obj, action, Phase.Editor);
//             public static bool ChildrenSelectables<T>(ConfiguratorObject obj, Action<T> action) => Msg.ChildrenSelectables (obj, action, Phase.Editor);
//         }

//         // HELPERS
//         static IEnumerable<T> GetBehaviours<T>(GameObject gameObject) =>
//             gameObject.GetComponents<MonoBehaviour> ().Where (c => c is T && c.enabled).Cast<T> ();

//         static bool InvokeAction<T>(IEnumerable<T> items, Action<T> action)
//         {
//             items.ForAll (c => action (c));
//             return true;
//         }

//         static bool IsPhase(Phase phase)
//         {
//             switch (phase)
//             {
//                 case Phase.Runtime: return Application.isPlaying;
//                 case Phase.Editor: return !Application.isPlaying;
//             }
//             return true;
//         }

//         public enum Phase
//         {
//             Runtime = 0,
//             Editor = 1,
//             RuntimeAndEditor = 2,
//         }
//     }
// }