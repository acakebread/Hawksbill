// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/08/2021 14:17:27 by seantcooper
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Allows customization of a Configurator Selectable</summary>
    [AddComponentMenu ("")]
    // [RequireComponent (typeof (ConfiguratorSelectable))]
    [ExtensionExclude]
    public abstract class ConfiguratorCustomize : ConfiguratorExtension
    {
        public int currentIndex = 0;

        public abstract int count { get; }
        public abstract void select(int index);

        void Start()
        {
            select (currentIndex);
        }

        void Update()
        {
        }

        void controlInput()
        {
            // debug only
            if (!Mouse.InputUsable || !selectable) return;
            if (Input.GetKey (KeyCode.Alpha1)) select (0);
            if (Input.GetKey (KeyCode.Alpha2)) select (1);
            if (Input.GetKey (KeyCode.Alpha3)) select (2);
            if (Input.GetKey (KeyCode.Alpha4)) select (3);
            if (Input.GetKey (KeyCode.Alpha5)) select (4);
            if (Input.GetKey (KeyCode.Alpha6)) select (5);
            if (Input.GetKey (KeyCode.Alpha7)) select (6);
            if (Input.GetKey (KeyCode.Alpha8)) select (7);
            if (Input.GetKey (KeyCode.Alpha9)) select (8);
        }

        protected override void OnValidate()
        {
            base.OnValidate ();
            currentIndex = Mathf.Clamp (currentIndex, 0, count - 1);
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                UnityEditor.EditorApplication.delayCall += () => select (currentIndex);
#endif
        }
    }
}