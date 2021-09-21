// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/08/2021 18:01:44 by seantcooper
using System;
using Cinemachine;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Controls and manages the Virtual Camera</summary>
    [ExecuteInEditMode]
    [AddComponentMenu ("")]
    //[RequireComponent (typeof (ConfiguratorSelectable))]
    public sealed class ConfiguratorCameraStatic : ConfiguratorCamera
    {
        protected override void setView(ConfiguratorCameraView.View view)
        {
            throw new NotImplementedException ();
        }
    }
}