// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Nuqleon.Serialization
{
    /// <summary>
    /// Base class for serialization/deserialization rule tables.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    public abstract class RuleTableBase<TInput, TOutput, TContext> : IEnumerable<Rule<TInput, TOutput, TContext>>
    {
        #region Fields

        /// <summary>
        /// Function to wrap a series of recursively acquired serialization outputs.
        /// </summary>
        private readonly Func<IDictionary<string, Expression>, Expression> _wrap;

        /// <summary>
        /// Function to unwrap a piece of recursive serialization output.
        /// </summary>
        private readonly Func<Expression, string, Expression> _unwrap;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new serializer with the given configuration parameters.
        /// </summary>
        /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
        /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
        protected RuleTableBase(Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
        {
            _wrap = wrap;
            _unwrap = unwrap;
        }

        #endregion

        #region Methods

        #region Combinators

        /// <summary>
        /// Gets a read-only accessor to this table.
        /// </summary>
        /// <returns>Read-only accessor to the current table instance.</returns>
        public RuleTableBase<TInput, TOutput, TContext> AsReadOnly()
        {
            return new RuleTableReadOnly<TInput, TOutput, TContext>(this, _wrap, _unwrap);
        }

        /// <summary>
        /// Adds an extension table on top of the this table.
        /// </summary>
        /// <returns>Extension table on top of the current table instance.</returns>
        public RuleTableBase<TInput, TOutput, TContext> Extend()
        {
            return new RuleTableExtension<TInput, TOutput, TContext>(this, _wrap, _unwrap);
        }

        /// <summary>
        /// Combines this rule table with the given rule table.
        /// </summary>
        /// <param name="second">Rule table to combine with the current table instance.</param>
        /// <returns>Concatenation of current table instance with the given second rule table instance.</returns>
        public RuleTableBase<TInput, TOutput, TContext> Concat(RuleTableBase<TInput, TOutput, TContext> second)
        {
            return new RuleTableConcat<TInput, TOutput, TContext>(this, second, _wrap, _unwrap);
        }

        #endregion

        #region Add (supporting collection initializer syntax)

        #region Rule

        /// <summary>
        /// Adds a serialization rule to the serializer's rule table.
        /// </summary>
        /// <param name="rule">Rule to add to the serializer's rule table.</param>
        public void Add(Rule<TInput, TOutput, TContext> rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            AddCore(rule);
        }

        #endregion

#pragma warning disable IDE0060 // Remove unused parameter (witness parameters for type inference)
#pragma warning disable CA1801 // Review unused parameters (same as above)

        #region Roundtrip function

        #region Without filter

        /// <summary>
        /// Adds a serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        /// <example>
        /// The following will serialize and deserialize a point by recursing into X and Y:
        /// <code>serializer.Add((Point p) => new Point(p.X, p.Y))</code>
        /// During serialization, the values of p.X and p.Y will be extracted from the Point object for recursive serialization, and will be wrapped using the wrap function specified in the serializer's constructor.
        /// During deserialization, the unwrap function specified in the serializer's constructor will be used to obtain the payloads for recursive deserialization of X and Y, and the resulting values will be used to invoke the Point constructor.
        /// </example>
        /// <remarks>The added rule's name is based on the type name of T. If multiple serialization rules for objects of type T are needed, avoid using this overload and specify a name explicitly in order to distinguish between rules.</remarks>
        public void Add<T>(Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            if (roundtrip == null)
                throw new ArgumentNullException(nameof(roundtrip));

            AddImpl<T>(typeof(T).Name, True<T>, roundtrip);
        }

        /// <summary>
        /// Adds a serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        /// <example>
        /// The following will serialize and deserialize a point by recursing into X and Y:
        /// <code>serializer.Add("Point", (Point p) => new Point(p.X, p.Y))</code>
        /// During serialization, the values of p.X and p.Y will be extracted from the Point object for recursive serialization, and will be wrapped using the wrap function specified in the serializer's constructor.
        /// During deserialization, the unwrap function specified in the serializer's constructor will be used to obtain the payloads for recursive deserialization of X and Y, and the resulting values will be used to invoke the Point constructor.
        /// </example>
        /// <remarks>The added rule's name is based on the type name of T. If multiple serialization rules for objects of type T are needed, avoid using this overload and specify a name explicitly in order to distinguish between rules.</remarks>
        public void Add<T>(string name, Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (roundtrip == null)
                throw new ArgumentNullException(nameof(roundtrip));

            AddImpl<T>(name, True<T>, roundtrip);
        }

        /// <summary>
        /// Adds a serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="witness">Phantom object used to infer the type of the rule from. This parameter's value is not used.</param>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        /// <example>
        /// The following will serialize and deserialize a point by recursing into X and Y:
        /// <code>serializer.Add("Point", default(Point), p => new Point(p.X, p.Y))</code>
        /// During serialization, the values of p.X and p.Y will be extracted from the Point object for recursive serialization, and will be wrapped using the wrap function specified in the serializer's constructor.
        /// During deserialization, the unwrap function specified in the serializer's constructor will be used to obtain the payloads for recursive deserialization of X and Y, and the resulting values will be used to invoke the Point constructor.
        /// </example>
        public void Add<T>(T witness, Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            if (roundtrip == null)
                throw new ArgumentNullException(nameof(roundtrip));

            AddImpl<T>(typeof(T).Name, True<T>, roundtrip);
        }

        /// <summary>
        /// Adds a serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="witness">Phantom object used to infer the type of the rule from. This parameter's value is not used.</param>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        /// <example>
        /// The following will serialize and deserialize a point by recursing into X and Y:
        /// <code>serializer.Add("Point", default(Point), p => new Point(p.X, p.Y))</code>
        /// During serialization, the values of p.X and p.Y will be extracted from the Point object for recursive serialization, and will be wrapped using the wrap function specified in the serializer's constructor.
        /// During deserialization, the unwrap function specified in the serializer's constructor will be used to obtain the payloads for recursive deserialization of X and Y, and the resulting values will be used to invoke the Point constructor.
        /// </example>
        public void Add<T>(string name, T witness, Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (roundtrip == null)
                throw new ArgumentNullException(nameof(roundtrip));

            AddImpl<T>(name, True<T>, roundtrip);
        }

        private static bool True<T>(T value) => true;

        #endregion

        #region With filter

        /// <summary>
        /// Adds a predicated serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        /// <remarks>The added rule's name is based on the type name of T. If multiple serialization rules for objects of type T are needed, avoid using this overload and specify a name explicitly in order to distinguish between rules.</remarks>
        /// <example>
        /// The following will serialize and deserialize a point in the first quadrant by recursing into X and Y:
        /// <code>serializer.Add(default(Point), p => p.X >= 0 &amp;&amp; p.Y >= 0, p => new Point(p.X, p.Y))</code>
        /// During serialization, the predicate is evaluated for the given Point object. If it matches, the values of p.X and p.Y will be extracted from the Point object for recursive serialization, and will be wrapped using the wrap function specified in the serializer's constructor.
        /// During deserialization, the unwrap function specified in the serializer's constructor will be used to obtain the payloads for recursive deserialization of X and Y, and the resulting values will be used to invoke the Point constructor.
        /// </example>
        public void Add<T>(Func<T, bool> filter, Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (roundtrip == null)
                throw new ArgumentNullException(nameof(roundtrip));

            AddImpl<T>(typeof(T).Name, filter, roundtrip);
        }

        /// <summary>
        /// Adds a predicated serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="witness">Phantom object used to infer the type of the rule from. This parameter's value is not used.</param>
        /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        /// <remarks>The added rule's name is based on the type name of T. If multiple serialization rules for objects of type T are needed, avoid using this overload and specify a name explicitly in order to distinguish between rules.</remarks>
        /// <example>
        /// The following will serialize and deserialize a point in the first quadrant by recursing into X and Y:
        /// <code>serializer.Add(default(Point), p => p.X >= 0 &amp;&amp; p.Y >= 0, p => new Point(p.X, p.Y))</code>
        /// During serialization, the predicate is evaluated for the given Point object. If it matches, the values of p.X and p.Y will be extracted from the Point object for recursive serialization, and will be wrapped using the wrap function specified in the serializer's constructor.
        /// During deserialization, the unwrap function specified in the serializer's constructor will be used to obtain the payloads for recursive deserialization of X and Y, and the resulting values will be used to invoke the Point constructor.
        /// </example>
        public void Add<T>(T witness, Func<T, bool> filter, Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (roundtrip == null)
                throw new ArgumentNullException(nameof(roundtrip));

            AddImpl<T>(typeof(T).Name, filter, roundtrip);
        }

        /// <summary>
        /// Adds a predicated serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="witness">Phantom object used to infer the type of the rule from. This parameter's value is not used.</param>
        /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        /// <example>
        /// The following will serialize and deserialize a point in the first quadrant by recursing into X and Y:
        /// <code>serializer.Add("Point_FirstQuadrant", default(Point), p => p.X >= 0 &amp;&amp; p.Y >= 0, p => new Point(p.X, p.Y))</code>
        /// During serialization, the predicate is evaluated for the given Point object. If it matches, the values of p.X and p.Y will be extracted from the Point object for recursive serialization, and will be wrapped using the wrap function specified in the serializer's constructor.
        /// During deserialization, the unwrap function specified in the serializer's constructor will be used to obtain the payloads for recursive deserialization of X and Y, and the resulting values will be used to invoke the Point constructor.
        /// </example>
        public void Add<T>(string name, T witness, Func<T, bool> filter, Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (roundtrip == null)
                throw new ArgumentNullException(nameof(roundtrip));

            AddImpl<T>(name, filter, roundtrip);
        }

        #endregion

        #endregion

        #region Serialization/deserialization pair

        /// <summary>
        /// Adds a serialization rule based on a pair of serialization and deserialization functions.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="serialize">Serialization function taking an object of type T, a recursive serialization function, and a context object as inputs.</param>
        /// <param name="deserialize">Deserialization function taking a serialization output, a recursive deserialization function, and a context object as inputs.</param>
        public void Add<T>(string name, Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> serialize, Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> deserialize)
            where T : TInput
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (serialize == null)
                throw new ArgumentNullException(nameof(serialize));
            if (deserialize == null)
                throw new ArgumentNullException(nameof(deserialize));

            AddImpl<T>(name, True<T>, serialize, deserialize);
        }

        /// <summary>
        /// Adds a serialization rule based on a pair of serialization and deserialization functions.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="witness">Phantom object used to infer the type of the rule from. This parameter's value is not used.</param>
        /// <param name="serialize">Serialization function taking an object of type T, a recursive serialization function, and a context object as inputs.</param>
        /// <param name="deserialize">Deserialization function taking a serialization output, a recursive deserialization function, and a context object as inputs.</param>
        public void Add<T>(string name, T witness, Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> serialize, Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> deserialize)
            where T : TInput
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (serialize == null)
                throw new ArgumentNullException(nameof(serialize));
            if (deserialize == null)
                throw new ArgumentNullException(nameof(deserialize));

            AddImpl<T>(name, True<T>, serialize, deserialize);
        }

        /// <summary>
        /// Adds a predicated serialization rule based on a pair of serialization and deserialization functions.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
        /// <param name="serialize">Serialization function taking an object of type T, a recursive serialization function, and a context object as inputs.</param>
        /// <param name="deserialize">Deserialization function taking a serialization output, a recursive deserialization function, and a context object as inputs.</param>
        public void Add<T>(string name, Func<T, bool> filter, Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> serialize, Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> deserialize)
            where T : TInput
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (serialize == null)
                throw new ArgumentNullException(nameof(serialize));
            if (deserialize == null)
                throw new ArgumentNullException(nameof(deserialize));

            AddImpl<T>(name, filter, serialize, deserialize);
        }

        /// <summary>
        /// Adds a predicated serialization rule based on a pair of serialization and deserialization functions.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="witness">Phantom object used to infer the type of the rule from. This parameter's value is not used.</param>
        /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
        /// <param name="serialize">Serialization function taking an object of type T, a recursive serialization function, and a context object as inputs.</param>
        /// <param name="deserialize">Deserialization function taking a serialization output, a recursive deserialization function, and a context object as inputs.</param>
        public void Add<T>(string name, T witness, Func<T, bool> filter, Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> serialize, Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> deserialize)
            where T : TInput
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (serialize == null)
                throw new ArgumentNullException(nameof(serialize));
            if (deserialize == null)
                throw new ArgumentNullException(nameof(deserialize));

            AddImpl<T>(name, filter, serialize, deserialize);
        }

        #endregion

