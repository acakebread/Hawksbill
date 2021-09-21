// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 25/08/2021 20:07:14 by seantcooper
using System.Collections;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [AddComponentMenu ("")]
    // [RequireComponent (typeof (ConfiguratorSelectable))]
    public sealed class ConfiguratorTooltip : ConfiguratorExtension
    {
        [TextArea (2, 5)] public string tooltip;
        [Range (0, 2)] public float activationDuration = 0.25f;

        Coroutine activation;
        void OnMouseEnter()
        {
            activation = StartCoroutine (waitForActivation ());
        }

        void OnMouseExit()
        {
            if (activation != null) StopCoroutine (activation);
        }

        IEnumerator waitForActivation()
        {
            yield return new WaitForSeconds (activationDuration);
            activate ();
        }

        void activate() => Components.Runtime.Scene<ITooltipHandler> (c => c.OnActivate (this));
        void deactivate() => Components.Runtime.Scene<ITooltipHandler> (c => c.OnDeactivate (this));
    }

    public interface ITooltipHandler
    {
        void OnActivate(ConfiguratorTooltip tooltip);
        void OnDeactivate(ConfiguratorTooltip tooltip);
    }
}