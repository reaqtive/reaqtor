// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;

#if USE_SLIM
namespace System.Linq.CompilerServices.Bonsai
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using Type = System.Reflection.TypeSlim;

    #endregion
#endif

    /// <summary>
    /// Substitutes occurrences of the specified types in a given type by recursing over its structure.
    /// Matches for substitution are based on referential equality for types. In particular, subtyping relationships are not taken into consideration.
    /// </summary>
#if USE_SLIM
    public class TypeSlimSubstitutor : System.Reflection.TypeSlimVisitor
#else
    public class TypeSubstitutor : TypeVisitor
#endif
    {
        private readonly IDictionary<Type, Type> _map;

        /// <summary>
        /// Creates a new type substitutor with the specified map for replacements.
        /// </summary>
        /// <param name="map">Map used for replacements. Each key represents a types to be substituted by its corresponding value.</param>
#if USE_SLIM
        public TypeSlimSubstitutor(IDictionary<Type, Type> map)
#else
        public TypeSubstitutor(IDictionary<Type, Type> map)
#endif
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        /// <summary>
        /// Visits a CLR type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        public override Type Visit(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (_map.TryGetValue(type, out Type res))
            {
                return res;
            }

            return base.Visit(type);
        }
    }
}
