// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2017 - Created this file.
//

using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Represents a table with <see cref="Type"/> objects.
    /// </summary>
    public partial class TypeTable : IEnumerable<Type>
    {
        /// <summary>
        /// Indicates whether the table is read-only.
        /// </summary>
        private bool _readOnly;

        /// <summary>
        /// A set of types.
        /// </summary>
        private readonly HashSet<Type> Types = new();

        /// <summary>
        /// Marks the current type table as read-only, preventing subsequent mutation.
        /// </summary>
        /// <returns>The current type table after being marked as read-only.</returns>
        public TypeTable ToReadOnly()
        {
            _readOnly = true;
            return this;
        }

        /// <summary>
        /// Copies the entries in the specified type <paramref name="table"/> to the current table.
        /// </summary>
        /// <param name="table">The type table whose entries to copy.</param>
        public void Add(TypeTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            CheckReadOnly();

            foreach (var type in table.Types)
            {
                Types.Add(type);
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="type"/> to the table.
        /// </summary>
        /// <param name="type">The type to add to the table.</param>
        public void Add(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            CheckReadOnly();

            Types.Add(type);
        }

        /// <summary>
        /// Gets a sequence of types in the current type table.
        /// </summary>
        /// <returns>A sequence of types in the current type table.</returns>
        public IEnumerator<Type> GetEnumerator() => Types.GetEnumerator();

        /// <summary>
        /// Gets a sequence of types in the current type table.
        /// </summary>
        /// <returns>A sequence of types in the current type table.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Checks if the specified <paramref name="type"/> is present in the table.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> is present in the table; otherwise, <c>false</c>.</returns>
        public bool Contains(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (Types.Contains(type))
            {
                return true;
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var def = type.GetGenericTypeDefinition();

                if (Types.Contains(def))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the current type table is marked as read-only.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current type table is marked as read-only.
        /// </exception>
        private void CheckReadOnly()
        {
            if (_readOnly)
                throw new InvalidOperationException("The table is marked as read-only.");
        }
    }
}
