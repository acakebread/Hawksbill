// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/08/2021 14:17:27 by seantcooper
using System.Linq;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Allows customization of a Configurator Selectable</summary>
    [AddComponentMenu ("")]
    // [RequireComponent (typeof (ConfiguratorSelectable))]
    public sealed class ConfiguratorCustomizeMaterials : ConfiguratorCustomize
    {
        [Line]
        // [HelpBoxIf (nameof (targetBool), "This requires a material to remap to and needs to be in this object and it's children")]
        public Material target;
        [SerializeField, ReadOnly] Material currentTarget;
        public Material[] materials;

        // editor
        bool targetBool => target;

        public override int count => materials == null ? 0 : materials.Length;

        public override void select(int index)
        {
            // currentIndex = Mathf.Clamp (index, 0, count - 1);
            // if (!this.target || count == 0) return;

            // var renderers = GetComponentsInChildren<MeshRenderer> (true);
            // var target = currentTarget ? currentTarget : this.target;
            // currentTarget = materials[currentIndex];

            // foreach (var renderer in renderers)
            //     renderer.sharedMaterials = renderer.sharedMaterials.Select (m => m == target ? currentTarget : m).ToArray ();
        }
    }
}