// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/08/2021 17:20:48 by seantcooper
using System.Linq;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [AddComponentMenu ("")]
    // [RequireComponent (typeof (ConfiguratorSelectable))]
    public sealed class ConfiguratorColliders : ConfiguratorExtension, IColliderContainer
    {
        public Usage usage;
        [Show (nameof (containerEnabled), ShowAttribute.Action.Disable)]
        public Collider[] colliders = new Collider[0];

        Collider[] IColliderContainer.getColliders() => updateUsageColliders ();
        bool containerEnabled => usage == Usage.Assignment;

        protected override void OnValidate()
        {
            base.OnValidate ();
            updateUsageColliders ();
        }

        // void Start() => colliders.ForAll (c => c.AddComponent<BubbleMouse> ().target = gameObject);

        Collider[] updateUsageColliders()
        {
            switch (usage)
            {
                case Usage.Assignment:
                    break;
                case Usage.UseChildren:
                    colliders = getChildrenComponents<Collider> ().ToArray ();
                    break;
                case Usage.UseAllChildren:
                    colliders = GetComponentsInChildren<Collider> ();
                    break;
            }
            return colliders;
        }

        public enum Usage
        {
            Assignment = 0,
            UseChildren = 1,
            UseAllChildren = 2,
        }

    }
}