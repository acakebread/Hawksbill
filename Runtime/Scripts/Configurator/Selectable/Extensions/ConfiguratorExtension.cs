// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 23/08/2021 08:53:48 by seantcooper
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Base extension object</summary>
    [AddComponentMenu ("")]
    [ExtensionExclude]
    // [RequireComponent (typeof (ConfiguratorSelectable))]

    // TODO: 
    // Removing Selectable should remove all Extensions
    public class ConfiguratorExtension : ConfiguratorObject
    {
        // new public Transform transform;
    }

}