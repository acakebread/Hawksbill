// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/09/2021 09:16:37 by seantcooper
using UnityEditor;
using UnityEngine;

namespace Hawksbill.Configurator
{
    [CustomEditor (typeof (ConfiguratorExtension), true), CanEditMultipleObjects]
    public class ConfiguratorExtension_Editor : Hawksbill.Editor { }
}