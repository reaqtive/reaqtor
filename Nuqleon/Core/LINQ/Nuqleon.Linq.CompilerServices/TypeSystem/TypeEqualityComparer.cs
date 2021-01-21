// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - April 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Equality comparer for CLR types, based on their structure.
    /// </summary>
    public class TypeEqualityComparer : IEqualityComparer<Type>
    {
        private static readonly IEqualityComparer<Type> s_defaultComparer = EqualityComparer<Type>.Default;

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        public virtual bool Equals(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.IsArray && y.IsArray)
            {
                return EqualsArray(x, y);
            }

            if (x.IsGenericType && y.IsGenericType)
            {
                return EqualsGeneric(x, y);
            }

            if (x.IsGenericParameter && y.IsGenericParameter)
            {
                return EqualsGenericParameter(x, y);
            }

            if (x.IsByRef && y.IsByRef)
            {
                return EqualsByRef(x, y);
            }

            if (x.IsPointer && y.IsPointer)
            {
                return EqualsPointer(x, y);
            }

            return EqualsSimple(x, y);
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsArray(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var rx = x.GetArrayRank();
            var ry = y.GetArrayRank();

            if (rx != ry)
            {
                return false;
            }

            if (rx == 1)
            {
                var kx = x.IsSZArray();
                var ky = y.IsSZArray();

                if (kx ^ ky)
                {
                    return false;
                }

                return EqualsArrayVector(x, y);
            }
            else
            {
                return EqualsArrayMultidimensional(x, y);
            }
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsArrayVector(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var ex = x.GetElementType();
            var ey = y.GetElementType();

            return Equals(ex, ey);
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsArrayMultidimensional(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var ex = x.GetElementType();
            var ey = y.GetElementType();

            return Equals(ex, ey);
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsGeneric(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.IsGenericTypeDefinition)
            {
                if (!y.IsGenericTypeDefinition)
                {
                    return false;
                }

                return EqualsGenericTypeDefinition(x, y);
            }
            else
            {
                return EqualsGenericClosed(x, y);
            }
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsGenericTypeDefinition(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return s_defaultComparer.Equals(x, y);
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsGenericClosed(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return Equals(x.GetGenericTypeDefinition(), y.GetGenericTypeDefinition()) && Equals(x.GetGenericArguments(), y.GetGenericArguments());
        }

        /// <summary>
        /// Checks whether the two given sequences of types are equal.
        /// </summary>
        /// <param name="x">First sequence of types.</param>
        /// <param name="y">Second sequence of types.</param>
        /// <returns>true if both type sequences are equal; otherwise, false.</returns>
        protected bool Equals(IEnumerable<Type> x, IEnumerable<Type> y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(x, y, this);
        }

        /// <summary>
        /// Checks whether the two given arrays of types are equal.
        /// </summary>
        /// <param name="x">First array of types.</param>
        /// <param name="y">Second array of types.</param>
        /// <returns>true if both type arrays are equal; otherwise, false.</returns>
        protected bool Equals(Type[] x, Type[] y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Length != y.Length)
            {
                return false;
            }

            for (var i = 0; i < x.Length; i++)
            {
                if (!Equals(x[i], y[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsGenericParameter(Type x, Type y) => s_defaultComparer.Equals(x, y);

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsByRef(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var ex = x.GetElementType();
            var ey = y.GetElementType();

            return Equals(ex, ey);
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsPointer(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var ex = x.GetElementType();
            var ey = y.GetElementType();

            return Equals(ex, ey);
        }

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected virtual bool EqualsSimple(Type x, Type y) => s_defaultComparer.Equals(x, y);

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        public virtual int GetHashCode(Type obj)
        {
            if (obj == null)
            {
                return 17;
            }

            if (obj.IsArray)
            {
                return GetHashCodeArray(obj);
            }

            if (obj.IsGenericType)
            {
                return GetHashCodeGeneric(obj);
            }

            if (obj.IsGenericParameter)
            {
                return GetHashCodeGenericParameter(obj);
            }

            if (obj.IsByRef)
            {
                return GetHashCodeByRef(obj);
            }

            if (obj.IsPointer)
            {
                return GetHashCodePointer(obj);
            }

            return GetHashCodeSimple(obj);
        }

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeArray(Type obj)
        {
            if (obj == null)
            {
                return 17;
            }

            if (obj.IsSZArray())
            {
                return GetHashCodeArrayVector(obj);
            }
            else
            {
                return GetHashCodeArrayMultidimensional(obj);
            }
        }

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeArrayVector(Type obj) => obj == null ? 17 : GetHashCode(obj.GetElementType()) * 19 + 17;

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeArrayMultidimensional(Type obj) => obj == null ? 17 : (GetHashCode(obj.GetElementType()) * 23 + obj.GetArrayRank().GetHashCode()) * 23 + 17;

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeGeneric(Type obj)
        {
            if (obj == null)
            {
                return 17;
            }

            if (obj.IsGenericTypeDefinition)
            {
                return GetHashCodeGenericTypeDefinition(obj);
            }
            else
            {
                return GetHashCodeGenericClosed(obj);
            }
        }

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeGenericTypeDefinition(Type obj) => obj == null ? 17 : obj.GetHashCode();

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeGenericClosed(Type obj) => obj == null ? 17 : Enumerable.Aggregate(obj.GetGenericArguments(), GetHashCode(obj.GetGenericTypeDefinition()), (h, t) => h * 23 + GetHashCode(t)); // PERF: Eliminate closure here

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeGenericParameter(Type obj) => obj == null ? 17 : obj.GetHashCode();

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeByRef(Type obj) => obj == null ? 17 : GetHashCode(obj.GetElementType()) * 29 + 13;

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodePointer(Type obj) => obj == null ? 17 : GetHashCode(obj.GetElementType()) * 31 + 37;

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected virtual int GetHashCodeSimple(Type obj) => obj == null ? 17 : obj.GetHashCode();
    }
}
