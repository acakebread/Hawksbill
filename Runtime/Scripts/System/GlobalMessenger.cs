// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace Hawksbill
{
    public static class Messenger
    {
        const SendMessageOptions options = SendMessageOptions.RequireReceiver;

        public static void Broadcast<T>(string method, params object[] parameters) =>
            Searcher.GetAllRootGameObjects ().SelectMany (g => g.gameObject.GetComponents<Component> ().Where (c => c is T)).
                ForAll (c => c.message<T> (method, parameters));

        public static void Broadcast(string method) => Broadcast (method, null);
        public static void Broadcast(string method, object data = null) =>
            Searcher.GetAllRootGameObjects ().ForAll (g => g.BroadcastMessage (method, data, SendMessageOptions.DontRequireReceiver));
    }

    public interface IMessage
    {
        void OnMessage(string message);
    }

}