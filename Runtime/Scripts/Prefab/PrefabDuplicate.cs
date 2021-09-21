// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Hawksbill.Network;
// using TMPro;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

// public class PrefabDuplicate : MonoBehaviour
// {
//     public GameObject prefab;
//     public bool write;

//     void OnValidate()
//     {
//         if (write)
//         {
//             write = false;
//             foreach (var item in data)
//             {
//                 var go = Instantiate (prefab, Vector3.zero, Quaternion.identity);
//                 go.GetComponent<Product> ().sku = item[0];
//                 go.name = item[1];
//                 if (go.GetComponentInChildren<TextMeshPro> ())
//                 {
//                     go.GetComponentInChildren<TextMeshPro> ().text = go.name;
//                     go.SetActive (false);
//                 }
//                 //var path = "Assets/App/Types/Products/Blank/Prefabs/" + go.name + ".prefab";
//                 //PrefabUtility.SaveAsPrefabAsset (go, path, out bool success);
//                 //print ("Writing " + path + " " + success);
//                 //AssetDatabase.CreateAsset (go, "Assets/App/Types/Products/Blank/Prefabs/" + go.name + ".prefab");
//             }
//         }
//     }

//     string[][] data ={
//         new string[]{"TG194252029299","iPhone 12 64GB Black"},
//         new string[]{"TG194252029633","iPhone 12 64GB White"},
//         new string[]{"TG194252029978","iPhone 12 64GB RED"},
//         new string[]{"TG194252030318","iPhone 12 64GB Blue"},
//         new string[]{"TG194252030653","iPhone 12 64GB Green"},
//         new string[]{"TG194252030998","iPhone 12 128GB Black"},
//         new string[]{"TG194252031339","iPhone 12 128GB White"},
//         new string[]{"TG194252031674","iPhone 12 128GB RED"},
//         new string[]{"TG194252032015","iPhone 12 128GB Blue"},
//         new string[]{"TG194252032350","iPhone 12 128GB Green"},
//         new string[]{"TG194252032695","iPhone 12 256GB Black"},
//         new string[]{"TG194252033036","iPhone 12 256GB White"},
//         new string[]{"TG194252033371","iPhone 12 256GB RED"},
//         new string[]{"TG194252033715","iPhone 12 256GB Blue"},
//         new string[]{"TG194252034057","iPhone 12 256GB Green"},
//         new string[]{"TG194252037379","iPhone 12 Pro 128GB Graphite"},
//         new string[]{"TG194252037713","iPhone 12 Pro 128GB Silver"},
//         new string[]{"TG194252038055","iPhone 12 Pro 128GB Gold"},
//         new string[]{"TG194252038390","iPhone 12 Pro 128GB Pacific Blue"},
//         new string[]{"TG194252038734","iPhone 12 Pro 256GB Graphite"},
//         new string[]{"TG194252039076","iPhone 12 Pro 256GB Silver"},
//         new string[]{"TG194252039410","iPhone 12 Pro 256GB Gold"},
//         new string[]{"TG194252039755","iPhone 12 Pro 256GB Pacific Blue"},
//         new string[]{"TG194252040096","iPhone 12 Pro 512GB Graphite"},
//         new string[]{"TG194252040430","iPhone 12 Pro 512GB Silver"},
//         new string[]{"TG194252040775","iPhone 12 Pro 512GB Gold"},
//         new string[]{"TG194252041116","iPhone 12 Pro 512GB Pacific Blue"},
//         new string[]{"TG194252020883","iPhone 12 Pro Max 128GB Graphite"},
//         new string[]{"TG194252021224","iPhone 12 Pro Max 128GB Silver"},
//         new string[]{"TG194252021569","iPhone 12 Pro Max 128GB Gold"},
//         new string[]{"TG194252021903","iPhone 12 Pro Max 128GB Pacific Blue"},
//         new string[]{"TG194252022245","iPhone 12 Pro Max 256GB Graphite"},
//         new string[]{"TG194252022580","iPhone 12 Pro Max 256GB Silver"},
//         new string[]{"TG194252022924","iPhone 12 Pro Max 256GB Gold"},
//         new string[]{"TG194252023266","iPhone 12 Pro Max 256GB Pacific Blue"},
//         new string[]{"TG194252023600","iPhone 12 Pro Max 512GB Graphite"},
//         new string[]{"TG194252023945","iPhone 12 Pro Max 512GB Silver"},
//         new string[]{"TG194252024287","iPhone 12 Pro Max 512GB Gold"},
//         new string[]{"TG194252024621","iPhone 12 Pro Max 512GB Pacific Blue"},
//         new string[]{"TG190199246850","AirPods Pro"},
//         new string[]{"TG194252244944","AirPods Max"},
//         new string[]{"TG190199882256","Apple Watch Series 6 GPS 40mm Red Aluminium Case with Red Sport Band Regular"},
//         new string[]{"TG190199884915","Apple Watch Nike Series 6 GPS 40mm Silver Aluminium Case with Pure Platinum Black Nike Sport Band Regular"},
//         new string[]{"TG190199885677","Apple Watch Nike Series 6 GPS 40mm Space Gray Aluminium Case with Anthracite Black Nike Sport Band Regular"},
//         new string[]{"TG190199865372","Apple Watch Series 6 GPS 40mm Gold Aluminium Case with Pink Sand Sport Band Regular"},
//         new string[]{"TG190199865754","Apple Watch Series 6 GPS 40mm Space Gray Aluminium Case with Black Sport Band Regular"},
//         new string[]{"TG190199866140","Apple Watch Series 6 GPS 40mm Blue Aluminium Case with Deep Navy Sport Band Regular"},
//         new string[]{"TG190199870666","Apple Watch Series 6 GPS 40mm Silver Aluminium Case with White Sport Band Regular"},
//         new string[]{"TG190199866928","Apple Watch Nike Series 6 GPS 44mm Space Gray Aluminium Case with Anthracite Black Nike Sport Band Regular"},
//         new string[]{"TG190199871045","Apple Watch Nike Series 6 GPS 44mm Silver Aluminium Case with Pure Platinum Black Nike Sport Band Regular"},
//         new string[]{"TG190199882638","Apple Watch Series 6 GPS 44mm Silver Aluminium Case with White Sport Band Regular"},
//         new string[]{"TG190199883017","Apple Watch Series 6 GPS 44mm Gold Aluminium Case with Pink Sand Sport Band Regular"},
//         new string[]{"TG190199883390","Apple Watch Series 6 GPS 44mm Space Gray Aluminium Case with Black Sport Band Regular"},
//         new string[]{"TG190199883772","Apple Watch Series 6 GPS 44mm Blue Aluminium Case with Deep Navy Sport Band Regular"},
//         new string[]{"TG190199884151","Apple Watch Series 6 GPS 44mm Red Aluminium Case with Red Sport Band Regular"},
//         new string[]{"TG190199837508","Apple Watch Nike Series 6 GPS Cellular 40mm Silver Aluminium Case with Pure Platinum Black Nike Sport Band Regular"},
//         new string[]{"TG190199838161","Apple Watch Nike Series 6 GPS Cellular 40mm Space Gray Aluminium Case with Anthracite Black Nike Sport Band Regular"},
//         new string[]{"TG190199833548","Apple Watch Series 6 GPS Cellular 40mm Silver Aluminium Case with White Sport Band Regular"},
//         new string[]{"TG190199833876","Apple Watch Series 6 GPS Cellular 40mm Gold Aluminium Case with Pink Sand Sport Band Regular"},
//         new string[]{"TG190199834200","Apple Watch Series 6 GPS Cellular 40mm Space Gray Aluminium Case with Black Sport Band Regular"},
//         new string[]{"TG190199834538","Apple Watch Series 6 GPS Cellular 40mm Blue Aluminium Case with Deep Navy Sport Band Regular"},
//         new string[]{"TG190199834866","Apple Watch Series 6 GPS Cellular 40mm Red Aluminium Case with Red Sport Band Regular"},
//         new string[]{"TG190199835191","Apple Watch Series 6 GPS Cellular 40mm Silver Stainless Steel Case with White Sport Band Regular"},
//         new string[]{"TG190199835528","Apple Watch Series 6 GPS Cellular 40mm Silver Stainless Steel Case with Silver Milanese Loop"},
//         new string[]{"TG190199835856","Apple Watch Series 6 GPS Cellular 40mm Gold Stainless Steel Case with Cyprus Green Sport Band Regular"},
//         new string[]{"TG190199836181","Apple Watch Series 6 GPS Cellular 40mm Gold Stainless Steel Case with Gold Milanese Loop"},
//         new string[]{"TG190199836518","Apple Watch Series 6 GPS Cellular 40mm Graphite Stainless Steel Case with Black Sport Band Regular"},
//         new string[]{"TG190199836846","Apple Watch Series 6 GPS Cellular 40mm Graphite Stainless Steel Case with Graphite Milanese Loop"},
//         new string[]{"TG190199839489","Apple Watch Series 6 GPS Cellular 44mm Blue Aluminium Case with Deep Navy Sport Band Regular"},
//         new string[]{"TG190199839816","Apple Watch Series 6 GPS Cellular 44mm Red Aluminium Case with Red Sport Band Regular"},
//         new string[]{"TG190199840140","Apple Watch Series 6 GPS Cellular 44mm Silver Stainless Steel Case with White Sport Band Regular"},
//         new string[]{"TG190199840478","Apple Watch Series 6 GPS Cellular 44mm Silver Stainless Steel Case with Silver Milanese Loop"},
//         new string[]{"TG190199840805","Apple Watch Series 6 GPS Cellular 44mm Gold Stainless Steel Case with Cyprus Green Sport Band Regular"},
//         new string[]{"TG190199841130","Apple Watch Series 6 GPS Cellular 44mm Gold Stainless Steel Case with Gold Milanese Loop"},
//         new string[]{"TG190199841468","Apple Watch Series 6 GPS Cellular 44mm Graphite Stainless Steel Case with Black Sport Band Regular"},
//         new string[]{"TG190199841796","Apple Watch Series 6 GPS Cellular 44mm Graphite Stainless Steel Case with Graphite Milanese Loop"},
//         new string[]{"TG190199842458","Apple Watch Nike Series 6 GPS Cellular 44mm Silver Aluminium Case with Pure Platinum Black Nike Sport Band Regular"},
//         new string[]{"TG190199843110","Apple Watch Nike Series 6 GPS Cellular 44mm Space Grey Aluminium Case with Anthracite Black Nike Sport Band Regular"},
//         new string[]{"TG190199872967","Apple Watch Series 6 GPS Cellular 44mm Silver Aluminium Case with White Sport Band Regular"},
//         new string[]{"TG190199873292","Apple Watch Series 6 GPS Cellular 44mm Gold Aluminium Case with Pink Sand Sport Band Regular"},
//         new string[]{"TG190199873629","Apple Watch Series 6 GPS Cellular 44mm Space Grey Aluminium Case with Black Sport Band Regular"},
//         new string[]{"TG194252055946","13-inch MacBook Air Apple M1 chip with 8-core CPU and 7-core GPU, 256GB Space Grey"},
//         new string[]{"TG194252056400","13-inch MacBook Air Apple M1 chip with 8-core CPU and 8-core GPU, 512GB Space Grey"},
//         new string[]{"TG194252057179","13-inch MacBook Air Apple M1 chip with 8-core CPU and 7-core GPU, 256GB Silver"},
//         new string[]{"TG194252057636","13-inch MacBook Air Apple M1 chip with 8-core CPU and 8-core GPU, 512GB Silver"},
//         new string[]{"TG194252058404","13-inch MacBook Air Apple M1 chip with 8-core CPU and 7-core GPU, 256GB Gold"},
//         new string[]{"TG194252058862","13-inch MacBook Air Apple M1 chip with 8-core CPU and 8-core GPU, 512GB Gold"},
//         new string[]{"TG194252165737","13-inch MacBook Pro Apple M1 chip with 8‑core CPU and 8‑core GPU, 256GB SSD Space Grey"},
//         new string[]{"TG194252166185","13-inch MacBook Pro Apple M1 chip with 8‑core CPU and 8‑core GPU, 512GB SSD Space Grey"},
//         new string[]{"TG194252166635","13-inch MacBook Pro Apple M1 chip with 8‑core CPU and 8‑core GPU, 256GB SSD Silver"},
//         new string[]{"TG194252167083","13-inch MacBook Pro Apple M1 chip with 8‑core CPU and 8‑core GPU, 512GB SSD Silver"},
//         new string[]{"TG190199806948","10.2-inch iPad Wi-Fi 32GB Space Grey"},
//         new string[]{"TG190199807228","10.2-inch iPad Wi-Fi 32GB Silver"},
//         new string[]{"TG190199807501","10.2-inch iPad Wi-Fi 32GB Gold"},
//         new string[]{"TG190199807785","10.2-inch iPad Wi-Fi 128GB Space Grey"},
//         new string[]{"TG190199808065","10.2-inch iPad Wi-Fi 128GB Silver"},
//         new string[]{"TG190199808348","10.2-inch iPad Wi-Fi 128GB Gold"},
//         new string[]{"TG190199808744","10.2-inch iPad Wi-Fi Cellular 32GB Space Grey"},
//         new string[]{"TG190199809024","10.2-inch iPad Wi-Fi Cellular 32GB Silver"},
//         new string[]{"TG190199809307","10.2-inch iPad Wi-Fi Cellular 32GB Gold"},
//         new string[]{"TG190199809581","10.2-inch iPad Wi-Fi Cellular 128GB Space Grey"},
//         new string[]{"TG190199809864","10.2-inch iPad Wi-Fi Cellular 128GB Silver"},
//         new string[]{"TG190199810143","10.2-inch iPad Wi-Fi Cellular 128GB Gold"},
//         new string[]{"TG190199777163","10.9-inch iPad Air Wi-Fi 64GB Space Grey"},
//         new string[]{"TG190199777446","10.9-inch iPad Air Wi-Fi 64GB Silver"},
//         new string[]{"TG190199777729","10.9-inch iPad Air Wi-Fi 64GB Rose Gold"},
//         new string[]{"TG190199778009","10.9-inch iPad Air Wi-Fi 64GB Sky Blue"},
//         new string[]{"TG190199778283","10.9-inch iPad Air Wi-Fi 64GB Green"},
//         new string[]{"TG190199778566","10.9-inch iPad Air Wi-Fi 256GB Space Grey"},
//         new string[]{"TG190199778849","10.9-inch iPad Air Wi-Fi 256GB Silver"},
//         new string[]{"TG190199779129","10.9-inch iPad Air Wi-Fi 256GB Rose Gold"},
//         new string[]{"TG190199779402","10.9-inch iPad Air Wi-Fi 256GB Sky Blue"},
//         new string[]{"TG190199779686","10.9-inch iPad Air Wi-Fi 256GB Green"},
//         new string[]{"TG190199788718","10.9-inch iPad Air Wi-Fi Cellular 64GB Space Grey"},
//         new string[]{"TG190199788992","10.9-inch iPad Air Wi-Fi Cellular 64GB Silver"},
//         new string[]{"TG190199789272","10.9-inch iPad Air Wi-Fi Cellular 64GB Rose Gold"},
//         new string[]{"TG190199789555","10.9-inch iPad Air Wi-Fi Cellular 64GB Sky Blue"},
//         new string[]{"TG190199789838","10.9-inch iPad Air Wi-Fi Cellular 64GB Green"},
//         new string[]{"TG190199790117","10.9-inch iPad Air Wi-Fi Cellular 256GB Space Grey"},
//         new string[]{"TG190199790391","10.9-inch iPad Air Wi-Fi Cellular 256GB Silver"},
//         new string[]{"TG190199790674","10.9-inch iPad Air Wi-Fi Cellular 256GB Rose Gold"},
//         new string[]{"TG190199790957","10.9-inch iPad Air Wi-Fi Cellular 256GB Sky Blue"},
//         new string[]{"TG190199791237","10.9-inch iPad Air Wi-Fi Cellular 256GB Green"},
//         new string[]{"TG190199656079","12.9-inch iPad Pro Wi‑Fi 128GB Space Grey"},
//         new string[]{"TG190199656352","12.9-inch iPad Pro Wi‑Fi 128GB Silver"},
//         new string[]{"TG190199416697","12.9-inch iPad Pro Wi‑Fi 256GB Space Grey"},
//         new string[]{"TG190199416970","12.9-inch iPad Pro Wi‑Fi 256GB Silver"},
//         new string[]{"TG190199417250","12.9-inch iPad Pro Wi‑Fi 512GB Space Grey"},
//         new string[]{"TG190199417533","12.9-inch iPad Pro Wi‑Fi 512GB Silver"},
//         new string[]{"TG190199417816","12.9-inch iPad Pro Wi‑Fi 1TB Space Grey"},
//         new string[]{"TG190199418097","12.9-inch iPad Pro Wi‑Fi 1TB Silver"},
//         new string[]{"TG190199657311","12.9-inch iPad Pro Wi‑Fi Cellular 128GB Space Grey"},
//         new string[]{"TG190199657595","12.9-inch iPad Pro Wi‑Fi Cellular 128GB Silver"},
//         new string[]{"TG190199456877","12.9-inch iPad Pro Wi‑Fi Cellular 256GB Space Grey"},
//         new string[]{"TG190199456884","12.9-inch iPad Pro Wi‑Fi Cellular 256GB Silver"},
//         new string[]{"TG190199453951","12.9-inch iPad Pro Wi‑Fi Cellular 512GB Space Grey"},
//         new string[]{"TG190199453968","12.9-inch iPad Pro Wi‑Fi Cellular 512GB Silver"},
//         new string[]{"TG190199455412","12.9-inch iPad Pro Wi‑Fi Cellular 1TB Space Grey"},
//         new string[]{"TG190199455429","12.9-inch iPad Pro Wi‑Fi Cellular 1TB Silver"},
//   };
// }
