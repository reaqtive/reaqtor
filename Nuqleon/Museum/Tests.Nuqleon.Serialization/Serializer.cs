// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace Tests.Microsoft.Serialization
{
    [TestClass]
    public class Serializer
    {
        [TestMethod]
        public void SerializerBase_Constructor_NullArguments()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new Serializer<int, string, object>(tag: null, _ => new TaggedByRule<string>(), () => "ctx", _ => "a", s => new Contextual<object, string>(), _ => Expression.Constant(0), (x, y) => Expression.Constant(0)), ex => Assert.AreEqual(ex.ParamName, "tag"));
            AssertEx.ThrowsException<ArgumentNullException>(() => new Serializer<int, string, object>(_ => "a", untag: null, () => "ctx", c => "a", s => new Contextual<object, string>(), d => Expression.Constant(0), (e, s) => Expression.Constant(0)), ex => Assert.AreEqual(ex.ParamName, "untag"));
            AssertEx.ThrowsException<ArgumentNullException>(() => new Serializer<int, string, object>(_ => "a", _ => new TaggedByRule<string>(), newContext: null, c => "a", s => new Contextual<object, string>(), d => Expression.Constant(0), (e, s) => Expression.Constant(0)), ex => Assert.AreEqual(ex.ParamName, "newContext"));
            AssertEx.ThrowsException<ArgumentNullException>(() => new Serializer<int, string, object>(_ => "a", s => new TaggedByRule<string>(), () => "ctx", addContext: null, s => new Contextual<object, string>(), d => Expression.Constant(0), (e, s) => Expression.Constant(0)), ex => Assert.AreEqual(ex.ParamName, "addContext"));
            AssertEx.ThrowsException<ArgumentNullException>(() => new Serializer<int, string, object>(_ => "a", s => new TaggedByRule<string>(), () => "ctx", c => "a", getContext: null, d => Expression.Constant(0), (e, s) => Expression.Constant(0)), ex => Assert.AreEqual(ex.ParamName, "getContext"));
            AssertEx.ThrowsException<ArgumentNullException>(() => new SerializerBase<int, string, object>(_ => "a", s => new TaggedByRule<string>(), () => "ctx", c => "a", s => new Contextual<object, string>(), ruleTable: null), ex => Assert.AreEqual(ex.ParamName, "ruleTable"));
        }
    }
}
