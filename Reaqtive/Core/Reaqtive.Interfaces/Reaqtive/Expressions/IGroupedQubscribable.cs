// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Expressions
{
    /// <summary>
    /// Represents a grouped subscribable source with an expression tree representation.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TSource">Type of the elements produced by the subscribable source.</typeparam>
    public interface IGroupedQubscribable<out TKey, out TSource> : IGroupedSubscribable<TKey, TSource>, IQubscribable<TSource>
    {
    }
}
