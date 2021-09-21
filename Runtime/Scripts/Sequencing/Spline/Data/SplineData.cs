// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using Unity.Mathematics;
using System.Runtime.CompilerServices;
using Hawksbill.Geometry;
using Hawksbill.Analytics;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill.Sequencing
{
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Spline/Spline Data")]
    public class SplineData : ScriptableObject, ISerializationCallbackReceiver
    {
        public const float RotationDistance = 0.01f;
        public Color color = Color.white;
        public bool loop = false;
        [Line]
        public NodeAction definition = NodeAction.Custom;
        public List<Node> nodes = new List<Node> ();
        [Line]
        [ReadOnly] public float totalLength;

        public Node this[float index] => hasNodes ? nodes[(int) scope (index)] : Node.zero;
        public Node next(Node node) => this[node.index + 1]; // nodes[loop ? (int) (node.index + 1) % nodes.Count : Mathf.Clamp ((int) node.index + 1, 0, nodes.Count - 1)];
        public float nodeRange => loop ? nodes.Count : nodes.Count - 1;
        public float scope(float f) => loop ? Mathf.Repeat (f, nodes.Count) : Mathf.Clamp (f, 0, nodes.Count - 1);
        public bool hasNodes => nodes != null && nodes.Count > 1;

        // INTERFACE
        public Node getNodeAt(float f) => this[f].lerp (next (this[f]), scope (f) % 1);
        public V3 getPositionAt(float f) => this[f].lerpPosition (next (this[f]), scope (f) % 1);

        public IEnumerable<V3> getPositions() => nodes.Take ((int) nodeRange - 2).
            SelectMany (n => n.spline.positions.Take (n.spline.positions.Length - 1)).
            Concat (nodes[(int) nodeRange - 1].spline.positions);
        public IEnumerable<V3> getPositions(Node node) => node.getPositions (next (node));

        public Vector3 getDirectionAt(float f, float distance = 0.01f)
        {
            f = scope (f);
            V3 d = getPositionAt (f + distance) - getPositionAt (f - distance);
            return d == V3.zero ? this[f].rotation * V3.forward : d.normalized;
        }

        // Awake
        void Awake()
        {
            validate ();
        }

        // SERIALIZE
        void OnValidate()
        {
            if (definition != NodeAction.Custom)
                applyAction ();
            validate ();
        }
        public void validate()
        {
            nodes.ForAll ((n, i) => n.validate (i, this[i + 1]));
            totalLength = nodes.Sum (n => n.distance);
        }
        void ISerializationCallbackReceiver.OnBeforeSerialize() => validate ();
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }

        // SELECT (Editor)
        int _selectedIndex;
        public SplineData.Node selectedNode => this[_selectedIndex];
        public void select(SplineData.Node node = null) => _selectedIndex = (int) (node ? node.index : 0f);

        // NODE
        [Serializable]
        public class Node
        {
            [SerializeField, FormerlySerializedAs ("localPosition")] V3 _position;
            [SerializeField, FormerlySerializedAs ("localRotation")] V3 _euler;
            [ObjectColumns (25)] public Scale scale = new Scale ();
            [HideInInspector] public float index;
            [ReadOnly] public float distance;

            public static Node zero = new Node ();

            public V3 position { get => _position; set => _position = value; }
            public V3 euler { get => _euler; set => _euler = value; }
            public Quaternion rotation { get => Quaternion.Euler (_euler); set => _euler = value.eulerAngles; }

            // control points
            public V3 cpIn => _position - rotation * V3.forward * Mathf.Max (0.00001f, scale.In);
            public V3 cpOut => _position + rotation * V3.forward * Mathf.Max (0.00001f, scale.Out);

            // caching
            SplineQ _spline;
            internal SplineQ spline => _spline;

            public Node() { }
            public Node(V3 position) => _position = position;
            public Node(V3 position, V3 rotation, Scale scale, float index)
            {
                _position = position;
                _euler = rotation;
                this.scale = scale;
                this.index = index;
            }

            public override string ToString() => _position + "," + _euler + "," + scale;
            public Node clone() => new Node (_position, _euler, new Scale (scale.In, scale.Out), index);

            public void validate(int index, Node next)
            {
                this.index = index;
                if (spline == null || !spline.equals (position, cpOut, next.cpIn, next.position))
                    _spline = new SplineQ (position, cpOut, next.cpIn, next._position);
                distance = spline.length;
            }

            public Node lerp(Node next, float f)
            {
                f = Mathf.Clamp01 (f);
                if (f == 0) return clone ();
                if (f == 1) return next.clone ();
                V3 p1 = lerpPosition (next, Mathf.Clamp01 (f - 0.001f)), p2 = lerpPosition (next, Mathf.Clamp01 (f + 0.001f));
                return new Node ((p1 + p2) / 2, Quaternion.LookRotation (p2 - p1).eulerAngles, new Node.Scale (2), index + f);
            }

            public V3 lerpPosition(Node next, float f)
            {
                if (this == next) return _position;
                return spline.getPositionAt (f);
            }

            public IEnumerable<V3> getPositions(Node next) =>
                EnumerableUtility.Range (0f, 1, 1f / SplineQ.Resolution).Select (f => lerpPosition (next, f));

            [Serializable]
            public class Scale
            {
                public float In, Out;
                public Scale(float In, float Out) { this.In = In; this.Out = Out; }
                public Scale(float value = 5) { In = Out = value; }
                public override string ToString() => In + "," + Out;
            }

            public static implicit operator bool(Node empty) => empty != null;
        }

        // SUPPORT
        public IEnumerable<float> getSequence(float step) => EnumerableUtility.Range (0f, nodeRange, 1f / Mathf.RoundToInt (1 / step * nodeRange));

        // PRIMITIVES
        public enum NodeAction
        {
            Custom = 0,
            Nothing = 1,
            Line = 2,
            Circle = 3,
            Square = 4,
            // RoundedSquare = 5,
        }

        void applyAction()
        {
            const float Size = 50;
            NodeAction action = definition;
            definition = NodeAction.Custom;
            switch (action)
            {
                case NodeAction.Custom: break;
                case NodeAction.Nothing: nodes = new List<Node> (); break;
                case NodeAction.Line:
                    {
                        float len = Size, scale = len / 2;
                        nodes = new List<Node>
                        {
                            new Node (new V3 (0, 0, 0), new V3 (0, 0, 0), new Node.Scale (scale), 0),
                            new Node (new V3 (0, 0, len), new V3 (0, 0, 0), new Node.Scale (scale), 1),
                        };
                        loop = false;
                    }
                    break;
                case NodeAction.Circle:
                    {
                        float radius = Size, scale = 4 * (Mathf.Sqrt (2) - 1) / 3 * radius;
                        nodes = new List<Node>
                        {
                            new Node (new V3 (0, 0, radius), new V3 (0, 90, 0), new Node.Scale (scale), 0),
                            new Node (new V3 (radius, 0, 0), new V3 (0, 180, 0), new Node.Scale (scale), 1),
                            new Node (new V3 (0, 0, -radius), new V3 (0, 270, 0), new Node.Scale (scale), 2),
                            new Node (new V3 (-radius, 0, 0), new V3 (0, 0, 0), new Node.Scale (scale), 3),
                        };
                        loop = true;
                    }
                    break;
                case NodeAction.Square:
                    {
                        float scale = 0.01f;
                        nodes = new List<Node>
                        {
                            new Node (new V3 (50, 0, 50), new V3 (0, 135, 0), new Node.Scale (scale), 0),
                            new Node (new V3 (50, 0, -50), new V3 (0, 225, 0), new Node.Scale (scale), 1),
                            new Node (new V3 (-50, 0, -50), new V3 (0, 315, 0), new Node.Scale (scale), 2),
                            new Node (new V3 (-50, 0, 50), new V3 (0, 45, 0), new Node.Scale (scale), 3),
                        };
                        loop = true;
                    }
                    break;

                    // case NodeAction.RoundedSquare:
                    //     {
                    //         float radius = Size / 5, scale = 4 * (Mathf.Sqrt (2) - 1) / 3 * radius;
                    //         float len = Size - radius;
                    //         float width = Size;
                    //         nodes = new List<Node>
                    //         {
                    //             new Node (new V3 (-len, 0, width), new V3 (0, 90, 0), new Node.Scale (scale), 0),
                    //             new Node (new V3 (+len, 0, width), new V3 (0, 90, 0), new Node.Scale (scale), 0),
                    //             new Node (new V3 (width, 0, +len), new V3 (0, 180, 0), new Node.Scale (scale), 1),
                    //             new Node (new V3 (width, 0, -len), new V3 (0, 180, 0), new Node.Scale (scale), 1),
                    //             new Node (new V3 (width, 0, +len), new V3 (0, 180, 0), new Node.Scale (scale), 1),
                    //             new Node (new V3 (width, 0, -len), new V3 (0, 180, 0), new Node.Scale (scale), 1),
                    //             new Node (new V3 (0, 0, -width), new V3 (0, 270, 0), new Node.Scale (scale), 2),
                    //             new Node (new V3 (-width, 0, 0), new V3 (0, 0, 0), new Node.Scale (scale), 3),
                    //         };
                    //         loop = true;
                    //     }
                    //     break;
            }
        }


        // // CACHE
        // public Dictionary<SplinePlayable, PointCache> pointCache;
        // public void addPointCache(SplinePlayable playable, PointCache cache)
        // {
        // }

        // public class PointCache
        // {
        //     public SplinePlayable playable;
        //     public V3[] points;

        //     public PointCache(SplinePlayable playable)
        //     {
        //     }

        //     // class SampleCache
        //     // {
        //     //     const float Step = 0.02f;
        //     //     public V3[] points;
        //     //     public float fStart, fEnd;

        //     //     public SampleCache(SplinePlayable playable, float position, float deviation)
        //     //     {
        //     //         this.fStart = (position - deviation);
        //     //         this.fEnd = (position + deviation);
        //     //         EnumerableUtility.Range (this.fStart, this.fEnd, Step);
        //     //     }

        //     //     public V3 getPosition(float f)
        //     //     {
        //     //         float fi = ((f - fStart) / (fEnd - fStart) * points.Count - 1);
        //     //         return V3.Lerp (points[(int) fi], points[(int) fi + 1], fi % 1;
        //     //     }
        //     // }
        // }
    }

#if UNITY_EDITOR
    // Inline Scriptable Object editors
    [UnityEditor.CustomPropertyDrawer (typeof (SplineData), true)]
    public class SplineDataDrawer : ScriptableObjectInline_Drawer<SplineData> { }
#endif
}
