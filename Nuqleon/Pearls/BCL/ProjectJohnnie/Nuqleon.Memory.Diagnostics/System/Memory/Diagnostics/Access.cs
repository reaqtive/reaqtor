// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/05/2017 - Created this type.
//

using System.Linq.Expressions;
using System.Reflection;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Represents the access pattern used to traverse an object graph.
    /// </summary>
    public abstract class Access
    {
        /// <summary>
        /// Gets the access type.
        /// </summary>
        public abstract AccessType AccessType { get; }

        /// <summary>
        /// Creates a field access.
        /// </summary>
        /// <param name="field">The field to access.</param>
        /// <returns>An object representing a field access.</returns>
        /// <example>
        /// The following example creates an edge representing accessing the <c>_ticks</c> field on
        /// a <see cref="TimeSpan"/> instance.
        /// <code>
        /// Edge.Create(
        ///     new TimeSpan(3, 14, 15),
        ///     Access.Field(
        ///         typeof(TimeSpan).GetField("_ticks", BindingFlags.NonPublic | BindingFlags.Instance)
        ///     )
        /// )
        /// </code>
        /// </example>
        public static FieldAccess Field(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return new FieldAccess(field);
        }

        /// <summary>
        /// Creates a vector element access.
        /// </summary>
        /// <param name="index">The index of the element to access.</param>
        /// <returns>An object representing a single-dimensional array element access.</returns>
        /// <example>
        /// The following example creates an edge representing accessing the element at index <c>5</c>
        /// in an array.
        /// <code>
        /// Edge.Create(
        ///     new[] { 2, 3, 5, 7, 11, 13, 17, 19 },
        ///     Access.VectorElement(5)
        /// )
        /// </code>
        /// </example>
        public static VectorElementAccess VectorElement(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new VectorElementAccess(index);
        }

        /// <summary>
        /// Creates a multi-dimensional array element access.
        /// </summary>
        /// <param name="indexes">The indexes of the element to access.</param>
        /// <returns>An object representing a multi-dimensional array element access.</returns>
        /// <example>
        /// The following example creates an edge representing accessing the element at indexes <c>1</c>
        /// and <c>2</c> in multi-dimensional array.
        /// <code>
        /// Edge.Create(
        ///     new int[2, 3] { { 2, 3, 5 }, { 7, 11, 13 } },
        ///     Access.MultidimensionalArrayElementAccess(1, 2)
        /// )
        /// </code>
        /// </example>
        public static MultidimensionalArrayElementAccess MultidimensionalArrayElement(params int[] indexes)
        {
            if (indexes == null)
                throw new ArgumentNullException(nameof(indexes));
            if (indexes.Length == 0)
                throw new ArgumentException("Indexes should not be empty.", nameof(indexes));

            for (var i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] < 0)
                    throw new ArgumentOutOfRangeException(nameof(indexes) + "[" + i + "]");
            }

            return new MultidimensionalArrayElementAccess(indexes);
        }

        /// <summary>
        /// Creates a composite access.
        /// </summary>
        /// <param name="accesses">The sequence of accesses to compose left-to-right.</param>
        /// <returns>An object representing a composite access.</returns>
        /// <example>
        /// The following example creates an edge representing two successive field accesses to
        /// obtain the <c>m_dateTime</c> field of a <see cref="DateTimeOffset"/> value, followed
        /// by obtaining the <c>dateData</c> field on <see cref="DateTime"/>.
        /// <code>
        /// Edge.Create(
        ///     DateTimeOffset.Now,
        ///     Access.Composite(
        ///         Access.Field(
        ///             typeof(DateTimeOffset).GetField("m_dateTime", BindingFlags.NonPublic | BindingFlags.Instance)
        ///         ),
        ///         Access.Field(
        ///             typeof(DateTime).GetField("dateData", BindingFlags.NonPublic | BindingFlags.Instance)
        ///         )
        ///     )
        /// )
        /// </code>
        /// </example>
        public static CompositeAccess Composite(params Access[] accesses)
        {
            if (accesses == null)
                throw new ArgumentNullException(nameof(accesses));
            if (accesses.Length == 0)
                throw new ArgumentException("Accesses should not be empty.", nameof(accesses));

            for (var i = 0; i < accesses.Length; i++)
            {
                if (accesses[i] == null)
                    throw new ArgumentNullException(nameof(accesses) + "[" + i + "]");
            }

            return new CompositeAccess(accesses);
        }

        /// <summary>
        /// Converts the access to an expression applied to the specified expression representing an object instance.
        /// </summary>
        /// <param name="obj">The expression representing an object instance to apply the access to.</param>
        /// <returns>An expression that represents applying the access to the specified object instance.</returns>
        public abstract Expression ToExpression(Expression obj);

        /// <summary>
        /// Applies the access to the specified object <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the access to.</param>
        /// <returns>The result of applying the access to the specified object.</returns>
        public abstract object Apply(object obj);
    }
}
