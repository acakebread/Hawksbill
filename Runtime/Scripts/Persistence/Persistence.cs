// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 15/02/2021 12:40:19 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    public static class Persistence
    {
        public static string Version = "1.00";
        public static Prefs Player = new Persistence.Prefs ();
        // public static Prefs Editor = new Persistence.Editor ();

#if UNITY_EDITOR
        [MenuItem ("Hawksbill/Clear PlayerPrefs")]
        public static void clear() => Player.clear ();
#endif

        //[Serializable]
        public class Prefs
        {
            const int SizeLimit = 1000000;
            TypeCode[] supported = new TypeCode[]{
                TypeCode.Boolean,TypeCode.Char,TypeCode.SByte,TypeCode.Byte,TypeCode.Int16,TypeCode.UInt16,TypeCode.Int32,TypeCode.UInt32,
                TypeCode.Int64,TypeCode.UInt64,TypeCode.Single,TypeCode.Double,TypeCode.Decimal,TypeCode.DateTime,TypeCode.String,
            };

            Dictionary<string, object> cache = new Dictionary<string, object> ();
            public string version { get => get<string> ("CurrentVersion", "xxx"); private set => set<string> ("CurrentVersion", value); }

            public void clear()
            {
                Pretty.Log (Color.black, ">>>>>>>>>> PlayerPrefs.DeleteAll () <<<<<<<<<");
                PlayerPrefs.DeleteAll ();
                PlayerPrefs.Save ();
                cache = new Dictionary<string, object> ();
            }

            public void checkVersion()
            {
                if (version != Application.version)
                {
                    clear ();
                    version = Application.version;
                }
            }

            bool isSupported<T>()
            {
                var code = Type.GetTypeCode (typeof (T));
                if (supported.Any (s => s == code)) return true;
                Pretty.Log (new Color (1, 0.4f, 1), "Type '" + code + "' is not supported!");
                return false;
            }

            public T get<T>(string key) => get (key, default (T));
            public T get<T>(string key, T defaultValue) => isSupported<T> () ? read (key, defaultValue) : defaultValue;
            public void set<T>(string key, T value) { if (isSupported<T> ()) write (key, value); }

            T read<T>(string key, T defaultValue)
            {
                if (!cache.ContainsKey (key))
                {
                    cache[key] = PlayerPrefs.HasKey (key) ? (T) Convert.ChangeType (PlayerPrefs.GetString (key), typeof (T)) : defaultValue;
                    Pretty.Log (new Color (1, 0.4f, 1), "Persistent.Prefs::Read " + key + ": '", cache[key] + "'");
                }
                return (T) cache[key];
            }

            void write<T>(string key, T value)
            {
                cache[key] = value;
                var str = value.ToString ();
                if (str.Length > SizeLimit)
                {
                    Debug.LogWarning ("Cannot write data due to size limit " + str.Length + " > " + SizeLimit + "!");
                    return;
                }
                Pretty.Log (new Color (1, 0.4f, 1), "Persistent.Prefs::Write " + key + " (" + str.Length + "b): '", value.ToString () + "'");
                PlayerPrefs.SetString (key, str);
                PlayerPrefs.Save ();
            }
        }
    }
}


// public static float getFloat(string key, float defaultValue);
// public static float GetFloat(string key);
// public static int GetInt(string key, int defaultValue);
// public static int GetInt(string key);
// public static string GetString(string key, string defaultValue);
// public static string GetString(string key);
// public static bool HasKey(string key);
// public static void Save();
// public static void SetFloat(string key, float value);
// public static void SetInt(string key, int value);
// public static void SetString(string key, string value);
// public static void SetObject(string key, object obj);
// public abstract void deleteAll();
// public abstract void delete(string key);
// public abstract bool has(string key);