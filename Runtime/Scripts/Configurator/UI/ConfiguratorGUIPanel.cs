// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/09/2021 08:32:30 by seantcooper
using System.Linq;
using UnityEngine;

namespace Hawksbill.Configurator
{
    [AddComponentMenu ("Configurator/Configurator GUI Panel")]
    public class ConfiguratorGUIPanel : MonoBehaviour
    {
        // public FieldString panelID;
        public FieldObject panelID;
        public Transform container;
        public bool initialVisibility = true;

        void Awake()
        {
            if (panelID) panelID.value = this;
            // gameObject.SetActive (initialVisibility);
        }

        void OnValidate()
        {
            if (!container) container = transform;
        }

        public ConfiguratorGUIControl addControl(ConfiguratorGUIControl control, UnityEngine.Object reference = null)
        {
            if (!control) return null;
            var instance = Instantiate (control, transform);
            instance.name = control.name + (reference ? " (" + reference.name + ")" : "");
            return instance;
        }

        public void removeControl(ConfiguratorGUIControl control)
        {
            if (control) Destroy (control.gameObject);
        }
    }
}