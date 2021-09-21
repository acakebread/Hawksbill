// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/09/2021 09:15:14 by seantcooper
using UnityEditor;
using UnityEngine;

namespace Hawksbill.Configurator
{
    [CustomEditor (typeof (ConfiguratorSelectable), true), CanEditMultipleObjects]
    public class ConfiguratorSelectable_Editor : ExtensionsEditor<ConfiguratorExtension> { }
}