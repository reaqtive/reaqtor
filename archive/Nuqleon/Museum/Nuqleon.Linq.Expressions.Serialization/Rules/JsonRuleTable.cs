// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.Serialization;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Json.Expressions;

    /// <summary>
    /// Rule type for serialization and deserialization of common types to and from JSON.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    internal class JsonRuleTable<TContext> : RuleTable<object, Json.Expression, TContext>
    {
        #region Constructors

        /// <summary>
        /// Creates a new JSON serialization/deserialization rule table.
        /// </summary>
        protected JsonRuleTable()
            : base(
                members =>
                {
                    var add = (MethodInfo)ReflectionHelpers.InfoOf((Dictionary<string, Json.Expression> d) => d.Add(null, null));
                    var ctor = (MethodInfo)ReflectionHelpers.InfoOf(() => Json.Expression.Object(null));

                    return
                        Expression.Call(
                            ctor,
                            Expression.ListInit(
                                Expression.New(typeof(Dictionary<string, Json.Expression>)),
                                from member in members
                                select Expression.ElementInit(add, Expression.Constant(member.Key), Expression.Convert(member.Value, typeof(Json.Expression)))
                            )
                        );
                },
                (obj, member) =>
                {
                    var members = (PropertyInfo)ReflectionHelpers.InfoOf((Json.ObjectExpression json) => json.Members);
                    var index = (MethodInfo)ReflectionHelpers.InfoOf((Json.ObjectExpression json) => json.Members[null]);

                    return
                        Expression.Call(
                            Expression.Property(
                                Expression.Convert(
                                    obj,
                                    typeof(Json.ObjectExpression)
                                ),
                                members
                            ),
                            index,
                            Expression.Constant(member)
                        );
                }
            )
        {
            AddNullReferenceRule();
            AddDateTimeRules();
            AddNumericTypeRules();
            AddTextualTypeRules();
            AddMiscellaneousRules();
            AddCollectionRules();
        }

        #endregion

        #region Methods

#pragma warning disable IDE0034 // Simplify 'default' expression (used in type tables)

        /// <summary>
        /// Adds a rule to support the null reference.
        /// </summary>
        private void AddNullReferenceRule()
        {
            Add(
                "Null", default(object), o => o == null,
                (o, _, ctx) => Json.Expression.Null(),
                (j, _, ctx) => null
            );
        }

        /// <summary>
        /// Adds rules to support date and time related value types.
        /// </summary>
        private void AddDateTimeRules()
        {
            Add((TimeSpan ts) => TimeSpan.FromTicks(ts.Ticks));
            Add((DateTime dt) => new DateTime(dt.Ticks));
            Add((DateTimeOffset dto) => new DateTimeOffset(dto.DateTime, dto.Offset));
        }

        /// <summary>
        /// Adds rules to support numeric value types.
        /// </summary>
        private void AddNumericTypeRules()
        {
            Add(
                "SByte", default(sbyte),
                (b, _, ctx) => Json.Expression.Number(b.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => sbyte.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "Byte", default(byte),
                (b, _, ctx) => Json.Expression.Number(b.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => byte.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "Short", default(short),
                (b, _, ctx) => Json.Expression.Number(b.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => short.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "UShort", default(ushort),
                (b, _, ctx) => Json.Expression.Number(b.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => ushort.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "Int32", default(int),
                (i, _, ctx) => Json.Expression.Number(i.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => int.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "UInt32", default(uint),
                (i, _, ctx) => Json.Expression.Number(i.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => uint.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "Int64", default(long),
                (l, _, ctx) => Json.Expression.Number(l.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => long.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "UInt64", default(ulong),
                (l, _, ctx) => Json.Expression.Number(l.ToString(CultureInfo.InvariantCulture)),
                (j, _, ctx) => ulong.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "Single", default(float),
                (f, _, ctx) => Json.Expression.Number(f.ToString("R", CultureInfo.InvariantCulture)),
                (j, _, ctx) => float.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "Double", default(double),
                (d, _, ctx) => Json.Expression.Number(d.ToString("R", CultureInfo.InvariantCulture)),
                (j, _, ctx) => double.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
            Add(
                "Decimal", default(decimal),
                (d, _, ctx) => Json.Expression.Number(d.ToString("G", CultureInfo.InvariantCulture)),
                (j, _, ctx) => decimal.Parse(((Json.ConstantExpression)j).ToString(), CultureInfo.InvariantCulture)
            );
        }

        /// <summary>
        /// Adds rules to support textual types such as strings and characters.
        /// </summary>
        private void AddTextualTypeRules()
        {
            Add(
                "String", default(string),
                (s, _, ctx) => Json.Expression.String(s),
                (j, _, ctx) => (string)((Json.ConstantExpression)j).Value
            );
            Add(
                "Char", default(char),
                (c, _, ctx) => Json.Expression.String(c.ToString()),
                (j, _, ctx) => ((string)((Json.ConstantExpression)j).Value).Single()
            );
        }

        /// <summary>
        /// Adds miscellaneous rules for other types.
        /// </summary>
        private void AddMiscellaneousRules()
        {
            Add(
                "Boolean", default(bool),
                (b, _, ctx) => Json.Expression.Boolean(b),
                (j, _, ctx) => bool.Parse(((Json.ConstantExpression)j).ToString())
            );
            Add(
                "Guid", default(Guid),
                (g, rec, ctx) => rec(g.ToString("D"), ctx),
                (j, rec, ctx) => new Guid((string)rec(j, ctx))
            );
            Add(
                "Tuple", default(object), // Could be interned through our heap serializer (used for closures now).
                o =>
                {
                    if (o is null or not IStructuralEquatable)
                        return false;

                    var t = o.GetType();
                    var iTuple = typeof(Tuple<int>).GetInterfaces().Where(i => i.Name == "ITuple").Single();
                    if (t.GetInterfaces().Contains(iTuple))
                        return true;

                    return false;
                },
                (ci, serialize, ctx) =>
                    ci.GetType().Let(t =>
                        t.GetGenericArguments().Let(ga =>
                            Json.Expression.Array(
                                from p in t.GetProperties()
                                where p.Name.StartsWith("Item", StringComparison.Ordinal)
                                let i = int.Parse(p.Name.Substring("Item".Length), CultureInfo.InvariantCulture)
                                orderby i ascending
                                let value = p.GetValue(ci, index: null)
                                select Json.Expression.Object(new Dictionary<string, Json.Expression>
                                            {
                                                { "Type", serialize(ga[i - 1], ctx) }, // Could have nulls or variance, need to know the exact type to reconstruct the generic type.
                                                { "Value", serialize(value, ctx) }
                                            })
                            )
                        )
                    ),
                (j, deserialize, ctx) =>
                    ((Json.ArrayExpression)j).Let(a =>
                        (from e in a.Elements
                         let o = (Json.ObjectExpression)e
                         let type = (Type)deserialize(o.Members["Type"], ctx)
                         let value = deserialize(o.Members["Value"], ctx)
                         select (type, value))
                        .ToArray().Let(items =>
                        {
                            var t1 = typeof(Tuple<int>).GetGenericTypeDefinition();
                            var tn = t1.Assembly.GetType(t1.FullName.TrimEnd('1') + items.Length);
                            var type = tn.MakeGenericType(items.Select(item => item.type).ToArray());
                            return Activator.CreateInstance(type, items.Select(item => item.value).ToArray());
                        })
                    )
            );
        }

        /// <summary>
        /// Adds rules to support common collection types.
        /// </summary>
        private void AddCollectionRules()
        {
            //
            // Notice this rule depends on serialization of System.Type objects and doesn't work in
            // the absence of such a rule added by a derived rule table.
            //
            Add(
                "ListOfT", default(IList), list =>
                {
                    var type = list.GetType();

                    if (type.IsArray)
                    {
                        return true;
                    }

                    if (type.IsGenericType)
                    {
                        var genericType = type.GetGenericTypeDefinition();
                        if (genericType == typeof(ReadOnlyCollection<>) || genericType == typeof(List<>))
                        {
                            return true;
                        }
                    }

                    return false;
                },
                (rc, serialize, ctx) =>
                {
                    var type = rc.GetType();
                    var elementType = type.IsArray ? type.GetElementType() : type.GetGenericArguments().Single();
                    var kind = type.IsArray ? "Array" : (type.GetGenericTypeDefinition() == typeof(ReadOnlyCollection<>) ? "ReadOnlyList" : "List");

                    return Json.Expression.Object(new Dictionary<string, Json.Expression>
                    {
                        { "Type", serialize(elementType, ctx) },
                        { "Kind", serialize(kind, ctx) },
                        { "Elements", Json.Expression.Array(rc.Cast<object>().Select(element => serialize(element, ctx))) }
                    });
                },
                (jo, deserialize, ctx) =>
                {
                    return ((Json.ObjectExpression)jo).Members.Let(m =>
                    {
                        var elementType = (Type)deserialize(m["Type"], ctx);
                        var kind = (string)deserialize(m["Kind"], ctx);
                        var elements = ((Json.ArrayExpression)m["Elements"]).Elements.Select(e => deserialize(e, ctx));

                        var makeListOfT = ((MethodInfo)ReflectionHelpers.InfoOf(() => MakeList<int>(null, ""))).GetGenericMethodDefinition();
                        return (IList)makeListOfT.MakeGenericMethod(elementType).Invoke(obj: null, new object[] { elements, kind });
                    });
                }
            );
        }

#pragma warning restore IDE0034 // Simplify 'default' expression

        /// <summary>
        /// Helper method to instantiate a list with homogenous element types.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="elements">Elements to add to the result list.</param>
        /// <param name="kind">Kind of list to return.</param>
        /// <returns>List of the requested kind with the given elements.</returns>
        private static IList MakeList<T>(IEnumerable<object> elements, string kind)
        {
            var list = elements.Cast<T>();

            return kind switch
            {
                "Array" => list.ToArray(),
                "List" => list.ToList(),
                "ReadOnlyList" => list.ToList().AsReadOnly(),
                _ => throw new NotSupportedException("Unknown list kind."),
            };
        }

        #endregion
    }
}
