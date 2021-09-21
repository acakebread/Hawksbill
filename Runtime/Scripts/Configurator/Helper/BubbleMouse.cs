// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/08/2021 19:09:19 by seantcooper
using System.Reflection;
using UnityEngine;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    [AddComponentMenu ("")]
    public class BubbleMouse : MonoBehaviour
    {
        [ReadOnly] public GameObject target;
        void OnMouseEnter() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseExit() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseDown() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseDrag() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseOver() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseUp() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseUpAsButton() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void sendMessage(string message) => target.SendMessage (message, SendMessageOptions.DontRequireReceiver);
    }
}