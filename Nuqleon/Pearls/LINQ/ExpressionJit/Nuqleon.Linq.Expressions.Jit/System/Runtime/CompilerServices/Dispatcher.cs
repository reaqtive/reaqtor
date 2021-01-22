// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Base class for dispatchers.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate exposed by the thunk.</typeparam>
    /// <typeparam name="TClosure">The type of the closure parameter.</typeparam>
    /// <typeparam name="TInner">The type of the internal delegate used by the thunk. This delegate differs from <typeparamref name="TDelegate"/> by having an additional first parameter of type <typeparamref name="TClosure"/>.</typeparam>
    public class Dispatcher<TDelegate, TClosure, TInner> : FunctionContext<TClosure>
    {
        //
        // NB: We adding constructors, don't forget to update the GetConstructors() logic in the runtime
        //     thunk type compiler to ensure it picks the right one when emitting IL code.
        //

#pragma warning disable CA1051 // Do not declare visible instance field. (Usage of field is by design.)

        /// <summary>
        /// Gets the parent thunk.
        /// </summary>
        public Thunk<TDelegate, TClosure, TInner> Parent;

#pragma warning restore CA1051
    }
}
