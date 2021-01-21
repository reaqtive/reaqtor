// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class GenericTypeSlimTests : TestBase
    {
        [TestMethod]
        public void GenericTypeSlim_ArgumentChecks()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(typeDefinition: null, default(TypeSlim)), ex => Assert.AreEqual("typeDefinition", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(typeDefinition: null, default(TypeSlim[])), ex => Assert.AreEqual("typeDefinition", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(typeDefinition: null, default(ReadOnlyCollection<TypeSlim>)), ex => Assert.AreEqual("typeDefinition", ex.ParamName));

            var typ = typeof(int).ToTypeSlim();
            var def = TypeSlim.GenericDefinition(SlimType.Assembly, "Foo");

            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(def, default(TypeSlim[])), ex => Assert.AreEqual("arguments", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(def, default(ReadOnlyCollection<TypeSlim>)), ex => Assert.AreEqual("arguments", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(def, default(TypeSlim)), ex => Assert.AreEqual("argument1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(def, default(TypeSlim), typ), ex => Assert.AreEqual("argument1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(def, default(TypeSlim), typ, typ), ex => Assert.AreEqual("argument1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Generic(def, default(TypeSlim), typ, typ, typ), ex => Assert.AreEqual("argument1", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void GenericTypeSlim_Optimized1()
        {
            var arg1 = typeof(int).ToTypeSlim();

            var gen1 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<>).ToTypeSlim(), arg1);
            var gen2 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<>).ToTypeSlim(), new[] { arg1 });
            var gen3 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<>).ToTypeSlim(), new ReadOnlyCollection<TypeSlim>(new[] { arg1 }));

            foreach (var gen in new[] { gen1, gen2, gen3 })
            {
                AssertOptimized(gen, 1, arg1);
            }
        }

        [TestMethod]
        public void GenericTypeSlim_Optimized2()
        {
            var arg1 = typeof(int).ToTypeSlim();
            var arg2 = typeof(long).ToTypeSlim();

            var gen1 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,>).ToTypeSlim(), arg1, arg2);
            var gen2 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,>).ToTypeSlim(), new[] { arg1, arg2 });
            var gen3 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,>).ToTypeSlim(), new ReadOnlyCollection<TypeSlim>(new[] { arg1, arg2 }));

            foreach (var gen in new[] { gen1, gen2, gen3 })
            {
                AssertOptimized(gen, 2, arg1, arg2);
            }
        }

        [TestMethod]
        public void GenericTypeSlim_Optimized3()
        {
            var arg1 = typeof(int).ToTypeSlim();
            var arg2 = typeof(long).ToTypeSlim();
            var arg3 = typeof(byte).ToTypeSlim();

            var gen1 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,,>).ToTypeSlim(), arg1, arg2, arg3);
            var gen2 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,,>).ToTypeSlim(), new[] { arg1, arg2, arg3 });
            var gen3 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,,>).ToTypeSlim(), new ReadOnlyCollection<TypeSlim>(new[] { arg1, arg2, arg3 }));

            foreach (var gen in new[] { gen1, gen2, gen3 })
            {
                AssertOptimized(gen, 3, arg1, arg2, arg3);
            }
        }

        [TestMethod]
        public void GenericTypeSlim_Optimized4()
        {
            var arg1 = typeof(int).ToTypeSlim();
            var arg2 = typeof(long).ToTypeSlim();
            var arg3 = typeof(byte).ToTypeSlim();
            var arg4 = typeof(char).ToTypeSlim();

            var gen1 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,,,>).ToTypeSlim(), arg1, arg2, arg3, arg4);
            var gen2 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,,,>).ToTypeSlim(), new[] { arg1, arg2, arg3, arg4 });
            var gen3 = TypeSlim.Generic((GenericDefinitionTypeSlim)typeof(Func<,,,>).ToTypeSlim(), new ReadOnlyCollection<TypeSlim>(new[] { arg1, arg2, arg3, arg4 }));

            foreach (var gen in new[] { gen1, gen2, gen3 })
            {
                AssertOptimized(gen, 4, arg1, arg2, arg3, arg4);
            }
        }

        private static void AssertOptimized(GenericTypeSlim type, int arity, params TypeSlim[] args)
        {
            Assert.IsTrue(type.GetType().Name.EndsWith(arity.ToString()));

            Assert.AreEqual(arity, type.GenericArgumentCount);

            for (var i = 0; i < args.Length; i++)
            {
                Assert.AreSame(args[i], type.GetGenericArgument(i));
            }

            var genArgs = type.GenericArguments;

            Assert.AreSame(genArgs, type.GenericArguments);

            Assert.AreEqual(arity, genArgs.Count);

            for (var i = 0; i < args.Length; i++)
            {
                Assert.AreSame(args[i], genArgs[i]);
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => type.GetGenericArgument(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => type.GetGenericArgument(arity));
        }
    }
}
