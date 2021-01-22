// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    /// <summary>
    /// Base class for `CombineLatest` sinks of different arities.
    /// </summary>
    /// <typeparam name="TCombineLatest">The type of `CombineLatest` subscribable parent, used to access parameters.</typeparam>
    /// <typeparam name="TResult">The type of result produced by the operator.</typeparam>
    internal abstract class CombineLatestCommon<TCombineLatest, TResult> : StatefulOperator<TCombineLatest, TResult>, ICombineLatest
    {
        //
        // PERF: The original implementation used two bool[] fields to keep track of the "has value"
        //       and "has completed" states on all the observers, paired with a bool field to keep
        //       track of the steady state where all values are present. This led to a lot of memory
        //       overhead that can be avoided by keeping a single bitmask below. In addition, the
        //       states of "any has completed", "all have completed", and "all have a value" were
        //       computed using LINQ `Any` and `All` operators, adding additional CPU overhead.
        //

        /// <summary>
        /// The state flag, as described in the constructor documentation.
        /// </summary>
        private uint _state;

        /// <summary>
        /// Creates a new instance of <see cref="CombineLatestCommon{TCombineLatest, TResult}"/> for a `CombineLatest`
        /// sink implementation with the specified <paramref name="arity"/>.
        /// </summary>
        /// <param name="arity">The arity of the `CombineLatest` operation, i.e. the number of sources.</param>
        /// <param name="params">The parent subscribable sequence, used to access parameters.</param>
        /// <param name="observer">The downstream observer.</param>
        public CombineLatestCommon(int arity, TCombineLatest @params, IObserver<TResult> observer)
            : base(@params, observer)
        {
            Debug.Assert(arity is >= 2 and <= 16, "The bitmask can only support up to 16 sources.");

            //
            // Compute the initial state bit representation. Bits 0..15 denote whether the observer
            // at index 0..15 has received at least one value. Bits 16..31 denote whether the observer
            // at index 0..15 has received a completion message.
            //
            // The mask used below sets the top (16 - arity) bits within the 4 high bytes and the 4
            // low bytes to 1, which makes the common check to see if all observers have received at
            // least one value very easy, i.e. check whether the low 16 bits are all set to 1.
            //
            //              complete        hasvalue
            //  arity   /              \/              \
            //      2   11111111111111001111111111111100
            //      3   11111111111110001111111111111000
            //      4   11111111111100001111111111110000
            //      5   11111111111000001111111111100000
            //      6   11111111110000001111111111000000
            //      7   11111111100000001111111110000000
            //      8   11111111000000001111111100000000
            //      9   11111110000000001111111000000000
            //     10   11111100000000001111110000000000
            //     11   11111000000000001111100000000000
            //     12   11110000000000001111000000000000
            //     13   11100000000000001110000000000000
            //     14   11000000000000001100000000000000
            //     15   10000000000000001000000000000000
            //     16   00000000000000000000000000000000
            //

            var mask = (uint)GetMask(arity);
            _state = ~(mask | (mask << 16));
        }

        /// <summary>
        /// Gets 16 bits where the low <paramref name="arity"/> bits are set to <c>1</c> and the remaining
        /// top bits are set to <c>0</c>.
        /// </summary>
        /// <param name="arity">The arity of the operator.</param>
        /// <returns>A bit mask to select the low bits that denote a valid 2 byte state for the current arity.</returns>
        private static ushort GetMask(int arity) => (ushort)~(0xFFFF << arity);

        /// <summary>
        /// Gets the lock used to synchronize access to the downstream observer and to protected state.
        /// </summary>
        public object Lock { get; } = new object();

        /// <summary>
        /// Gets the arity of the operator.
        /// </summary>
        protected abstract int Arity { get; }

        /// <summary>
        /// Reports that the source observer with the specified <paramref name="index"/> has received a value.
        /// </summary>
        /// <param name="index">The index of the source observer that received a value.</param>
        public void Next(int index)
        {
            //
            // Call the selector function if every slot has a value. Terminate if not every slot has a value,
            // and any source observer has completed (because no combining can take place).
            //

            Debug.Assert(index <= 16);

            lock (Lock)
            {
                _state |= (1U << index);

                StateChanged = true;

                if ((ushort)_state == ushort.MaxValue) // all values present
                {
                    TResult result;
                    try
                    {
                        result = GetResult();
                    }
                    catch (Exception ex)
                    {
                        Output.OnError(ex);
                        Dispose();
                        return;
                    }

                    Output.OnNext(result);
                }
                else
                {
                    if (((_state >> 16) & GetMask(Arity)) != 0) // any completed
                    {
                        Output.OnCompleted();
                        Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Applies the selector function to the latest values received by every source observer.
        /// </summary>
        /// <returns>The result of invoking the selector to the latest value of each source observer.</returns>
        protected abstract TResult GetResult();

        /// <summary>
        /// Reports an <paramref name="error"/> received by a source observer.
        /// </summary>
        /// <param name="error">The error received by a source observer.</param>
        public void Fail(Exception error)
        {
            lock (Lock)
            {
                Output.OnError(error);
                Dispose();
            }
        }

        /// <summary>
        /// Reports that the source observer with the specified <paramref name="index"/> has completed.
        /// </summary>
        /// <param name="index">The index of the source observer that has completed.</param>
        public void Done(int index)
        {
            Debug.Assert(index <= 16);

            lock (Lock)
            {
                _state |= (1U << (index + 16));

                StateChanged = true;

                if ((_state >> 16) == ushort.MaxValue) // all completed
                {
                    Output.OnCompleted();
                    Dispose();
                }
            }
        }

        //
        // CONSIDER: Consider upgrading the state version and simply storing the _state value rather
        //           than serializing it into separate Boolean values.
        //

        /// <summary>
        /// Saves the state of the operator.
        /// </summary>
        /// <param name="writer">The writer to write state to.</param>
        protected override void SaveStateCore(IOperatorStateWriter writer)
        {
            base.SaveStateCore(writer);

            var hasValue = _state;
            var complete = _state >> 16;

            for (int i = 0; i < Arity; i++, hasValue >>= 1)
            {
                writer.Write((hasValue & 1U) != 0U);
            }

            for (int i = 0; i < Arity; i++, complete >>= 1)
            {
                writer.Write((complete & 1U) != 0U);
            }
        }

        /// <summary>
        /// Loads the state of the operator.
        /// </summary>
        /// <param name="reader">The reader to load state from.</param>
        protected override void LoadStateCore(IOperatorStateReader reader)
        {
            base.LoadStateCore(reader);

            var hasValue = (uint)0;
            var complete = (uint)0;

            for (int i = 0; i < Arity; i++)
            {
                hasValue |= (reader.Read<bool>() ? 1U : 0U) << i;
            }

            for (int i = 0; i < Arity; i++)
            {
                complete |= (reader.Read<bool>() ? 1U : 0U) << i;
            }

            _state |= (complete << 16) | hasValue;
        }
    }

    /// <summary>
    /// Represents an observer for a source used by an n-ary `CombineLatest` operator.
    /// </summary>
    /// <typeparam name="TInput">The type of the events processed by the nth observer.</typeparam>
    internal sealed class CombineLatestObserver<TInput> : IObserver<TInput>
    {
        /// <summary>
        /// The parent sink to report observer events to.
        /// </summary>
        private readonly ICombineLatest _parent;

        /// <summary>
        /// The index of the observer, i.e. which of the sources it's subscribed to. This value
        /// is used to report values and completion messages to the parent, in order for it to
        /// update the corresponding value and completion tracking state.
        /// </summary>
        private readonly int _index;

        /// <summary>
        /// The subscription of the source sequence with this observer. This field is set after
        /// constructing the instance, using the <see cref="Subscription"/> property.
        /// </summary>
        private ISubscription _subscription;

        public CombineLatestObserver(ICombineLatest parent, int index)
        {
            _parent = parent;
            _index = index;
        }

        /// <summary>
        /// Sets the subscription of the source sequence with this observer.
        /// </summary>
        public ISubscription Subscription
        {
            set
            {
                Debug.Assert(_subscription == null, "Expecting that the subscription will only be set once.");

                _subscription = value;
            }
        }

        /// <summary>
        /// Gets the latest value received by this observer, used by the parent to apply the
        /// selector function over the latest value of each child observer.
        /// </summary>
        public TInput LastValue { get; private set; }

        /// <summary>
        /// Saves the state of the observer.
        /// </summary>
        /// <param name="writer">State writer to write state to.</param>
        public void SaveState(IOperatorStateWriter writer)
        {
            writer.Write<TInput>(LastValue);
        }

        /// <summary>
        /// Loads the state of the observer.
        /// </summary>
        /// <param name="reader">State reader to read state from.</param>
        public void LoadState(IOperatorStateReader reader)
        {
            LastValue = reader.Read<TInput>();
        }

        /// <summary>
        /// Defines behavior after we complete use of `CombineLatest`.
        /// </summary>
        public void OnCompleted()
        {
            _subscription.Dispose();
            _parent.Done(_index);
        }

        /// <summary>
        /// Reports the received <paramref name="error"/> to the parent.
        /// </summary>
        /// <param name="error">The error received by the observer.</param>
        public void OnError(Exception error)
        {
            _parent.Fail(error);
        }

        /// <summary>
        /// Sets the observer's last seen value to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value received by the observer.</param>
        public void OnNext(TInput value)
        {
            lock (_parent.Lock)
            {
                LastValue = value;
                _parent.Next(_index);
            }
        }
    }

    /// <summary>
    /// Interface that every n-ary `CombineLatest` sink must implement.
    /// </summary>
    internal interface ICombineLatest
    {
        /// <summary>
        /// Gets the lock used to synchronize access to the downstream observer and to protected state.
        /// </summary>
        object Lock { get; }

        /// <summary>
        /// Reports that the source observer with the specified <paramref name="index"/> has received a value.
        /// </summary>
        /// <param name="index">The index of the source observer that received a value.</param>
        void Next(int index);

        /// <summary>
        /// Reports an <paramref name="error"/> received by a source observer.
        /// </summary>
        /// <param name="error">The error received by a source observer.</param>
        void Fail(Exception error);

        /// <summary>
        /// Reports that the source observer with the specified <paramref name="index"/> has completed.
        /// </summary>
        /// <param name="index">The index of the source observer that has completed.</param>
        void Done(int index);
    }
}
