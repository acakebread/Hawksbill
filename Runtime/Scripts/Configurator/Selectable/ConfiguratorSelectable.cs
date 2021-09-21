// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/08/2021 16:24:44 by seantcooper
using System;
using UnityEngine;
using System.Linq;

namespace Hawksbill.Configurator
{
    [AddComponentMenu ("Configurator/Configurator Selectable")]
    [ExecuteInEditMode]
    public class ConfiguratorSelectable : ConfiguratorObject, IColliderContainer, IEditorCreateHandler,
        IEditorDestroyHandler, IControlClickedHandler
    {
        public Type type = Type.Button;
        public Selection selection = Selection.IsSelectable;

        public override bool isSelected => selected;
        bool isSelectable => ((selection & Selection.IsSelectable) == 0 || type == Type.None || !active);

        bool selected, active;

        public Collider[] getColliders() => GetComponents<Collider> ();

        void IControlClickedHandler.OnControlClicked() => select ();

        internal void activate()
        {
            if (active) return;
            active = true;
            Components.Runtime.Object<IObjectActivatedHandler> (this, c => c.OnObjectActivated ());
            Components.Runtime.Scene<IActivatedHandler> (c => c.OnActivated (this));
        }

        internal void deactivate()
        {
            if (!active) return;
            active = false;
            Components.Runtime.Object<IObjectDeactivatedHandler> (this, c => c.OnObjectDeactivated ());
            Components.Runtime.Scene<IDeactivatedHandler> (c => c.OnDeactivated (this));
        }

        internal void select()
        {
            if (!isSelectable) return;
            if (selected)
            {
                if (type == Type.Toggle) deselect ();
                return;
            }
            selected = true; // always keep one selected
            FindObjectsOfType<ConfiguratorSelectable> ().Where (s => s.isSelected && s != this).ForAll (s => s.deselect ());
            Components.Runtime.Object<IObjectSelectedHandler> (this, c => c.OnObjectSelected ());
            Components.Runtime.Parent<IChildSelectedHandler> (this, c => c.OnChildSelected (this));
            Components.Runtime.Children<IParentSelectedHandler> (this, c => c.OnParentSelected (this));
            Components.Runtime.Scene<ISelectedHandler> (c => c.OnSelected (this));
        }

        internal void deselect()
        {
            if (!isSelectable) return;
            if (!selected) return;
            selected = false;
            Components.Runtime.Object<IObjectDeselectedHandler> (this, c => c.OnObjectDeselected ());
            Components.Runtime.Parent<IChildDeselectedHandler> (this, c => c.OnChildDeselected (this));
            Components.Runtime.Children<IParentDeselectedHandler> (this, c => c.OnParentDeselected (this));
            Components.Runtime.Scene<IDeselectedHandler> (c => c.OnDeselected (this));
        }

        [Flags]
        public enum Selection
        {
            IsSelectable = 1 << 0,
            PrimarySelected = 1 << 1,
            // SelectOnChildSelected = 1 << 2,
            // Exclusive
        }

        public enum Type
        {
            None = -1,
            Button = 0,
            Toggle = 1,
        }

        public bool created => gameObject.GetComponent<HierarchyTag> ();
        void IEditorCreateHandler.OnEditorCreate()
        {
            if (!ConfiguratorBrain.GetInstance ()) Camera.main?.AddComponent<ConfiguratorBrain> ();
            gameObject.GetComponent<HierarchyTag> (out HierarchyTag c, true);
        }

        void IEditorDestroyHandler.OnEditorDestroy()
        {
#if UNITY_EDITOR
            var extensions = GetComponents<ConfiguratorExtension> ();
            var tag = GetComponent<HierarchyTag> ();
            UnityEditor.EditorApplication.delayCall += () => {
                extensions.ForAll (c => DestroyImmediate (c));
                DestroyImmediate (tag);
            };
#endif
        }
    }
}