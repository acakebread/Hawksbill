// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:22 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hawksbill.Serialization.Json;
using static Hawksbill.Firebase.Firestore.Conversion;
using static Hawksbill.Firebase.Firestore.TypeConversion;
using System.Reflection;

namespace Hawksbill.Firebase.Firestore
{
    public static class JsonParser
    {
        /// <summary>Convert Firestore Json to documents, 2 kinds: "[{\"document\":{ or "{\"documents\":["</summary>
        public static bool FromJson(string json, out Dictionary<string, object>[] result)
        {
            if (json == null) { result = null; return false; }
            var obj = JsonUtils.FromJson (json);
            if (obj.GetType ().IsArray) result = (obj as object[]).Select (o => createDocument (DSO (o)["document"])).ToArray ();
            else result = (DSO (obj)["documents"] as object[]).Select (o => createDocument (DSO (o))).ToArray ();
            return true;
        }

        public static bool FromJson(string json, out Dictionary<string, object> result)
        {
            if (json == null) { result = null; return false; }
            result = createDocument (JsonUtils.FromJson (json));
            return true;
        }

        static Dictionary<string, object> createDocument(object o)
        {
            Dictionary<string, object> body = (Dictionary<string, object>) o;
            body["createTime"] = Conversion.UnityDateTime (DateTime.Parse (body["createTime"] as string));
            body["updateTime"] = Conversion.UnityDateTime (DateTime.Parse (body["updateTime"] as string));
            body["fields"] = DSO (body["fields"]).ToDictionary (p => p.Key, p => Convert (DSO (p.Value).First ()));
            return body;
        }
    }

    public static class Conversion
    {
        public static Dictionary<string, object> DSO(object v) => v as Dictionary<string, object>;

        public static object Convert(KeyValuePair<string, object> pair) => Convert (pair.Key, pair.Value);
        public static object Convert(string type, object value)
        {
            switch (type)
            {
                case booleanValue: return bool.Parse (value.ToString ());
                case doubleValue: return double.Parse (value.ToString ());
                case integerValue: return long.Parse (value.ToString ());
                case referenceValue: return value;
                case stringValue: return value;
                case timestampValue: return DateTime.Parse (value as String);
                case arrayValue: return (DSO (value).First ().Value as object[]).Select (o => Convert (DSO (o).First ())).ToArray ();
            }
            throw new Exception (type + " not supported!");
        }

        public static DateTime Beginning = new DateTime (2020, 1, 1);
        public static long UnityDateTime(DateTime time) => (long) time.Subtract (Beginning).TotalSeconds;
    }
}