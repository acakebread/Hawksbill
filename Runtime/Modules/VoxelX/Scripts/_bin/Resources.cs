// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.IO;
// using UnityEditor;
// using UnityEngine;
// using Unity.Mathematics;
// using V3 = UnityEngine.Vector3;
// using V2 = UnityEngine.Vector2;
// using static Hawksbill.SplineData;

// namespace Hawksbill.Voxel
// {
//     // Resources
//     static class Resources
//     {
//         internal const string IconGUID = "49cb6f2a491de4c6e942b1c03ee5ab03";
//         public static Sprite[] IconSprites;
//         static Resources()
//         {
//             var items = AssetDatabase.LoadAllAssetsAtPath (AssetDatabase.GUIDToAssetPath (IconGUID));
//             IconSprites = items.Skip (1).Select (o => (Sprite) o).ToArray ();
//         }

//         public enum Icon
//         {
//             Blank = 0,
//             Brush = 7,
//             Pencil = 1,
//             Picker = 3,
//             Fill = 5,
//             Extrude = 24,
//             Select = 9,
//             Axis = 16,
//             Square = 40,
//             Circle = 41,
//             Line = 42,
//             MagicWand = 8,
//         }

//         public static Texture2D[] GetIcons(IEnumerable<int> indices) =>
//            indices.Select (i => AssetPreview.GetAssetPreview (IconSprites[(int) i])).ToArray ();
//     }
// }