// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Created this type.
//

namespace System.Memory
{
    /// <summary>
    /// An interface for disposable references to objects.
    /// </summary>
    /// <typeparam name="T">Type of the referenced object.</typeparam>
    public interface IDiscardable<T> : IReference<T>, IDisposable
    {
    }
}
