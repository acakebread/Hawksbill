// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 26/08/2021 12:22:33 by seantcooper
// using UnityEngine;
// using UnityEngine.Playables;

// namespace Hawksbill.Configurator
// {
//     ///<summary>How to explode this object!</summary>
//     [AddComponentMenu ("Configurator/Configurator Explode")]
//     [ExecuteInEditMode]
//     public class ConfiguratorExplode : MonoBehaviour
//     {
//         public PlayableDirector playableDirector;
//         [SerializeField, HideInInspector] PlayableDirector playableDirectorCreated;

//         void OnEnable()
//         {
//             print ("OnEnable");
//             if (Application.isPlaying)
//             {
//             }
//             else if (!playableDirector)
//             {
//                 if (playableDirectorCreated)
//                 {
//                     playableDirector = playableDirectorCreated;
//                 }
//                 else
//                 {
//                     playableDirectorCreated = playableDirector = gameObject.AddComponent<PlayableDirector> ();
//                     playableDirector.playOnAwake = false;
//                 }
//             }
//         }

//         void OnValidate()
//         {
//             if (playableDirectorCreated != playableDirector)
//             {
//             }
//         }

//         void Reset()
//         {
//             if (playableDirectorCreated)
//                 playableDirectorCreated?.destroy ();
//         }
//     }
// }