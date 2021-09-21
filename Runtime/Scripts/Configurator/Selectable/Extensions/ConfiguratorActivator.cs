// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 10/09/2021 12:19:32 by seantcooper
using System;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    // [DisallowMultipleComponent]
    // [ExecuteInEditMode]
    // [ExtensionExclude]
    [AddComponentMenu ("")]
    public class ConfiguratorActivator : ConfiguratorExtension
    {
        public bool active;
        public Group group;
        public ConfiguratorActivator activators;
        public Type type = Type.Everything;

        public enum Group
        {
            Assignment,
            Object,
            Children,
            AllChildren,
        }

        [Flags]
        public enum Type
        {
            // [InspectorName ("Naming enums")]
            Colliders = 1 << 0,
            MeshRenderer = 1 << 1,
            Everything = Colliders | MeshRenderer,
        }
    }
}