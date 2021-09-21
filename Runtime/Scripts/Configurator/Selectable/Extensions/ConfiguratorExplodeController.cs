// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 15/09/2021 08:54:03 by seantcooper
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [AddComponentMenu ("")]
    public class ConfiguratorExplodeController : ConfiguratorExplode, ISelectedHandler, IDeselectedHandler
    {
        public FieldObject panelID;
        public ConfiguratorGUIControl control;
        public bool exploded;
        protected ConfiguratorGUIPanel panel => panelID?.getValue<ConfiguratorGUIPanel> () ?? null;

        // The whole adding process
        [SerializeField, HideInInspector] protected ConfiguratorGUIControl controlInstance;

        void Start() => populateControl ();
        void ISelectedHandler.OnSelected(ConfiguratorSelectable selectable) => populateControl ();
        void IDeselectedHandler.OnDeselected(ConfiguratorSelectable selectable) => populateControl ();

        protected override void OnDisable()
        {
            base.OnDisable ();
            populateControl ();
        }

        void populateControl()
        {
            if (!Application.isPlaying) return;
            if (isGUIVisible ()) { if (!controlInstance) addControl (); }
            else if (controlInstance) removeControl ();
        }

        protected void addControl()
        {
            if ((controlInstance = panel?.addControl (control, this)))
            {
                controlInstance.set (this, c => doExplode ());
            }
        }

        protected void removeControl() => panel?.removeControl (controlInstance);

        bool isGUIVisible() => rootSelectable?.hasChildSelected (true) ?? false;

        IEnumerable<ConfiguratorExplode> exploders => GetComponentsInChildren<ConfiguratorExplode> ().Where (e => e != this);

        void doExplode()
        {
            if (exploded) implode (); else explode ();
        }

        ConfiguratorExplode[] explodedComponents;
        internal override void explode()
        {
            if (exploded) return;
            exploded = true;
            explodedComponents = exploders.ToArray ();
            explodedComponents.ForAll (e => e.explode ());
        }

        internal override void implode()
        {
            if (!exploded) return;
            exploded = false;
            if (explodedComponents != null) explodedComponents.ForAll (e => e.implode ());
            explodedComponents = null;
        }
    }
}