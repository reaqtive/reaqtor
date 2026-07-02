// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// NB: These helpers were retired from Common/TestUtilities/AssertEx.cs during the .NET 10
//     modernization: MSTest 4's Assert.ThrowsExactly{Async}<T> returns the caught exception,
//     which supersedes the callback-based pattern (all live call sites were rewritten to
//     `var ex = Assert.ThrowsExactly<T>(...);` followed by the assertions). They are kept
//     here because archived test projects still reference them. If an archived project is
//     resurrected, include this file alongside the linked AssertEx.cs (the class is partial).
//

using System;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static partial class AssertEx
    {
        public static void ThrowsException<T>(Action action, Action<T> assert)
            where T : Exception
        {
            var hasThrown = false;

            try
            {
                action();
            }
            catch (T ex)
            {
                if (typeof(T) != ex.GetType())
                    Assert.Fail();

                hasThrown = true;
                assert(ex);
            }

            Assert.IsTrue(hasThrown);
        }

        public static void ThrowsException<T>(Func<object> action, Action<T> assert)
            where T : Exception
        {
            ThrowsException(() => { _ = action(); }, assert);
        }

        public static async Task ThrowsExceptionAsync<T>(Func<Task> action, Action<T> assert)
            where T : Exception
        {
            try
            {
                await action();
            }
            catch (T ex)
            {
                if (typeof(T) != ex.GetType())
                    Assert.Fail();

                assert(ex);
                return;
            }

            Assert.Fail();
        }
    }
}
