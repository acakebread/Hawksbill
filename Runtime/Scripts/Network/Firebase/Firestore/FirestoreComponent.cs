// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:22 by seancooper
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hawksbill.Serialization.Json;

namespace Hawksbill.Firebase.Firestore
{
    public class FirestoreComponent<T> : ScriptableObject
    {
        public Details details;
        // public Location location;
        // public string documentPath;
        //public new string name;
        // public float readTime, createTime, updateTime;

        public class TestClass
        {
            public Vector3 testVector;
            public DateTime testTime;
            public int[] testArray;
        }

        public async void load()
        {
            using (FirestoreRest rest = new FirestoreRest (details.location))
            {
                // var login = location.login ();
                // if (login != null)
                // {
                //     await location.login ();
                // }

                //public static T FromJSONObject<T>(Dictionary<string, object> source, SurrogateSelector selector = null)
                object[] test = new object[0];
                //var result = JsonFormatter.FromJSONObject<object[]> (new Dictionary<string, object> ());
                object t2 = new int[100];
                var type = t2.GetType ();
                var array = Array.CreateInstance (type, ((Array) t2).Length);
                Debug.Log ("type: " + array.GetType () + " Element: " + type.GetElementType ());

                var t = new TestClass { testVector = Vector3.up, testTime = DateTime.Now, testArray = new int[] { 1, 2, 3, 4 } };
                var json = JsonUtils.ToJson (t);
                Debug.Log ("json1: " + json);
                Debug.Log ("json2: " + JsonUtility.ToJson (t));
                var jsonObject = JsonUtils.FromJson (json);
                var o = JsonFormatter.FromJSONObject<TestClass> (jsonObject);


                //Debug.Log ("JSON: " + JsonUtility.ToJson (t));

                var text = await rest.getDocument (details.documentPath);
                Debug.Log ("D1: " + text);

                var text2 = await rest.getDocuments ("hawksbill");
                Debug.Log ("D2: " + text2);

                var text3 = await rest.getDocuments ("hawksbill", FirestoreRest.QWhere ("version", "1.0.0"));
                Debug.Log ("D3: " + text3);


                //name = document.name;
            }
        }

        public void save()
        {

        }

        [Serializable]
        public class Details
        {
            public Location location;
            public string documentPath;
            //public string name;
            //public float readTime, createTime, updateTime;
        }
    }
}
