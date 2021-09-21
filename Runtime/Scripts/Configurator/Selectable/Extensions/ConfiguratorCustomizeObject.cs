// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/08/2021 14:17:27 by seantcooper
using System.Linq;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Allows customization of a Configurator Selectable</summary>
    [AddComponentMenu ("")]
    // [RequireComponent (typeof (ConfiguratorSelectable))]
    public sealed class ConfiguratorCustomizeObject : ConfiguratorCustomize
    {
        [Line]
        public Transform container;
        public GameObject[] objects;

        public override int count => objects == null ? 0 : objects.Length;

        public override void select(int index)
        {
            currentIndex = Mathf.Clamp (index, 0, count - 1);
            if (!container || count == 0) return;
            // container.destroyChildren ();
            // Instantiate (objects[currentIndex], container);
        }
    }
}