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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32> Sum(this ISubscribable<Int32> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumInt32 : SubscribableBase<Int32>
    {
        private readonly ISubscribable<Int32> _source;

        public SumInt32(ISubscribable<Int32> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int32> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumInt32, Int32>, IObserver<Int32>
        {
            private Int32 _sum = 0;

            public _(SumInt32 parent, IObserver<Int32> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Int32 value)
            {
                try
                {
                    checked
                    {
                        var old = _sum;
                        _sum += value;
                        StateChanged = StateChanged || _sum != old;
                    }
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
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

                _sum = reader.Read<Int32>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int32>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64> Sum(this ISubscribable<Int64> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumInt64 : SubscribableBase<Int64>
    {
        private readonly ISubscribable<Int64> _source;

        public SumInt64(ISubscribable<Int64> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int64> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumInt64, Int64>, IObserver<Int64>
        {
            private Int64 _sum = 0;

            public _(SumInt64 parent, IObserver<Int64> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Int64 value)
            {
                try
                {
                    checked
                    {
                        var old = _sum;
                        _sum += value;
                        StateChanged = StateChanged || _sum != old;
                    }
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
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

                _sum = reader.Read<Int64>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Sum(this ISubscribable<Single> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Single> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumSingle : SubscribableBase<Single>
    {
        private readonly ISubscribable<Single> _source;

        public SumSingle(ISubscribable<Single> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumSingle, Single>, IObserver<Single>
        {
            private Single _sum = 0;

            public _(SumSingle parent, IObserver<Single> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Single value)
            {
                var old = _sum;
                _sum += value;
                StateChanged = StateChanged || _sum != old;
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _sum = reader.Read<Single>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Single>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Sum(this ISubscribable<Double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Double> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumDouble : SubscribableBase<Double>
    {
        private readonly ISubscribable<Double> _source;

        public SumDouble(ISubscribable<Double> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumDouble, Double>, IObserver<Double>
        {
            private Double _sum = 0;

            public _(SumDouble parent, IObserver<Double> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Double value)
            {
                var old = _sum;
                _sum += value;
                StateChanged = StateChanged || _sum != old;
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _sum = reader.Read<Double>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Sum(this ISubscribable<Decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumDecimal : SubscribableBase<Decimal>
    {
        private readonly ISubscribable<Decimal> _source;

        public SumDecimal(ISubscribable<Decimal> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumDecimal, Decimal>, IObserver<Decimal>
        {
            private Decimal _sum = 0;

            public _(SumDecimal parent, IObserver<Decimal> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Decimal value)
            {
                try
                {
                    checked
                    {
                        var old = _sum;
                        _sum += value;
                        StateChanged = StateChanged || _sum != old;
                    }
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
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

                _sum = reader.Read<Decimal>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Decimal>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32?> Sum(this ISubscribable<Int32?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumNullableInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int32?> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32?> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumNullableInt32 : SubscribableBase<Int32?>
    {
        private readonly ISubscribable<Int32?> _source;

        public SumNullableInt32(ISubscribable<Int32?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int32?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumNullableInt32, Int32?>, IObserver<Int32?>
        {
            private Int32 _sum = 0;

            public _(SumNullableInt32 parent, IObserver<Int32?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumNullableInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Int32? value)
            {
                if (value == null)
                    return;

                try
                {
                    checked
                    {
                        var old = _sum;
                        _sum += value.Value;
                        StateChanged = StateChanged || _sum != old;
                    }
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
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

                _sum = reader.Read<Int32>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int32>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64?> Sum(this ISubscribable<Int64?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumNullableInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Int64?> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64?> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumNullableInt64 : SubscribableBase<Int64?>
    {
        private readonly ISubscribable<Int64?> _source;

        public SumNullableInt64(ISubscribable<Int64?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Int64?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumNullableInt64, Int64?>, IObserver<Int64?>
        {
            private Int64 _sum = 0;

            public _(SumNullableInt64 parent, IObserver<Int64?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumNullableInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Int64? value)
            {
                if (value == null)
                    return;

                try
                {
                    checked
                    {
                        var old = _sum;
                        _sum += value.Value;
                        StateChanged = StateChanged || _sum != old;
                    }
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
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

                _sum = reader.Read<Int64>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Sum(this ISubscribable<Single?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumNullableSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Single?> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumNullableSingle : SubscribableBase<Single?>
    {
        private readonly ISubscribable<Single?> _source;

        public SumNullableSingle(ISubscribable<Single?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumNullableSingle, Single?>, IObserver<Single?>
        {
            private Single _sum = 0;

            public _(SumNullableSingle parent, IObserver<Single?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumNullableSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Single? value)
            {
                if (value == null)
                    return;

                var old = _sum;
                _sum += value.Value;
                StateChanged = StateChanged || _sum != old;
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _sum = reader.Read<Single>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Single>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Sum(this ISubscribable<Double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumNullableDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Double?> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumNullableDouble : SubscribableBase<Double?>
    {
        private readonly ISubscribable<Double?> _source;

        public SumNullableDouble(ISubscribable<Double?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumNullableDouble, Double?>, IObserver<Double?>
        {
            private Double _sum = 0;

            public _(SumNullableDouble parent, IObserver<Double?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumNullableDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Double? value)
            {
                if (value == null)
                    return;

                var old = _sum;
                _sum += value.Value;
                StateChanged = StateChanged || _sum != old;
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _sum = reader.Read<Double>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double>(_sum);
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
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Sum(this ISubscribable<Decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SumNullableDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Sum<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal?> selector)
        {
            return source.Select(selector).Sum();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class SumNullableDecimal : SubscribableBase<Decimal?>
    {
        private readonly ISubscribable<Decimal?> _source;

        public SumNullableDecimal(ISubscribable<Decimal?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SumNullableDecimal, Decimal?>, IObserver<Decimal?>
        {
            private Decimal _sum = 0;

            public _(SumNullableDecimal parent, IObserver<Decimal?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SumNullableDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_sum);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Core to the OnError design.")]
            public void OnNext(Decimal? value)
            {
                if (value == null)
                    return;

                try
                {
                    checked
                    {
                        var old = _sum;
                        _sum += value.Value;
                        StateChanged = StateChanged || _sum != old;
                    }
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
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

                _sum = reader.Read<Decimal>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Decimal>(_sum);
            }
        }
    }
}

