// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Playground
{
    class Program
    {
        static void Main()
        {
            var ev = new MyEvaluator();

            foreach (var e in new Expression[]
            {
                Expression.Constant(1),
                Expression.Add(Expression.Constant(1), Expression.Constant(2)),
                ((Expression<Func<int>>)(() => "foo".Length)).Body,
                ((Expression<Func<int>>)(() => "foo".ToUpper().Length)).Body,
                ((Expression<Func<int>>)(() => "foo".Substring(1, 2).Length)).Body,
                ((Expression<Func<string>>)(() => "foo".ToUpper())).Body,
                ((Expression<Func<List<int>>>)(() => new List<int> { 1, 2, "bar".Length })).Body,
                ((Expression<Func<Bar>>)(() => new Bar { Foo = "bar".Length, Qux = { int.Parse("42") }, Baz = "foo".Length })).Body,
                ((Expression<Func<int>>)(() => Enumerable.Range(0, 10).Sum())).Body,
                ((Expression<Func<int>>)(() => Enumerable.Range(0, 10).Select(x => x).Sum())).Body,
                ((Expression<Func<int>>)(() => new[] { 1, 2, 3 }.Sum())).Body,
                ((Expression<Func<string>>)(() => Console.ReadLine())).Body, // side-effect
            })
            {
                var r = ev.Reduce(e);
                Console.WriteLine(e + " --> " + r);
            }
        }
    }

    class Bar
    {
        public Bar()
        {
            Qux = new List<int>();
        }

        public int Foo { get; set; }
        public List<int> Qux { get; set; }
        public int Baz { get; set; }
    }

    class MyEvaluator : PartialExpressionEvaluatorBase
    {
        private static readonly HashSet<Type> s_types = new()
        {
            typeof(int),
            typeof(string),
            typeof(Bar),
            //typeof(Func<int, int>),
        };

        protected override bool CanEvaluate(ConstantExpression node)
        {
            return s_types.Contains(node.Type);
        }

        protected override bool CanEvaluate(Type type)
        {
            return s_types.Contains(type);
        }

        protected override bool CanEvaluate(MethodInfo method)
        {
            if (method.DeclaringType == typeof(Console))
            {
                return false;
            }
            
            return true;
        }

        protected override bool CanEvaluate(ConstructorInfo constructor)
        {
            return true;
        }

        protected override bool CanEvaluate(PropertyInfo property)
        {
            if (property.DeclaringType == typeof(Bar) && property.Name == "Baz")
            {
                return false;
            }

            return true;
        }

        protected override bool CanEvaluate(FieldInfo field)
        {
            return true;
        }

        protected override Expression EvaluateCore(Expression e)
        {
            // TODO: error handling
            return Expression.Constant(Expression.Lambda(e).Compile().DynamicInvoke(), e.Type);
        }
    }
}
