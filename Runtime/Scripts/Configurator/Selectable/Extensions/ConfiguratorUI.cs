// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 10/09/2021 16:37:02 by seantcooper
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    // [DisallowMultipleComponent]
    // [ExecuteInEditMode]
    [ExtensionExclude]
    [AddComponentMenu ("")]
    public abstract class ConfiguratorUI : ConfiguratorExtension, ISelectedHandler, IDeselectedHandler //IParentSelectedHandler, IParentDeselectedHandler, 
    {
        public FieldObject panelID;
        public int order = 10;
        public ConfiguratorGUIControl control;
        public Populate populate = Populate.Always;

        // [Line] 
        protected abstract bool hasInstances { get; }

        protected ConfiguratorGUIPanel panel => panelID?.getValue<ConfiguratorGUIPanel> () ?? null;

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
            if (isGUIVisible ()) { if (!hasInstances) addControls (); }
            else if (hasInstances) removeControls ();
        }

        protected abstract void addControls();
        protected abstract void removeControls();

        bool isGUIVisible()
        {
            switch (populate)
            {
                case Populate.Global: return true;
                case Populate.Always: return rootSelectable?.hasChildSelected (true) ?? false;
                case Populate.ParentSelected: return parentSelectable && parentSelectable.hasChildSelected (true);
                case Populate.AnyParentSelected: return rootSelectable.hasChildSelected (true);
                default: return false;
            }
        }

        // DEFINE
        public enum Populate
        {
            // [InspectorName ("Paren")]
            Always = 0,
            ParentSelected = 1,
            AnyParentSelected = 2,
            Global = 3,
        }
    }
}