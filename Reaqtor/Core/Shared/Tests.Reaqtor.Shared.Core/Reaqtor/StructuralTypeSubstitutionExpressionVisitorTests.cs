// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Shared.Core.Reaqtor
{
    [TestClass]
    public class StructuralTypeSubstitutionExpressionVisitorTests
    {
        [TestMethod]
        public void StructuralTypeSubstitutionExpressionVisitor_IntToLongMappingMissing_ThrowsInvalidOperation()
        {
            var type1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(long) }
            }, valueEquality: true);

            var expr = Expression.Constant(Activator.CreateInstance(type1));

            var visitor = new StructuralTypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { type1, type2 } });

            Assert.ThrowsException<InvalidOperationException>(() => visitor.Visit(expr));
        }

        [TestMethod]
        public void StructuralTypeSubstitutionExpressionVisitor_Record_MissingProperty_ThrowsInvalidOperation()
        {
            var type1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Bar", typeof(long) }
            }, valueEquality: true);

            var expr = Expression.Constant(Activator.CreateInstance(type1));

            var visitor = new StructuralTypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { type1, type2 } });

            Assert.ThrowsException<InvalidOperationException>(() => visitor.Visit(expr));
        }

        [TestMethod]
        public void StructuralTypeSubstitutionExpressionVisitor_Anonymous_MissingProperty_ThrowsInvalidOperation()
        {
            var type1 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            });

            var type2 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Bar", typeof(int) }
            });

            var expr = Expression.Constant(Activator.CreateInstance(type1, 0));

            var visitor = new StructuralTypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { type1, type2 } });

            Assert.ThrowsException<InvalidOperationException>(() => visitor.Visit(expr));
        }

        [TestMethod]
        public void StructuralTypeSubstitutionExpressionVisitor_RecordToAnonymous_ThrowsInvalidOperation()
        {
            var type1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type2 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            });

            var expr = Expression.Constant(Activator.CreateInstance(type1));

            var visitor = new StructuralTypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { type1, type2 } });

            Assert.ThrowsException<InvalidOperationException>(() => visitor.Visit(expr));
        }

        [TestMethod]
        public void StructuralTypeSubstitutionExpressionVisitor_Record_Success()
        {
            var type1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var expr = Expression.Constant(Activator.CreateInstance(type1));

            var visitor = new StructuralTypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { type1, type2 } });

            var result = visitor.Visit(expr);

            Assert.AreEqual(type2, result.Type);
        }

        [TestMethod]
        public void StructuralTypeSubstitutionExpressionVisitor_Anonymous_Success()
        {
            var type1 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            });

            var type2 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            });

            var expr = Expression.Constant(Activator.CreateInstance(type1, 0));

            var visitor = new StructuralTypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { type1, type2 } });

            var result = visitor.Visit(expr);

            Assert.AreEqual(type2, result.Type);
        }

        [TestMethod]
        public void StructuralTypeSubstitutionExpressionVisitor_NullValue_Success()
        {
            var type1 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            });

            var type2 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            });

            var expr = Expression.Constant(null, type1);

            var visitor = new StructuralTypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { type1, type2 } });

            var result = visitor.Visit(expr);

            Assert.AreEqual(type2, result.Type);
        }
    }
}
