// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;

namespace Nuqleon.Serialization
{
    /// <summary>
    /// Abstract base class for serialization/deserialization rules.
    /// </summary>
    /// <typeparam name="TInput">Type of the objects to serialize.</typeparam>
    /// <typeparam name="TOutput">Type of the serialization output.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    public abstract class Rule<TInput, TOutput, TContext>
    {
        #region Constructors

        /// <summary>
        /// Creates a new serialization/deserialization rule.
        /// </summary>
        /// <param name="name">Name of the rule, which needs to be unique in a serialization rule table.</param>
        protected internal Rule(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the rule.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of input processed by this rule.
        /// </summary>
        public virtual Type InputType => typeof(TInput);

        #endregion

        #region Abstract methods

        /// <summary>
        /// Tries to serialize the given object. If the rule doesn't match the object, this method returns false. Otherwise, the serialization result is available through the output parameter.
        /// </summary>
        /// <param name="input">Object to attempt to serialize.</param>
        /// <param name="recurse">Recursive serialization function.</param>
        /// <param name="context">Serialization context.</param>
        /// <param name="result">Result of the serialization.</param>
        /// <returns>true if the rule applies and serialization was successful; otherwise, false.</returns>
        public abstract bool TrySerialize(TInput input, Func<TInput, TContext, TOutput> recurse, TContext context, out TOutput result);

        /// <summary>
        /// Deserializes the given object. This method should only be called if the input was obtained by a successful call to TrySerialize for this rule instance.
        /// </summary>
        /// <param name="output">Object to deserialize.</param>
        /// <param name="recurse">Recursive deserialization function.</param>
        /// <param name="context">Deserialization context.</param>
        /// <returns>Deserialized object.</returns>
        public abstract TInput Deserialize(TOutput output, Func<TOutput, TContext, TInput> recurse, TContext context);

        #endregion
    }

    /// <summary>
    /// Base class to represent a predicated serialization/deserialization rule for an object of type T.
    /// </summary>
    /// <typeparam name="TInput">Type of the objects to serialize.</typeparam>
    /// <typeparam name="TOutput">Type of the serialization output.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
    public abstract class FilteredRuleBase<TInput, TOutput, TContext, T> : Rule<TInput, TOutput, TContext>
        where T : TInput
    {
        #region Fields

        /// <summary>
        /// Predicate to check whether the serialization rule applies to the given input object.
        /// </summary>
        private readonly Func<T, bool> _filter;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new rule with the given predicate, serialization and deserialization functions.
        /// </summary>
        /// <param name="name">Name of the rule, which needs to be unique in a serialization rule table.</param>
        /// <param name="filter">Predicate to check whether the serialization rule applies to the given input object.</param>
        protected FilteredRuleBase(string name, Func<T, bool> filter)
            : base(name)
        {
            _filter = filter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of input processed by this rule.
        /// </summary>
        public override Type InputType => typeof(T);

        #endregion

        /// <summary>
        /// Tries to serialize the given object. If the rule doesn't match the object, this method returns false. Otherwise, the serialization result is available through the output parameter.
        /// </summary>
        /// <param name="input">Object to attempt to serialize.</param>
        /// <param name="recurse">Recursive serialization function.</param>
        /// <param name="context">Serialization context.</param>
        /// <param name="result">Result of the serialization.</param>
        /// <returns>true if the rule applies and serialization was successful; otherwise, false.</returns>
        public override bool TrySerialize(TInput input, Func<TInput, TContext, TOutput> recurse, TContext context, out TOutput result)
        {
            if ((input == null && typeof(TInput) == typeof(object) || input is T) && _filter((T)input))
            {
                result = SerializeCore((T)input, recurse, context);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <param name="input">Object to serialize.</param>
        /// <param name="recurse">Recursive serialization function.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Result of the serialization.</returns>
        protected abstract TOutput SerializeCore(T input, Func<TInput, TContext, TOutput> recurse, TContext context);
    }

    /// <summary>
    /// Class to represent a serialization/deserialization rule for an object of type T.
    /// </summary>
    /// <typeparam name="TInput">Type of the objects to serialize.</typeparam>
    /// <typeparam name="TOutput">Type of the serialization output.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    /// <typeparam name="T">Type of the object to serialize and deserialize. Needs to derive from TInput.</typeparam>
    public class Rule<TInput, TOutput, TContext, T> : FilteredRuleBase<TInput, TOutput, TContext, T>
        where T : TInput
    {
        #region Fields

        /// <summary>
        /// Serialization function.
        /// </summary>
        private readonly Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> _serialize;

        /// <summary>
        /// Deserialization function.
        /// </summary>
        private readonly Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> _deserialize;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new rule with the given predicate, serialization and deserialization functions.
        /// </summary>
        /// <param name="name">Name of the rule, which needs to be unique in a serialization rule table.</param>
        /// <param name="filter">Predicate to check whether the serialization rule applies to the given input object.</param>
        /// <param name="serialize">Serialization function.</param>
        /// <param name="deserialize">Deserialization function.</param>
        internal Rule(string name, Func<T, bool> filter, Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> serialize, Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> deserialize)
            : base(name, filter)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }

        #endregion

        #region Abstract method overrides

        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <param name="input">Object to serialize.</param>
        /// <param name="recurse">Recursive serialization function.</param>
        /// <param name="context">Serialization context.</param>
        /// <returns>Result of the serialization.</returns>
        protected override TOutput SerializeCore(T input, Func<TInput, TContext, TOutput> recurse, TContext context)
        {
            return _serialize(input, recurse, context);
        }

        /// <summary>
        /// Deserializes the given object. This method should only be called if the input was obtained by a successful call to TrySerialize for this rule instance.
        /// </summary>
        /// <param name="output">Object to deserialize.</param>
        /// <param name="recurse">Recursive deserialization function.</param>
        /// <param name="context">Deserialization context.</param>
        /// <returns>Deserialized object.</returns>
        public override TInput Deserialize(TOutput output, Func<TOutput, TContext, TInput> recurse, TContext context)
        {
            return _deserialize(output, recurse, context);
        }

        #endregion
    }
}
