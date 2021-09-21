// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 01/09/2021 19:48:08 by seantcooper
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [AddComponentMenu ("")]
    public sealed class ConfiguratorExpander : ConfiguratorExtension, IObjectSelectedHandler
    {
        [ReadOnly (ReadOnlyAttribute.State.Runtime)] public GameObject expandObject;

        // TODO
        // Basic Motion for the Expander, Splines, Line, Animation, Parent to Position (Cubes coming out of cubes)
        // 

        void Awake()
        {
            if (!expandObject) return;
            if (!expandObject.inHierarchy ()) expandObject = Instantiate (expandObject, transform);
            expandObject.SetActive (false);
        }

        void expand()
        {
            if (!expandObject) return;
            expandObject.SetActive (true);

            if (!expandObject.activeInHierarchy)
            {
                Debug.LogWarning ("Object is not active in Hierarchy!");
                return;
            }

            IEnumerator waitForCollapse()
            {
                yield return new WaitWhile (() => hasChildSelected (true));
                // while (hasChildSelected (true)) yield return null;
                yield return new WaitForEndOfFrame ();
                collapse ();
            }
            StartCoroutine (waitForCollapse ());
        }

        void collapse() => expandObject.SetActive (false);

        void IObjectSelectedHandler.OnObjectSelected() => expand ();

    }
}