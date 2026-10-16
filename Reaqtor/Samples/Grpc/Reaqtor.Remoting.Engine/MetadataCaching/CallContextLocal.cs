// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

// TODO: Relocate to IRP Core and change namespace.

namespace Reaqtor.Remoting.QueryEvaluator;

//
// ADAPTATION (plan §2.5, adaptation #4): the archived CallContextLocal<T> stored its per-logical-call-context
// Lazy<T> in a GUID-named slot via System.Runtime.Remoting.Messaging.CallContext (GetData / SetData /
// FreeNamedDataSlot). System.Runtime.Remoting does not exist on net10.0, so the slot is reimplemented over
// System.Threading.AsyncLocal<Lazy<T>>, which flows the value across the logical async/call context with the
// same per-context isolation. Because each AsyncLocal<T> field is already its own independent slot, the named
// "_slot"/Guid + FreeNamedDataSlot machinery is dropped: Clear() simply resets the AsyncLocal value back to
// null. The public surface (Value, Clear) and the lazy-init semantics that LeveledCacheQueryableDictionary
// relies on are preserved exactly.
//

/// <summary>
/// Provides call context local storage using a lazy initialization pattern.
/// </summary>
/// <typeparam name="T">Type of the object made available to the logical call context.</typeparam>
internal class CallContextLocal<T>
{
    /// <summary>
    /// Value factory invoked to instantiate the object stored in the call context local storage slot.
    /// Notice this delegate is invoked under the initialization lock. As such, care should be taken about locks within the delegate (cf. Lazy&lt;T&gt; and ThreadLocal&lt;T&gt;).
    /// </summary>
    private readonly Func<T> _valueFactory;

    /// <summary>
    /// Initialization lock, used to protect the write to the logical slot.
    /// Notice a CallContext can be concurrently accessed on many threads that belong to the same context.
    /// </summary>
    private readonly Lock _gate;

    /// <summary>
    /// Underlying call context local storage slot, flowing across the logical async/call context.
    /// </summary>
    private readonly AsyncLocal<Lazy<T>> _slot = new();

    /// <summary>
    /// Creates a lazily initialized call context local storage location, using the specified value factory.
    /// </summary>
    /// <param name="valueFactory">Value factory invoked to instantiate the object stored in the call context local storage slot.</param>
    public CallContextLocal(Func<T> valueFactory)
    {
        _valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
        _gate = new Lock();
    }

    /// <summary>
    /// Gets the object stored in the call context local storage slot.
    /// If the object hasn't been created yet, the factory is invoked.
    /// </summary>
    public T Value
    {
        get
        {
            // Using Lazy<T> as a handy container for the value, without having to worry about
            // the use of a sentinel null value or caching of exceptions.
            var value = _slot.Value;

            if (value == null)
            {
                lock (_gate)
                {
                    value = _slot.Value;

                    if (value == null)
                    {
                        value = new Lazy<T>(_valueFactory);
                        _slot.Value = value;
                    }
                }
            }

            return value.Value;
        }
    }

    /// <summary>
    /// Clears the call context local storage slot.
    /// </summary>
    public void Clear()
    {
        lock (_gate)
        {
            _slot.Value = null;
        }
    }
}
