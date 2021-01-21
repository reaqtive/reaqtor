// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// BD - June 2017 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace System.Linq.Expressions
{
    internal sealed class ExpressionSlimPrettyPrinter : ExpressionSlimVisitor
    {
        /// <summary>
        /// Shared StringBuilder pool with the following rationale for settings:
        ///
        /// - Capacity set to ProcessorCount to accommodate for printing at maximum concurrency. Note that
        ///   printing is not recursive, so no nested calls to ToString are expected.
        /// - A default capacity of 1KB for newly allocated StringBuilder instances. This is large enough
        ///   for relatively big expression trees.
        /// - A maximum capacity of 8KB for StringBuilder instances that get returned to the pool. This is
        ///   sufficiently large for a lot of expressions but not too large to cause worries in the case of
        ///   maximum concurrency (e.g. 32 cores x 8KB each is less than 1MB).
        ///
        /// Note that it's very unlikely to see maximum concurrency in practice with many StringBuilder
        /// instances checked out from the pool at the same time, unless expressions are really big causing
        /// a lot of time to be spent printing, increasing the likelihood for a concurrent print operation
        /// to take place. However, huge expressions will cause the pooled instances to get dropped, bringing
        /// us back to a place no worse than allocating fresh StringBuilder instances for each print operation.
        /// </summary>
        public static readonly StringBuilderPool StringBuilderPool = StringBuilderPool.Create(Environment.ProcessorCount, 1024, 8 * 1024);

        private static readonly Dictionary<int, string> s_expressionTypeToString = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToDictionary(e => (int)e, e => e.ToString());
        private static readonly Dictionary<int, string> s_gotoExpressionKindToString = Enum.GetValues(typeof(GotoExpressionKind)).Cast<GotoExpressionKind>().ToDictionary(e => (int)e, e => e.ToString());

        private readonly StringBuilder _sb;

        public ExpressionSlimPrettyPrinter(StringBuilder sb)
        {
            _sb = sb;
        }

        protected internal override ExpressionSlim VisitBinary(BinaryExpressionSlim node)
        {
            // CONSIDER: Conversion has been historically omitted here. We can consider adding it later.
            // CONSIDER: Method has been historically omitted here. We can consider adding it later.

            Append(node.NodeType);
            Append('(');
            Visit(node.Left);
            Append(", ");
            Visit(node.Right);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitBlock(BlockExpressionSlim node)
        {
            // CONSIDER: Block can have a custom type which has been historically omitted here. We can consider adding it later.

            Append("Block(");
            Visit(", ", node.Variables);
            if (node.Variables.Count > 0)
            {
                Append("; ");
            }
            Visit("; ", node.Expressions);
            Append(')');

            return node;
        }

        protected internal override CatchBlockSlim VisitCatchBlock(CatchBlockSlim node)
        {
            Append("CatchBlock(");
            Append(node.Test);
            VisitNonNull(", ", node.Variable);
            VisitNonNull(", ", node.Filter);
            VisitNonNull(", ", node.Body);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitConditional(ConditionalExpressionSlim node)
        {
            Append("Conditional(");
            Visit(node.Test);
            Append(", ");
            Visit(node.IfTrue);
            Append(", ");
            Visit(node.IfFalse);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitConstant(ConstantExpressionSlim node)
        {
            Append("Constant(");
            Append(node.Value);
            Append(", ");
            Append(node.Type);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitDefault(DefaultExpressionSlim node)
        {
            Append("Default(");
            Append(node.Type);
            Append(')');

            return node;
        }

        protected internal override ElementInitSlim VisitElementInit(ElementInitSlim node)
        {
            Append("ElementInit(");
            Append(node.AddMethod);
            Append(", ");
            Visit(", ", node.Arguments);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitGoto(GotoExpressionSlim node)
        {
            Append(node.Kind);
            Append('(');
            VisitLabelTarget(node.Target);
            VisitNonNull(", ", node.Value);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitIndex(IndexExpressionSlim node)
        {
            Append("Index(");
            if (node.Indexer != null)
            {
                Append(node.Indexer);
                Append(", ");
            }
            Visit(node.Object);
            Append(", ");
            Visit(", ", node);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitInvocation(InvocationExpressionSlim node)
        {
            Append("Invoke(");
            Visit(node.Expression);
            if (node.ArgumentCount > 0)
            {
                Append(", ");
                Visit(", ", node);
            }
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitLabel(LabelExpressionSlim node)
        {
            Append("Label(");
            VisitLabelTarget(node.Target);
            VisitNonNull(", ", node.DefaultValue);
            Append(')');

            return node;
        }

        protected internal override LabelTargetSlim VisitLabelTarget(LabelTargetSlim node)
        {
            Append("LabelTarget(");
            Append(node.Name ?? "Label");
            Append(", ");
            Append(node.Type);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitLambda(LambdaExpressionSlim node)
        {
            // CONSIDER: Historically, this has omitted the delegate type. We can consider to add it later.

            Append("Lambda(");
            Visit(node.Body);
            if (node.Parameters.Count > 0)
            {
                Append(", ");
                Visit(", ", node.Parameters);
            }
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitListInit(ListInitExpressionSlim node)
        {
            Append("ListInit(");
            Visit(node.NewExpression);
            Append(", ");
            Visit(", ", node.Initializers, VisitElementInit);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitLoop(LoopExpressionSlim node)
        {
            var count = 0;

            Append("Loop(");
            DoLabel(node.ContinueLabel);
            DoExpr(node.Body);
            DoLabel(node.BreakLabel);
            Append(')');

            return node;

            void DoLabel(LabelTargetSlim target)
            {
                if (target != null)
                {
                    if (count > 0)
                    {
                        Append(", ");
                    }

                    VisitLabelTarget(target);

                    count++;
                }
            }

            void DoExpr(ExpressionSlim expr)
            {
                if (count > 0)
                {
                    Append(", ");
                }

                Visit(expr);

                count++;
            }
        }

        protected internal override MemberAssignmentSlim VisitMemberAssignment(MemberAssignmentSlim node)
        {
            Append("Assignment(");
            Append(node.Member);
            Append(", ");
            Visit(node.Expression);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitMember(MemberExpressionSlim node)
        {
            Append("MemberAccess(");
            Append(node.Member);
            VisitNonNull(", ", node.Expression);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitMemberInit(MemberInitExpressionSlim node)
        {
            Append("MemberInit(");
            Visit(node.NewExpression);
            Append(", ");
            Visit(", ", node.Bindings, VisitMemberBinding);
            Append(')');

            return node;
        }

        protected internal override MemberListBindingSlim VisitMemberListBinding(MemberListBindingSlim node)
        {
            Append("ListBinding(");
            Append(node.Member);
            Append(", ");
            Visit(", ", node.Initializers, VisitElementInit);
            Append(')');

            return node;
        }

        protected internal override MemberMemberBindingSlim VisitMemberMemberBinding(MemberMemberBindingSlim node)
        {
            Append("MemberBinding(");
            Append(node.Member);
            Append(", ");
            Visit(", ", node.Bindings, VisitMemberBinding);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitMethodCall(MethodCallExpressionSlim node)
        {
            Append("Call(");
            Append(node.Method);
            VisitNonNull(", ", node.Object);
            if (node.ArgumentCount > 0)
            {
                Append(", ");
                Visit(", ", node);
            }
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitNew(NewExpressionSlim node)
        {
            // CONSIDER: This doesn't print the Members collection if present. We can consider adding this in the future.

            Append("New(");
            if (node.Constructor != null)
            {
                Append(node.Constructor);
                if (node.ArgumentCount > 0)
                {
                    Append(", ");
                    Visit(", ", node);
                }
            }
            else
            {
                Append(node.Type);
            }
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitNewArray(NewArrayExpressionSlim node)
        {
            Append(node.NodeType);
            Append('(');
            Append(node.ElementType);
            Append(", ");
            Visit(", ", node.Expressions);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
        {
            Append("Parameter(");
            Append(node.Type);
            Append(", ");
            Append(node.Name ?? "Param");
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitSwitch(SwitchExpressionSlim node)
        {
            Append("Switch(");
            Visit(node.SwitchValue);
            if (node.Comparison != null)
            {
                Append(", ");
                Append(node.Comparison);
            }
            Append(", ");
            Visit(", ", node.Cases, VisitSwitchCase);
            VisitNonNull(", ", node.DefaultBody);
            Append(')');

            return node;
        }

        protected internal override SwitchCaseSlim VisitSwitchCase(SwitchCaseSlim node)
        {
            Append("SwitchCase(");
            Visit(node.Body);
            Append(", ");
            Visit(", ", node.TestValues);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitTry(TryExpressionSlim node)
        {
            Append("Try(");
            Visit(node.Body);
            if (node.Handlers.Count > 0)
            {
                Append(", ");
                Visit(", ", node.Handlers, VisitCatchBlock);
            }
            VisitNonNull(", ", node.Finally);
            VisitNonNull(", ", node.Fault);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitTypeBinary(TypeBinaryExpressionSlim node)
        {
            Append(node.NodeType);
            Append('(');
            Visit(node.Expression);
            Append(", ");
            Append(node.TypeOperand);
            Append(')');

            return node;
        }

        protected internal override ExpressionSlim VisitUnary(UnaryExpressionSlim node)
        {
            // CONSIDER: Method has been historically omitted here. We can consider adding it later.

            Append(node.NodeType);
            Append('(');

            if (node.Operand != null)
            {
                Visit(node.Operand);

                if (node.Type != null)
                {
                    Append(", ");
                }
            }

            if (node.Type != null)
            {
                Append(node.Type);
            }

            Append(')');

            return node;
        }

        private void VisitNonNull(string prefix, ExpressionSlim expression)
        {
            if (expression != null)
            {
                Append(prefix);
                Visit(expression);
            }
        }

        private void Visit<TExpression>(string separator, ReadOnlyCollection<TExpression> expressions)
            where TExpression : ExpressionSlim
        {
            for (int i = 0, n = expressions.Count; i < n; i++)
            {
                Visit(expressions[i]);

                if (i != n - 1)
                {
                    Append(separator);
                }
            }
        }

        private void Visit<T>(string separator, ReadOnlyCollection<T> nodes, Func<T, T> visit)
        {
            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                visit(nodes[i]);

                if (i != n - 1)
                {
                    Append(separator);
                }
            }
        }

        private void Visit(string separator, IArgumentProviderSlim node)
        {
            for (int i = 0, n = node.ArgumentCount; i < n; i++)
            {
                Visit(node.GetArgument(i));

                if (i != n - 1)
                {
                    Append(separator);
                }
            }
        }

        private void Append(ExpressionType type) => _sb.Append(s_expressionTypeToString[(int)type]);

        private void Append(GotoExpressionKind kind) => _sb.Append(s_gotoExpressionKindToString[(int)kind]);

        private void Append(ObjectSlim value) => _sb.Append(value?.ToString());

        private void Append(MemberInfoSlim member) => _sb.Append(member?.ToString());

        private void Append(TypeSlim type) => _sb.Append(type?.ToString());

        private void Append(char c) => _sb.Append(c);

        private void Append(string s) => _sb.Append(s);
    }
}
