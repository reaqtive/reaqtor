// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

namespace System.Linq.Expressions
{
    internal static class Utils
    {
        private static readonly ConstantExpression s_true = Expression.Constant(true);
        private static readonly ConstantExpression s_false = Expression.Constant(false);

        public static Expression Constant(bool value)
        {
            return value ? s_true : s_false;
        }

        public static Expression Constant(int value)
        {
            return Expression.Constant(value, typeof(int));
        }

        public static Expression Constant(object value, Type type)
        {
            if (type == typeof(bool))
            {
                return Constant((bool)value);
            }
            else if (type == typeof(int))
            {
                return Constant((int)value);
            }
            else if (type == typeof(void))
            {
                return Expression.Empty();
            }

            return Expression.Constant(value, type);
        }

        public static Expression ConvertTo(Expression original, Type type)
        {
            if (original.Type == type)
            {
                return original;
            }

            if (type == typeof(void))
            {
                // REVIEW: Can we just use a Convert node here?

                return Expression.Block(typeof(void), original);
            }
            else
            {
                return Expression.Convert(original, type);
            }
        }
    }
}
