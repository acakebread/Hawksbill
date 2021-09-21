// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 08/08/2021 12:25:45 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Linq;
using System.Collections.Generic;
using System;
using static Hawksbill.DynamicDescription;
using static Hawksbill.DynamicDescription.Field;
using System.Reflection;

namespace Hawksbill
{
    [Serializable]
    public class DynamicQuery
    {
        [SerializeField] public Transform container;
        [SerializeField] public DynamicDescription description;
        [SerializeField] public ExpressionCollection expressions;

        public Dynamic[] results;

        [Delayed] public string group = "A";

        public Dynamic[] items => container ? container.GetComponentsInChildren<Dynamic> () : new Dynamic[0];
        // public IEnumerable<Dictionary<string, object>> getObjects()
        // {
        //     // foreach (var item in items)
        //     // {
        //     //     var o = description.fields.ToSafeDictionary (k => k.name, v => item.getSystemValue (v.name));
        //     //     o["__item"] = item;
        //     //     yield return o;
        //     // }
        // }

        public void OnValidate()
        {

            var items = this.items;
            var objects = items.Select (item => item.ToObject ()).ToArray ();

            Debug.Log ("objects.Count " + objects.Count ());

            results = expressions.run (this.items).ToArray ();

            // var parameter = Expression.Parameter (typeof (Dictionary<string, object>), "o");
            // var propertyInfo = typeof (Dictionary<string, object>).GetProperty ("Item");
            // var value = Expression.Constant (group);

            // var arguments = new List<Expression> { Expression.Constant ("Group") };
            // var index = Expression.Convert (Expression.MakeIndex (parameter, propertyInfo, arguments), typeof (String));
            // var expression = Expression.MakeBinary (ExpressionType.Equal, index, value);
            // var lambda = Expression.Lambda<Func<Dictionary<string, object>, bool>> (expression, parameter);
            // var result = objects.AsQueryable ().Where (lambda);

            // Debug.Log ("results.Count " + result.Count ());


            // var parameter = Expression.Parameter (typeof (Dynamic), "x");
            // var propertyInfo = typeof (Dynamic).GetProperty ("value");

            // var value = Expression.Constant (group);

            // var arguments = new List<Expression> { Expression.Constant ("Group") };
            // var index = Expression.Convert (Expression.MakeIndex (parameter, propertyInfo, arguments), typeof (String));
            // var expression = Expression.MakeBinary (ExpressionType.Equal, index, value);
            // var lambda = Expression.Lambda<Func<Dynamic, bool>> (expression, parameter);
            // var result = items.AsQueryable ().Where (lambda);

            // Debug.Log ("results.Count " + result.Count ());


        }

        [Serializable]
        public class ExpressionCollection
        {
            public Expression[] expressions;
            public Criteria criteria = Criteria.And;

            public void compile(DynamicDescription description)
            {
                if (!description) return;
            }

            public IEnumerable<Dynamic> run(Dynamic[] items)
            {
                if (expressions.Length > 0)
                {
                    foreach (var item in items)
                        if (criteria == Criteria.And ? expressions.All (e => e.run (item)) : expressions.Any (e => e.run (item)))
                            yield return item;
                }
            }
        }


        [Serializable]
        public class Expression
        {
            public List<Element> elements;
            public Criteria criteria = Criteria.And;
            public bool run(Dynamic item) => criteria == Criteria.And ? elements.All (e => e.run (item)) : elements.Any (e => e.run (item));
            [Serializable]
            public class Element
            {
                public string name;
                public Comparison comparison;
                public Value value;
                public bool run(Dynamic dynamic)
                {
                    switch (comparison)
                    {
                        case Comparison.EQ: return compare (dynamic) == 0;
                        case Comparison.NE: return compare (dynamic) != 0;
                        case Comparison.GT: return compare (dynamic) > 0;
                        case Comparison.GE: return compare (dynamic) >= 0;
                        case Comparison.LT: return compare (dynamic) < 0;
                        case Comparison.LE: return compare (dynamic) <= 0;
                    }
                    return false;
                }
                int compare(Dynamic dynamic) => dynamic[name].compareTo (value);
            }
            public enum Comparison { EQ = 0, NE = 1, GT = 2, GE = 3, LT = 4, LE = 5 }
        }

        public enum Criteria { And = 0, Or = 1 }

        // class test
        // {
        //     public Value.Type type;
        //     public string value;
        // }


        // static void Main()
        // {
        //     const string exp = @"(Person.Age > 3 AND Person.Weight > 50) OR Person.Age < 3";
        //     var p = Expression.Parameter (typeof (Person), "Person");
        //     var e = DynamicExpression.ParseLambda (new[] { p }, null, exp);
        //     var bob = new Person
        //     {
        //         Name = "Bob",
        //         Age = 30,
        //         Weight = 213,
        //         FavouriteDay = new DateTime (2000, 1, 1)
        //     };

        //     var result = e.Compile ().DynamicInvoke (bob);
        //     Console.WriteLine (result);
        //     Console.ReadKey ();

        //     dynamic person = new ExpandoObject ();
        //     person.Name = "Matt";
        //     person.Surname = "Smith";
        // }

        // public Match match;
        // public Comparer[] comparers;

        // [Serializable]
        // public class Comparer
        // {
        //     public string name;
        //     public Op op;
        //     public Field.Value value;
        //     public enum Op { EQ = 0, NE = 1, GT = 2, GE = 3, LT = 4, LE = 5 }
        //     public bool compare(FieldValue field)
        //     {
        //         switch (field.field.type)
        //         {

        //         }
        //     }
        // }

        // [Serializable]
        // public enum Match
        // {
        //     Any, All
        // }

        // void OnValidate()
        // {

        // }

        // public IEnumerable<GenericItem> query(Comparer[] comparers, Match match)
        // {

        //     switch (match)
        //     {
        //         case Match.All:
        //             items.Where (item => comparers.All ())
        //                 break;

        //         case Match.Any:

        //             break;

        //     }
        // }
    }

    public static class GenericItemQueryX
    {
        public static IEnumerable<Dynamic> getValidItems(this IEnumerable<Dynamic> items, string name) =>
            items.Where (item => item.hasField (name));

        public static IEnumerable<Value> getDistinctValues(this IEnumerable<Dynamic> items, string name) =>
            items.getValidItems (name).Select (item => item[name]).Distinct ();
    }
}