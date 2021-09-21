// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace Hawksbill
{
    public class GlobalSearcher : SingletonMonoBehaviour<GlobalSearcher>
    {
        protected override void Awake()
        {
            base.Awake ();
            GameObject.DontDestroyOnLoad (gameObject);
        }
    }

    public static class Searcher
    {
        static Searcher()
        {
            if (Application.isPlaying) GlobalManager.GetOrAddComponent<GlobalSearcher> ();
        }

        /// <summary>Get all gameObjects implementing Interface/Type</summary>
        public static IEnumerable<GameObject> GetGameObjectsWith<T>() =>
            GetComponentsWith<T> ().Select (c => c.gameObject).Distinct ();

        /// <summary>Get all components implementing Interface/Type</summary>
        public static IEnumerable<Component> GetComponentsWith<T>() =>
            GetTypesImplementing<T> ().SelectMany (t => UnityEngine.Object.FindObjectsOfType (t).Select (o => o as Component));

        /// <summary>Get root GameObjects</summary>
        public static IEnumerable<GameObject> GetAllRootGameObjects()
        {
            var gameObjects = GetScenes ().SelectMany (s => s.GetRootGameObjects ());
            return gameObjects;
        }

        /// <summary>Get all Scenes</summary>
        public static IEnumerable<Scene> GetScenes()
        {
            IEnumerable<Scene> scenes = Enumerable.Range (0, SceneManager.sceneCount).Select (i => SceneManager.GetSceneAt (i));
            if (GlobalSearcher.GetInstance ()) scenes = scenes.Concat (new Scene[] { GlobalSearcher.GetInstance ().gameObject.scene });
            return scenes;
        }

        /// <summary>Get all Executing and Referenced Assemblies. Warning Slow!</summary>
        static IEnumerable<Assembly> ExecutingAndReferencedAssemblies => new Assembly[] { Assembly.GetExecutingAssembly () }.
            Concat (Assembly.GetExecutingAssembly ().GetReferencedAssemblies ().Select (a => Assembly.Load (a.FullName)));

        static Type[] _ExecutingAndReferencedTypes;
        /// <summary>Get all Executing and Referenced Types. Warning Slow!</summary>
        public static Type[] ExecutingAndReferencedTypes => _ExecutingAndReferencedTypes == null ?
            _ExecutingAndReferencedTypes = ExecutingAndReferencedAssemblies.SelectMany (a => a.GetTypes ()).ToArray () : _ExecutingAndReferencedTypes;

        static Dictionary<Type, Type[]> GetTypesImplementing_Cache_Type = new Dictionary<Type, Type[]> ();
        /// <summary>Get all classes implementing Interface/Type</summary>
        public static Type[] GetTypesImplementing<T>()
        {
            if (!GetTypesImplementing_Cache_Type.ContainsKey (typeof (T)))
            {
                var matches = ExecutingAndReferencedTypes.Where (t => t.IsSubclassOf (typeof (T))).ToArray ();
                GetTypesImplementing_Cache_Type[typeof (T)] = matches.ToArray ();
            }
            return GetTypesImplementing_Cache_Type[typeof (T)];
        }
    }
}

// Group by layer
//var groups = transforms.GroupBy (t => t.gameObject.layer).ToDictionary (g => LayerMask.LayerToName (g.Key), g => g.Count ());