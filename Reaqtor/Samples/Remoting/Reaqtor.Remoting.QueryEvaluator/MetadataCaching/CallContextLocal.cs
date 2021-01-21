// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Remoting.Messaging;

// TODO: Relocate to IRP Core and change namespace.

namespace Reaqtor.Remoting.QueryEvaluator
{
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
        private readonly object _gate;

        /// <summary>
        /// Name for the underlying call context slot, allowing declaration of multiple call context local storage slots.
        /// </summary>
        private readonly string _slot = "<>__LazyCallContextLocal_" + Guid.NewGuid().ToString();

        /// <summary>
        /// Creates a lazily initialized call context local storage location, using the specified value factory.
        /// </summary>
        /// <param name="valueFactory">Value factory invoked to instantiate the object stored in the call context local storage slot.</param>
        public CallContextLocal(Func<T> valueFactory)
        {
            _valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
            _gate = new object();
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
                var value = (Lazy<T>)CallContext.GetData(_slot);

                if (value == null)
                {
                    lock (_gate)
                    {
                        value = (Lazy<T>)CallContext.GetData(_slot);

                        if (value == null)
                        {
                            value = new Lazy<T>(_valueFactory);
                            CallContext.SetData(_slot, value);
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
                CallContext.FreeNamedDataSlot(_slot);
            }
        }
    }
}
