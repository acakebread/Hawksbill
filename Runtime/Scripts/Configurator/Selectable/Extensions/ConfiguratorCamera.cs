// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 03/09/2021 10:08:35 by seantcooper
using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [ExtensionExclude]
    [AddComponentMenu ("")]
    public abstract class ConfiguratorCamera : ConfiguratorExtension,
        IEditorHierarchyChangedHandler, IEditorCreateHandler, IEditorDestroyHandler,
        IEditorSelectionHandler, IObjectSelectedHandler, IObjectDeselectedHandler,
        ISelectedHandler, IDeselectedHandler
    {
        [ReadOnly] new public CinemachineVirtualCamera camera;
        [Line]
        public ConfiguratorCameraView views;

        // TODO
        // Display only the selected gizmos (Selection.activeGameObject == gameObject)
        // (Don't display children)
        // Better visualization of the look camera
        // camera offset == selectable offset? - i.e. offset eh objects
        // SceneEditor for Look Camera (and others)

        void Start()
        {
            if (Application.isPlaying && !isSelected)
                deactivateCamera ();
            populateControl ();

            if (Application.isPlaying)
            {
                if (views && views.initialView) setView (views.initialView);
            }
        }

        void activateCamera() => camera.gameObject.SetActive (true);
        void deactivateCamera() => camera.gameObject.SetActive (false);
        void destroyCamera() => DestroyImmediate (camera.gameObject);

        // ISelection
        void IObjectSelectedHandler.OnObjectSelected() => activateCamera ();
        void IObjectDeselectedHandler.OnObjectDeselected() => deactivateCamera ();

        ////////////////////////////////////////////////////////////////////////////////////////////
        protected ConfiguratorGUIPanel panel => views?.panelID?.getValue<ConfiguratorGUIPanel> () ?? null;

        // The whole adding process
        protected ConfiguratorGUIControl[] controlInstances;
        void ISelectedHandler.OnSelected(ConfiguratorSelectable selectable) => populateControl ();
        void IDeselectedHandler.OnDeselected(ConfiguratorSelectable selectable) => populateControl ();
        protected override void OnDisable()
        {
            base.OnDisable ();
            populateControl ();
        }

        void populateControl()
        {
            if (isGUIVisible ()) { if (controlInstances == null) addControls (); }
            else if (controlInstances != null) removeControls ();
        }

        protected void addControls()
        {
            var panel = this.panel;
            if (!panel) return;

            IEnumerable<ConfiguratorGUIControl> _addControls()
            {
                foreach (var view in views.views.Where (v => v.control))
                {
                    var instance = panel.addControl (view.control, this);
                    yield return instance;
                }
            }
            controlInstances = _addControls ().ToArray ();
            controlInstances.ForAll (c => c.set (this, control => selectView (control)));
        }

        void selectView(ConfiguratorGUIControl control)
        {
            int index = ((IEnumerable<ConfiguratorGUIControl>) controlInstances).FindIndex (c => c == control);
            if (index == -1) return;
            print ("select view " + index);
            controlInstances.Where (c => c != control).ForAll (c => c.deselectButton ());
            control.selectButton ();
            setView (views[index]);
        }

        protected abstract void setView(ConfiguratorCameraView.View view);

        protected void removeControls()
        {
            var panel = this.panel;
            if (!panel) return;
            controlInstances?.ForAll (panel.removeControl);
            controlInstances = null;
        }

        bool isGUIVisible() => rootSelectable?.hasChildSelected (true) ?? false;
        ////////////////////////////////////////////////////////////////////////////////////////////

        // IEditorCreation
        void IEditorCreateHandler.OnEditorCreate() => createCamera ();
        public bool created => camera;

        void createCamera()
        {
            if (created)
            {
                return;
            }

            if (ProjectSettings.Cinemachine.defaultCamera)
            {
                camera = Instantiate (ProjectSettings.Cinemachine.defaultCamera, transform);
            }
            else
            {
                camera = new GameObject () { transform = { parent = transform } }.AddComponent<CinemachineVirtualCamera> ();
                camera.AddCinemachineComponent<CinemachineComposer> ();
            }

            camera.transform.localPosition = Vector3.forward * 5;
            camera.LookAt = transform;
            camera.Follow = transform;
            camera.Priority = 100;
            camera.name = "vcam";
            camera.m_Transitions.m_BlendHint = CinemachineVirtualCameraBase.BlendHint.CylindricalPosition;

            if (Camera.main && !Camera.main.GetComponent<CinemachineBrain> ())
            {
                var brain = Camera.main.AddComponent<CinemachineBrain> ();
                brain.m_CustomBlends = ProjectSettings.Instance.cinemachine.defaultCustomBlends;
            }
        }
        void IEditorHierarchyChangedHandler.OnEditorHierarchyChanged() => setChildName (camera, "vcam");
        void IEditorDestroyHandler.OnEditorDestroy() => DestroyImmediate (camera.gameObject);

        void IEditorSelectionHandler.OnEditorSelection()
        {
            if (!camera) return;
            if (camera.gameObject.isSelectedInEditor () || gameObject.isSelectedInEditor ()) activateCamera ();
            else deactivateCamera ();
        }

    }
}