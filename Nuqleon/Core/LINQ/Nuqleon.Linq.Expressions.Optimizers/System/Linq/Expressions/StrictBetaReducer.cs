// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// A conservative beta-reducer for invocations of lambda expressions which allows inlining of arguments
    /// if they are either pure (and don't cause capturing of variables in scope) or have side-effects that
    /// are guaranteed to be evaluated exactly once, unconditionally, and in the original order.
    /// </summary>
    internal class StrictBetaReducer
    {
        /// <summary>
        /// The semantic provider to use when performing various checks against expressions and reflection objects.
        /// </summary>
        private readonly ISemanticProvider _provider;

        /// <summary>
        /// Creates a new beta-reducer instance using the specified semantic <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">The semantic provider to use when performing various checks against expressions and reflection objects.</param>
        public StrictBetaReducer(ISemanticProvider provider)
        {
            _provider = provider;
        }

        // REVIEW: How can this be overridden by the expression optimizer?
        //         Should we pass in more policy mechanisms?
        public virtual bool CanInlineMany(Expression node) => _provider.IsPure(node);

        /// <summary>
        /// Tries to beta-reduce the specified <paramref name="expression"/> if it's semantically sound to do so. This
        /// includes preserving the order and plurality of side-effects of the arguments passed to the lambda expression.
        /// </summary>
        /// <param name="expression">The expression to beta-reduce.</param>
        /// <param name="result">The result of beta-reducing the specified <paramref name="expression"/>, if semantically sound to do so.</param>
        /// <returns><c>true</c> if a beta reduction was performed; otherwise, <c>false</c>.</returns>
        public bool TryReduce(InvocationExpression expression, out Expression result)
        {
            var lambda = (LambdaExpression)expression.Expression;

            var parameters = lambda.Parameters;
            var arguments = expression.Arguments;

            var n = arguments.Count;

            if (n == 0)
            {
                result = lambda.Body;
                return true;
            }

            var findRestrictions = new FindInliningRestrictions();

            var bindings = new Dictionary<ParameterExpression, Binding>(n);
            var bindingOrder = new List<ParameterExpression>(n);

            for (var i = 0; i < n; i++)
            {
                var parameter = parameters[i];
                var argument = arguments[i];

                findRestrictions.Visit(argument);

                if (findRestrictions.Unsafe)
                {
                    break;
                }

                var fvs = new FreeVariableFinder();
                fvs.Visit(argument);

                var binding = new Binding
                {
                    Argument = argument,
                    FreeVariables = fvs.FreeVariables,
                    IsConstant = _provider.HasConstantValue(argument),
                    IsPure = _provider.IsPure(argument),
                    CanRepeat = CanInlineMany(argument),
                };

                bindings.Add(parameter, binding);

                if (!binding.IsPure)
                {
                    bindingOrder.Add(parameter);
                }
            }

            //
            // If we found any lval in an expression we'd be inlining, we can't proceed because we risk reordering
            // reads and writes. For example:
            //
            //   (x => a + x)(a = 1)
            //
            // would evaluate as:
            //
            //   a = 1;
            //   (x => a + x)(a);
            //
            // and produces 2. However, if we inline the side-effect of assignment, we end up with:
            //
            //   a + (a = 1)
            //
            // which will read the original value of a for the left operand.
            //
            // CONSIDER: The analysis could be made more precise to pick up on the storage locations being written
            //           to and the reads being performed by the lambda body.
            //

            if (!findRestrictions.Unsafe)
            {
                var impl = new Impl(this, bindings, bindingOrder);

                var tmp = impl.Visit(lambda.Body);

                impl.EnsureAllBound();

                if (impl.Status == Status.AllBound)
                {
                    result = tmp;
                    return true;
                }
            }

            result = null;
            return false;
        }

        private sealed class FindInliningRestrictions : LvalExpressionVisitor
        {
            //
            // CONSIDER: The rethrow restriction could be lifted and analyzed in a similar way as variable capture
            //           analysis during the inlining step. In particular, it's possible to have a naked rethrow
            //           that won't change meaning if it doesn't get inlined in another surrounding catch block.
            //

            private bool _inCatch;

            public bool Unsafe => HasLval || HasRethrow;

            public bool HasLval = false;
            public bool HasRethrow = false;

            public override Expression Visit(Expression node)
            {
                if (Unsafe)
                {
                    return node;
                }

                return base.Visit(node);
            }

            protected override Expression VisitLval(Expression node)
            {
                HasLval = true;
                return node;
            }

            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                var wasInCatch = _inCatch;
                _inCatch = true;

                var res = base.VisitCatchBlock(node);

                _inCatch = wasInCatch;

                return res;
            }

            protected override Expression VisitUnaryNonQuote(UnaryExpression node, bool isAssignment)
            {
                if (node.NodeType == ExpressionType.Throw && node.Operand == null && !_inCatch)
                {
                    HasRethrow = true;
                }

                return base.VisitUnaryNonQuote(node, isAssignment);
            }
        }

        private sealed class Impl : LvalExpressionVisitor
        {
            private readonly StrictBetaReducer _parent;
            private readonly Dictionary<ParameterExpression, Binding> _bindings;
            private readonly List<ParameterExpression> _bindingOrder;
            private readonly Stack<IEnumerable<ParameterExpression>> _environment = new();

            private int _nextInPureBindingIndex;
            private bool _inQuote;
            private bool _inLambda;
            private bool _mayRepeat;

            public Status Status = Status.Continue;

            public Impl(StrictBetaReducer parent, Dictionary<ParameterExpression, Binding> bindings, List<ParameterExpression> bindingOrder)
            {
                _parent = parent;
                _bindings = bindings;
                _bindingOrder = bindingOrder;
            }

            public override Expression Visit(Expression node)
            {
                //
                // As soon as we have an error (indicated by an enum flag above AllBound), we can bail out by
                // stopping any recursive visits. The caller will discard changes made to the tree during the
                // visit and conclude that semantics preserving beta reduction was not possible.
                //
                if (Status > Status.AllBound)
                {
                    return node;
                }

                return base.Visit(node);
            }

            protected override Expression VisitBlock(BlockExpression node)
            {
                if (node.Variables.Count > 0)
                {
                    _environment.Push(node.Variables);
                }

                //
                // Expressions execute top-to-bottom. Any branches will be taken care of by visits to Label, Goto,
                // Conditional, Try, or Loop expressions, so we can just proceed in order.
                //
                var expressions = Visit(node.Expressions);

                if (node.Variables.Count > 0)
                {
                    _environment.Pop();
                }

                return node.Update(node.Variables, expressions);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                if (node.Parameters.Count > 0)
                {
                    _environment.Push(node.Parameters);
                }

                //
                // The lambda body is not guaranteed to run in a deterministic manner, so we can't bind any
                // bindings that are not pure. However, we can inline constants or default expressions just
                // fine, so we have to go through the body to ensure we don't leave any binding sites unbound.
                // To track this state, we use _inLambda.
                //

                var wasInLambda = _inLambda;
                _inLambda = true;

                var body = Visit(node.Body);

                _inLambda = wasInLambda;

                if (node.Parameters.Count > 0)
                {
                    _environment.Pop();
                }

                return node.Update(body, node.Parameters);
            }

            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                if (node.Variable != null)
                {
                    _environment.Push(new[] { node.Variable });
                }

                //
                // Just visit children to perform inlining of constants or defaults. Note that the parent
                // try expression requires that non-trivial bindings are satisfied by the protected region
                // so we don't have to worry about that over here.
                //

                var filter = Visit(node.Filter);
                var body = Visit(node.Body);

                if (node.Variable != null)
                {
                    _environment.Pop();
                }

                return node.Update(node.Variable, filter, body);
            }

            protected override Expression VisitParameter(ParameterExpression node, bool isLval)
            {
                foreach (var scope in _environment)
                {
                    foreach (var variable in scope)
                    {
                        if (variable == node)
                        {
                            return node;
                        }
                    }
                }

                return Bind(node, isLval);
            }

            private Expression Bind(ParameterExpression node, bool isLval)
            {
                if (_bindings.TryGetValue(node, out var binding))
                {
                    if (isLval)
                    {
                        Finish(Status.HasLvalBinding);
                        return node;
                    }

                    if (_inQuote)
                    {
                        //
                        // Occurrences of variables in quotes result in the present of StrongBox<T> instances
                        // in Constant nodes. We don't want to take these away.
                        //
                        Finish(Status.HasBindingInQuote);
                        return node;
                    }

                    if (_inLambda && !binding.IsConstant)
                    {
                        //
                        // Bindings in lambdas are not guaranteed to run, so if the binding has side-effects, we
                        // can't inline it because the evaluation count is non-deterministic. We can make an
                        // exception for constants and default values.
                        //
                        // NB: We make the check here a bit stronger than IsPure to avoid inlining parameters that
                        //     are defined in a higher scope. Putting these within a nested lambda would cause their
                        //     repeated lookup at runtime (for each lambda invocation) which can be an unsafe thing
                        //     to do.
                        //
                        //     Example: f = (x => () => x)(y);
                        //              y++;
                        //              f();
                        //
                        // REVIEW: Is it generally safe to inline variables?
                        //
                        Finish(Status.HasBindingInLambda);
                        return node;
                    }

                    if (binding.UsageCount > 0 || _mayRepeat)
                    {
                        if (!binding.IsPure || !binding.CanRepeat)
                        {
                            Finish(Status.RepeatsArgument);
                            return node;
                        }
                    }

                    if (!binding.IsPure)
                    {
                        if (_bindingOrder[_nextInPureBindingIndex] != node)
                        {
                            Finish(Status.ChangesSideEffectOrder);
                            return node;
                        }

                        _nextInPureBindingIndex++;
                    }

                    if (binding.FreeVariables != null)
                    {
                        var captured = new HashSet<ParameterExpression>(_environment.SelectMany(scope => scope));

                        captured.IntersectWith(binding.FreeVariables);

                        if (captured.Count > 0)
                        {
                            //
                            // CONSIDER: We could perform alpha renaming to resolve this issue. For now, we'll
                            //           just bail out.
                            //
                            Finish(Status.CausesCapture);
                            return node;
                        }
                    }

                    binding.UsageCount++;

                    return binding.Argument;
                }

                return node;
            }

            protected override Expression VisitBinaryWithConversion(BinaryExpression node, bool isAssignment)
            {
                return VisitBinary(node, isAssignment);
            }

            protected override Expression VisitBinaryWithoutConversion(BinaryExpression node, bool isAssignment)
            {
                return VisitBinary(node, isAssignment);
            }

            private Expression VisitBinary(BinaryExpression node, bool isAssignment)
            {
                var conversion = node.Conversion;

                //
                // Left always evaluates.
                //
                var left = isAssignment ? VisitLval(node.Left) : Visit(node.Left);

                //
                // In case we're dealing with a Coalesce node, the conversion (if any) runs on the left
                // operand provided that operand is guaranteed to be non-null.
                //
                if (!isAssignment && node.Conversion != null)
                {
                    Debug.Assert(node.NodeType == ExpressionType.Coalesce);

                    if (MayBeNull(left))
                    {
                        //
                        // We don't know if we'll end up evaluating the conversion expression, so we need
                        // to have all side-effects evaluated by now if we want to proceed.
                        //
                        if (!EnsureAllBound())
                        {
                            return node;
                        }
                    }

                    //
                    // We got here either because left *can't* be null or because we've ensured that all
                    // side-effects have been evaluated and we still need to visit the conversion lambda
                    // to guarantee pure expressions get inlined.
                    //
                    conversion = VisitBinaryConversion(node.Conversion);
                }

                //
                // Right may or may not evaluate based on short-circuiting behavior.
                //
                if (!BinaryRightEvaluationIsGuaranteed(node, left))
                {
                    //
                    // If we're not guaranteed that right gets evaluated, we need to make sure all bindings
                    // have been satisfied.
                    //
                    if (!EnsureAllBound())
                    {
                        return node;
                    }
                }

                //
                // Visit right regardless in order to check for duplicate bindings that may cause repetition
                // of side-effects.
                //
                var right = Visit(node.Right);

                //
                // Evaluating the binary operator may cause exceptions which may prevent subsequent evaluation
                // to take place. If this may happen, we need to ensure binding.
                //
                if (BinaryMayThrow(node) && !EnsureAllBound())
                {
                    return node;
                }

                //
                // For assignments with conversions, the conversion is applied after the binary operator is
                // evaluated, and prior to assignment to the lhs.
                //
                if (isAssignment && node.Conversion != null)
                {
                    //
                    // The emitted code invokes the conversion lambda with a temporary holding the result of
                    // evaluating the binary operation. Thus, the evaluation of the lambda is not deferred and
                    // we can pretend the body of the lambda gets eagerly evaluated.
                    //
                    conversion = VisitBinaryConversion(node.Conversion);
                }

                return node.Update(left, conversion, right);
            }

            protected override LambdaExpression VisitBinaryConversion(LambdaExpression node)
            {
                //
                // NB: Unlike VisitLambda, these conversion lambdas are part of eager Invoke(Lambda, temp)
                //     reduction at runtime, so we can assume the body will be run. We still need to do scope
                //     tracking though, in order to avoid captures.
                //

                _environment.Push(node.Parameters);

                var body = Visit(node.Body);

                _environment.Pop();

                if (body != node.Body)
                {
                    return Expression.Lambda(node.Type, body, node.Name, node.TailCall, node.Parameters);
                }

                return node;
            }

            private bool BinaryRightEvaluationIsGuaranteed(BinaryExpression node, Expression left) => node.NodeType switch
            {
                //
                // CONSIDER: Should we add support for static truthiness checks?
                //
                ExpressionType.AndAlso or ExpressionType.OrElse => false,

                ExpressionType.Coalesce => IsConstantNullAndDoesNotThrow(left),

                //
                // NB: Lifted operators are fine; both operands still evaluate prior to decision making about
                //     null propagation.
                //
                _ => true,
            };

            private bool BinaryMayThrow(BinaryExpression node)
            {
                if (!IsNullOrDoesNotThrow(node.Method))
                {
                    return true;
                }

                return node.NodeType switch
                {
                    //
                    // May cause overflow.
                    //
                    ExpressionType.AddChecked or
                    ExpressionType.AddAssignChecked or
                    ExpressionType.MultiplyChecked or
                    ExpressionType.MultiplyAssignChecked or
                    ExpressionType.SubtractChecked or
                    ExpressionType.SubtractAssignChecked => true,

                    //
                    // May cause division by zero.
                    //
                    ExpressionType.Divide or
                    ExpressionType.DivideAssign or
                    ExpressionType.Modulo or
                    ExpressionType.ModuloAssign => TypeUtils.IsInteger(node.Type),

                    //
                    // May cause null reference or out-of-range.
                    //
                    ExpressionType.ArrayIndex => true,

                    _ => false,
                };
            }

            protected override Expression VisitConditional(ConditionalExpression node)
            {
                //
                // Only the test is guaranteed to run, thus all bindings have to be satisfied afterwards
                // because the branching causes non-determinism.
                //
                // CONSIDER: We could perform richer control flow analysis to check that effects along
                //           all branches satisfy the evaluation order we're enforcing.
                //
                var test = Visit(node.Test);

                if (!EnsureAllBound())
                {
                    return node;
                }

                var ifTrue = Visit(node.IfTrue);
                var ifFalse = Visit(node.IfFalse);

                return node.Update(test, ifTrue, ifFalse);
            }

            protected override Expression VisitDynamicCore(DynamicExpression node)
            {
                //
                // Evaluation proceeds left-to-right.
                //
                var arguments = Visit(node.Arguments);

                //
                // Dynamic dispatch can't be predicted at runtime, so we have to be done with bindings.
                //
                if (!EnsureAllBound())
                {
                    return node;
                }

                return node.Update(arguments);
            }

            protected override ElementInit VisitElementInit(ElementInit node)
            {
                //
                // Evaluation proceeds left-to-right.
                //
                var arguments = Visit(node.Arguments);

                //
                // If the add method can throw we have to be done with bindings.
                //
                if (MayThrow(node.AddMethod) && !EnsureAllBound())
                {
                    return node;
                }

                return node.Update(arguments);
            }

            protected override Expression VisitGoto(GotoExpression node)
            {
                //
                // Only the value (if any) is guaranteed to be evaluated. Afterwards, the branching causes non-
                // determinism, so we have to be conservative and require all bindings to have been satisfied.
                //
                var value = Visit(node.Value);

                if (!EnsureAllBound())
                {
                    return node;
                }

                return node.Update(node.Target, value);
            }

            protected override Expression VisitIndex(IndexExpression node, bool isLval)
            {
                //
                // The object and arguments are guaranteed to be evaluated left-to-right.
                //
                var @object = isLval ? VisitLval(node.Object) : Visit(node.Object);
                var arguments = Visit(node.Arguments);

                var ensureAllBound = false;

                //
                // If the object may be null, we have to be done with all bindings because a NullReferenceException
                // could prevent further progress.
                //
                if (MayBeNull(@object))
                {
                    ensureAllBound = true;
                }

                if (node.Indexer != null)
                {
                    //
                    // If there's an indexer, it may cause an exception, so we have to have all bindings in place.
                    //
                    if (MayThrow(node.Indexer))
                    {
                        ensureAllBound = true;
                    }
                }
                else
                {
                    //
                    // If there's no indexer, we may end up with an out-of-range exception during array access.
                    //
                    ensureAllBound = true;
                }

                if (ensureAllBound && !EnsureAllBound())
                {
                    return node;
                }

                return node.Update(@object, arguments);
            }

            protected override Expression VisitInvocationCore(InvocationExpression node)
            {
                //
                // The expression and the arguments evaluate left-to-right.
                //
                var expression = Visit(node.Expression);

                var parameters = GetInvokeMethod(expression).GetParameters();

                var arguments = VisitArguments(node.Arguments, parameters);

                //
                // Many things can happen, including NullReferenceException or an exception being thrown by
                // the target of the invocation. As such, we'll be conservative and require all bindings to
                // have been satisfied.
                //
                // CONSIDER: Should we perform recursive beta reduction here, if the expression is a lambda?
                //
                if (!EnsureAllBound())
                {
                    return node;
                }

                return node.Update(expression, arguments);
            }

            protected override Expression VisitLabel(LabelExpression node)
            {
                //
                // We've hit a label, which means we could branch back to it. As such, all subsequent evaluations
                // may run more than once. If we inline an expression with side-effects only once, we may miscount
                // the possible repetition, so we have to toggle a flag for Bind to pick up on. Note that in the
                // absence of control flow analysis, this is more conservative than strictly needed: any occurrence
                // of a label in the whole tree will cause us to enter this mode (and never get out of it).
                //
                _mayRepeat = true;

                //
                // Note that the default value is subject to the repetition restriction we toggled above. If it's
                // a pure expression, it's fine to be evaluated many times; otherwise, we have to play safe.
                //
                var defaultValue = Visit(node.DefaultValue);

                return node.Update(node.Target, defaultValue);
            }

            protected override NewExpression VisitListInitNew(NewExpression node)
            {
                return VisitAndConvert(node, nameof(VisitListInitNew));
            }

            protected override Expression VisitLoop(LoopExpression node)
            {
                //
                // The body of the loop may evaluate many times, so any bindings occurring within the body should
                // be pure. See remarks in VisitLabel for similar observations.
                //
                var wasMayRepeat = _mayRepeat;
                _mayRepeat = true;

                var body = Visit(node.Body);

                _mayRepeat = wasMayRepeat;

                //
                // Even though we're guaranteed to enter the loop body at least once, we may never get out of it
                // and thus never get to evaluate the side-effect of some bindings. Thus, we need to ensure all
                // bindings are satisfied by now.
                //
                // NB: This observation implies that the semantic hints provided by ISemanticProvider are to be
                //    taken very strict. E.g. a method can't claim to be pure if it may never terminate. In
                //    most cases, one will want to be ultra-conservative with these hints, so that beta reduction
                //    is limited to inlining of constants and default values, and effects in "left-most" positions
                //    of the tree.
                //
                if (!EnsureAllBound())
                {
                    return node;
                }

                return node.Update(node.BreakLabel, node.ContinueLabel, body);
            }

            protected override Expression VisitMember(MemberExpression node, bool isLval)
            {
                //
                // The expression (if any) is guaranteed to run.
                //
                var expression = isLval ? VisitLval(node.Expression) : Visit(node.Expression);

                var ensureAllBound = false;

                //
                // If the expression may be null, we have to be done with all bindings because a NullReferenceException
                // could prevent further progress.
                //
                if (expression != null && MayBeNull(expression))
                {
                    ensureAllBound = true;
                }

                //
                // If the member may throw, we require all bindings to have been satisfied.
                //
                if (MayThrow(node.Member))
                {
                    ensureAllBound = true;
                }

                if (ensureAllBound && !EnsureAllBound())
                {
                    return node;
                }

                return node.Update(expression);
            }

            protected override NewExpression VisitMemberInitNew(NewExpression node)
            {
                return VisitAndConvert(node, nameof(VisitListInitNew));
            }

            protected override Expression VisitNewArray(NewArrayExpression node)
            {
                //
                // Expressions evaluate left-to-right.
                //
                var expressions = Visit(node.Expressions);

                //
                // NewArrayBounds could throw if an operand is negative.
                //
                if (node.NodeType == ExpressionType.NewArrayBounds && !EnsureAllBound())
                {
                    return node;
                }

                return node.Update(expressions);
            }

            protected override Expression VisitNewCore(NewExpression node)
            {
                //
                // Arguments evaluate left-to-right.
                //
                var arguments = Visit(node.Arguments);

                //
                // If the constructor may throw, we require all bindings to have been satisfied.
                //
                if (node.Constructor != null && MayThrow(node.Constructor) && !EnsureAllBound())
                {
                    return node;
                }

                return node.Update(arguments);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
            {
                //
                // The expression is guaranteed to run.
                //
                var expression = Visit(node.Expression);

                //
                // We'll assume the side-effect of assignment to be a deal-breaker to continue performing
                // inlining of non-pure bindings (being conservative).
                //
                if (!EnsureAllBound())
                {
                    return node;
                }

                return node.Update(expression);
            }

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
            {
                //
                // See remarks for member assignment. We're calling the getter of the member we're initializing
                // and could check if it's considered pure. However, because this node is all about mutation,
                // we'll be conservative and require all bindings to have been satisfied by now.
                //

                if (!EnsureAllBound())
                {
                    return node;
                }

                return base.VisitMemberListBinding(node);
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
            {
                //
                // See remarks for member list bindings.
                //

                if (!EnsureAllBound())
                {
                    return node;
                }

                return base.VisitMemberMemberBinding(node);
            }

            protected override Expression VisitMethodCallCore(MethodCallExpression node)
            {
                //
                // The object (if any) and arguments evaluate left-to-right.
                //
                var @object = Visit(node.Object);
                var arguments = Visit(node.Arguments);

                var ensureAllBound = false;

                //
                // If the object may be null, we have to be done with all bindings because a NullReferenceException
                // could prevent further progress.
                //
                if (@object != null && MayBeNull(@object))
                {
                    ensureAllBound = true;
                }

                //
                // If the method may throw, we require all bindings to have been satisfied.
                //
                if (MayThrow(node.Method))
                {
                    ensureAllBound = true;
                }

                if (ensureAllBound && !EnsureAllBound())
                {
                    return node;
                }

                return node.Update(@object, arguments);
            }

            protected override Expression VisitSwitch(SwitchExpression node)
            {
                //
                // Only the switch value is guaranteed to run, thus all bindings have to be satisfied afterwards
                // because the branching causes non-determinism.
                //
                var switchValue = Visit(node.SwitchValue);

                if (!EnsureAllBound())
                {
                    return node;
                }

                var cases = Visit(node.Cases, VisitSwitchCase);
                var defaultBody = Visit(node.DefaultBody);

                return node.Update(switchValue, cases, defaultBody);
            }

            protected override Expression VisitTry(TryExpression node)
            {
                var body = Visit(node.Body);

                //
                // Inlining a non-trivial expression in the body of the try block can change the meaning of the
                // program, e.g. due to a handler being executed.
                //
                if (!NeverThrows(body))
                {
                    Status |= Status.HasBindingInProtectedRegion;
                    return node;
                }

                //
                // Only the protected region is guaranteed to run. Branching into handlers is non-deterministic
                // at compile time and the order of side-effects (such as filters) complicates manners further,
                // thus we'll require all bindings to have been satisfied by now.
                //
                if (!EnsureAllBound())
                {
                    return node;
                }

                //
                // Go into handlers in order to bind constants etc. (see remarks on VisitLambda).
                //
                var handlers = Visit(node.Handlers, VisitCatchBlock);
                var @finally = Visit(node.Finally);
                var fault = Visit(node.Fault);

                return node.Update(body, handlers, @finally, fault);
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                //
                // Operand is guaranteed to run.
                //
                var expression = Visit(node.Expression);

                // REVIEW: Can we cause any exceptions here? (Don't think so.)

                return node.Update(expression);
            }

            protected override Expression VisitUnaryNonQuote(UnaryExpression node, bool isAssignment)
            {
                //
                // The operand is guaranteed to be evaluated.
                //
                var operand = isAssignment ? VisitLval(node.Operand) : Visit(node.Operand);

                //
                // Further evaluation may be prevented by exceptions.
                //
                if (UnaryMayThrow(node, operand) && !EnsureAllBound())
                {
                    return node;
                }

                return node.Update(operand);
            }

            protected override LambdaExpression VisitUnaryQuoteOperand(LambdaExpression node)
            {
                var wasInQuote = _inQuote;
                _inQuote = true;

                var res = base.VisitUnaryQuoteOperand(node);

                _inQuote = wasInQuote;

                return res;
            }

            private bool UnaryMayThrow(UnaryExpression node, Expression operand)
            {
                if (!IsNullOrDoesNotThrow(node.Method))
                {
                    return true;
                }

                return node.NodeType switch
                {
                    //
                    // May cause overflow.
                    //
                    ExpressionType.NegateChecked => TypeUtils.IsInteger(node.Type),

                    //
                    // May cause conversion exceptions.
                    //
                    ExpressionType.Convert or ExpressionType.ConvertChecked => true,

                    //
                    // May cause null reference.
                    //
                    ExpressionType.ArrayLength or ExpressionType.Unbox => MayBeNull(operand),

                    //
                    // Will throw (d'oh).
                    //
                    ExpressionType.Throw => true,

                    _ => false
                };
            }

            public bool EnsureAllBound()
            {
                if (_nextInPureBindingIndex == _bindingOrder.Count)
                {
                    Finish(Status.AllBound);
                    return true;
                }

                Finish(Status.NotDeterministic);
                return false;
            }

            private void Finish(Status status)
            {
                Status |= status;
            }

            private bool IsNullOrDoesNotThrow(MethodInfo method) => method == null || DoesNotThrow(method);
            private bool DoesNotThrow(MemberInfo member) => _parent._provider.NeverThrows(member);

            private bool MayThrow(MemberInfo member) => !DoesNotThrow(member);
            private bool IsConstantNullAndDoesNotThrow(Expression node) => _parent._provider.IsAlwaysNull(node) && _parent._provider.NeverThrows(node);

            private bool MayBeNull(Expression node) => !IsNeverNull(node);
            private bool IsNeverNull(Expression node) => _parent._provider.IsNeverNull(node);

            private bool NeverThrows(Expression node) => _parent._provider.NeverThrows(node);
        }

        /// <summary>
        /// Represents a binding for a variable, including the expression being bound, semantic information about it,
        /// as well as mutable state used during beta reduction.
        /// </summary>
        private sealed class Binding
        {
            /// <summary>
            /// The expression being bound to the associated parameter.
            /// </summary>
            public Expression Argument;

            /// <summary>
            /// The set of free variables that occur in the expression being bound. This is used to check for captures
            /// of variables that would change variable binding semantics.
            /// </summary>
            public HashSet<ParameterExpression> FreeVariables;

            /// <summary>
            /// Indicates whether the expression being bound represents a runtime constant. This setting implies that
            /// the expression is also pure (see <see cref="IsPure"/>) and is used to determine whether inlining
            /// can take place within a nested lambda expression (see <see cref="Impl.VisitLambda"/> for more info).
            /// </summary>
            public bool IsConstant;

            /// <summary>
            /// Indicates whether the expression being bound is deemed pure (which includes the guarantee not to throw
            /// any exception). Pure expressions can be reordered at will, and may be inlined zero or more times (see
            /// <see cref="CanRepeat"/> for more remarks).
            /// </summary>
            public bool IsPure;

            /// <summary>
            /// Indicates whether the binding can be performed multiple times (see <see cref="UsageCount"/>) safely.
            ///
            /// Examples where this may be set to <c>true</c> include <see cref="ExpressionType.Default"/> nodes which
            /// don't have a side-effect.
            ///
            /// One may want to set this to <c>false</c> even for <see cref="ExpressionType.Constant"/> nodes in case
            /// inlining may cause the introduction of many copies of the same constant value, e.g. when serializing
            /// an expression tree.
            /// </summary>
            public bool CanRepeat;

            /// <summary>
            /// The number of times this binding has been performed, used to detect possible repetition of side-effects.
            /// </summary>
            public int UsageCount;
        }

        /// <summary>
        /// The status of the beta reduction expression visit.
        /// </summary>
        [Flags]
        private enum Status
        {
            /// <summary>
            /// Rewriting the tree should continue; no deal-breakers for beta-reduction have been encountered yet.
            /// </summary>
            Continue = 0,

            /// <summary>
            /// All arguments passed to the invocation expression have been bound to lambda parameters.
            /// </summary>
            AllBound = 1,

            /// <summary>
            /// An argument is being inlined in an lval position.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <example>
            /// <c>(x => { x = 1; })(42)</c>
            /// </example>
            HasLvalBinding = 2,

            /// <summary>
            /// Inlining of the arguments causes an observable change in the order of side-effects.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <example>
            /// <c>((x, y) => y + x)(F(), G())</c>
            /// </example>
            ChangesSideEffectOrder = 4,

            /// <summary>
            /// Inlining of an argument causes an observable side-effects to be repeated.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <example>
            /// <c>(x => x + x)(F())</c>
            /// </example>
            RepeatsArgument = 8,

            /// <summary>
            /// Inlining of an argument causes a change in variable binding.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <example>
            /// <c>(x => { int y; ... x + y; })(y)</c>
            /// </example>
            CausesCapture = 16,

            /// <summary>
            /// Inlining of an argument happens in a <see cref="ExpressionType.Quote"/> node.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <remarks>
            /// This is a conservative approach to avoid the elimination of strong box wrappers around quoted variables.
            /// </remarks>
            /// <example>
            /// <c>(x => @(y => x + y))(42)</c>
            /// </example>
            HasBindingInQuote = 32,

            /// <summary>
            /// Inlining of an argument occurs within a nested lambda body that is not eagerly evaluated.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <example>
            /// <c>(x => y => x + y)(F())</c>
            /// </example>
            HasBindingInLambda = 64,

            /// <summary>
            /// Inlining of an argument occurs in an expression tree node where execution is not guaranteed due to branching.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <example>
            /// <c>(x => c ? x : 0)(F())</c>
            /// </example>
            NotDeterministic = 128,

            /// <summary>
            /// Inlining of an argument happens in a <see cref="TryExpression.Body"/> position where exception handling behavior may change.
            /// Beta reduction can't proceed.
            /// </summary>
            /// <example>
            /// <c>(x => try { x } catch { ... })(F())</c>
            /// </example>
            HasBindingInProtectedRegion = 256,
        }
    }
}
