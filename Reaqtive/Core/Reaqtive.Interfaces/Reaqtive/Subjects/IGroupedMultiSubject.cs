// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents a subject that supports multiple observers and has a key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key associated with the subject.</typeparam>
    /// <typeparam name="TSource">Type of the elements processed by the subject.</typeparam>
    public interface IGroupedMultiSubject<TKey, TSource> : IGroupedSubscribable<TKey, TSource>, IMultiSubject<TSource>, IDisposable
    {
    }
}
