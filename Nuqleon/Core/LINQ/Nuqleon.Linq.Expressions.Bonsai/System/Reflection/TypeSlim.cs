// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a type.
    /// </summary>
    public abstract partial class TypeSlim : IEquatable<TypeSlim>
    {
        internal int _hashCode;

        /// <summary>
        /// Creates a new type representation object.
        /// </summary>
        protected TypeSlim()
        {
        }

        /// <summary>
        /// Gets the kind of the type.
        /// </summary>
        public abstract TypeSlimKind Kind { get; }

        /// <summary>
        /// The equals method for comparing against other class instances.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><b>true</b> if the given instance equals this instance, <b>false</b> otherwise.</returns>
        public virtual bool Equals(TypeSlim other) => TypeSlimEqualityComparer.Default.Equals(this, other);

        /// <summary>
        /// Returns a friendly string representation of the type.
        /// </summary>
        /// <returns>Friendly string representation of the type.</returns>
        public override string ToString() => new TypeSlimPrettyPrinter().Visit(this);

        /// <summary>
        /// Gets a string representation of the type using C# syntax.
        /// The resulting string is not guaranteed to be semantically equivalent and should be used for diagnostic purposes only.
        /// </summary>
        /// <returns>String representation of the type using C# syntax.</returns>
        public string ToCSharpString() => new TypeSlimCSharpPrinter().Visit(this);

        /// <summary>
        /// The default equals method.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns><b>true</b> if the given object equals this object, <b>false</b> otherwise.</returns>
        public sealed override bool Equals(object obj) => obj is TypeSlim other && Equals(other);

        /// <summary>
        /// Returns a hash code for this object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            //
            // NB: See GenericParameterTypeSlim.UnboundHashCode for a place where the
            //     hash code is stable upon initialization of an instance, thus avoiding
            //     entering into the conditional below.
            //

            if (_hashCode == 0)
            {
                _hashCode = TypeSlimEqualityComparer.Default.GetHashCode(this);
            }

            return _hashCode;
        }

        /// <summary>
        /// Operator overload for equality checks between slim types.
        /// </summary>
        /// <param name="typeSlimA">The left slim type.</param>
        /// <param name="typeSlimB">The right slim type.</param>
        /// <returns>
        /// <b>true</b> if the slim types are equal, <b>false</b> otherwise.
        /// </returns>
        public static bool operator ==(TypeSlim typeSlimA, TypeSlim typeSlimB)
        {
            if (typeSlimA is null && typeSlimB is null)
                return true;
            if (typeSlimA is null || typeSlimB is null)
                return false;

            return typeSlimA.Equals(typeSlimB);
        }

        /// <summary>
        /// Operator overload for inequality checks between slim types.
        /// </summary>
        /// <param name="typeSlimA">The left slim type.</param>
        /// <param name="typeSlimB">The right slim type.</param>
        /// <returns>
        /// <b>true</b> if the slim types are not equal, <b>false</b> otherwise.
        /// </returns>
        public static bool operator !=(TypeSlim typeSlimA, TypeSlim typeSlimB) => !(typeSlimA == typeSlimB);

        internal static void RequireNotNull(object value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
