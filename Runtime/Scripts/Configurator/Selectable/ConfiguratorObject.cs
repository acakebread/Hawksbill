// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 01/09/2021 17:18:02 by seantcooper
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Hawksbill.Configurator
{
    ///<summary>Base class should not be used!</summary>
    [AddComponentMenu ("")]
    public class ConfiguratorObject : MonoBehaviour
    {
        public static IEnumerable<ConfiguratorSelectable> Empty => Enumerable.Empty<ConfiguratorSelectable> ();

        ///<summary>Get all root ConfiguratorSelectable!</summary>
        public static IEnumerable<ConfiguratorSelectable> Selectables => FindObjectsOfType<ConfiguratorSelectable> ().OrderBy (s => s.transform.getHierarchyOrder ());

        ///<summary>Get all root ConfiguratorSelectable!</summary>
        public static IEnumerable<ConfiguratorSelectable> RootSelectables => Selectables.Where (s => s.isRootSelectable);

        ///<summary>Get all ConfiguratorSelectables that are Selected!</summary>
        public static IEnumerable<ConfiguratorSelectable> SelectedSelectables => Selectables.Where (s => s.isSelected);

        public ConfiguratorSelectable selectable => GetComponent<ConfiguratorSelectable> ();
        public ConfiguratorSelectable parentSelectable => transform.getParentComponent<ConfiguratorSelectable> ();
        public IEnumerable<ConfiguratorSelectable> immediateChildSelectables => getSelectableChildren ().Where (s => s.parentSelectable == selectable);

        public virtual bool isSelected => selectable?.isSelected ?? false;

        ///<summary>The root ConfiguratorSelectable!</summary>
        public ConfiguratorSelectable rootSelectable => getSelectableParents (true).LastOrDefault ();

        ///<summary>Get all ConfiguratorSelectable children!</summary>
        public IEnumerable<ConfiguratorSelectable> getSelectableChildren(bool includeSelf = false) =>
            GetComponentsInChildren<ConfiguratorSelectable> ().Where (s => includeSelf || s != selectable).OrderBy (s => s.transform.getHierarchyOrder ());

        ///<summary>Get all ConfiguratorSelectable parents!</summary>
        public IEnumerable<ConfiguratorSelectable> getSelectableParents(bool includeSelf = false) =>
            GetComponentsInParent<ConfiguratorSelectable> ().Where (s => includeSelf || s != selectable).OrderBy (s => s.transform.getHierarchyOrder ()).Reverse ();

        ///<summary>Is the root ConfiguratorSelectable selected?</summary>
        public bool isRootSelectable => selectable.GetComponentsInParent<ConfiguratorSelectable> ().Length <= 1;

        ///<summary>Has a child in the hierarchy selected?</summary>
        public bool hasChildSelected(bool includeSelf = false) => getSelectableChildren (includeSelf).Any (s => s.isSelected);

        ///<summary>Has a parent in the hierarchy selected?</summary>
        public bool hasParentSelected(bool includeSelf = false) => getSelectableParents (includeSelf).Any (s => s.isSelected);

        ///<summary>Gets all children's components excluding children's Selectable and their children!</summary>
        protected IEnumerable<T> getChildrenComponents<T>() where T : Component
        {
            IEnumerable<T> recursive(Transform target)
            {
                var component = target.GetComponent<T> ();
                if (component) yield return component;
                foreach (var child in target.children ().Where (t => !t.GetComponent<ConfiguratorSelectable> ()))
                    foreach (var c in recursive (child)) yield return c;
            }
            return recursive (transform);
        }

        // rename 
        string getChildName(string name) => String.Format (name + " [{0}]", this.name);
        protected string setChildName(UnityEngine.Object obj, string name) =>
            obj && obj.name != getChildName (name) ? obj.name = getChildName (name) : obj.name;

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnValidate() { }

        protected void Reset() => Components.Editor.Self<IEditorCreateHandler> (this, e => e.OnEditorCreate ());
        protected void OnDestroy() => Components.Editor.Self<IEditorDestroyHandler> (this, e => e.OnEditorDestroy ());
    }
}