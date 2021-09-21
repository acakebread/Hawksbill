// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 12/09/2021 21:07:48 by seantcooper
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    // [DisallowMultipleComponent]
    // [ExecuteInEditMode]
    // [ExtensionExclude]
    [AddComponentMenu ("")]
    public class ConfiguratorIcon : ConfiguratorExtension, ISelectedHandler, IDeselectedHandler
    {
        public FieldObject panelID;
        public ConfiguratorGUIControl control;

        protected ConfiguratorGUIPanel panel => panelID?.getValue<ConfiguratorGUIPanel> () ?? null;

        // The whole adding process
        [SerializeField, ReadOnly] protected ConfiguratorGUIControl controlInstance;
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
            if (isGUIVisible ()) { if (!controlInstance) addControl (); }
            else if (controlInstance) removeControl ();
        }

        protected void addControl()
        {
            if ((controlInstance = panel?.addControl (control, this)))
            {
                controlInstance.set (this);
            }
        }

        protected void removeControl() => panel?.removeControl (controlInstance);

        bool isGUIVisible() => true; //rootSelectable?.hasChildSelected (true) ?? false;
    }
}