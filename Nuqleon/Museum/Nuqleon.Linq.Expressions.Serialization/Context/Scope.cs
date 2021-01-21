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
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Nuqleon.Json.Expressions;
    using TypeSystem;

    /// <summary>
    /// Scope tracking facility for parameters in lambda expressions and block expressions.
    /// </summary>
    internal class Scope
    {
        #region Constructors

        /// <summary>
        /// Creates a new scope tracking object for use during serialization.
        /// </summary>
        public Scope()
        {
            Serialization = new SerializationState();
        }

        /// <summary>
        /// Creates a new scope tracking object for use during deserialization.
        /// </summary>
        /// <param name="state">Deserialization state.</param>
        private Scope(DeserializationState state)
        {
            Deserialization = state;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the serialization state.
        /// Returns null when the scope tracking is initialized in deserialization mode.
        /// </summary>
        public SerializationState Serialization { get; }

        /// <summary>
        /// Gets the deserialization state.
        /// Returns null when the scope tracking is initialized in serialization mode.
        /// </summary>
        public DeserializationState Deserialization { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the JSON representation of the scope tracking information.
        /// </summary>
        /// <param name="types">Type space used to register referenced types.</param>
        /// <returns>JSON representation of the scope tracking information.</returns>
        public Json.Expression ToJson(TypeSpace types)
        {
            if (Serialization == null)
                throw new InvalidOperationException("Type map constructed for serialization only.");

            return Serialization.ToJson(types);
        }

        /// <summary>
        /// Restores scope tracking information from the given JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of scope tracking information.</param>
        /// <param name="types">Type space used to lookup referenced types.</param>
        /// <returns>Scope tracking instance used during deserialization.</returns>
        public static Scope FromJson(Json.Expression json, TypeSpace types)
        {
            var state = new DeserializationState(json, types);
            return new Scope(state);
        }

        #endregion
    }

    /// <summary>
    /// Representation of an environment frame during serialization.
    /// </summary>
    internal class SerializationFrame
    {
        #region Fields

        /// <summary>
        /// Prefix for parameter name mappings in this frame.
        /// </summary>
        private readonly string _scopeId;

        /// <summary>
        /// List of parameters in the order of addition.
        /// </summary>
        private readonly List<ParameterExpression> _parameters;

        /// <summary>
        /// Mapping of parameters onto names used for referencing purposes during serialization.
        /// This is required because of reference equality rather than name equality semantics of ParameterExpression.
        /// </summary>
        private readonly Dictionary<ParameterExpression, string> _mapping;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new serialization frame with no initial parameters in scope.
        /// </summary>
        /// <param name="scopeId">Prefix for parameter name mappings in this frame.</param>
        public SerializationFrame(string scopeId)
            : this(scopeId, Enumerable.Empty<ParameterExpression>())
        {
        }

        /// <summary>
        /// Creates a new serialization frame with the specified parameters in scope.
        /// </summary>
        /// <param name="scopeId">Prefix for parameter name mappings in this frame.</param>
        /// <param name="parameters">Initial parameters to add to the newly created frame.</param>
        public SerializationFrame(string scopeId, IEnumerable<ParameterExpression> parameters)
        {
            _scopeId = scopeId;
            _parameters = new List<ParameterExpression>();
            _mapping = new Dictionary<ParameterExpression, string>();

            foreach (var parameter in parameters)
                Add(parameter);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the JSON representation of the frame.
        /// </summary>
        /// <param name="types">Type space to register parameter types in.</param>
        /// <returns>JSON representation of the frame.</returns>
        public Json.Expression ToJson(TypeSpace types)
        {
            //
            // Note: keep in sync with DeserializationFrame::.ctor code.
            //
            return Json.Expression.Array(
                from p in _parameters
                select (Json.Expression)Json.Expression.Object(
                    new Dictionary<string, Json.Expression>
                    {
                        { "Name", Json.Expression.String(_mapping[p]) },
                        { "FriendlyName", Json.Expression.String(p.Name) },
                        { "Type", types.Register(p.Type).ToJson() }
                    })
            );
        }

        /// <summary>
        /// Searches for the specified parameter in the current frame, returning its mapped name.
        /// </summary>
        /// <param name="parameter">Parameter to lookup.</param>
        /// <param name="name">Name to refer to the parameter in serialization output.</param>
        /// <returns>true if the parameter was found in the current frame; otherwise, false.</returns>
        public bool TryGetValue(ParameterExpression parameter, out string name)
        {
            return _mapping.TryGetValue(parameter, out name);
        }

        /// <summary>
        /// Searches for the specified parameter in the current frame, returning its mapped name.
        /// If the parameter is not found, it's added to the frame and a new name is returned.
        /// </summary>
        /// <param name="parameter">Parameter to lookup or add.</param>
        /// <returns>Name to refer to the parameter in serialization output.</returns>
        public string GetOrAdd(ParameterExpression parameter)
        {
            if (_mapping.TryGetValue(parameter, out string name))
                return name;

            return Add(parameter);
        }

        /// <summary>
        /// Adds the specified parameter to the frame.
        /// </summary>
        /// <param name="parameter">Parameter to add to the frame.</param>
        /// <returns>Name to refer to the parameter in serialization output.</returns>
        private string Add(ParameterExpression parameter)
        {
            Debug.Assert(!_mapping.ContainsKey(parameter));

            var id = _parameters.Count;
            _parameters.Add(parameter);

            var name = _scopeId + "." + id;
            _mapping[parameter] = name;

            return name;
        }

        #endregion
    }

    /// <summary>
    /// State for scope tracking during serialization.
    /// </summary>
    internal class SerializationState
    {
        #region Fields

        /// <summary>
        /// Stacked environment to track the scope of parameters and maintain their mappings onto names for use during serialization.
        /// </summary>
        private readonly Stack<SerializationFrame> _environment;

        /// <summary>
        /// Map of globals which aren't in scope in any environment frame onto names for use during serialization.
        /// </summary>
        private readonly SerializationFrame _globals;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new serialization state object.
        /// </summary>
        public SerializationState()
        {
            _environment = new Stack<SerializationFrame>();
            _globals = new SerializationFrame("g");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Pushes a new frame, e.g. when starting to process a lambda expression or a block expression.
        /// </summary>
        /// <param name="parameters">Parameters to add to the new frame.</param>
        /// <returns>Frame containing mappings for the specified parameters.</returns>
        public SerializationFrame PushFrame(IEnumerable<ParameterExpression> parameters)
        {
            //
            // E.g. when serializing "a => f(a, (b, c) => g(a, b, c))"
            //
            //  1. Serialization of "a => ..." causes extension of the environment with one frame:
            //                         ^^
            //        [ a -> p0.0 ] <--
            //
            //  2. Serialization of the body "f(a, (b, c) => g(a, b, c))" causes a lookup for "a":
            //                                  ^
            //        [ a -> p0.0 ]
            //               ^^^
            //
            //  3. Serialization of "(b, c) => g(a, b, c)" causes extension of the environment with one frame:
            //
            //        [ b -> p1.0, c -> p1.1 ] <--
            //        [ a -> p0.0 ]
            //
            //  4. Serialization of the body "g(a, b, c)" causes a lookup for "a", "b", and "c", in a top-down.
            //     fashion. Notice if both parameters are reference identical, the scoped lookup causes the
            //     desired effect of shadowing.
            //
            var scope = _environment.Count;
            var frame = new SerializationFrame("p" + scope, parameters);
            _environment.Push(frame);
            return frame;
        }

        /// <summary>
        /// Pops the last frame, e.g. when finishing the processing of a lambda expression or a block expression.
        /// </summary>
        public void PopFrame()
        {
            _environment.Pop();
        }

        /// <summary>
        /// Gets the name to use to refer to the given parameter by performing a lookup in the scoped environment.
        /// If the parameter is not found, it's added to the global parameter table, and an identifier is returned.
        /// </summary>
        /// <param name="parameter">Parameter to lookup in the environment.</param>
        /// <returns>Name to use to refer to the given parameter in serialization output.</returns>
        public string GetParameter(ParameterExpression parameter)
        {
            //
            // Enumeration over the stack of environments performs a search starting from the closest
            // enclosing scope.
            //
            var name = default(string);
            foreach (var frame in _environment)
            {
                if (frame.TryGetValue(parameter, out name))
                    break;
            }

            //
            // If the parameter is not found, we add it to the table of globals.
            //
            if (name == null)
            {
                name = _globals.GetOrAdd(parameter);
            }

            return name;
        }

        /// <summary>
        /// Gets the JSON representation of the scope tracking serialization state.
        /// </summary>
        /// <param name="types">Type space to register and lookup parameter types.</param>
        /// <returns>JSON representation of the scope tracking serialization state.</returns>
        public Json.Expression ToJson(TypeSpace types)
        {
            return _globals.ToJson(types);
        }

        #endregion
    }

    /// <summary>
    /// Representation of an environment frame during deserialization.
    /// </summary>
    internal class DeserializationFrame
    {
        #region Fields

        /// <summary>
        /// List of parameters in the order of retrieval from the JSON representation.
        /// </summary>
        private readonly List<ParameterExpression> _parameters;

        /// <summary>
        /// Mapping of parameter names (used during serialization) onto parameter expression instances.
        /// This is required because of reference equality rather than name equality semantics of ParameterExpression.
        /// </summary>
        private readonly Dictionary<string, ParameterExpression> _mapping;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a deserialization frame from the given JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of a frame.</param>
        /// <param name="types">Type space used to lookup parameter types.</param>
        public DeserializationFrame(Json.Expression json, TypeSpace types)
        {
            _parameters = new List<ParameterExpression>();
            _mapping = new Dictionary<string, ParameterExpression>();

            foreach (Json.ObjectExpression p in ((Json.ArrayExpression)json).Elements)
            {
                //
                // Note: keep in sync with SerializationFrame::ToJson code.
                //
                Add(
                    (string)((Json.ConstantExpression)p.Members["Name"]).Value,
                    types.Lookup(TypeRef.FromJson(p.Members["Type"])),
                    (string)((Json.ConstantExpression)p.Members["FriendlyName"]).Value
                );
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a sequence of parameters in their original serialization order.
        /// </summary>
        public IEnumerable<ParameterExpression> Parameters => _parameters;

        #endregion

        #region Methods

        /// <summary>
        /// Searches for a parameter with the given name in the current frame.
        /// </summary>
        /// <param name="name">Parameter name to lookup.</param>
        /// <param name="parameter">Parameter expression instance matching the specified name.</param>
        /// <returns>true if the parameter was found in the current frame; otherwise, false.</returns>
        public bool TryGetValue(string name, out ParameterExpression parameter)
        {
            return _mapping.TryGetValue(name, out parameter);
        }

        /// <summary>
        /// Adds a mapping entry with the specified parameter.
        /// </summary>
        /// <param name="name">Name of the parameter as it occurs in the serialization output.</param>
        /// <param name="type">Type of the parameter.</param>
        /// <param name="friendlyName">Friendly name to use for the reconstructed parameter instance. Can be null.</param>
        private void Add(string name, Type type, string friendlyName)
        {
            var parameter = Expression.Parameter(type, friendlyName);
            _parameters.Add(parameter);
            _mapping[name] = parameter;
        }

        #endregion
    }

    /// <summary>
    /// State for scope tracking during deserialization.
    /// </summary>
    internal class DeserializationState
    {
        #region Fields

        /// <summary>
        /// Stacked environment to map parameter names onto their expression tree object instance, using proper scoping.
        /// </summary>
        private readonly Stack<DeserializationFrame> _environment;

        /// <summary>
        /// Map of names of globals which aren't in scope in any environment frame onto their expression tree object instance.
        /// </summary>
        private readonly DeserializationFrame _globals;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new deserialization state object.
        /// </summary>
        /// <param name="globals">JSON representation of globals to make available.</param>
        /// <param name="types">Type space for lookup of types.</param>
        public DeserializationState(Json.Expression globals, TypeSpace types)
        {
            _environment = new Stack<DeserializationFrame>();
            _globals = new DeserializationFrame(globals, types);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Pushes a new frame, e.g. when starting to process a lambda expression or a block expression.
        /// </summary>
        /// <param name="json">JSON representation of the serialization frame.</param>
        /// <param name="types">Type space used to lookup parameter types.</param>
        /// <returns>Frame containing mappings for the specified parameters.</returns>
        public DeserializationFrame PushFrame(Json.Expression json, TypeSpace types)
        {
            var frame = new DeserializationFrame(json, types);
            _environment.Push(frame);
            return frame;
        }

        /// <summary>
        /// Pops the last frame, e.g. when finishing the processing of a lambda expression or a block expression.
        /// </summary>
        public void PopFrame()
        {
            _environment.Pop();
        }

        /// <summary>
        /// Gets the parameter expression instance corresponding to the given parameter name used in serialization output.
        /// If the parameter is not found in the scoped environment, a lookyp in the global parameter table is attempted.
        /// </summary>
        /// <param name="name">Parameter name to lookup.</param>
        /// <returns>Parameter expression instance corresponding to given parameter name.</returns>
        public ParameterExpression GetParameter(string name)
        {
            var parameter = default(ParameterExpression);
            foreach (var frame in _environment)
            {
                if (frame.TryGetValue(name, out parameter))
                    break;
            }

            if (parameter == null)
            {
                if (!_globals.TryGetValue(name, out parameter))
                    throw new InvalidOperationException("Unknown parameter " + name);
            }

            return parameter;
        }

        #endregion
    }
}
