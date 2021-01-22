// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Reaqtive;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public class Versioning
    {
        [TestMethod]
        public void UniqueTags()
        {
            var operators = (from t in typeof(Subscribable).Assembly.GetTypes()
                             let b = t.BaseType
                             where b != null && b.IsGenericType
                             let d = b.IsGenericTypeDefinition ? b : b.GetGenericTypeDefinition()
                             where d == typeof(SubscribableBase<>)
                             select t)
                            .ToArray();

            var sinks = (from o in operators
                         from i in o.GetNestedTypes(BindingFlags.NonPublic)
                         where typeof(IVersioned).IsAssignableFrom(i)
                         where !i.IsAbstract
                         let j = i.IsGenericTypeDefinition ? i.MakeGenericType(i.GetGenericArguments().Select(_ => typeof(object)).ToArray()) : i
                         let v = (IVersioned)FormatterServices.GetSafeUninitializedObject(j)
                         select (Operator: o, Sink: i, v.Version, v.Name))
                        .ToArray();

            var minVer = new Version(1, 0, 0, 0);

            var tooOld = sinks.Where(s => s.Version < minVer).ToList();

            if (tooOld.Count > 0)
            {
                Assert.Fail("Operators with version < " + minVer + " detected: " + string.Join(", ", tooOld.Select(t => t.Name)));
            }

            var conflictingNames = sinks.GroupBy(g => g.Name).Where(g => g.Count() > 1).ToList();

            if (conflictingNames.Count > 0)
            {
                Assert.Fail("Operators with conflicting names detected: " + string.Join(", ", conflictingNames.Select(t => t.Key)));
            }
        }
    }
}
