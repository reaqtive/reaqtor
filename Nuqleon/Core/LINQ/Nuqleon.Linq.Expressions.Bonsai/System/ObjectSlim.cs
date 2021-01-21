// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Reflection;

namespace System
{
    /// <summary>
    /// A slim representation of a constant value.
    /// </summary>
    public abstract class ObjectSlim
    {
        #region Constructors

        /// <summary>
        /// Creates a new slim representation of a constant value with the given slim type.
        /// </summary>
        /// <param name="value">The underlying value, which can be either a liftable CLR object or a reducible Bonsai value.</param>
        /// <param name="typeSlim">The slim type representation of the object.</param>
        protected ObjectSlim(object value, TypeSlim typeSlim)
        {
            Value = value;
            TypeSlim = typeSlim;
        }

        #endregion

        #region Properties

        /// <summary>
        /// true if the Lift method can be called, false otherwise
        /// </summary>
        public abstract bool CanLift { get; }

        /// <summary>
        /// true if the Reduce method can be called, false otherwise
        /// </summary>
        public abstract bool CanReduce { get; }

        /// <summary>
        /// The value held by the object.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets the slim type representation of the object.
        /// </summary>
        public TypeSlim TypeSlim { get; }

        /// <summary>
        /// Gets the original type of the liftable object representation.
        /// </summary>
        public abstract Type OriginalType { get; }

        #endregion

        #region Methods

        // REVIEW: The asymmetry here is slightly disturbing. We should review the lift/reduce constructs here.

        /// <summary>
        /// Creates a new slim representation of a constant value that can be lifted to a Bonsai representation.
        /// </summary>
        /// <param name="value">The underlying value that can be lifted to a Bonsai representation.</param>
        /// <param name="typeSlim">The slim type to use in the Bonsai representation.</param>
        /// <param name="type">The CLR type of the underlying value.</param>
        /// <returns>A new slim representation of a constant value that can be lifted to a Bonsai representation.</returns>
        public static ObjectSlim Create(object value, TypeSlim typeSlim, Type type)
        {
            if (typeSlim == null)
                throw new ArgumentNullException(nameof(typeSlim));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return new LiftableObjectSlim(value, typeSlim, type);
        }

        /// <summary>
        /// Creates a new slim representation of a constant value that can be reduced to a CLR object representation.
        /// </summary>
        /// <param name="liftedValue">The lifted value that can be reduced to a CLR object representation.</param>
        /// <param name="typeSlim">The slim type to use in the Bonsai representation.</param>
        /// <param name="reduceFactory">The factory used to obtain a strongly typed reducer to convert the object to a CLR object representation.</param>
        /// <returns>A new slim representation of a constant value that can be reduced to a CLR object representation.</returns>
        public static ObjectSlim Create<TLifted>(TLifted liftedValue, TypeSlim typeSlim, Func<Type, Func<TLifted, object>> reduceFactory)
        {
            if (typeSlim == null)
                throw new ArgumentNullException(nameof(typeSlim));
            if (reduceFactory == null)
                throw new ArgumentNullException(nameof(reduceFactory));

            return new ReducibleObjectSlim<TLifted>(liftedValue, typeSlim, reduceFactory);
        }

        // REVIEW: Should we have a specialized subclass that is parameterized by a lift factory in order to restore symmetry?

        /// <summary>
        /// Lifts a CLR object value to a slim object value.
        /// </summary>
        /// <typeparam name="TLifted">The type of the lifted representation.</typeparam>
        /// <param name="liftFactory">Factory to get a lift method for a given CLR type.</param>
        /// <returns>The slim object value.</returns>
        public abstract TLifted Lift<TLifted>(Func<Type, Func<object, TLifted>> liftFactory);

        /// <summary>
        /// Reduces a slim object value to a CLR object value.
        /// </summary>
        /// <param name="type">The CLR type to reduce to.</param>
        /// <returns>The CLR object value.</returns>
        public abstract object Reduce(Type type);

        /// <summary>
        /// Provides a friendly string representation of the object's value.
        /// </summary>
        /// <returns>Friendly string representation of the object's value.</returns>
        public override string ToString()
        {
            return Value != null ? Value.ToString() : "null";
        }

        /// <summary>
        /// Creates a new slim object representation given the new underlying value and slim type.
        /// </summary>
        /// <param name="value">The new underlying value.</param>
        /// <param name="typeSlim">The new slim type representation.</param>
        /// <returns>The current instance if nothing changes; otherwise, a new slim object representation.</returns>
        public abstract ObjectSlim Update(object value, TypeSlim typeSlim);

        #endregion

        #region Types

        private sealed class LiftableObjectSlim : ObjectSlim
        {
            public LiftableObjectSlim(object value, TypeSlim typeSlim, Type type)
                : base(value, typeSlim)
            {
                OriginalType = type;
            }

            public override bool CanLift => true;
            public override bool CanReduce => false;

            public override Type OriginalType { get; }

            public override TLifted Lift<TLifted>(Func<Type, Func<object, TLifted>> liftFactory)
            {
                if (liftFactory == null)
                    throw new ArgumentNullException(nameof(liftFactory));

                var lift = liftFactory(OriginalType);
                return lift(Value);
            }

            public override object Reduce(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                return Value;
            }

            public override ObjectSlim Update(object value, TypeSlim typeSlim)
            {
                if (value != Value || typeSlim != TypeSlim)
                {
                    return new LiftableObjectSlim(value, typeSlim, OriginalType);
                }

                return this;
            }
        }

        private sealed class ReducibleObjectSlim<TLifted> : ObjectSlim
        {
            public ReducibleObjectSlim(TLifted liftedValue, TypeSlim typeSlim, Func<Type, Func<TLifted, object>> reduceFactory)
                : base(liftedValue, typeSlim)
            {
                ReduceFactory = reduceFactory;
            }

            public override bool CanLift => false;
            public override bool CanReduce => true;

            public Func<Type, Func<TLifted, object>> ReduceFactory { get; }

            public override Type OriginalType => throw new InvalidOperationException("The lifted object representation has no type information.");

            public override TIgnore Lift<TIgnore>(Func<Type, Func<object, TIgnore>> liftFactory)
            {
                throw new InvalidOperationException("This object representation is already lifted.");
            }

            public override object Reduce(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                var reduce = ReduceFactory(type);
                return reduce((TLifted)Value);
            }

            public override ObjectSlim Update(object value, TypeSlim typeSlim)
            {
                if (value != Value || typeSlim != TypeSlim)
                {
                    return new ReducibleObjectSlim<TLifted>((TLifted)value, typeSlim, ReduceFactory);
                }

                return this;
            }
        }

        #endregion
    }
}
