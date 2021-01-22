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
using System.Linq.Expressions;

namespace Nuqleon.Serialization
{
    /// <summary>
    /// Defines a serializer for inputs assignable to TInput and with serialized output of type TObject.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    public class Serializer<TInput, TOutput, TContext> : SerializerBase<TInput, TOutput, TContext>
    {
        #region Constructors

        /// <summary>
        /// Creates a new serializer with the given configuration parameters.
        /// </summary>
        /// <param name="tag">Function to tag serialization output with a rule identifier.</param>
        /// <param name="untag">Function to extract a rule identifier and a payload from serialization output.</param>
        /// <param name="newContext">Function to get a new serialization context during serialization.</param>
        /// <param name="addContext">Function to add serialization context to the serialization output.</param>
        /// <param name="getContext">Function to get deserialization context from deserialization input.</param>
        /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
        /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
        public Serializer(Func<TaggedByRule<TOutput>, TOutput> tag, Func<TOutput, TaggedByRule<TOutput>> untag, Func<TContext> newContext, Func<Contextual<TContext, TOutput>, TOutput> addContext, Func<TOutput, Contextual<TContext, TOutput>> getContext, Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
            : base(tag, untag, newContext, addContext, getContext, new RuleTable<TInput, TOutput, TContext>(wrap, unwrap))
        {
        }

        #endregion
    }

    /// <summary>
    /// Defines a serializer for inputs assignable to TInput and with serialized output of type TObject.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    public class SerializerBase<TInput, TOutput, TContext>
    {
        #region Fields

        /// <summary>
        /// Function to tag serialization output with a rule identifier.
        /// </summary>
        private readonly Func<TaggedByRule<TOutput>, TOutput> _tag;

        /// <summary>
        /// Function to extract a rule identifier and a payload from serialization output.
        /// </summary>
        private readonly Func<TOutput, TaggedByRule<TOutput>> _untag;

        /// <summary>
        /// Function to get a new serialization context during serialization.
        /// </summary>
        private readonly Func<TContext> _newContext;

        /// <summary>
        /// Function to add serialization context to the serialization output.
        /// </summary>
        private readonly Func<Contextual<TContext, TOutput>, TOutput> _addContext;

        /// <summary>
        /// Function to get deserialization context from deserialization input.
        /// </summary>
        private readonly Func<TOutput, Contextual<TContext, TOutput>> _getContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new serializer with the given configuration parameters.
        /// </summary>
        /// <param name="tag">Function to tag serialization output with a rule identifier.</param>
        /// <param name="untag">Function to extract a rule identifier and a payload from serialization output.</param>
        /// <param name="newContext">Function to get a new serialization context during serialization.</param>
        /// <param name="addContext">Function to add serialization context to the serialization output.</param>
        /// <param name="getContext">Function to get deserialization context from deserialization input.</param>
        /// <param name="ruleTable">Rule table with serialization and deserialization rules.</param>
        public SerializerBase(Func<TaggedByRule<TOutput>, TOutput> tag, Func<TOutput, TaggedByRule<TOutput>> untag, Func<TContext> newContext, Func<Contextual<TContext, TOutput>, TOutput> addContext, Func<TOutput, Contextual<TContext, TOutput>> getContext, RuleTableBase<TInput, TOutput, TContext> ruleTable)
        {
            _tag = tag ?? throw new ArgumentNullException(nameof(tag));
            _untag = untag ?? throw new ArgumentNullException(nameof(untag));
            _newContext = newContext ?? throw new ArgumentNullException(nameof(newContext));
            _addContext = addContext ?? throw new ArgumentNullException(nameof(addContext));
            _getContext = getContext ?? throw new ArgumentNullException(nameof(getContext));

            Rules = ruleTable ?? throw new ArgumentNullException(nameof(ruleTable));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the rule table.
        /// </summary>
        public RuleTableBase<TInput, TOutput, TContext> Rules { get; }

        #endregion

        #region Methods

        #region Serialize and Deserialize

        /// <summary>
        /// Serializes the given serialization input object.
        /// </summary>
        /// <param name="input">Object to serialize into an object of type TOutput.</param>
        /// <returns>Serialized object.</returns>
        public TOutput Serialize(TInput input)
        {
            var context = _newContext();
            var obj = Serialize_(input, context);
            var res = _addContext(new Contextual<TContext, TOutput> { Context = context, Payload = obj });
            return res;
        }

        /// <summary>
        /// Deserializes the given serialization output object.
        /// </summary>
        /// <param name="output">Object to deserialize into an object of type TInput.</param>
        /// <returns>Deserialized object.</returns>
        public TInput Deserialize(TOutput output)
        {
            var ctx = _getContext(output);
            return Deserialize_(ctx.Payload, ctx.Context);
        }

        #endregion

        #region Private implementation

        /// <summary>
        /// Serializes the given object using the supplied context.
        /// </summary>
        /// <param name="input">Object to serialize.</param>
        /// <param name="context">Mutable context object used during recursive serialization.</param>
        /// <returns>Serialization result.</returns>
        private TOutput Serialize_(TInput input, TContext context)
        {
            var inputType = input is null ? typeof(object) : input.GetType();

            foreach (var type in GetAncestorsAndSelf(inputType))
            {
                foreach (var rule in Rules[type])
                {
                    if (rule.TrySerialize(input, Serialize_, context, out TOutput res))
                        return _tag(new TaggedByRule<TOutput> { Name = rule.Name, Value = res });
                }
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable serialization rule found for type '{0}'.", input != null ? input.GetType() : typeof(object)));
        }

        /// <summary>
        /// Deserializes the given object using the supplied context.
        /// </summary>
        /// <param name="output">Object to deserialize.</param>
        /// <param name="context">Mutable context object used during recursive deserialization.</param>
        /// <returns>Deserialization result.</returns>
        private TInput Deserialize_(TOutput output, TContext context)
        {
            var tag = _untag(output);
            var rule = Rules[tag.Name];
            return rule.Deserialize(tag.Value, Deserialize_, context);
        }

        /// <summary>
        /// Helper method to get the ancestors of the specified type, including base types and implemented interfaces.
        /// Starting with the type itself, the resulting sequence returns the base classes in the order of the ancestry, followed by the interfaces implemented by the type.
        /// </summary>
        /// <param name="type">Type to get the parent hierarchy for.</param>
        /// <returns>Enumerable sequence of the given type's ancestry.</returns>
        /// <remarks>This method doesn't return open generic types for a given closed generic type.</remarks>
        private static IEnumerable<Type> GetAncestorsAndSelf(Type type)
        {
            var t = type;
            while (t != null)
            {
                yield return t;
                t = t.BaseType;
            }

            foreach (var i in type.GetInterfaces())
                yield return i;
        }

        #endregion

        #endregion
    }
}
