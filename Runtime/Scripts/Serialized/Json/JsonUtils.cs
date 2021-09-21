// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine;

namespace Hawksbill.Serialization.Json
{
    public static class JsonUtils
    {
        /// <summary>Create object from JSON string</summary>
        public static object FromJson(string json)
        {
            XString str = new XString (ref json);
            return JsonUtils.ReadObject (ref str);
        }

        /// <summary>Create JSON string from object</summary>
        public static string ToJson(object src)
        {
            string dst = "";
            if (null == src) return dst + "null";
            Type type = src.GetType ();
            switch (Type.GetTypeCode (type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return dst + src.ToString ();

                case TypeCode.Boolean:
                    return dst + src.ToString ().ToLower ();

                case TypeCode.String:
                    return dst + "\"" + Regex.Replace (((string) src), "[\"\\\\]", @"\$0") + "\""; //Write as Escape String

                case TypeCode.DateTime:
                    return dst + "\"" + ((DateTime) src).ToString ("yyyy-MM-ddTHH:mm:ss.fffffffZ") + "\"";
            }

            #region "inline"
            string writeAsArray(IEnumerable enumerable) =>
                "[" + String.Join (",", enumerable.Cast<object> ().Select (o => ToJson ((object) o))) + "]";
            string writeAsDictionary(IDictionary<string, object> dict) =>
                "{" + String.Join (",", dict.Select (p => "\"" + p.Key + "\":" + ToJson (p.Value))) + "}";
            #endregion

            if (type.IsArray || type.IsGenericType (typeof (List<>)) || type.IsGenericType (typeof (HashSet<>)))
                return dst + writeAsArray (src as IEnumerable);

            if (type.IsGenericType (typeof (Dictionary<,>)))
            {
                IEnumerable<DictionaryEntry> asDict() { foreach (DictionaryEntry p in src as IDictionary) yield return p; }
                return dst + writeAsDictionary (asDict ().ToDictionary (k => k.Key.ToString (), v => v.Value));
            }

            FieldInfo[] fieldinfo = type.GetFields (BindingFlags.Instance | BindingFlags.Public);
            if (fieldinfo.Length > 0)
                return dst + writeAsDictionary (fieldinfo.ToDictionary (f => f.Name, f => f.GetValue (src)));

            Debug.LogWarning ("Source object type is not support " + type);
            return dst;
        }

        class XString : IEnumerable<char>
        {
            internal int pos = 0;
            string str;
            public XString(ref string src) => str = src;

            public char this[int i] => str[pos + i];
            public int Length => str.Length - pos;

            public string Substring(int startIndex, int length) => str.Substring (pos + startIndex, length);
            public XString Substring(int startIndex)
            {
                pos += startIndex;
                return this;
            }
            public XString TrimStart(params char[] trimChars)
            {
                for (; pos < str.Length && trimChars.Any (c => c == str[pos]); pos++) ;
                return this;
            }

            public bool StartsWith(string src) => Length >= src.Length && Substring (0, src.Length) == src;
            public int IndexOf(string c) => str.IndexOf (c, pos) - pos;

            public IEnumerator<char> GetEnumerator() { for (int i = pos; i < str.Length; i++) yield return str[i]; }
            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator ();

            string actual => Substring (0, Length);
            public override string ToString() => Substring (0, Length);
        }

        static readonly char[] WHITESPACE = { ' ', '\r', '\n', '\t', '\uFEFF', '\u0009' };

        static object ReadNull(ref XString src)
        {
            src = src.Substring ("null".Length);
            return null;
        }

        static object ReadBool(ref XString src)
        {
            bool result = src.StartsWith ("true");
            src = src.Substring (result ? "true".Length : "false".Length);
            return result;
        }

        static object ReadString(ref XString src)
        {
            int pos = 1;
            while (pos < src.Length && (src[pos] != '"' || src[pos - 1] == '\\')) pos++;
            string result = Regex.Unescape (src.Substring (1, pos - 1));
            src = src.Substring (pos + 1);
            return result;
        }

        static bool IsNumber(XString src)
        {
            int n = 0;
            if (src[n] == '-') n++;
            if (src[n] == '.') n++;
            return Char.IsDigit (src[n]);
        }

        static readonly char[] NUM = "-+.eE0123456789".ToCharArray ();
        static object ReadNumber(ref XString src)
        {
            string num = new String (src.TakeWhile (c => NUM.Contains (c)).ToArray ());
            src = src.Substring (num.Length);
            if (!num.Contains (".") && Int64.TryParse (num, out long LONG)) return LONG;
            return double.TryParse (num, out double DOUBLE) ? DOUBLE : double.MaxValue;
        }

        static object ReadArray(ref XString src)
        {
            List<object> list = new List<object> ();
            src = src.Substring (src.IndexOf ("[") + 1);
            while (src[0] != ']')
            {
                object v = ReadObject (ref src);
                list.Add (v);
                src = src.TrimStart (WHITESPACE); //skip whitespace
                if (true == src.StartsWith (",")) src = src.Substring (1); //skip comma if present
            }
            src = src.Substring (src.IndexOf ("]") + 1);

            if (list.Count <= 0) return new object[0];
            object[] dst = new object[list.Count];
            list.CopyTo (dst);
            return dst;
        }

        static object ReadObject(ref XString src)
        {
            src = src.TrimStart (WHITESPACE);

            // end of source
            if (src.Length <= 0) return null;

            // array detected
            if (src[0] == '[') return ReadArray (ref src);

            // object detected
            if (src[0] == '{')
            {
                object obj = new Dictionary<string, object> ();//we use dictionaries as objects
                src = src.Substring (src.IndexOf ("{") + 1);
                while (src.Length > 0 && src[0] != '}')
                {
                    object name = ReadObject (ref src);
                    if (name as string == null) break;
                    src = src.Substring (src.IndexOf (":") + 1);
                    ((Dictionary<string, object>) obj)[(string) name] = ReadObject (ref src);
                    src = src.TrimStart (WHITESPACE); //skip whitespace
                    if (true == src.StartsWith (",")) src = src.Substring (1);//skip comma if present
                }
                src = src.Substring (src.IndexOf ("}") + 1);
                return obj;
            }

            // string
            if (src[0] == '\"') return ReadString (ref src);

            // null detected
            if (src.StartsWith ("null")) return ReadNull (ref src);

            // bool detected
            if (src.StartsWith ("true") || src.StartsWith ("false")) return ReadBool (ref src);

            // number double / int64
            if (IsNumber (src)) return ReadNumber (ref src);

            throw new ArgumentException (string.Format ("jsontocs encountered unknown type " + src.Substring (-20, 64)));
        }
    }
}
