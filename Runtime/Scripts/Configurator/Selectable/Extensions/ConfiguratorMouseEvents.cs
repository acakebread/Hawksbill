// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/08/2021 17:26:45 by seantcooper
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Hawksbill.Configurator
{
    ///<summary>Converts Mouse Messages to Events</summary>
    [AddComponentMenu ("")]
    // [RequireComponent (typeof (ConfiguratorSelectable))]
    public sealed class ConfiguratorMouseEvents : ConfiguratorExtension, IPointerEnterHandler
    {
        public Events events;

        void OnMouseEnter() => events.invokeEvent (MethodBase.GetCurrentMethod ().Name);
        void OnMouseExit() => events.invokeEvent (MethodBase.GetCurrentMethod ().Name);
        void OnMouseDown() => events.invokeEvent (MethodBase.GetCurrentMethod ().Name);
        void OnMouseDrag() => events.invokeEvent (MethodBase.GetCurrentMethod ().Name);
        void OnMouseOver() => events.invokeEvent (MethodBase.GetCurrentMethod ().Name);
        void OnMouseUp() => events.invokeEvent (MethodBase.GetCurrentMethod ().Name);
        void OnMouseUpAsButton() => events.invokeEvent (MethodBase.GetCurrentMethod ().Name);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException ();
        }

        [System.Serializable]
        public class Events
        {
            public UnityEvent OnMouseEnter;
            public UnityEvent OnMouseExit;
            public UnityEvent OnMouseDown;
            public UnityEvent OnMouseDrag;
            public UnityEvent OnMouseOver;
            public UnityEvent OnMouseUp;
            public UnityEvent OnMouseUpAsButton;

            public void invokeEvent(string name)
            {
                (typeof (Events).GetField (name).GetValue (this) as UnityEvent)?.Invoke ();
            }
        }

        // void sendMessage(MethodBase methodBase) =>
        //     transform.parent.gameObject.SendMessage (methodBase.Name);
    }
}