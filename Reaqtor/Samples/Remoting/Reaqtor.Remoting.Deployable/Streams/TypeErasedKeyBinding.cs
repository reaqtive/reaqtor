// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public abstract class TypeErasedKeyBinding<TElement>
    {
        // Seals this class outside of the assembly
        internal TypeErasedKeyBinding() { }

        public abstract Type KeyType { get; }
    }
}
