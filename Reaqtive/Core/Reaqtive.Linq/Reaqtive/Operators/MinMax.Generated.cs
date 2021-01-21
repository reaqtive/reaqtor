// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32> Min(this ISubscribable<Int32> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinInt32 : SubscribableBase<Int32>
    {
        private readonly ISubscribable<Int32> _source;

        public MinInt32(ISubscribable<Int32> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int32> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinInt32, Int32>, IObserver<Int32>
        {
            private Int32 _res;
            private bool _hasValue;

            public _(MinInt32 parent, IObserver<Int32> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int32 value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    if (value < _res)
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Int32>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Int32>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64> Min(this ISubscribable<Int64> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinInt64 : SubscribableBase<Int64>
    {
        private readonly ISubscribable<Int64> _source;

        public MinInt64(ISubscribable<Int64> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int64> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinInt64, Int64>, IObserver<Int64>
        {
            private Int64 _res;
            private bool _hasValue;

            public _(MinInt64 parent, IObserver<Int64> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int64 value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    if (value < _res)
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Int64>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Int64>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Min(this ISubscribable<Single> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Single> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinSingle : SubscribableBase<Single>
    {
        private readonly ISubscribable<Single> _source;

        public MinSingle(ISubscribable<Single> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinSingle, Single>, IObserver<Single>
        {
            private Single _res;
            private bool _hasValue;

            public _(MinSingle parent, IObserver<Single> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Single value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    if (value < _res || Single.IsNaN(value))
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Single>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Single>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Min(this ISubscribable<Double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Double> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinDouble : SubscribableBase<Double>
    {
        private readonly ISubscribable<Double> _source;

        public MinDouble(ISubscribable<Double> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinDouble, Double>, IObserver<Double>
        {
            private Double _res;
            private bool _hasValue;

            public _(MinDouble parent, IObserver<Double> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Double value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    if (value < _res || Double.IsNaN(value))
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Double>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Double>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Min(this ISubscribable<Decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinDecimal : SubscribableBase<Decimal>
    {
        private readonly ISubscribable<Decimal> _source;

        public MinDecimal(ISubscribable<Decimal> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinDecimal, Decimal>, IObserver<Decimal>
        {
            private Decimal _res;
            private bool _hasValue;

            public _(MinDecimal parent, IObserver<Decimal> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Decimal value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    if (value < _res)
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Decimal>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Decimal>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32?> Min(this ISubscribable<Int32?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinNullableInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32?> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinNullableInt32 : SubscribableBase<Int32?>
    {
        private readonly ISubscribable<Int32?> _source;

        public MinNullableInt32(ISubscribable<Int32?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int32?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinNullableInt32, Int32?>, IObserver<Int32?>
        {
            private Int32? _res;

            public _(MinNullableInt32 parent, IObserver<Int32?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinNullableInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int32? value)
            {
                if (value != null && (_res == null || value < _res))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Int32?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int32?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64?> Min(this ISubscribable<Int64?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinNullableInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64?> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinNullableInt64 : SubscribableBase<Int64?>
    {
        private readonly ISubscribable<Int64?> _source;

        public MinNullableInt64(ISubscribable<Int64?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int64?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinNullableInt64, Int64?>, IObserver<Int64?>
        {
            private Int64? _res;

            public _(MinNullableInt64 parent, IObserver<Int64?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinNullableInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int64? value)
            {
                if (value != null && (_res == null || value < _res))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Int64?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Min(this ISubscribable<Single?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinNullableSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Single?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinNullableSingle : SubscribableBase<Single?>
    {
        private readonly ISubscribable<Single?> _source;

        public MinNullableSingle(ISubscribable<Single?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinNullableSingle, Single?>, IObserver<Single?>
        {
            private Single? _res;

            public _(MinNullableSingle parent, IObserver<Single?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinNullableSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Single? value)
            {
                // Normally NaN < anything is false, as is anything < NaN
                // However, this leads to some irksome outcomes in Min and Max.
                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                // ordering where NaN is smaller than every value, including
                // negative infinity.
                if (value != null && (_res == null || value < _res || Single.IsNaN(value.Value)))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Single?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Single?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Min(this ISubscribable<Double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinNullableDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Double?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinNullableDouble : SubscribableBase<Double?>
    {
        private readonly ISubscribable<Double?> _source;

        public MinNullableDouble(ISubscribable<Double?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinNullableDouble, Double?>, IObserver<Double?>
        {
            private Double? _res;

            public _(MinNullableDouble parent, IObserver<Double?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinNullableDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Double? value)
            {
                // Normally NaN < anything is false, as is anything < NaN
                // However, this leads to some irksome outcomes in Min and Max.
                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                // ordering where NaN is smaller than every value, including
                // negative infinity.
                if (value != null && (_res == null || value < _res || Double.IsNaN(value.Value)))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Double?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Min(this ISubscribable<Decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MinNullableDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Min<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Min();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MinNullableDecimal : SubscribableBase<Decimal?>
    {
        private readonly ISubscribable<Decimal?> _source;

        public MinNullableDecimal(ISubscribable<Decimal?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MinNullableDecimal, Decimal?>, IObserver<Decimal?>
        {
            private Decimal? _res;

            public _(MinNullableDecimal parent, IObserver<Decimal?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MinNullableDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Decimal? value)
            {
                if (value != null && (_res == null || value < _res))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Decimal?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Decimal?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32> Max(this ISubscribable<Int32> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxInt32 : SubscribableBase<Int32>
    {
        private readonly ISubscribable<Int32> _source;

        public MaxInt32(ISubscribable<Int32> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int32> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxInt32, Int32>, IObserver<Int32>
        {
            private Int32 _res;
            private bool _hasValue;

            public _(MaxInt32 parent, IObserver<Int32> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int32 value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    if (value > _res)
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Int32>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Int32>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64> Max(this ISubscribable<Int64> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxInt64 : SubscribableBase<Int64>
    {
        private readonly ISubscribable<Int64> _source;

        public MaxInt64(ISubscribable<Int64> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int64> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxInt64, Int64>, IObserver<Int64>
        {
            private Int64 _res;
            private bool _hasValue;

            public _(MaxInt64 parent, IObserver<Int64> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int64 value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    if (value > _res)
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Int64>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Int64>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Max(this ISubscribable<Single> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Single> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxSingle : SubscribableBase<Single>
    {
        private readonly ISubscribable<Single> _source;

        public MaxSingle(ISubscribable<Single> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxSingle, Single>, IObserver<Single>
        {
            private Single _res;
            private bool _hasValue;

            public _(MaxSingle parent, IObserver<Single> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Single value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    // Normally NaN > anything is false, as is anything > NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Max(NaN, 5.0) is NaN, but
                    // Max(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    if (value > _res || Single.IsNaN(_res))
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Single>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Single>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Max(this ISubscribable<Double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Double> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxDouble : SubscribableBase<Double>
    {
        private readonly ISubscribable<Double> _source;

        public MaxDouble(ISubscribable<Double> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxDouble, Double>, IObserver<Double>
        {
            private Double _res;
            private bool _hasValue;

            public _(MaxDouble parent, IObserver<Double> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Double value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    // Normally NaN > anything is false, as is anything > NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Max(NaN, 5.0) is NaN, but
                    // Max(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    if (value > _res || Double.IsNaN(_res))
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Double>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Double>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Max(this ISubscribable<Decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxDecimal : SubscribableBase<Decimal>
    {
        private readonly ISubscribable<Decimal> _source;

        public MaxDecimal(ISubscribable<Decimal> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxDecimal, Decimal>, IObserver<Decimal>
        {
            private Decimal _res;
            private bool _hasValue;

            public _(MaxDecimal parent, IObserver<Decimal> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Decimal value)
            {
                if (!_hasValue)
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
                else
                {
                    if (value > _res)
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<Decimal>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<Decimal>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32?> Max(this ISubscribable<Int32?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxNullableInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32?> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxNullableInt32 : SubscribableBase<Int32?>
    {
        private readonly ISubscribable<Int32?> _source;

        public MaxNullableInt32(ISubscribable<Int32?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int32?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxNullableInt32, Int32?>, IObserver<Int32?>
        {
            private Int32? _res;

            public _(MaxNullableInt32 parent, IObserver<Int32?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxNullableInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int32? value)
            {
                if (value != null && (_res == null || value > _res))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Int32?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int32?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64?> Max(this ISubscribable<Int64?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxNullableInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64?> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxNullableInt64 : SubscribableBase<Int64?>
    {
        private readonly ISubscribable<Int64?> _source;

        public MaxNullableInt64(ISubscribable<Int64?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int64?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxNullableInt64, Int64?>, IObserver<Int64?>
        {
            private Int64? _res;

            public _(MaxNullableInt64 parent, IObserver<Int64?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxNullableInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Int64? value)
            {
                if (value != null && (_res == null || value > _res))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Int64?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Max(this ISubscribable<Single?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxNullableSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Single?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxNullableSingle : SubscribableBase<Single?>
    {
        private readonly ISubscribable<Single?> _source;

        public MaxNullableSingle(ISubscribable<Single?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxNullableSingle, Single?>, IObserver<Single?>
        {
            private Single? _res;

            public _(MaxNullableSingle parent, IObserver<Single?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxNullableSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Single? value)
            {
                // Normally NaN > anything is false, as is anything > NaN
                // However, this leads to some irksome outcomes in Min and Max.
                // If we use those semantics then Max(NaN, 5.0) is NaN, but
                // Max(5.0, NaN) is 5.0!  To fix this, we impose a total
                // ordering where NaN is smaller than every value, including
                // negative infinity.
                if (value != null && (_res == null || value > _res || Single.IsNaN(_res.Value)))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Single?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Single?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Max(this ISubscribable<Double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxNullableDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Double?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxNullableDouble : SubscribableBase<Double?>
    {
        private readonly ISubscribable<Double?> _source;

        public MaxNullableDouble(ISubscribable<Double?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxNullableDouble, Double?>, IObserver<Double?>
        {
            private Double? _res;

            public _(MaxNullableDouble parent, IObserver<Double?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxNullableDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Double? value)
            {
                // Normally NaN > anything is false, as is anything > NaN
                // However, this leads to some irksome outcomes in Min and Max.
                // If we use those semantics then Max(NaN, 5.0) is NaN, but
                // Max(5.0, NaN) is 5.0!  To fix this, we impose a total
                // ordering where NaN is smaller than every value, including
                // negative infinity.
                if (value != null && (_res == null || value > _res || Double.IsNaN(_res.Value)))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Double?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double?>(_res);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the largest element's value.
        /// </summary>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Max(this ISubscribable<Decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MaxNullableDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest value obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest value of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Max<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Max();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class MaxNullableDecimal : SubscribableBase<Decimal?>
    {
        private readonly ISubscribable<Decimal?> _source;

        public MaxNullableDecimal(ISubscribable<Decimal?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<MaxNullableDecimal, Decimal?>, IObserver<Decimal?>
        {
            private Decimal? _res;

            public _(MaxNullableDecimal parent, IObserver<Decimal?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxNullableDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(Decimal? value)
            {
                if (value != null && (_res == null || value > _res))
                {
                    StateChanged = true;
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<Decimal?>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Decimal?>(_res);
            }
        }
    }
}

