// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

namespace System
{
    internal static class ServiceProviderExtensions
    {
        internal static T GetService<T>(this IServiceProvider provider) => (T)provider.GetService(typeof(T));

        internal static object GetService(this object obj, Type serviceType) => obj is IServiceProvider sp ? sp.GetService(serviceType) : null;

        internal static T GetService<T>(this object obj) => (T)GetService(obj, typeof(T));
    }
}
