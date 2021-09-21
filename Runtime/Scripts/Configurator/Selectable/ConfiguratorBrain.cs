// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/09/2021 11:29:22 by seantcooper
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static Hawksbill.Configurator.ConfiguratorSelectable;
using static Hawksbill.Configurator.ConfiguratorObject;
using System.Reflection;
using UnityEngine.Rendering;

namespace Hawksbill.Configurator
{
    ///<summary>The brains behind the Configurator!</summary>
    [ExecuteInEditMode]
    public class ConfiguratorBrain : SingletonMonoBehaviour<ConfiguratorBrain>
    {
        [SerializeField, HideInInspector] CameraEvents cameraEvents = new CameraEvents ();
        [SerializeField, HideInInspector] NotifyEvents notifyEvents = new NotifyEvents ();
        [SerializeField, HideInInspector] EditorEvents editorEvents = new EditorEvents ();
        [SerializeField] MouseInput mouseInput = new MouseInput ();

        public bool autoSelect = true;

        protected override void OnDestroy()
        {
            base.OnDestroy ();
            notifyEvents?.Dispose ();
            cameraEvents?.Dispose ();
            editorEvents?.Dispose ();
        }

        void OnValidate()
        {
            init ();
            editorEvents.raise ();
        }

        void init()
        {
            notifyEvents = new NotifyEvents ();
            cameraEvents = new CameraEvents ();
            editorEvents = new EditorEvents ();
            mouseInput = new MouseInput ();
        }

        void Start()
        {
            init ();
            if (autoSelect) GetPrimarySelectable ()?.select ();
        }

        void Update()
        {
            if (Application.isPlaying) updateRuntime ();
            else updateEditor ();
        }

        void updateRuntime()
        {
            mouseInput.update ();
        }

        void updateEditor()
        {
        }

        public ConfiguratorSelectable getPrimarySelectable() => GetPrimarySelectable ();
        public static ConfiguratorSelectable GetPrimarySelectable()
        {
            ConfiguratorSelectable currentlySelected = ConfiguratorObject.SelectedSelectables.FirstOrDefault ();

            ConfiguratorSelectable primarySelectable = null;
            if (currentlySelected)
            {
                primarySelectable = currentlySelected.rootSelectable.getSelectableChildren (true).FirstOrDefault (s => (s.selection & Selection.PrimarySelected) > 0);
                if (!primarySelectable) primarySelectable = currentlySelected.rootSelectable;
            }
            else
            {
                primarySelectable = ConfiguratorObject.Selectables.FirstOrDefault (s => (s.selection & Selection.PrimarySelected) > 0);
                if (!primarySelectable) primarySelectable = RootSelectables.FirstOrDefault ();
            }
            return primarySelectable;
        }

        [Serializable]
        class NotifyEvents : IDisposable
        {
            public NotifyEvents() { }
            public void update() { }
            public void Dispose() { }
        }

        [Serializable]
        class CameraEvents : IDisposable
        {
            internal List<Camera> cameras = new List<Camera> ();
            void beginCameraRendering(ScriptableRenderContext context, Camera camera)
            {
                if (cameras.Count == 0)
                {
                    Components.Selectables<IBeginRenderHandler> ((o) => o.OnBeginRender ());
                    Components.Selectables<IUpdateRenderHandler> ((o) => o.OnUpdateRender ());
                }
                cameras.Add (camera);
            }
            void endCameraRendering(ScriptableRenderContext context, Camera camera)
            {
                cameras.Remove (camera);
                if (cameras.Count == 0) Components.Selectables<IEndRenderHandler> ((o) => o.OnEndRender ());
            }

            public CameraEvents()
            {
                RenderPipelineManager.beginCameraRendering += beginCameraRendering;
                RenderPipelineManager.endCameraRendering += endCameraRendering;
            }

            public void Dispose()
            {
                RenderPipelineManager.beginCameraRendering -= beginCameraRendering;
                RenderPipelineManager.endCameraRendering -= endCameraRendering;
            }
        }

        [Serializable]
        class EditorEvents : IDisposable
        {
            public void raise()
            {
                hierarchyChanged ();
                selectionChanged ();
            }

            void hierarchyChanged() => Components.Editor.Selectables<IEditorHierarchyChangedHandler> (c => c.OnEditorHierarchyChanged ());
            void selectionChanged() => Components.Editor.Selectables<IEditorSelectionHandler> (c => c.OnEditorSelection ());

            public EditorEvents()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.hierarchyChanged += hierarchyChanged;
                UnityEditor.Selection.selectionChanged += selectionChanged;
#endif
            }

            public void Dispose()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.hierarchyChanged -= hierarchyChanged;
                UnityEditor.Selection.selectionChanged -= selectionChanged;
#endif
            }
        }

        [Serializable]
        public class MouseInput
        {
            [Range (0, 1)] public float rayThickness = 0;
            [HideInInspector] public ConfiguratorSelectable over;

            public void update()
            {
                setOver (getSelectable ());
                if (Mouse.InputUsable)
                {
                    if (over)
                    {
                        if (Input.GetMouseButtonDown (0)) Components.Object<IControlPressedHandler> (over, (c) => c.OnControlPressed ());
                        if (Input.GetMouseButtonUp (0)) Components.Object<IControlClickedHandler> (over, (c) => c.OnControlClicked ());
                    }
                    else if (Input.GetMouseButtonUp (0)) GetPrimarySelectable ().select ();
                }
            }

            ConfiguratorSelectable getSelectable()
            {
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                foreach (RaycastHit hit in raycast (ray))
                {
                    var selectable = hit.collider.GetComponent<ConfiguratorSelectable> ();
                    if (!selectable)
                        selectable = FindObjectsOfType<ConfiguratorSelectable> ().
                        Where (s => containsCollider (s, hit.collider)).
                        OrderBy (s => s.transform.getHierarchyOrder ()).FirstOrDefault ();

                    if (selectable) return selectable;
                }
                return null;
            }

            IEnumerable<RaycastHit> raycast(Ray ray) =>
                (rayThickness == 0 ? Physics.RaycastAll (ray) : Physics.SphereCastAll (ray, rayThickness)).OrderBy (h => h.distance);

            bool containsCollider(ConfiguratorSelectable selectable, Collider collider) =>
                selectable.GetComponents<IColliderContainer> ().SelectMany (c => c.getColliders ()).Contains (collider);

            void setOver(ConfiguratorSelectable selectable)
            {
                if (selectable != over)
                {
                    if (over)
                        Components.Object<IControlOutHandler> (over, (c) => c.OnControlOut ());
                    if ((over = selectable))
                        Components.Object<IControlOverHandler> (over, (c) => c.OnControlOver ());
                }
            }
        }
    }

    public class Mouse
    {
        public static int LastSelectionFrame;
        public static bool IsMousePressed(Button button) => Input.GetMouseButton ((int) button);
        public static bool IsMouseDown(Button button) => Input.GetMouseButtonDown ((int) button);
        public static bool IsMouseUp(Button button) => Input.GetMouseButtonUp ((int) button);
        public static bool IsMouseSelection => LastSelectionFrame != Time.frameCount && Input.GetMouseButtonUp (0);
        public static float WheelDeltaY => Input.mouseScrollDelta.y;

        public enum Button
        {
            None = -1,
            Left = 0,
            Right = 1,
            Middle = 2,
            Forward = 3,
            Back = 4,
        }

        public static bool InputUsable => !(EventSystem.current?.IsPointerOverGameObject () ?? false);
    }

}