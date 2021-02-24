// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/05/2017 - Created this type.
//

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Represents the type of a <see cref="Access"/> operator.
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// The operator represents a field access.
        /// </summary>
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
        Field,

        /// <summary>
        /// The operator represents an indexing operation into a single-dimensional array.
        /// </summary>
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
        VectorElement,

        /// <summary>
        /// The operator represents an indexing operation into a multi-dimensional array.
        /// </summary>
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
        MultidimensionalArrayElement,

        /// <summary>
        /// The operator represents a composite access.
        /// </summary>
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
        Composite,
    }
}
