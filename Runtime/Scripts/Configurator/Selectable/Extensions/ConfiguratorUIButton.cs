// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/09/2021 14:56:15 by seantcooper
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    // [DisallowMultipleComponent]
    // [ExecuteInEditMode]
    // [ExtensionExclude]
    [AddComponentMenu ("")]
    public class ConfiguratorUIButton : ConfiguratorUI
    {
        [SerializeField, ReadOnly, HideInInspector] ConfiguratorGUIControl controlInstance;
        protected override bool hasInstances => controlInstance;

        [Line]
        public string overrideText;
        public Sprite overrideImage;

        protected override void addControls()
        {
            if ((controlInstance = panel?.addControl (control, this)))
            {
                controlInstance.set (this);

                if (overrideImage && controlInstance.GetComponent<Image> ())
                    controlInstance.GetComponent<Image> ().sprite = overrideImage;

                if (!String.IsNullOrEmpty (overrideText) && controlInstance.GetComponentInChildren<Text> ())
                    controlInstance.GetComponentInChildren<Text> ().text = overrideText;

                controlInstance.gameObject.SetActive (true);
            }
        }

        protected override void removeControls()
        {
            panel?.removeControl (controlInstance);
        }
    }
}