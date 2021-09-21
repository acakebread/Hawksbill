// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/09/2021 13:33:46 by seantcooper
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hawksbill.Configurator
{
    // Selection
    public interface IObjectSelectedHandler { void OnObjectSelected(); }
    public interface IObjectDeselectedHandler { void OnObjectDeselected(); }
    public interface ISelectedHandler { void OnSelected(ConfiguratorSelectable selectable); }
    public interface IDeselectedHandler { void OnDeselected(ConfiguratorSelectable selectable); }
    public interface IChildSelectedHandler { void OnChildSelected(ConfiguratorSelectable selectable); }
    public interface IChildDeselectedHandler { void OnChildDeselected(ConfiguratorSelectable selectable); }
    public interface IParentSelectedHandler { void OnParentSelected(ConfiguratorSelectable selectable); }
    public interface IParentDeselectedHandler { void OnParentDeselected(ConfiguratorSelectable selectable); }

    // Activation
    public interface IObjectActivatedHandler { void OnObjectActivated(); }
    public interface IObjectDeactivatedHandler { void OnObjectDeactivated(); }
    public interface IActivatedHandler { void OnActivated(ConfiguratorSelectable selectable); }
    public interface IDeactivatedHandler { void OnDeactivated(ConfiguratorSelectable selectable); }

    // Control
    public interface IControlOverHandler { void OnControlOver(); }
    public interface IControlOutHandler { void OnControlOut(); }
    public interface IControlPressedHandler { void OnControlPressed(); }
    public interface IControlClickedHandler { void OnControlClicked(); }
    public interface IControlClickedSpaceHandler { void OnControlClickedSpace(); }

    // Render
    public interface IBeginRenderHandler { void OnBeginRender(); }
    public interface IUpdateRenderHandler { void OnUpdateRender(); }
    public interface IEndRenderHandler { void OnEndRender(); }

    // Notifications
    public interface IHierarchyChangedHandler { void OnHierarchyChanged(); }

    // UI
    public interface IAddUIControlHandler { void OnAddUIControl(ConfiguratorGUIControl control); }
    public interface IRemoveUIControlHandler { void OnRemoveUIControl(ConfiguratorGUIControl control); }

    // Editor
    public interface IEditorDestroyHandler { void OnEditorDestroy(); }
    public interface IEditorCreateHandler
    {
        void OnEditorCreate();
        bool created { get; }
    }
    public interface IEditorHierarchyChangedHandler { void OnEditorHierarchyChanged(); }
    public interface IEditorSelectionHandler { void OnEditorSelection(); }

    public static class Components
    {
        public static void Self<T>(ConfiguratorObject cobject, Action<T> action = null, Phase phase = Phase.Global) =>
            Invoke (phase, action, () => cobject is T ? new[] { cobject }.Cast<T> () : new T[0]);

        public static void Object<T>(ConfiguratorObject cobject, Action<T> action = null, Phase phase = Phase.Global) =>
            Invoke (phase, action, () => cobject.GetComponents<T> ());

        public static void Children<T>(ConfiguratorObject cobject, Action<T> action = null, Phase phase = Phase.Global) =>
            Invoke (phase, action, () => cobject.GetComponentsInChildren<ConfiguratorSelectable> ().
                Where (s => s.transform != cobject.transform).SelectMany (s => s.GetComponents<T> ()));

        public static void Parent<T>(ConfiguratorObject cobject, Action<T> action = null, Phase phase = Phase.Global) =>
            Invoke (phase, action, () => cobject.GetComponentsInParent<ConfiguratorSelectable> ().
                Where (s => s.transform != cobject.transform).SelectMany (s => s.GetComponents<T> ()));

        public static void Selectables<T>(Action<T> action = null, Phase phase = Phase.Global) =>
            Invoke (phase, action, () => GameObject.FindObjectsOfType<ConfiguratorSelectable> ().SelectMany (s => s.GetComponents<T> ()));

        public static void Scene<T>(Action<T> action = null, Phase phase = Phase.Global) =>
           Invoke (phase, action, () => GameObject.FindObjectsOfType<GameObject> ().SelectMany (g => g.GetComponents<T> ()));

        public static class Runtime
        {
            static Phase P = Phase.Runtime;
            public static void Self<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Self<T> (cobject, action, P);
            public static void Object<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Object<T> (cobject, action, P);
            public static void Children<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Children<T> (cobject, action, P);
            public static void Parent<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Parent<T> (cobject, action, P);
            public static void Selectables<T>(Action<T> action = null) => Components.Selectables<T> (action, P);
            public static void Scene<T>(Action<T> action = null) => Components.Scene<T> (action, P);
        }

        public static class Editor
        {
            static Phase P = Phase.Editor;
            public static void Self<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Self<T> (cobject, action, P);
            public static void Object<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Object<T> (cobject, action, P);
            public static void Children<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Children<T> (cobject, action, P);
            public static void Parent<T>(ConfiguratorObject cobject, Action<T> action = null) => Components.Parent<T> (cobject, action, P);
            public static void Selectables<T>(Action<T> action = null) => Components.Selectables<T> (action, P);
            public static void Scene<T>(Action<T> action = null) => Components.Scene<T> (action, P);
        }

        static void Invoke<T>(Phase phase, Action<T> action, Func<IEnumerable<T>> getObjects)
        {
            if (!IsPhase (phase)) return;

            IEnumerable<T> objects = getObjects ();

            // if (action == null)
            // {
            //     var method = GetMethod (typeof (T));
            //     objects.ForAll (c => method.Invoke (c, null));
            // }
            // else
            // {
            // int index = 0;
            // Type typeT = typeof (T);
            // if (typeof (T) == typeof (ISelectedHandler))
            // {
            //     Debug.Log ("target");
            // }
            try
            {
                // Pretty.Track ("Invoke " + typeof (T));
                // foreach (var o in objects)
                // {
                //     action (o);
                //     index++;
                // }
                objects.ForAll (action);
            }
            catch (Exception x)
            {
                Debug.LogError (x.Message);
                Debug.LogError ("   Objects: " + String.Join (",", getObjects ().Select (o => o)));
                Debug.LogError ("   Object types: " + String.Join (",", getObjects ().Select (o => o != null ? o.GetType ().Name : "null")));
                Debug.LogError ("   Type of T " + typeof (T));
                Debug.LogError ("   Type of First " + getObjects ().First ().GetType ());
            }
            // }
        }

        static Dictionary<Type, MethodInfo> Methods = new Dictionary<Type, MethodInfo> ();
        static MethodInfo GetMethod(Type type) =>
            Methods.ContainsKey (type) ? Methods[type] :
                Methods[type] = type.GetMethods (BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).FirstOrDefault ();

        static bool IsPhase(Phase phase) =>
            (phase == Phase.Runtime && Application.isPlaying) || (phase == Phase.Editor && !Application.isPlaying) || (phase == Phase.Global);

        public enum Phase
        {
            Runtime = 0,
            Editor = 1,
            Global = 2,
        }
    }
}