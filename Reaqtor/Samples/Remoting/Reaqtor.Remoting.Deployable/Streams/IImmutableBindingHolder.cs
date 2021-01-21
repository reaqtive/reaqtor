// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public interface IImmutableBindingHolder<THolder, TSource>
    {
        THolder AppendBindings(IList<TypeErasedKeyBinding<TSource>> bindings);

        IReadOnlyList<TypeErasedKeyBinding<TSource>> Bindings
        {
            get;
        }
    }
}
