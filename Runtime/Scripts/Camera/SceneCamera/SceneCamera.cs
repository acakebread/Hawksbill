// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:01 by seancooper

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using System;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    //[ExecuteInEditMode]
    public class SceneCamera : MonoBehaviour
    {
        const float MinDistance = 3;
        public new Camera camera => GetComponent<Camera> ();
        public float distance = 32;
        public bool updateSceneCamera = true;

        public ModeZoom modeZoom = new ModeZoom { scalar = 1 };
        public ModeLook modeLook = new ModeLook { scalar = 4 };
        public ModePivot modePivot = new ModePivot { scalar = 4 };
        public ModePan modePan = new ModePan { scalar = 0.036f };
        public Mode[] modes => new Mode[] { modeZoom, modeLook, modePivot, modePan };

        void Start()
        {
            startEditor ();
            modes.ForAll (m => m.target = this);
        }

        void Update()
        {
            updateMode ();
            updateKeys ();

            //Time.timeScale = Input.GetKey (KeyCode.Period) ? (10 * (Input.GetKey (KeyCode.RightShift) ? 5 : 1)) : 1;
        }

        void updateMode()
        {
            var mode = modes.FirstOrDefault (m => m.active);
            if (!mode) mode = modes.FirstOrDefault (m => m.start ());
            if (mode) mode.update ();
            if (!(mode && mode.active && mode.isolate)) modeZoom.update ();
        }

        void updateKeys()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown (KeyCode.F)) zoom (Selection.activeGameObject);
#endif
        }

        void OnSelectHierarchy(GameObject gameObject) => zoom (gameObject);

        void zoom(GameObject gameObject)
        {
            if (Application.isPlaying && gameObject != null && gameObject.activeInHierarchy)
            {
                var bounds = getWorldBounds (gameObject);
                distance = getCameraDistance (bounds);
                transform.position = bounds.center - transform.forward * distance;
            }
        }

        [Serializable]
        public class ModeLook : Mode
        {
            public override bool isolate => true;
            public override bool start() { if (mouseDown (1)) { activate (); return true; } return false; }
            public override void update()
            {
                if (stop ()) deactivate ();
                else transform.eulerAngles = transform.eulerAngles + new V3 (-mouseDelta.y, mouseDelta.x);
            }
            public override bool stop() { if (mouseUp (1)) { deactivate (); return true; } return false; }
        }

        [Serializable]
        public class ModePivot : Mode
        {
            public override bool isolate => true;
            public override bool start() { if (mouseDown (0, Modifiers.Alt)) { activate (); return true; } return false; }
            public override void update()
            {
                if (stop ()) deactivate ();
                else
                {
                    Vector3 pivot = transform.position + transform.forward * distance;
                    transform.RotateAround (pivot, transform.right, -mouseDelta.y);
                    transform.RotateAround (pivot, Vector3.up, mouseDelta.x);
                }
            }
            public override bool stop() { if (mouseUp (0)) { deactivate (); return true; } return false; }
        }

        [Serializable]
        public class ModePan : Mode
        {
            public override bool isolate => true;
            public override bool start()
            {
                if (mouseDown (0, Modifiers.Ctrl | Modifiers.Alt) || mouseDown (0)) { activate (); return true; }
                return false;
            }
            public override void update()
            {
                if (stop ()) deactivate ();
                else transform.position -= (transform.right * mouseDelta.x + transform.up * mouseDelta.y) * Mathf.Abs (distance);
            }
            public override bool stop() { if (mouseUp (0)) { deactivate (); return true; } return false; }
        }

        [Serializable]
        public class ModeZoom : Mode
        {
            public override void update()
            {
                distance -= mouseScrollDelta.y;
                transform.position += transform.forward * mouseScrollDelta.y;
            }
        }

        [Serializable]
        public class Mode
        {
            public float scalar = 5;
            const float defaultWidth = 1920;
            protected float unitScalar => defaultWidth * scalar;
            internal SceneCamera target;
            internal Transform transform => target.transform;
            internal float distance { get => target.distance; set => target.distance = value; }
            public virtual bool isolate => false;
            public bool active;
            public bool isActive => active;
            protected void activate() { active = true; }
            protected void deactivate() { active = false; }
            public virtual bool start() { return false; }
            public virtual void update() { }
            public virtual bool stop() { return false; }
            public static implicit operator bool(Mode empty) => empty != null;
            // control
            protected V3 mouseScrollDelta => Input.mouseScrollDelta * scalar;
            protected V3 mouseDelta => new V3 (Input.GetAxis ("Mouse X") / Screen.width, Input.GetAxis ("Mouse Y") / Screen.height) * unitScalar;
            protected bool mouseDown(int index, Modifiers mods = Modifiers.None) => Input.GetMouseButtonDown (index) && modifiers == mods;
            protected bool mouseUp(int index, Modifiers mods = Modifiers.None) => Input.GetMouseButtonUp (index);
            protected Modifiers modifiers
            {
                get
                {
                    var m = Modifiers.None;
                    if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) m |= Modifiers.Shft;
                    if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt)) m |= Modifiers.Alt;
                    if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) m |= Modifiers.Ctrl;
                    if (Input.GetKey (KeyCode.LeftCommand) || Input.GetKey (KeyCode.RightCommand)) m |= Modifiers.Ctrl;
                    return m;
                }
            }
            protected enum Modifiers { None = 0, Ctrl = 1 << 0, Alt = 1 << 1, Shft = 1 << 2, }
        }

        void copyCamera(Camera source, Camera dest)
        {
            // dest.orthographicSize = source.orthographicSize;
            // dest.orthographic = source.orthographic;
            dest.fieldOfView = source.fieldOfView;
            dest.transform.position = source.transform.position;
            dest.transform.rotation = source.transform.rotation;
        }

#if UNITY_EDITOR
        public void copyGameToSceneCamera()
        {
            copyCamera (GetComponent<Camera> (), SceneView.lastActiveSceneView.camera);
        }
        public void copySceneToGameCamera()
        {
            SceneView[] scenes = SceneView.sceneViews.OfType<SceneView> ().ToArray ();
            SceneView scene = SceneView.lastActiveSceneView;
            copyCamera (scene.camera, GetComponent<Camera> ());
            distance = scene.cameraDistance;
            EditorUtility.SetDirty (scene);
        }

        void startEditor()
        {
            void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
            {
                if (Event.current.clickCount > 1 && selectionRect.Contains (Event.current.mousePosition))
                {
                    print ("instanceID: " + instanceID + " " + EditorUtility.InstanceIDToObject (instanceID));
                    this.OnSelectHierarchy (EditorUtility.InstanceIDToObject (instanceID) as GameObject);
                }
            }
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }
#else
        public void copyGameToSceneCamera() { }
        public void copySceneToGameCamera() { }
        void startEditor() { }
#endif

        Bounds getWorldBounds(GameObject gameObject) =>
            gameObject.GetComponentsInChildren<Renderer> ().Select (r => r.bounds).
                Aggregate (new Bounds (gameObject.transform.position, Vector3.zero), (t, b) => { t.Encapsulate (b); return t; });

        float getCameraDistance(Bounds bounds)
        {
            float cameraView = 2.0f * Mathf.Tan (0.5f * Mathf.Deg2Rad * camera.fieldOfView);
            return Mathf.Max (MinDistance, 2 * bounds.extents.magnitude / cameraView + 0.5f * bounds.extents.magnitude);
        }

    }
}