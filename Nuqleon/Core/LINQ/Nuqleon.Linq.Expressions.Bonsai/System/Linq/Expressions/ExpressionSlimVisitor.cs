// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Visitor for lightweight expression trees.
    /// </summary>
    public class ExpressionSlimVisitor
    {
        /// <summary>
        /// Visits the specified expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        public virtual ExpressionSlim Visit(ExpressionSlim node)
        {
            if (node != null)
            {
                return node.Accept(this);
            }

            return null;
        }

        /// <summary>
        /// Visits a binary expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitBinary(BinaryExpressionSlim node)
        {
            return node.Update(Visit(node.Left), VisitAndConvert(node.Conversion), Visit(node.Right));
        }

        /// <summary>
        /// Visits a block expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitBlock(BlockExpressionSlim node)
        {
            return node.Update(VisitAndConvert(node.Variables), Visit(node.Expressions));
        }

        /// <summary>
        /// Visits a block expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual CatchBlockSlim VisitCatchBlock(CatchBlockSlim node)
        {
            return node.Update(VisitAndConvert(node.Variable), Visit(node.Filter), Visit(node.Body));
        }

        /// <summary>
        /// Visits a conditional expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitConditional(ConditionalExpressionSlim node)
        {
            return node.Update(Visit(node.Test), Visit(node.IfTrue), Visit(node.IfFalse));
        }

        /// <summary>
        /// Visits a constant expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitConstant(ConstantExpressionSlim node)
        {
            return node;
        }

        /// <summary>
        /// Visits a default expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitDefault(DefaultExpressionSlim node)
        {
            return node;
        }

        /// <summary>
        /// Visits an element initializer node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ElementInitSlim VisitElementInit(ElementInitSlim node)
        {
            return node.Update(Visit(node.Arguments));
        }

        /// <summary>
        /// Visits an goto expression node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitGoto(GotoExpressionSlim node)
        {
            return node.Update(VisitIfNotNull(node.Target, VisitLabelTarget), Visit(node.Value));
        }

        /// <summary>
        /// Visits an index expression node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitIndex(IndexExpressionSlim node)
        {
            return node.Update(Visit(node.Object), Visit(node.Arguments));
        }

        /// <summary>
        /// Visits an invocation expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitInvocation(InvocationExpressionSlim node)
        {
            var expression = Visit(node.Expression);
            var arguments = VisitArguments(node);

            if (expression != node.Expression || arguments != null)
            {
                return node.Rewrite(expression, arguments);
            }

            return node;
        }

        /// <summary>
        /// Visits an invocation expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitLabel(LabelExpressionSlim node)
        {
            return node.Update(VisitIfNotNull(node.Target, VisitLabelTarget), Visit(node.DefaultValue));
        }

        /// <summary>
        /// Visits an invocation expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual LabelTargetSlim VisitLabelTarget(LabelTargetSlim node)
        {
            return node;
        }

        /// <summary>
        /// Visits a lambda expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitLambda(LambdaExpressionSlim node)
        {
            return node.Update(Visit(node.Body), VisitAndConvert(node.Parameters));
        }

        /// <summary>
        /// Visits a list initializer expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitListInit(ListInitExpressionSlim node)
        {
            return node.Update(VisitAndConvert(node.NewExpression), Visit(node.Initializers, VisitElementInit));
        }

        /// <summary>
        /// Visits a loop expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitLoop(LoopExpressionSlim node)
        {
            return node.Update(VisitIfNotNull(node.BreakLabel, VisitLabelTarget), VisitIfNotNull(node.ContinueLabel, VisitLabelTarget), Visit(node.Body));
        }

        /// <summary>
        /// Visits a member lookup expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitMember(MemberExpressionSlim node)
        {
            return node.Update(Visit(node.Expression));
        }

        /// <summary>
        /// Visits a member assignment binding node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual MemberAssignmentSlim VisitMemberAssignment(MemberAssignmentSlim node)
        {
            return node.Update(Visit(node.Expression));
        }

        /// <summary>
        /// Visits a member binding node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual MemberBindingSlim VisitMemberBinding(MemberBindingSlim node)
        {
            return node.Accept(this);
        }

        /// <summary>
        /// Visits a member initializer expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitMemberInit(MemberInitExpressionSlim node)
        {
            return node.Update(VisitAndConvert(node.NewExpression), Visit(node.Bindings, VisitMemberBinding));
        }

        /// <summary>
        /// Visits a member list binding node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual MemberListBindingSlim VisitMemberListBinding(MemberListBindingSlim node)
        {
            return node.Update(Visit(node.Initializers, VisitElementInit));
        }

        /// <summary>
        /// Visits a member member binding node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual MemberMemberBindingSlim VisitMemberMemberBinding(MemberMemberBindingSlim node)
        {
            return node.Update(Visit(node.Bindings, VisitMemberBinding));
        }

        /// <summary>
        /// Visits a method call expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitMethodCall(MethodCallExpressionSlim node)
        {
            var @object = Visit(node.Object);
            var arguments = VisitArguments(node);

            if (@object != node.Object || arguments != null)
            {
                return node.Rewrite(@object, arguments);
            }

            return node;
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (Name of expression tree node.)

        /// <summary>
        /// Visits an object creation expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitNew(NewExpressionSlim node)
        {
            var arguments = VisitArguments(node);

            if (arguments != null)
            {
                return node.Rewrite(arguments);
            }

            return node;
        }

#pragma warning restore CA1711
#pragma warning restore IDE0079

        /// <summary>
        /// Visits an array creation expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitNewArray(NewArrayExpressionSlim node)
        {
            return node.Update(Visit(node.Expressions));
        }

        /// <summary>
        /// Visits a parameter expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitParameter(ParameterExpressionSlim node)
        {
            return node;
        }

        /// <summary>
        /// Visits a switch expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitSwitch(SwitchExpressionSlim node)
        {
            return node.Update(Visit(node.SwitchValue), Visit(node.Cases, VisitSwitchCase), Visit(node.DefaultBody));
        }

        /// <summary>
        /// Visits a switch case tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual SwitchCaseSlim VisitSwitchCase(SwitchCaseSlim node)
        {
            return node.Update(Visit(node.TestValues), Visit(node.Body));
        }

        /// <summary>
        /// Visits a try expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitTry(TryExpressionSlim node)
        {
            return node.Update(Visit(node.Body), Visit(node.Handlers, VisitCatchBlock), Visit(node.Finally), Visit(node.Fault));
        }

        /// <summary>
        /// Visits a type binary expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitTypeBinary(TypeBinaryExpressionSlim node)
        {
            return node.Update(Visit(node.Expression));
        }

        /// <summary>
        /// Visits a unary expression tree node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual ExpressionSlim VisitUnary(UnaryExpressionSlim node)
        {
            return node.Update(Visit(node.Operand));
        }

        /// <summary>
        /// Visits and converts an expression tree node.
        /// </summary>
        /// <typeparam name="T">Type of the expression to convert to.</typeparam>
        /// <param name="expression">Node to visit.</param>
        /// <returns>Result of visiting and converting the node.</returns>
        public T VisitAndConvert<T>(T expression)
            where T : ExpressionSlim
        {
            return VisitAndConvert<T>(expression, callerName: null);
        }

        /// <summary>
        /// Visits and converts an expression tree node.
        /// </summary>
        /// <typeparam name="T">Type of the expression to convert to.</typeparam>
        /// <param name="expression">Node to visit.</param>
        /// <param name="callerName">Name of the caller.</param>
        /// <returns>Result of visiting and converting the node.</returns>
        public T VisitAndConvert<T>(T expression, string callerName)
            where T : ExpressionSlim
        {
            if (expression == null)
            {
                return default;
            }

            if (Visit(expression) is T newNode)
            {
                return newNode;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Node must rewrite the the same type (caller = '{0}').", callerName ?? "<unspecified>"));
        }

        /// <summary>
        /// Visits a collection of expression tree nodes.
        /// </summary>
        /// <param name="nodes">Nodes to visit.</param>
        /// <returns>Result of visiting the nodes.</returns>
        public ReadOnlyCollection<ExpressionSlim> Visit(ReadOnlyCollection<ExpressionSlim> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var res = default(ExpressionSlim[]);

            var n = nodes.Count;
            for (int i = 0; i < n; i++)
            {
                var oldNode = nodes[i];
                var newNode = Visit(oldNode);

                if (res != null)
                {
                    res[i] = newNode;
                }
                else
                {
                    if (oldNode != newNode)
                    {
                        res = new ExpressionSlim[n];

                        for (int j = 0; j < i; j++)
                        {
                            res[j] = nodes[j];
                        }

                        res[i] = newNode;
                    }
                }
            }

            if (res != null)
            {
                return new TrueReadOnlyCollection<ExpressionSlim>(/* transfer ownership */ res);
            }

            return nodes;
        }

        /// <summary>
        /// Visits and converts a collection of expression tree nodes.
        /// </summary>
        /// <typeparam name="T">Type of the expression to convert to.</typeparam>
        /// <param name="nodes">Nodes to visit.</param>
        /// <returns>Result of visiting and converting the nodes.</returns>
        public ReadOnlyCollection<T> VisitAndConvert<T>(ReadOnlyCollection<T> nodes)
            where T : ExpressionSlim
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var res = default(T[]);

            var n = nodes.Count;
            for (int i = 0; i < n; i++)
            {
                var oldNode = nodes[i];
                var newNode = Visit(oldNode) as T;

                if (res != null)
                {
                    res[i] = newNode;
                }
                else
                {
                    if (!ReferenceEquals(oldNode, newNode))
                    {
                        res = new T[n];

                        for (int j = 0; j < i; j++)
                        {
                            res[j] = nodes[j];
                        }

                        res[i] = newNode;
                    }
                }
            }

            if (res != null)
            {
                return new TrueReadOnlyCollection<T>(/* transfer ownership */ res);
            }

            return nodes;
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1801 // Remove unused parameter

        /// <summary>
        /// Visits and converts a collection of expression tree nodes.
        /// </summary>
        /// <typeparam name="T">Type of the expression to convert to.</typeparam>
        /// <param name="nodes">Nodes to visit.</param>
        /// <param name="callerName">Name of the caller.</param>
        /// <returns>Result of visiting and converting the nodes.</returns>
        public ReadOnlyCollection<T> VisitAndConvert<T>(ReadOnlyCollection<T> nodes, string callerName)
            where T : ExpressionSlim
        {
            return VisitAndConvert(nodes);
        }

#pragma warning restore CA1801
#pragma warning restore IDE0060
#pragma warning restore IDE0079

        /// <summary>
        /// Visits a collection of objects using the specified visit function.
        /// </summary>
        /// <typeparam name="T">Type of the objects in the collection.</typeparam>
        /// <param name="nodes">Nodes to visit.</param>
        /// <param name="elementVisitor">Function to visit each of the elements in the collection.</param>
        /// <returns>Result of visiting the nodes.</returns>
        public static ReadOnlyCollection<T> Visit<T>(ReadOnlyCollection<T> nodes, Func<T, T> elementVisitor)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));
            if (elementVisitor == null)
                throw new ArgumentNullException(nameof(elementVisitor));

            var res = default(T[]);

            var n = nodes.Count;
            for (int i = 0; i < n; i++)
            {
                var oldNode = nodes[i];
                var newNode = elementVisitor(oldNode);

                if (res != null)
                {
                    res[i] = newNode;
                }
                else
                {
                    if (!ReferenceEquals(oldNode, newNode))
                    {
                        res = new T[n];

                        for (int j = 0; j < i; j++)
                        {
                            res[j] = nodes[j];
                        }

                        res[i] = newNode;
                    }
                }
            }

            if (res != null)
            {
                return new TrueReadOnlyCollection<T>(/* transfer ownership */ res);
            }

            return nodes;
        }

        /// <summary>
        /// Visits the specified node using the specified visitor function if it's not null.
        /// </summary>
        /// <typeparam name="T">Type of the node to visit.</typeparam>
        /// <param name="node">Node to visit.</param>
        /// <param name="nodeVisitor">Visitor function to apply to the node.</param>
        /// <returns>Result of applying the visitor function to the node.</returns>
        protected static T VisitIfNotNull<T>(T node, Func<T, T> nodeVisitor)
        {
            if (nodeVisitor == null)
                throw new ArgumentNullException(nameof(nodeVisitor));

            if (node != null)
            {
                return nodeVisitor(node);
            }

            return default;
        }

        /// <summary>
        /// Visits the arguments in an argument provider.
        /// </summary>
        /// <param name="nodes">The argument provider whose arguments to visit.</param>
        /// <returns>The rewritten arguments, if any; otherwise, null.</returns>
        protected internal ExpressionSlim[] VisitArguments(IArgumentProviderSlim nodes)
        {
            var res = default(ExpressionSlim[]);

            for (int i = 0, n = nodes.ArgumentCount; i < n; i++)
            {
                var argument = nodes.GetArgument(i);
                var expression = Visit(argument);

                if (res != null)
                {
                    res[i] = expression;
                }
                else if (expression != argument)
                {
                    res = new ExpressionSlim[n];

                    for (var j = 0; j < i; j++)
                    {
                        res[j] = nodes.GetArgument(j);
                    }

                    res[i] = expression;
                }
            }

            return res;
        }
    }
}
