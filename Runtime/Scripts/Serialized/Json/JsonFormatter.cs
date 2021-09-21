// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Hawksbill.Serialization.Json
{
    public static class JsonFormatter
    {
        static SurrogateSelector DefaultSelector = new SurrogateSelector (new ISurrogate[]
        {
            //new SurrogateVector2(),new SurrogateVector3(),new SurrogateVector4(),
        });

        static SurrogateSelector selector = DefaultSelector;
        public static T FromJSONObject<T>(object source, SurrogateSelector selector = null)
        {
            DefaultSelector = selector == null ? DefaultSelector : selector;
            return (T) SetValue (typeof (T), source);
        }

        static object SetValue(Type type, object source)
        {
            var typeCode = Type.GetTypeCode (type);
            switch (typeCode)
            {
                default: return System.Convert.ChangeType (source, type);
                case TypeCode.Empty: throw new Exception (typeCode + " is not supported!");
                case TypeCode.DBNull: throw new Exception (typeCode + " is not supported!");
                case TypeCode.DateTime: return DateTime.Parse (source.ToString ());
                case TypeCode.Object:
                    if (DefaultSelector.has (type)) return DefaultSelector.get (type).getObjectData (source, DefaultSelector);
                    else if (type.IsArray)
                    {
                        object[] sourceArray = (object[]) source;
                        var elementType = type.GetElementType ();
                        var target = Array.CreateInstance (elementType, sourceArray.Length);
                        for (int i = target.GetLowerBound (0); i <= target.GetUpperBound (0); i++)
                            target.SetValue (SetValue (elementType, sourceArray[i]), i);
                        return target;
                    }
                    else
                    {
                        Dictionary<string, object> dict = (Dictionary<string, object>) source;
                        object target = Activator.CreateInstance (type);
                        target.GetType ().GetFields ().
                            Where (f => dict.ContainsKey (f.Name)).
                            ForAll (f => f.SetValue (target, SetValue (f.FieldType, dict[f.Name])));
                        return target;
                    }
            }
        }
    }
}

// ??????
// static object TryChangeType(object source, Type type)
// {
//     if (type == null || source == null || (source as IConvertible) == null) return Activator.CreateInstance (type);
//     return System.Convert.ChangeType (source, type);
// }