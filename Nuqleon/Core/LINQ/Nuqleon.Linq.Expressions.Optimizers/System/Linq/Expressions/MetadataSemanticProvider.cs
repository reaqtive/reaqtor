// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Semantic information provider for expressions and reflection objects with support for metadata-based lookups.
    /// </summary>
    public class MetadataSemanticProvider : DefaultSemanticProvider
    {
        /// <summary>
        /// Gets a table of members that are deemed to be pure.
        /// </summary>
        public MemberTable PureMembers { get; set; } = new MemberTable();

        /// <summary>
        /// Gets a table of parameters that are used in a <c>const</c> way, i.e. the callee doesn't mutate the values
        /// passed to these parameters.
        /// </summary>
        public ParameterTable ConstParameters { get; set; } = new ParameterTable();

        /// <summary>
        /// Gets a table of types whose instances are immutable.
        /// </summary>
        public TypeTable ImmutableTypes { get; set; } = new TypeTable();

        /// <summary>
        /// Checks if the specified <paramref name="member"/> is pure and doesn't have any side-effects for
        /// evaluation. Members can be retrieved from the <see cref="PureMembers"/> table.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="member"/> is considered pure; otherwise, <c>false</c>.</returns>
        public override bool IsPure(MemberInfo member)
        {
            return base.IsPure(member) || PureMembers.Contains(member);
        }

        /// <summary>
        /// Checks if the specified <paramref name="parameter"/> uses the argument in a <c>const</c> (<c>readonly</c>)
        /// fashion, i.e. it doesn't cause any mutation to the argument passed to it.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns><c>true</c> if the specified parameter has <c>const</c> behavior; otherwise, <c>false</c>.</returns>
        public override bool IsConst(ParameterInfo parameter)
        {
            return base.IsConst(parameter) || ConstParameters.Contains(parameter);
        }

        /// <summary>
        /// Checks if instances of the specified <paramref name="type"/> are immutable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type is immutable; otherwise, <c>false</c>.</returns>
        public override bool IsImmutable(Type type)
        {
            return base.IsImmutable(type) || ImmutableTypes.Contains(type);
        }
    }
}
