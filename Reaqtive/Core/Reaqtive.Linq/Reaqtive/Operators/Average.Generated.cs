// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Average(this ISubscribable<Int32> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageInt32 : SubscribableBase<Double>
    {
        private readonly ISubscribable<Int32> _source;

        public AverageInt32(ISubscribable<Int32> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageInt32, Double>, IObserver<Int32>
        {
            private Int64 _sum;
            private long _count;

            public _(AverageInt32 parent, IObserver<Double> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                var res = (Double)((Double)_sum / _count);

                Output.OnNext(res);
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
                        _sum += value;
                        _count++;
                        StateChanged = true;
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
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Average(this ISubscribable<Int64> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageInt64 : SubscribableBase<Double>
    {
        private readonly ISubscribable<Int64> _source;

        public AverageInt64(ISubscribable<Int64> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageInt64, Double>, IObserver<Int64>
        {
            private Int64 _sum;
            private long _count;

            public _(AverageInt64 parent, IObserver<Double> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                var res = (Double)((Double)_sum / _count);

                Output.OnNext(res);
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
                        _sum += value;
                        _count++;
                        StateChanged = true;
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
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Average(this ISubscribable<Single> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Single> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageSingle : SubscribableBase<Single>
    {
        private readonly ISubscribable<Single> _source;

        public AverageSingle(ISubscribable<Single> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageSingle, Single>, IObserver<Single>
        {
            private Double _sum;
            private long _count;

            public _(AverageSingle parent, IObserver<Single> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                var res = (Single)(_sum / _count);

                Output.OnNext(res);
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
                try
                {
                    checked
                    {
                        _sum += value;
                        _count++;
                        StateChanged = true;
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

                _sum = reader.Read<Double>();
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Average(this ISubscribable<Double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Double> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageDouble : SubscribableBase<Double>
    {
        private readonly ISubscribable<Double> _source;

        public AverageDouble(ISubscribable<Double> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageDouble, Double>, IObserver<Double>
        {
            private Double _sum;
            private long _count;

            public _(AverageDouble parent, IObserver<Double> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                var res = (Double)(_sum / _count);

                Output.OnNext(res);
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
                try
                {
                    checked
                    {
                        _sum += value;
                        _count++;
                        StateChanged = true;
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

                _sum = reader.Read<Double>();
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Average(this ISubscribable<Decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageDecimal : SubscribableBase<Decimal>
    {
        private readonly ISubscribable<Decimal> _source;

        public AverageDecimal(ISubscribable<Decimal> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageDecimal, Decimal>, IObserver<Decimal>
        {
            private Decimal _sum;
            private long _count;

            public _(AverageDecimal parent, IObserver<Decimal> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                    Dispose();
                    return;
                }

                var res = (Decimal)(_sum / _count);

                Output.OnNext(res);
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
                        _sum += value;
                        _count++;
                        StateChanged = true;
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
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Decimal>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Average(this ISubscribable<Int32?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageNullableInt32(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Int32?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageNullableInt32 : SubscribableBase<Double?>
    {
        private readonly ISubscribable<Int32?> _source;

        public AverageNullableInt32(ISubscribable<Int32?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageNullableInt32, Double?>, IObserver<Int32?>
        {
            private Int64 _sum;
            private long _count;

            public _(AverageNullableInt32 parent, IObserver<Double?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageNullableInt32";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnNext(null);
                    Output.OnCompleted();
                    Dispose();
                    return;
                }

                var res = (Double?)((Double)_sum / _count);

                Output.OnNext(res);
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
                        _sum += value.Value;
                        _count++;
                        StateChanged = true;
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
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Average(this ISubscribable<Int64?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageNullableInt64(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Int64?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageNullableInt64 : SubscribableBase<Double?>
    {
        private readonly ISubscribable<Int64?> _source;

        public AverageNullableInt64(ISubscribable<Int64?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageNullableInt64, Double?>, IObserver<Int64?>
        {
            private Int64 _sum;
            private long _count;

            public _(AverageNullableInt64 parent, IObserver<Double?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageNullableInt64";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnNext(null);
                    Output.OnCompleted();
                    Dispose();
                    return;
                }

                var res = (Double?)((Double)_sum / _count);

                Output.OnNext(res);
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
                        _sum += value.Value;
                        _count++;
                        StateChanged = true;
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
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Int64>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Average(this ISubscribable<Single?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageNullableSingle(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Single?> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Single?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageNullableSingle : SubscribableBase<Single?>
    {
        private readonly ISubscribable<Single?> _source;

        public AverageNullableSingle(ISubscribable<Single?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Single?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageNullableSingle, Single?>, IObserver<Single?>
        {
            private Double _sum;
            private long _count;

            public _(AverageNullableSingle parent, IObserver<Single?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageNullableSingle";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnNext(null);
                    Output.OnCompleted();
                    Dispose();
                    return;
                }

                var res = (Single?)(_sum / _count);

                Output.OnNext(res);
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

                try
                {
                    checked
                    {
                        _sum += value.Value;
                        _count++;
                        StateChanged = true;
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

                _sum = reader.Read<Double>();
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Average(this ISubscribable<Double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageNullableDouble(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Double?> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Double?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageNullableDouble : SubscribableBase<Double?>
    {
        private readonly ISubscribable<Double?> _source;

        public AverageNullableDouble(ISubscribable<Double?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Double?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageNullableDouble, Double?>, IObserver<Double?>
        {
            private Double _sum;
            private long _count;

            public _(AverageNullableDouble parent, IObserver<Double?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageNullableDouble";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnNext(null);
                    Output.OnCompleted();
                    Dispose();
                    return;
                }

                var res = (Double?)(_sum / _count);

                Output.OnNext(res);
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

                try
                {
                    checked
                    {
                        _sum += value.Value;
                        _count++;
                        StateChanged = true;
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

                _sum = reader.Read<Double>();
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Double>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

namespace Reaqtive
{
    using Operators;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of primitive types.")]
    public static partial class Subscribable
    {
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Average(this ISubscribable<Decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AverageNullableDecimal(source);
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence. If the source sequence is empty, a <c>null</c> value is returned.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Go and learn about functional programming and type systems.")]
        public static ISubscribable<Decimal?> Average<TSource>(this ISubscribable<TSource> source, Func<TSource, Decimal?> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Select(selector).Average();
        }
    }
}

namespace Reaqtive.Operators
{
    internal sealed class AverageNullableDecimal : SubscribableBase<Decimal?>
    {
        private readonly ISubscribable<Decimal?> _source;

        public AverageNullableDecimal(ISubscribable<Decimal?> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<Decimal?> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<AverageNullableDecimal, Decimal?>, IObserver<Decimal?>
        {
            private Decimal _sum;
            private long _count;

            public _(AverageNullableDecimal parent, IObserver<Decimal?> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:AverageNullableDecimal";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_count == 0)
                {
                    Output.OnNext(null);
                    Output.OnCompleted();
                    Dispose();
                    return;
                }

                var res = (Decimal?)(_sum / _count);

                Output.OnNext(res);
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
                        _sum += value.Value;
                        _count++;
                        StateChanged = true;
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
                _count = reader.Read<long>();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Caller is trusted.")]
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Decimal>(_sum);
                writer.Write<long>(_count);
            }
        }
    }
}

