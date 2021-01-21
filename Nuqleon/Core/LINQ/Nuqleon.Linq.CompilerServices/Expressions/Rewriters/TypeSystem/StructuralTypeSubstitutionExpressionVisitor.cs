// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Globalization;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Visitor to rewrite the expression nodes whose type is a structural type.
    /// </summary>
    public class StructuralTypeSubstitutionExpressionVisitor : TypeSubstitutionExpressionVisitor
    {
        /// <summary>
        /// Creates a new structural type substitution visitor using the specified type map.
        /// </summary>
        /// <param name="typeMap">Dictionary mapping original types to their rewrite targets.</param>
        public StructuralTypeSubstitutionExpressionVisitor(IDictionary<Type, Type> typeMap)
            : base(typeMap)
        {
        }

        // TODO: There is a known limitation that this visitor does not support conversion of
        //       all constants that are data model compliant. E.g. list, array, and tuple constants
        //       containing structural types will fail to convert.

        /// <summary>
        /// Converts constant nodes containing a rewritten type.
        /// </summary>
        /// <param name="originalValue">Original constant expression node value.</param>
        /// <param name="newType">New structural type to rewrite the constant to.</param>
        /// <returns>Resulting object obtained from converting the original value to the new type.</returns>
        protected override object ConvertConstant(object originalValue, Type newType)
        {
            if (AreStructurallyComparable(originalValue, newType))
            {
                return ConvertConstantStructural(originalValue, newType);
            }

            return base.ConvertConstant(originalValue, newType);
        }

        /// <summary>
        /// Converts values of structural types to a new representation.
        /// </summary>
        /// <param name="originalValue">Original constant expression node value.</param>
        /// <param name="newType">New structural type to rewrite the constant to.</param>
        /// <returns>Resulting object obtained from converting the original value to the new type.</returns>
        protected virtual object ConvertConstantStructural(object originalValue, Type newType)
        {
            if (newType.IsAnonymousType())
            {
                return ConvertConstantAnonymous(originalValue, newType);
            }
            else if (newType.IsRecordType())
            {
                return ConvertConstantRecord(originalValue, newType);
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expected either anonymous or record type, instead received '{0}'.", newType));
            }
        }

        private object ConvertConstantAnonymous(object originalValue, Type newType)
        {
            var originalType = originalValue.GetType();
            var originalProps = originalType.GetProperties();
            var newCtor = newType.GetConstructors().Single();
            var newCtorParams = newCtor.GetParameters();

            if (originalProps.Select(p => p.Name).Intersect(newCtorParams.Select(p => p.Name)).Count() != originalProps.Length)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find mappings for all properties in type '{0}' on type '{1}'.", originalType, newType));

            var originalValueMap = new Dictionary<string, object>();
            foreach (var originalProperty in originalProps)
            {
                originalValueMap.Add(originalProperty.Name, originalProperty.GetValue(originalValue));
            }

            var newPropertyValues = new object[newCtorParams.Length];
            for (var i = 0; i < newCtorParams.Length; ++i)
            {
                var newParam = newCtorParams[i];
                var oldPropertyValue = originalValueMap[newParam.Name];
                newPropertyValues[i] = ConvertConstant(oldPropertyValue, newParam.ParameterType);
            }

            return Activator.CreateInstance(newType, newPropertyValues);
        }

        private object ConvertConstantRecord(object originalValue, Type newType)
        {
            var originalType = originalValue.GetType();
            var originalProps = originalType.GetProperties();
            var newProps = newType.GetProperties();

            if (originalProps.Select(p => p.Name).Intersect(newProps.Select(p => p.Name)).Count() != originalProps.Length)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find mappings for all properties in type '{0}' on type '{1}'.", originalType, newType));

            var newValue = Activator.CreateInstance(newType);

            var originalValueMap = new Dictionary<string, object>();
            foreach (var originalProperty in originalProps)
            {
                originalValueMap.Add(originalProperty.Name, originalProperty.GetValue(originalValue));
            }

            foreach (var newProperty in newProps)
            {
                var oldPropertyValue = originalValueMap[newProperty.Name];
                newProperty.SetValue(newValue, ConvertConstant(oldPropertyValue, newProperty.PropertyType));
            }

            return newValue;
        }

        /// <summary>
        /// Checks whether the value's type is structurally comparable with the specified type.
        /// </summary>
        /// <param name="originalValue">Value whose type to check for structural comparability with the specified type.</param>
        /// <param name="newType">Candidate type the value will get converted to.</param>
        /// <returns><c>true</c> if the value's type is structurally comparable with the specified type; otherwise, <c>false</c>.</returns>
        protected virtual bool AreStructurallyComparable(object originalValue, Type newType)
        {
            if (originalValue == null)
            {
                return false;
            }

            var originalType = originalValue.GetType();
            if (originalType.IsAnonymousType() && newType.IsAnonymousType() ||
                originalType.IsRecordType() && newType.IsRecordType())
            {
                return true;
            }

            return false;
        }
    }
}
