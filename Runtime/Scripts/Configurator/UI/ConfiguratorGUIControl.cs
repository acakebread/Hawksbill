// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/09/2021 15:59:54 by seantcooper
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hawksbill.Configurator
{
    ///<summary>Communicates between the Configurator and the UI!</summary>
    [AddComponentMenu ("Configurator/Configurator GUI Control")]
    public class ConfiguratorGUIControl : MonoBehaviour, ISelectedHandler, IDeselectedHandler
    {
        ConfiguratorExtension target;
        Action<ConfiguratorGUIControl> action;

        Button button => GetComponent<Button> ();

        public void set(ConfiguratorExtension target, Action<ConfiguratorGUIControl> action = null)
        {
            this.target = target;
            this.action = action;
        }

        public void OnClick() => click ();
        public void click()
        {
            print (name + "::click");
            if (!checkTarget ()) return;
            target.selectable.select ();
            if (action != null) action (this);
        }

        public void selectButton()
        {
            print ("selectButton " + name);
        }

        public void deselectButton()
        {
            print ("deselectButton " + name);
        }

        void IDeselectedHandler.OnDeselected(ConfiguratorSelectable selectable)
        {
            if (!checkTarget ()) return;
            if (target.selectable == selectable)
                print ("Deselected " + selectable.name);
        }

        void ISelectedHandler.OnSelected(ConfiguratorSelectable selectable)
        {
            if (!checkTarget ()) return;
            if (target.selectable == selectable)
                print ("Selected " + selectable.name);
        }

        bool checkTarget()
        {
            if (!target)
            {
                Debug.LogWarning ("ConfiguratorUI is null " + name);
                return false;
            }
            return true;
        }

        // void Update()
        // {
        //     if (button.interactable != target.gameObject.activeInHierarchy)
        //         button.interactable = target.gameObject.activeInHierarchy;
        // }
    }
}