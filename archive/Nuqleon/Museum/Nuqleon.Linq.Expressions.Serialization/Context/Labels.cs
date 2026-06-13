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

namespace Nuqleon.Linq.Expressions.Serialization
{
    using TypeSystem;

    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Tracking facility for labels in statement blocks.
    /// </summary>
    internal class Labels
    {
        #region Fields

        /// <summary>
        /// Maps labels onto label registrations. Used during serialization.
        /// </summary>
        private readonly Dictionary<LabelTarget, LabelRegistration> _serializationLabels;

        /// <summary>
        /// Maps labels onto a unique identifier. Used during serialization.
        /// </summary>
        private readonly Dictionary<LabelTarget, int> _serializationLabelIds;

        /// <summary>
        /// Map of labels, using equality based on a unique identifier. Used during deserialization.
        /// </summary>
        private readonly Dictionary<int, LabelTarget> _deserializationLabels;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new label tracking instance for use during serialization.
        /// </summary>
        public Labels()
        {
            _serializationLabels = new Dictionary<LabelTarget, LabelRegistration>();
            _serializationLabelIds = new Dictionary<LabelTarget, int>();
        }

        /// <summary>
        /// Creates a new label tracking instance for use during deserialization.
        /// </summary>
        /// <param name="deserializationLabels">Map of labels.</param>
        private Labels(Dictionary<int, LabelTarget> deserializationLabels)
        {
            _deserializationLabels = deserializationLabels;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers a label target for serialization.
        /// This method can only be used during serialization.
        /// </summary>
        /// <param name="target">Target label.</param>
        /// <param name="types">type space to lookup label types.</param>
        /// <returns>Label registration JSON representation.</returns>
        public Json.Expression RegisterLabel(LabelTarget target, TypeSpace types)
        {
            if (!_serializationLabelIds.TryGetValue(target, out int id))
            {
                id = _serializationLabelIds.Count;

                var type = types.Register(target.Type).ToJson();
                _serializationLabels[target] = new LabelRegistration(target, type);
                _serializationLabelIds[target] = id;
            }

            return Json.Expression.Number(id.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Retrieves a label target using the given label registration JSON representation.
        /// This method can only be used during deserialization.
        /// </summary>
        /// <param name="target">Label registration JSON representation.</param>
        /// <returns>Label target for the given label registration JSON representation.</returns>
        public LabelTarget GetLabel(Json.Expression target)
        {
            var i = int.Parse((string)((Json.ConstantExpression)target).Value, CultureInfo.InvariantCulture);

            if (!_deserializationLabels.TryGetValue(i, out LabelTarget t))
                throw new InvalidOperationException("Unknown label with index " + i);

            return t;
        }

        #region Conversion to/from JSON

        /// <summary>
        /// Gets the JSON representation of the label tracking instance.
        /// </summary>
        /// <returns>JSON representation of the label tracking instance.</returns>
        public Json.Expression ToJson()
        {
            return Json.Expression.Array(from label in _serializationLabelIds
                                         orderby label.Value
                                         select _serializationLabels[label.Key].ToJson());
        }

        /// <summary>
        /// Reconstructs a label tracking instance from the given JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of a label tracking instance.</param>
        /// <param name="types">Type space to lookup label types.</param>
        /// <returns>Label tracking instance corresponding to the given JSON representation.</returns>
        public static Labels FromJson(Json.Expression json, TypeSpace types)
        {
            var labels = (Json.ArrayExpression)json;
            return new Labels(labels.Elements
                .Select((label, i) => (Label: LabelRegistration.FromJson(label, types), Id: i))
                .ToDictionary(x => x.Id, x => x.Label)
            );
        }

        #endregion

        #endregion

        /// <summary>
        /// Registration of a single label.
        /// </summary>
        private sealed class LabelRegistration
        {
            #region Fields

            /// <summary>
            /// Target label.
            /// </summary>
            private readonly LabelTarget _target;

            /// <summary>
            /// Type reference JSON representation for the target label type.
            /// </summary>
            private readonly Json.Expression _type;

            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new label registration.
            /// </summary>
            /// <param name="target">Target label.</param>
            /// <param name="type">Type reference JSON representation for the target label type.</param>
            public LabelRegistration(LabelTarget target, Json.Expression type)
            {
                _target = target;
                _type = type;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets a JSON representation of the label registration.
            /// </summary>
            /// <returns>JSON representation of the label registration.</returns>
            public Json.Expression ToJson()
            {
                return Json.Expression.Object(new Dictionary<string, Json.Expression>
                {
                    { "Type", _type },
                    { "Name", Json.Expression.String(_target.Name ?? "") },
                });
            }

            /// <summary>
            /// Reconstructors a label registration from a JSON representation.
            /// </summary>
            /// <param name="label">JSON representation of a label registration.</param>
            /// <param name="types">Type space to look up label types.</param>
            /// <returns>Label registration corresponding to the JSON representation.</returns>
            public static LabelTarget FromJson(Json.Expression label, TypeSpace types)
            {
                var jo = (Json.ObjectExpression)label;
                var type = types.Lookup(TypeRef.FromJson(jo.Members["Type"]));
                var name = (string)((Json.ConstantExpression)jo.Members["Name"]).Value;
                return Expression.Label(type, name);
            }

            #endregion
        }
    }
}
