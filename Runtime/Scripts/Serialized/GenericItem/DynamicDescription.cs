// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/08/2021 18:08:16 by seantcooper
using UnityEngine;
using Hawksbill;
using Hawksbill.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Generic Item/Description")]
    public class DynamicDescription : ScriptableObject
    {
        [SerializeField] public Field[] fields;
        public event Action<DynamicDescription> onChanged;

        public Field this[string name] => fields.FirstOrDefault (f => f.name == name);
        public bool hasField(string name) => fields.Any (f => f.name == name);

        void OnValidate()
        {
            onChanged?.Invoke (this);
        }

        [Serializable]
        public class Field : IEquatable<Field>
        {
            [Delayed] public string name;
            public Value.Type type;
            [Show (nameof (showMinMax), ShowAttribute.Action.Hide), ObjectColumns (28)] public MinMax range;

            [Serializable] public class MinMax {[Delayed] public float min, max; }

            public bool Equals(Field f) => name == f.name && type == f.type && range.min == f.range.min && range.max == f.range.max;
            bool showMinMax => type == Value.Type.Int || type == Value.Type.Float;
        }

        [Serializable]
        public struct Value
        {
            [SerializeField] internal Value.Type type;
            [SerializeField] internal string rawValue;

            private Value(Value.Type type) { this.rawValue = GetDefaultValue (type); this.type = type; }
            private Value(string rawValue, Value.Type type) { this.rawValue = rawValue; this.type = type; }
            private Value(bool value) : this (value.ToString (), Type.Bool) { }
            private Value(float value) : this (value.ToString (), Type.Float) { }
            private Value(int value) : this (value.ToString (), Type.Int) { }
            private Value(string value) : this (value, Type.String) { }

            public override string ToString() => (string) this;

            public object systemValue => Convert.ChangeType (rawValue, GetSystemType ());

            public int compareTo(Value other)
            {
                switch (type)
                {
                    case Type.Bool: return ((bool) this).CompareTo ((bool) other);
                    case Type.Float: return ((float) this).CompareTo ((float) other);
                    case Type.Int: return ((int) this).CompareTo ((int) other);
                    default: case Type.String: return ((string) this).CompareTo ((string) other);
                }
            }

            // Conversion
            public static implicit operator Value(int v) => new Value (v);
            public static implicit operator int(Value v) => int.TryParse (v.rawValue, out int value) ? value : 0;
            public static implicit operator Value(float v) => new Value (v);
            public static implicit operator float(Value v) => float.TryParse (v.rawValue, out float value) ? value : 0f;
            public static implicit operator Value(bool v) => new Value (v);
            public static implicit operator bool(Value v) => bool.TryParse (v.rawValue, out bool value) ? value : false;
            public static implicit operator Value(string v) => new Value (v);
            public static implicit operator string(Value v) => v.rawValue;

            // System
            public static Value GetDefaultValue(Type type) => new[] { "", "0", "0", "false" }[(int) type];
            public System.Type GetSystemType() => new[] { typeof (String), typeof (Single), typeof (Int32), typeof (Boolean) }[(int) type];
            public enum Type { String = 0, Float = 1, Int = 2, Bool = 3, }
        }

        [Serializable]
        public class FieldValueCollection
        {
            [SerializeField] internal FieldValue[] values = new FieldValue[0];
            [SerializeField] DynamicDescription description;

            public FieldValue this[string name] => values.FirstOrDefault (v => v.field.name == name);
            public Dictionary<string, object> ToObject() => values.ToSafeDictionary (k => k.field.name, v => v.value.systemValue);

            internal void connect(DynamicDescription description)
            {
                if (this.description == description) return;
                if (this.description) this.description.onChanged -= validate;
                if ((this.description = description)) this.description.onChanged += validate;
            }
            internal void validate(DynamicDescription description)
            {
                if (description == null)
                {
                    values = new FieldValue[0];
                    return;
                }

                if (values.sequenceEqual (description.fields, (v, f) => v.field.Equals (f)))
                    return;

                IEnumerable<FieldValue> newValues()
                {
                    foreach (var field in description.fields)
                    {
                        int index = values.FindIndex (v => v.field.name == field.name);
                        yield return index == -1 ? new FieldValue (field) : new FieldValue (field, values[index].value);
                    }
                }
                values = newValues ().ToArray ();
            }

            [Serializable]
            public class FieldValue
            {
                [SerializeField] protected internal Field field;
                public Value value;
                // public object getSystemValue() => value.systemValue;
                public FieldValue(Field field) : this (field, Value.GetDefaultValue (field.type)) { }
                public FieldValue(Field field, Value value) { this.field = field; this.value = value; }
                public static implicit operator bool(FieldValue empty) => empty != null;
            }

            public static implicit operator bool(FieldValueCollection empty) => empty != null;
        }
    }

#if UNITY_EDITOR
    //    Inline Scriptable Object editors
    [UnityEditor.CustomPropertyDrawer (typeof (DynamicDescription), true), UnityEditor.CanEditMultipleObjects]
    public class GenericItem_Desc_Drawer : ScriptableObjectInline_Drawer<DynamicDescription> { }
#endif
}