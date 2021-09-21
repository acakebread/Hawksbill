// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:22 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Hawksbill.Firebase.Firestore
{
    // Session Token
    // [Serializable]
    // public class Token
    // {
    //     public const string Signin = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={key}";
    //     public const string Payload = "{\"email\":\"{email}\",\"password\":\"{password}\",\"returnSecureToken\":true}";
    //     public string kind, localId, email, displayName, idToken, registered, refreshToken;
    //     public long expiresIn;
    //     public DateTime timeRead;
    //     public DateTime timeExpires => timeRead.AddSeconds (expiresIn);
    //     public Token() { idToken = ""; timeRead = new DateTime (); }
    //     public void clear() { idToken = ""; timeRead = new DateTime (); }
    //     public bool expired => idToken == "" ? true : (timeExpires - DateTime.Now).Seconds < 60;
    // }

    public static class TypeConversion
    {
        //https://cloud.google.com/firestore/docs/reference/rest/v1/Value
        public const string arrayValue = "arrayValue";          // "arrayValue": { object (ArrayValue) }
        public const string booleanValue = "booleanValue";      // "booleanValue": boolean
        public const string bytesValue = "bytesValue";          // "bytesValue": string
        public const string doubleValue = "doubleValue";        // "doubleValue": number
        public const string integerValue = "integerValue";      // "integerValue": string
        public const string nullValue = "nullValue";            // "nullValue": null
        public const string referenceValue = "referenceValue";  // "referenceValue": string
        public const string stringValue = "stringValue";        // "stringValue": string
        public const string timestampValue = "timestampValue";  // "timestampValue": string
                                                                // unsupported
        public const string geoPointValue = "geoPointValue";    // "geoPointValue": { object (LatLng) }
        public const string mapValue = "mapValue";              // "mapValue": { object (MapValue) }

        static Dictionary<Type, string> TypeToString = new Dictionary<Type, string> {
                { typeof(object[]), arrayValue },
                { typeof(bool),     booleanValue },
                { typeof(byte[]),   bytesValue },
                { typeof(float),    doubleValue },
                { typeof(double),   doubleValue },
                { typeof(decimal),  doubleValue },
                { typeof(SByte),    integerValue },
                { typeof(Byte),     integerValue },
                { typeof(Int16),    integerValue },
                { typeof(UInt16),   integerValue },
                { typeof(Int32),    integerValue },
                { typeof(UInt32),   integerValue },
                { typeof(Int64),    integerValue },
                { typeof(UInt64),   integerValue },
                { typeof(string),   stringValue },
                { typeof(DateTime), timestampValue },
            };

        static Dictionary<string, Type> StringToType = new Dictionary<string, Type>
            {
                { arrayValue,       typeof(object[])},
                { booleanValue,     typeof(bool) },
                { bytesValue,       typeof(byte[]) },
                { doubleValue,      typeof(double) },
                { integerValue,     typeof(long) },
                { referenceValue,   typeof(string) },
                { stringValue,      typeof(string) },
                { timestampValue,   typeof(DateTime) },
            };

        static Dictionary<string, Func<string, object>> Conversion = new Dictionary<string, Func<string, object>>
            {
                //{ nullValue,        null },
                { arrayValue,       (s)=>throw new Exception(referenceValue + " not supported!") },
                { booleanValue,     (s)=>bool.Parse(s) },
                { bytesValue,       (s)=>throw new Exception(referenceValue + " not supported!") },
                { doubleValue,      (s)=>double.Parse(s) },
                { integerValue,     (s)=>long.Parse(s) },
                { referenceValue,   (s)=>throw new Exception(referenceValue + " not supported!") },
                { stringValue,      (s)=>s },
                { timestampValue,   (s)=>DateTime.Parse(s) },
            };

        public static string TypeToName(Type type)
        {
            if (TypeToString.ContainsKey (type)) return TypeToString[type];
            if (type.IsArray) return arrayValue;
            Debug.LogError ("Type not supported: '" + type.ToString () + "'");
            return null;
        }

        public static Type NameToType(string type)
        {
            if (StringToType.ContainsKey (type)) return StringToType[type];
            Debug.LogError ("Type not supported: '" + type.ToString () + "'");
            return null;
        }
    }

    public enum OP
    {
        OPERATOR_UNSPECIFIED,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
        EQUAL,
        ARRAY_CONTAINS,
        ARRAY_CONTAINS_ANY,
        IN,
    }
}