// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/05/2021 09:01:37 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Reflection;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class BubbleMouseMessages : MonoBehaviour
    {
        public Direction direction = Direction.Parent;

        void OnMouseDown() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseDrag() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseEnter() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseExit() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseOver() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseUp() => sendMessage (MethodBase.GetCurrentMethod ().Name);
        void OnMouseUpAsButton() => sendMessage (MethodBase.GetCurrentMethod ().Name);

        GameObject target => direction == Direction.Parent ? transform.parent.gameObject : null;

        void sendMessage(string message) => target.SendMessage (message, SendMessageOptions.DontRequireReceiver);

        public enum Direction { Parent }
    }
}