// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Deployable
{
    internal sealed class Requoter : ExpressionVisitor
    {
        //
        // This "requoter" turns a fully bound expression into an unbound expression based on the constructor calling
        // convention of EOP<T, R>, namely (string uri, T args). Any NewExpression referring to this constructor will
        // be normalized into an invocation expression of type IObserver<R> where the name is obtained from the "uri"
        // parameter and the args are passed as-is as an argument of the invocation.
        //
        // Alternative strategies could be employed, e.g. mapping known methods used in MethodCallExpression nodes to
        // a known resource function. Ideally, we won't need this and introduce the ability to quote at the unbound
        // level, but this poses a challenge of requiring access to the bound artifacts in order to determine the need
        // for quoting based on e.g. static typing properties. We currently don't have this information available in
        // any place other the quoter which runs post binding, so we need tricks like a "requoter" to undo the binding
        // operation (kind of a "damage control" situation of going to far only to backtrack from it).
        //

        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(EOP<,>))
            {
                if (node.Arguments.Count > 0 && node.Arguments[0].NodeType == ExpressionType.Constant && node.Arguments[0].Type == typeof(string))
                {
                    var args = Visit(node.Arguments);
                    var uri = (string)((ConstantExpression)args[0]).Value;
                    var remArgs = args.Skip(1).ToArray();
                    var funcArgs = remArgs.Select(a => a.Type).Concat(new[] { typeof(IObserver<>).MakeGenericType(node.Type.GetGenericArguments()[1]) }).ToArray();
                    var funcType = Expression.GetFuncType(funcArgs); // No tupletization right now
                    return Expression.Invoke(Expression.Parameter(funcType, uri), remArgs);
                }

            }

            return base.VisitNew(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            //
            // NB: This performs an optimization which strictly speaking is not needed. We should investigate if we can
            //     remove the need for it over here, upon integrating the expression optimizer in the query engine, where
            //     it can remove these spurious nodes in addition to performing a ton of unrelated optimizations.
            //

            var res = (UnaryExpression)base.VisitUnary(node);

            if (res.NodeType == ExpressionType.Convert && res.Method == null && res.Type == res.Operand.Type)
            {
                return res.Operand;
            }

            return res;
        }
    }
}
