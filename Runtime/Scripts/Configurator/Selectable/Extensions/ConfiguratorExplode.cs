// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 05/09/2021 11:44:41 by seantcooper
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [ExtensionExclude]
    [AddComponentMenu ("")]
    public abstract class ConfiguratorExplode : ConfiguratorExtension, IObjectSelectedHandler, IObjectDeselectedHandler
    {
        public Trigger type = Trigger.Manual;

        void IObjectSelectedHandler.OnObjectSelected() { if (type == Trigger.OnSelection) explode (); }
        void IObjectDeselectedHandler.OnObjectDeselected() { if (type == Trigger.OnSelection) implode (); }

        protected bool isRenderable => Application.isPlaying || gameObject.isSelectedInEditor ();

        internal abstract void explode();
        internal abstract void implode();

        public enum Trigger
        {
            Manual = 0,
            OnSelection = 1,
        }
    }
}