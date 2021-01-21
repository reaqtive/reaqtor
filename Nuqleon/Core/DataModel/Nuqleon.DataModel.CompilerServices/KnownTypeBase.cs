// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel
{
    /// <summary>
    /// Base class for types annotated with <see cref="KnownTypeAttribute" /> which provides common convenience operations,
    /// like true by-value equality (as opposed to the default by-reference equality that would cause operators like
    /// DistinctUntilChanged to fail).
    /// </summary>
    /// <typeparam name="T">
    /// Type parameter used for the <see cref="IEquatable{T}"/> implementation.
    /// Derived types should specify the type itself for this generic parameter.
    /// </typeparam>
    /// <example>
    /// <code>
    /// [KnownType]
    /// public sealed class Person : KnownTypeBase&lt;Person&gt;
    /// {
    ///     public string Name { get; }
    ///     public int Age { get; }
    /// }
    /// </code>
    /// </example>
    public class KnownTypeBase<T> : IEquatable<T>
    {
        /// <summary>
        /// Checks if another object has data model-based value equality to this object.
        /// </summary>
        /// <param name="other">The object to compare the current instance to.</param>
        /// <returns>true if the objects have value equality based on the data model; otherwise, false.</returns>
        public bool Equals(T other) => Equals((object)other);

        /// <summary>
        /// Checks if another object has data model-based value equality to this object.
        /// </summary>
        /// <param name="obj">The object to compare the current instance to.</param>
        /// <returns>true if the objects have value equality based on the data model; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null)
            {
                return false;
            }
            else
            {
                return DataTypeObjectEqualityComparer.Default.Equals(this, obj);
            }
        }

        /// <summary>
        /// Gets the data model-based hash code for this object.
        /// </summary>
        /// <returns>The data-model based hash code for this object.</returns>
        public override int GetHashCode() => DataTypeObjectEqualityComparer.Default.GetHashCode(this);
    }
}
