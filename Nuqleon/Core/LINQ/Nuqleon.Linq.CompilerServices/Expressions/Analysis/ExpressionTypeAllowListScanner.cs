// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Expression visitor used to check uses of types against a list of allowed types.
    /// </summary>
    public class ExpressionTypeAllowListScanner : ExpressionTypeAllowListScannerBase
    {
        private readonly HashSet<Type> _entries = new();

        /// <summary>
        /// Creates a new allow list scanner for types. To complete instantiation, initialize the Types property, e.g. by using collection initializers.
        /// </summary>
        public ExpressionTypeAllowListScanner() => Types = new TypeList(AddType);

        /// <summary>
        /// Gets the types which are allowed.
        /// </summary>
        public TypeList Types { get; }

        internal void AddType(Type type) => _entries.Add(type);

        /// <summary>
        /// Checks whether the specified type is supported.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if the type is supported; otherwise, false.</returns>
        protected override bool Check(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (_entries.Contains(type))
            {
                return true;
            }

            if (type.IsGenericType)
            {
                var genDef = type.GetGenericTypeDefinition();
                if (_entries.Contains(genDef))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
