// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public class PureMemberCatalogTests
    {
        private static readonly Dictionary<Type, HashSet<object>> s_values = new()
        {
            // TODO: Add more sample values for various types.

            { typeof(object), new HashSet<object> { false, 42, 42L, 3.14, 3.14f, "foo", 'c' } },

            { typeof(bool), new HashSet<object> { false, true } },
            { typeof(sbyte), new HashSet<object> { (sbyte)0, (sbyte)1, (sbyte)42 } },
            { typeof(byte), new HashSet<object> { (byte)0, (byte)1, (byte)42 } },
            { typeof(short), new HashSet<object> { (short)0, (short)1, (short)42 } },
            { typeof(ushort), new HashSet<object> { (ushort)0, (ushort)1, (ushort)42 } },
            { typeof(int), new HashSet<object> { 0, 1, 42 } },
            { typeof(uint), new HashSet<object> { 0U, 1U, 42U } },
            { typeof(long), new HashSet<object> { 0L, 1L, 42L } },
            { typeof(ulong), new HashSet<object> { 0UL, 1UL, 42UL } },
            { typeof(float), new HashSet<object> { 0f, 1f, float.NaN } },
            { typeof(double), new HashSet<object> { 0.0, 1.0, Math.PI } },
            { typeof(decimal), new HashSet<object> { 0.0m, 19.95m } },
            { typeof(string), new HashSet<object> { "", "bar" } },
            { typeof(char), new HashSet<object> { '0', 'a' } },

            { typeof(Guid), new HashSet<object> { Guid.Empty, Guid.NewGuid() } },
            { typeof(TimeSpan), new HashSet<object> { TimeSpan.Zero, new TimeSpan(3, 14, 15) } },
            { typeof(DateTime), new HashSet<object> { new DateTime(1983, 2, 11, 3, 14, 15) } },
            { typeof(DateTimeOffset), new HashSet<object> { new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(-8)) } },
            { typeof(Version), new HashSet<object> { new Version(1, 2, 3, 4) } },
            { typeof(Uri), new HashSet<object> { new Uri("http://www.bing.com") } },

            { typeof(DateTimeKind), new HashSet<object>(Enum.GetValues(typeof(DateTimeKind)).Cast<object>()) },
            { typeof(MidpointRounding), new HashSet<object>(Enum.GetValues(typeof(MidpointRounding)).Cast<object>()) },
            { typeof(NormalizationForm), new HashSet<object>(Enum.GetValues(typeof(NormalizationForm)).Cast<object>()) },
            { typeof(UriKind), new HashSet<object>(Enum.GetValues(typeof(UriKind)).Cast<object>()) },
            { typeof(UriComponents), new HashSet<object>(Enum.GetValues(typeof(UriComponents)).Cast<object>()) },
            { typeof(UriFormat), new HashSet<object>(Enum.GetValues(typeof(UriFormat)).Cast<object>()) },
            { typeof(UriPartial), new HashSet<object>(Enum.GetValues(typeof(UriPartial)).Cast<object>()) },

            { typeof(Base64FormattingOptions), new HashSet<object>(Enum.GetValues(typeof(Base64FormattingOptions)).Cast<object>()) },

            { typeof(Array), new HashSet<object> { Array.Empty<int>(), Array.Empty<string>(), new int[] { 1 }, new int[] { 1, 2, 3, 4, 5 } } }, // TODO: Add multi-dimensional arrays
        };

        [TestMethod]
        public void PureMemberCatalog_TestTheTests()
        {
            Assert.ThrowsException<AssertFailedException>(() =>
            {
                AssertNonGeneric(typeof(Guid).GetMethod(nameof(Guid.NewGuid)));
            });

            Assert.ThrowsException<AssertFailedException>(() =>
            {
                AssertNonGeneric(typeof(Environment).GetProperty(nameof(Environment.TickCount)));
            });
        }

        [TestMethod]
        public void PureMemberCatalog_All()
        {
            // NB: Excluding entries for System.Text.RegularExpressions because equality checks of Regex instances fail,
            //     so we can't assert value equality comparison of Regex instances.

            var catalog = new MemberTable
            {
                PureMemberCatalog.System.AllThisNamespaceOnly,
                PureMemberCatalog.System.Collections.Generic.AllThisNamespaceOnly,
            }.ToReadOnly();

            foreach (var member in catalog)
            {
                try
                {
                    if (!member.DeclaringType.IsGenericType)
                    {
                        AssertNonGeneric(member);
                    }
                    else
                    {
                        AssertGeneric(member);
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(member.ToString() + " --> " + ex.Message);
                }
            }
        }

        [TestMethod]
        public void PureMemberCatalog_Regex()
        {
            Expression<Func<string>> f = () => new Regex("[0-9]([a-z]*)[0-9]").Match("1bar2").Groups[1].Value;
            AssertOptimize(f.Body, Expression.Constant("bar"));
        }

        private static void AssertNonGeneric(MemberInfo member)
        {
            switch (member)
            {
                case MethodInfo method:
                    if (method.IsGenericMethod)
                    {
                        AssertGeneric(method);
                    }
                    else
                    {
                        AssertNonGeneric(method);
                    }
                    break;
                case FieldInfo field:
                    AssertNonGeneric(field);
                    break;
                case PropertyInfo property:
                    AssertNonGeneric(property);
                    break;
                case ConstructorInfo constructor:
                    AssertNonGeneric(constructor);
                    break;
            }
        }

        private static void AssertGeneric(MemberInfo member)
        {
            // TODO
            _ = member;
        }

        private static void AssertGeneric(MethodInfo method)
        {
            // NB: Assumes there are no constraints on the generic parameters.

            var arity = method.GetGenericArguments().Length;

            var args = Repeat(s_values.Keys).Take(arity).ToArray();

            var closedMethod = method.GetGenericMethodDefinition().MakeGenericMethod(args);

            // CONSIDER: Multiple iterations for different generic closed forms?

            AssertNonGeneric(closedMethod);
        }

        private static void AssertNonGeneric(MethodInfo method)
        {
            var parameters = method.GetParameters();

            if (method.IsStatic)
            {
                var types = parameters.Select(p => p.ParameterType).ToArray();
                var valuess = GetValues(types);

                foreach (var values in valuess)
                {
                    var expected = Eval(method, instance: null, values);

                    var argExprs = parameters.Zip(values, (p, v) => Expression.Constant(v, p.ParameterType)).ToArray();
                    var callExpr = Expression.Call(method, argExprs);

                    AssertOptimize(callExpr, expected);
                }
            }
            else
            {
                var instanceType = method.DeclaringType;

                var types = new[] { instanceType }.Concat(parameters.Select(p => p.ParameterType)).ToArray();
                var valuess = GetValues(types);

                foreach (var values in valuess)
                {
                    var expected = Eval(method, values[0], values.Skip(1).ToArray());

                    var instExpr = Expression.Constant(values[0], instanceType);
                    var argExprs = parameters.Zip(values.Skip(1), (p, v) => Expression.Constant(v, p.ParameterType)).ToArray();
                    var callExpr = Expression.Call(instExpr, method, argExprs);

                    AssertOptimize(callExpr, expected);
                }
            }
        }

        private static void AssertNonGeneric(FieldInfo field)
        {
            if (field.IsStatic)
            {
                var value = field.GetValue(obj: null);
                var expected = Expression.Constant(value, field.FieldType);

                var fieldExpr = Expression.Field(expression: null, field);

                AssertOptimize(fieldExpr, expected);
            }
            else
            {
                var instanceType = field.DeclaringType;

                foreach (var instance in GetValues(instanceType))
                {
                    var value = field.GetValue(instance);
                    var expected = Expression.Constant(value, field.FieldType);

                    var instanceExpr = Expression.Constant(instance, instanceType);
                    var fieldExpr = Expression.Field(instanceExpr, field);

                    AssertOptimize(fieldExpr, expected);
                }
            }
        }

        private static void AssertNonGeneric(PropertyInfo property)
        {
            var getMethod = property.GetGetMethod();

            AssertNonGeneric(getMethod);

            if (getMethod.IsStatic)
            {
                var expected = Eval(property, instance: null, Array.Empty<object>());

                var propertyExpr = Expression.Property(expression: null, property);

                AssertOptimize(propertyExpr, expected);
            }
            else
            {
                var indexParameters = property.GetIndexParameters();
                if (indexParameters.Length > 0)
                {
                    AssertNonGenericIndexer(property);
                }
                else
                {
                    var instanceType = property.DeclaringType;

                    foreach (var instance in GetValues(instanceType))
                    {
                        var expected = Eval(property, instance, Array.Empty<object>());

                        var instanceExpr = Expression.Constant(instance, instanceType);
                        var propertyExpr = Expression.Property(instanceExpr, property);

                        AssertOptimize(propertyExpr, expected);
                    }
                }
            }
        }

        private static void AssertNonGenericIndexer(PropertyInfo property)
        {
            var parameters = property.GetIndexParameters();

            var instanceType = property.DeclaringType;

            var types = new[] { instanceType }.Concat(parameters.Select(p => p.ParameterType)).ToArray();
            var valuess = GetValues(types);

            foreach (var values in valuess)
            {
                var expected = Eval(property, values[0], values.Skip(1).ToArray());

                var instExpr = Expression.Constant(values[0], instanceType);
                var argExprs = parameters.Zip(values.Skip(1), (p, v) => Expression.Constant(v, p.ParameterType)).ToArray();
                var indexExpr = Expression.MakeIndex(instExpr, property, argExprs);

                AssertOptimize(indexExpr, expected);
            }
        }

        private static void AssertNonGeneric(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            var instanceType = constructor.DeclaringType;

            var types = parameters.Select(p => p.ParameterType).ToArray();
            var valuess = GetValues(types);

            foreach (var values in valuess)
            {
                var expected = Eval(constructor, values);

                var argExprs = parameters.Zip(values, (p, v) => Expression.Constant(v, p.ParameterType)).ToArray();
                var newExpr = Expression.New(constructor, argExprs);

                AssertOptimize(newExpr, expected);
            }
        }

        private static void AssertOptimize(Expression original, Expression expected)
        {
            var opt = new ExpressionOptimizer(new MetadataSemanticProvider { PureMembers = PureMemberCatalog.All, ConstParameters = ConstParameterCatalog.All, ImmutableTypes = ImmutableTypeCatalog.All }, new DefaultEvaluatorFactory());
            ExpressionOptimizerTests.AssertOptimized(opt, original, expected);
        }

        private static Expression Eval(PropertyInfo property, object instance, object[] args)
        {
            object obj;

            try
            {
                obj = property.GetValue(instance, args);
            }
            catch (TargetInvocationException tie)
            {
                return Expression.Throw(Expression.Constant(tie.InnerException), property.PropertyType);
            }

            return Expected(property.PropertyType, obj);
        }

        private static Expression Eval(MethodInfo method, object instance, object[] args)
        {
            object obj;

            try
            {
                obj = method.Invoke(instance, args);
            }
            catch (TargetInvocationException tie)
            {
                return Expression.Throw(Expression.Constant(tie.InnerException), method.ReturnType);
            }

            return Expected(method.ReturnType, obj);
        }

        private static Expression Eval(ConstructorInfo constructor, object[] args)
        {
            object obj;

            try
            {
                obj = constructor.Invoke(args);
            }
            catch (TargetInvocationException tie)
            {
                return Expression.Throw(Expression.Constant(tie.InnerException), constructor.DeclaringType);
            }

            return Expected(constructor.DeclaringType, obj);
        }

        private static IEnumerable<object> GetValues(Type type)
        {
            if (s_values.TryGetValue(type, out var values))
            {
                return values;
            }
            else
            {
                if (type.IsArray)
                {
                    var elementType = type.GetElementType();

                    var elements = GetValues(elementType);
                    var castArray = s_castArray.MakeGenericMethod(elementType);

                    var random = new Random(1983);

                    var arrays = Enumerable.Range(0, 16).Select(n => Repeat(elements).Take(n).OrderBy(_ => random.Next()).ToArray());

                    var res = arrays.Select(a => castArray.Invoke(obj: null, new[] { a })).ToArray();

                    return res;
                }

                throw new InvalidOperationException($"Need example values for '{type}'.");
            }
        }

        private static IEnumerable<object[]> GetValues(params Type[] types)
        {
            var res = (IEnumerable<IEnumerable<object>>)new[] { Array.Empty<object>() };

            foreach (var type in types)
            {
                var values = GetValues(type);

                res = res.SelectMany(prev => values.Select(next => prev.Concat(new[] { next })));
            }

            return Sample(res).Select(values => values.ToArray());
        }

        private static Expression Expected(Type type, object value)
        {
            if (type == typeof(void))
            {
                return Expression.Empty();
            }
            else
            {
                return Expression.Constant(value, type);
            }
        }

        private static IEnumerable<T> Repeat<T>(IEnumerable<T> values)
        {
            while (true)
            {
                foreach (var value in values)
                {
                    yield return value;
                }
            }
        }

        private static readonly MethodInfo s_castArray = ((MethodInfo)ReflectionHelpers.InfoOf(() => CastArray<object>(null))).GetGenericMethodDefinition();

        private static T[] CastArray<T>(object[] array) => array.Cast<T>().ToArray();

        private static IEnumerable<T> Sample<T>(IEnumerable<T> values)
        {
            var rand = new Random(1983);
            var MAX = 16;
            return values.OrderBy(_ => rand.Next()).Take(MAX);
        }
    }
}