#pragma warning restore CA1801
#pragma warning restore IDE0060

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the rule with the specified name.
        /// </summary>
        /// <param name="name">Name of the rule to get.</param>
        /// <returns>Rule with the specified name.</returns>
        public Rule<TInput, TOutput, TContext> this[string name]
        {
            get
            {
                if (!TryGetRule(name, out Rule<TInput, TOutput, TContext> rule))
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No rule with name '{0}' found.", name));

                return rule;
            }
        }

        /// <summary>
        /// Tries to get the rule with the specified name.
        /// </summary>
        /// <param name="name">Name of the rule to get.</param>
        /// <param name="rule">Rule with the specified name.</param>
        /// <returns>true if a rule with the specified name was found; otherwise, false.</returns>
        public abstract bool TryGetRule(string name, out Rule<TInput, TOutput, TContext> rule);

#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers. (Not any different from a Dictionary<Type, ...>)

        /// <summary>
        /// Gets a list of rules that can process objects of the given type using an exact type match.
        /// </summary>
        /// <param name="type">Type to lookup rules for.</param>
        /// <returns>Rules that match the given type, not considering subtyping.</returns>
        public abstract IEnumerable<Rule<TInput, TOutput, TContext>> this[Type type]
        {
            get;
        }

#pragma warning restore CA1043

        #endregion

        #region IEnumerable<T> implementation (used for collection initializer syntax)

        /// <summary>
        /// Gets an enumerator to iterate over the rules in the rule table.
        /// </summary>
        /// <returns>Sequence of rules in the rule table, in evaluation order on a per-type basis.</returns>
        public IEnumerator<Rule<TInput, TOutput, TContext>> GetEnumerator() => GetEnumeratorCore();

        /// <summary>
        /// Gets an enumerator to iterate over the rules in the rule table.
        /// </summary>
        /// <returns>Sequence of rules in the rule table, in evaluation order on a per-type basis.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumeratorCore();

        /// <summary>
        /// Gets an enumerator to iterate over the rules in the rule table.
        /// </summary>
        /// <returns>Sequence of rules in the rule table, in evaluation order on a per-type basis.</returns>
        protected abstract IEnumerator<Rule<TInput, TOutput, TContext>> GetEnumeratorCore();

        #endregion

        #region Private implementation

        /// <summary>
        /// Adds a predicated serialization rule based on a roundtrip function. Member lookups in the roundtrip function will automatically be serialized and deserialized recursively.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
        /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
        private void AddImpl<T>(string name, Func<T, bool> filter, Expression<Func<T, T>> roundtrip)
            where T : TInput
        {
            AddCore(new RoundtripRuleThunk<T>(name, filter, roundtrip, this));
        }

        /// <summary>
        /// Adds a predicated serialization rule based on a pair of serialization and deserialization functions.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
        /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
        /// <param name="serialize">Serialization function taking an object of type T, a recursive serialization function, and a context object as inputs.</param>
        /// <param name="deserialize">Deserialization function taking a serialization output, a recursive deserialization function, and a context object as inputs.</param>
        private void AddImpl<T>(string name, Func<T, bool> filter, Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> serialize, Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> deserialize)
            where T : TInput
        {
            if (TryGetRule(name, out Rule<TInput, TOutput, TContext> ignored))
                throw new InvalidOperationException("A rule with the specified name already exists.");

            AddCore(new Rule<TInput, TOutput, TContext, T>(name, filter, serialize, deserialize));
        }

        /// <summary>
        /// Adds a serialization rule to the serializer's rule table.
        /// </summary>
        /// <param name="rule">Rule to add to the serializer's rule table.</param>
        protected abstract void AddCore(Rule<TInput, TOutput, TContext> rule);

        /// <summary>
        /// Thunk rule for lazy initialization of roundtrip-based serialization rules.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        private sealed class RoundtripRuleThunk<T> : FilteredRuleBase<TInput, TOutput, TContext, T>
            where T : TInput
        {
            #region Fields

            /// <summary>
            /// Lock to guard the compilation of the roundtrip rule and the initialization of private state.
            /// </summary>
            private readonly object _initializationLock = new();

            /// <summary>
            /// Serialization and deserialization pair set after compilation of the roundtrip rule.
            /// </summary>
            private volatile SerializationPair<TInput, TOutput, TContext, T> _pair;

            /// <summary>
            /// Roundtrip rule. Will be nulled out after compilation.
            /// </summary>
            private Expression<Func<T, T>> _roundtrip;

            /// <summary>
            /// Parent reference to retrieve wrap and unwrap functions. Will be nulled out after compilation.
            /// </summary>
            private RuleTableBase<TInput, TOutput, TContext> _parent;

            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new thunk for lazy initialization of a roundtrip-based rule.
            /// </summary>
            /// <param name="name">Name for the rule to add to the serializer's rule table. This name needs to be unique and should be match on the serializer and deserializer sides.</param>
            /// <param name="filter">Predicate function to evaluate whether the rule is applicable for a given object during serialization.</param>
            /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
            /// <param name="parent">Reference to the containing rule table.</param>
            public RoundtripRuleThunk(string name, Func<T, bool> filter, Expression<Func<T, T>> roundtrip, RuleTableBase<TInput, TOutput, TContext> parent)
                : base(name, filter)
            {
                _roundtrip = roundtrip;
                _parent = parent;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Serializes the given object. Compiles the roundtrip-based rule on demand.
            /// </summary>
            /// <param name="input">Object to serialize.</param>
            /// <param name="recurse">Recursive serialization function.</param>
            /// <param name="context">Serialization context.</param>
            /// <returns>Result of the serialization.</returns>
            protected override TOutput SerializeCore(T input, Func<TInput, TContext, TOutput> recurse, TContext context)
            {
                if (_pair == null)
                    Initialize();

                return _pair.Serialize(input, recurse, context);
            }

            /// <summary>
            /// Deserializes the given object.
            /// </summary>
            /// <param name="output">Object to deserialize.</param>
            /// <param name="recurse">Recursive deserialization function.</param>
            /// <param name="context">Deserialization context.</param>
            /// <returns>Deserialized object.</returns>
            public override TInput Deserialize(TOutput output, Func<TOutput, TContext, TInput> recurse, TContext context)
            {
                if (_pair == null)
                    Initialize();

                return _pair.Deserialize(output, recurse, context);
            }

            /// <summary>
            /// Compiles the roundtrip-based rule.
            /// </summary>
            private void Initialize()
            {
                lock (_initializationLock)
                {
                    if (_pair == null)
                    {
                        _pair = RoundtripFindMembers<T>.Extract(_roundtrip, _parent._wrap, _parent._unwrap);
                        _roundtrip = null;
                        _parent = null;
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Visitor to get a serialization/deserialization function pair from a roundtrip expression, based on detection of member lookups.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
        private sealed class RoundtripFindMembers<T> : ExpressionVisitor
            where T : TInput
        {
            #region Fields

            /// <summary>
            /// Input parameter of the roundtrip expression.
            /// </summary>
            private readonly ParameterExpression _parameter;

            /// <summary>
            /// List of member lookups found on the given parameter expression.
            /// </summary>
            private readonly List<MemberExpression> _members;

            /// <summary>
            /// Function to substitute discovered member expressions in the roundtrip expression for recursive deserialization and unwrap calls.
            /// </summary>
            private Func<MemberExpression, Expression> _substitute;

            #endregion

            #region Factory pattern

            /// <summary>
            /// Creates a new visitor to extract serialization/deserialization pairs.
            /// </summary>
            /// <param name="parameter">Input parameter of the roundtrip expression.</param>
            private RoundtripFindMembers(ParameterExpression parameter)
            {
                _parameter = parameter;
                _members = new List<MemberExpression>();
            }

            /// <summary>
            /// Extracts a serialization/deserialization pair from the given roundtrip function, using the wrap and unwrap functions for recursive serialization/deserialization invocations.
            /// </summary>
            /// <param name="roundtrip">Roundtrip function containing member lookups for recursive serialization and deserialization.</param>
            /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
            /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
            /// <returns>Serialization/deserialization pair derived from the given roundtrip function.</returns>
            /// <example>
            /// Consider the following roundtrip function:
            /// <code>(Point p) => new Point(p.X, p.Y)</code>
            /// The generated serialization function is:
            /// <code>(p, rec, ctx) => _wrap(new Dictionary&lt;string, Expression&gt; { { "X", rec(p.X, ctx) }, { "Y", rec(p.Y, ctx) } })</code>
            /// The generated deserialization function is:
            /// <code>(o, rec, ctx) => new Point(rec(_unwrap(o, "X"), ctx), rec(_unwrap(o, "Y"), ctx))</code>
            /// </example>
            public static SerializationPair<TInput, TOutput, TContext, T> Extract(Expression<Func<T, T>> roundtrip, Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
            {
                return new RoundtripFindMembers<T>(roundtrip.Parameters[0]).Extract(roundtrip.Body, wrap, unwrap);
            }

            #endregion

            #region Methods

            #region Private implementation

            /// <summary>
            /// Extracts a serialization/deserialization pair from the given roundtrip function, using the wrap and unwrap functions for recursive serialization/deserialization invocations.
            /// </summary>
            /// <param name="expression">Body of the roundtrip function containing member lookups for recursive serialization and deserialization.</param>
            /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
            /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
            /// <returns>Serialization/deserialization pair derived from the given roundtrip function.</returns>
            private SerializationPair<TInput, TOutput, TContext, T> Extract(Expression expression, Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
            {
                //
                // Order matters here! GetDeserializer will populate the _members field, used by GetSerializer.
                //
                var deserialize = GetDeserializer(expression, unwrap);
                var serialize = GetSerializer(wrap);

                return new SerializationPair<TInput, TOutput, TContext, T>(serialize, deserialize);
            }

            /// <summary>
            /// Gets the deserialization function by analyzing the expression for member lookups and substituting those for recursive deserialization and unwrap calls.
            /// </summary>
            /// <param name="expression">Roundtrip expression body to infer the deserialization function from.</param>
            /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
            /// <returns>Deserialization function that will use recursive deserialization based on the roundtrip expression's member lookups.</returns>
            private Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> GetDeserializer(Expression expression, Func<Expression, string, Expression> unwrap)
            {
                //
                // Parameters for the deserialization function, matching the return type of this method.
                //
                var obj = Expression.Parameter(typeof(TOutput), "_");
                var rec = Expression.Parameter(typeof(Func<TOutput, TContext, TInput>), "deserialize");
                var ctx = Expression.Parameter(typeof(TContext), "ctx");

                //
                // Substitution function that will be used during the expression tree visit to replace MemberExpression nodes with
                // a recursive deserialization and unwrap call.
                //
                _substitute = me =>
                    Expression.Convert(
                        Expression.Invoke(
                            rec,
                            unwrap(obj, me.Member.Name),
                            ctx
                        ),
                        me.Type
                    );

                //
                // Create the deserialization function by using the expression tree visitor to substitute member lookups. As a side-
                // effect of Visit, all of the referenced members are gathered in the _member field.
                //
                var res =
                    Expression.Lambda<Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T>>(
                        Visit(expression),
                        obj, rec, ctx
                    );

                return res.Compile();
            }

            /// <summary>
            /// Gets the serialization function by using recursive serialization and wrap calls for the members found in the roundtrip expression.
            /// </summary>
            /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
            /// <returns>Serialization function that will use recursive serialization based on the roundtrip expression's member lookups.</returns>
            private Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> GetSerializer(Func<IDictionary<string, Expression>, Expression> wrap)
            {
                //
                // Parameters for the serialization function, matching the return type of this method.
                //
                var obj = _parameter;
                var rec = Expression.Parameter(typeof(Func<TInput, TContext, TOutput>), "serialize");
                var ctx = Expression.Parameter(typeof(TContext), "ctx");

                //
                // Gathers all the members that were discovered during the GetDeserializer call and uses those to build up calls to
                // the wrap function, collecting a bag of members to serialize recursively.
                //
                var bag = _members.ToDictionary<MemberExpression, string, Expression>(
                    m => m.Member.Name,
                    m =>
                        Expression.Invoke(
                            rec,
                            Expression.Convert(
                                m,
                                typeof(TInput)
                            ),
                            ctx
                        )
                    );

                //
                // Create the serialization function which simply uses the discovered members and gathers those by recursive invocation
                // of the serialization function into a bag to wrap up.
                //
                var res =
                    Expression.Lambda<Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput>>(
                        wrap(bag),
                        obj, rec, ctx
                    );

                return res.Compile();
            }

            #endregion

            #region Expression tree visitor overrides

            /// <summary>
            /// Discovers member lookups on the roundtrip expression parameter.
            /// </summary>
            /// <param name="node">Member expression to analyze.</param>
            /// <returns>Expression for invocation of recursive deserialization code if the member lookup is applied to the roundtrip expression parameter; otherwise, the original expression.</returns>
            protected override Expression VisitMember(MemberExpression node)
            {
                if (node != null && node.Expression == _parameter)
                {
                    _members.Add(node);
                    return _substitute(node);
                }

                return base.VisitMember(node);
            }

            /// <summary>
            /// Detects invalid usage of the roundtrip expression parameter.
            /// </summary>
            /// <param name="node">Parameter expression to analyze.</param>
            /// <returns>Original parameter if it doesn't match the roundtrip expression parameter; otherwise, an exception is thrown.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == _parameter)
                    throw new InvalidOperationException("Unexpected reference to roundtrip object. Only member accesses can be serialized.");

                return base.VisitParameter(node);
            }

            #endregion

            #endregion
        }

        #endregion

        #endregion
    }
}
