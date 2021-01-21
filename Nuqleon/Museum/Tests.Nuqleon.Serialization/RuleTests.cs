// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014
//

using Nuqleon.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Microsoft.Serialization
{
    [TestClass]
    public class RuleTests
    {
        [TestMethod]
        public void Rule_Simple()
        {
            var rule = new MyRule("foo");

            Assert.AreEqual("foo", rule.Name);
            Assert.AreEqual(typeof(int), rule.InputType);
        }

        [TestMethod]
        public void Rule_Filtered()
        {
            var rule = new MyFilteredRule("foo", _ => true);

            Assert.AreEqual("foo", rule.Name);
            Assert.AreEqual(typeof(D), rule.InputType);
        }

        private sealed class MyRule : Rule<int, int, object>
        {
            public MyRule(string name)
                : base(name)
            {
            }

            public override bool TrySerialize(int input, Func<int, object, int> recurse, object context, out int result)
            {
                throw new NotImplementedException();
            }

            public override int Deserialize(int output, Func<int, object, int> recurse, object context)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MyFilteredRule : FilteredRuleBase<B, string, object, D>
        {
            public MyFilteredRule(string name, Func<D, bool> filter)
                : base(name, filter)
            {
            }

            protected override string SerializeCore(D input, Func<B, object, string> recurse, object context)
            {
                throw new NotImplementedException();
            }

            public override B Deserialize(string output, Func<string, object, B> recurse, object context)
            {
                throw new NotImplementedException();
            }
        }


        private class B
        {
        }

        private sealed class D : B
        {
        }
    }
}
