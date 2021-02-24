// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

using System;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices.TypeSystem
{
    [TestClass]
    public class TypeToTypeSlimConverterTests
    {
        [TestMethod]
        public void TypeToTypeSlimConverter_Mscorlib()
        {
            var ttsc = new TypeToTypeSlimConverter();
            var tstc = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);

            foreach (var t in typeof(int).Assembly.GetTypes().Where(t => t.Namespace == "System"))
            {
                Roundtrip(ttsc, tstc, t);
            }
        }

        [TestMethod]
        public void TypeToTypeSlimConverter_System()
        {
            var ttsc = new TypeToTypeSlimConverter();
            var tstc = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);

            foreach (var t in typeof(Uri).Assembly.GetTypes().Where(t => t.Namespace == "System"))
            {
                Roundtrip(ttsc, tstc, t);
            }
        }

        private static void Roundtrip(TypeToTypeSlimConverter ttsc, TypeSlimToTypeConverter tstc, global::System.Type t)
        {
            var slim = ttsc.Visit(t);

            var exp = t.IsGenericTypeDefinition ? TypeSlimKind.GenericDefinition : TypeSlimKind.Simple;
            Assert.AreEqual(exp, slim.Kind);

            var simple = slim as SimpleTypeSlimBase;
            Assert.IsNotNull(simple, t.Name);

            Assert.AreEqual(t.FullName, simple.Name);

            var asm = simple.Assembly;
            Assert.IsNotNull(asm, t.Name);

            Assert.AreEqual(t.Assembly.FullName, asm.Name);

            var res = tstc.Visit(slim);
            Assert.AreSame(t, res);
        }
    }
}
