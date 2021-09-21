// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/02/2021 16:57:22 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class ComponentMessaging : MonoBehaviour
    {
        public Message[] messages;

        void Start()
        {
        }

        void Update()
        {
        }

        void send(Action action)
        {
            //messages
        }

        [System.Serializable]
        public class Message
        {
            public Action action;
            public string message;
        }

        public enum Action
        {
            OnEnable = 0,
            OnDisable = 1,
            Awake = 2,
            Start = 3,
        }
    }
}