// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009, June 2013, June 2014 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Expression tree node representing a JSON object.
    /// </summary>
    public sealed class ObjectExpression : Expression
    {
        /// <summary>
        /// Creates a new object expression tree node object with the given members.
        /// </summary>
        /// <param name="members">Object members.</param>
        internal ObjectExpression(IDictionary<string, Expression> members) => Members = new ReadOnlyDictionary<string, Expression>(members);

        /// <summary>
        /// Gets the members defined on the object.
        /// </summary>
        public IReadOnlyDictionary<string, Expression> Members { get; }

        /// <summary>
        /// Gets the type of the JSON expression tree node.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Object;

        /// <summary>
        /// Appends the JSON fragment corresponding to the expression tree node to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal override void ToStringCore(StringBuilder builder)
        {
            builder.Append('{');

            var n = Members.Count;

            if (n > 0)
            {
                var i = 0;
                foreach (var member in Members)
                {
                    builder.AppendJsonString(member.Key);
                    builder.Append(':');
                    member.Value.ToStringCore(builder);

                    if (i < n - 1)
                    {
                        builder.Append(',');
                    }

                    i++;
                }
            }

            builder.Append('}');
        }
    }
}
