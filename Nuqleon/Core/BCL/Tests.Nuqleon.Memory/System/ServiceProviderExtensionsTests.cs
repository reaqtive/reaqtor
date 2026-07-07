// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/06/2015 - Wrote these tests.
//

namespace Tests;

// CA2263 (prefer the generic overload) is intentional here: this test deliberately exercises BOTH the
// generic GetService<T>() and the non-generic GetService(Type) extension overloads to verify they behave
// identically, so the typeof(object) call must stay to cover the Type-based overload (using the generic
// form would make it a duplicate of the line above and drop that coverage).
#pragma warning disable CA2263

[TestClass]
public class ServiceProviderExtensionsTests
{
    [TestMethod]
    public void ServiceProviderExtensions_GetServiceOfT()
    {
        var f = new Foo();
        Assert.AreEqual(42, f.GetService<int>());

        var o = (object)f;
        Assert.AreEqual(42, o.GetService<int>());

        Assert.IsNull("".GetService<object>());
        Assert.IsNull("".GetService(typeof(object)));
    }

    private sealed class Foo : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(int))
            {
                return 42;
            }

            return null;
        }
    }
}
