// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // No null checks in visitor methods.
#pragma warning disable CA1716 // Conflicts with reserved language keywords. (Mirrors .NET APIs.)

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#if !USE_SLIM
using System.Runtime.CompilerServices;
#endif

#if USE_SLIM
namespace System.Linq.CompilerServices.Bonsai
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using BinaryExpression = BinaryExpressionSlim;
    using BlockExpression = BlockExpressionSlim;
    using CatchBlock = CatchBlockSlim;
    using ConditionalExpression = ConditionalExpressionSlim;
    using ConstantExpression = ConstantExpressionSlim;
    using DefaultExpression = DefaultExpressionSlim;
    using Expression = ExpressionSlim;
    using GotoExpression = GotoExpressionSlim;
    using IndexExpression = IndexExpressionSlim;
    using InvocationExpression = InvocationExpressionSlim;
    using LabelTarget = LabelTargetSlim;
    using LabelExpression = LabelExpressionSlim;
    using LambdaExpression = LambdaExpressionSlim;
    using ListInitExpression = ListInitExpressionSlim;
    using LoopExpression = LoopExpressionSlim;
    using MemberExpression = MemberExpressionSlim;
    using MemberInitExpression = MemberInitExpressionSlim;
    using MethodCallExpression = MethodCallExpressionSlim;
    using NewArrayExpression = NewArrayExpressionSlim;
    using NewExpression = NewExpressionSlim;
    using ParameterExpression = ParameterExpressionSlim;
    using SwitchCase = SwitchCaseSlim;
    using SwitchExpression = SwitchExpressionSlim;
    using TryExpression = TryExpressionSlim;
    using TypeBinaryExpression = TypeBinaryExpressionSlim;
    using UnaryExpression = UnaryExpressionSlim;

    using ElementInit = ElementInitSlim;
    using MemberAssignment = MemberAssignmentSlim;
    using MemberBinding = MemberBindingSlim;
    using MemberListBinding = MemberListBindingSlim;
    using MemberMemberBinding = MemberMemberBindingSlim;

    using ConstructorInfo = ConstructorInfoSlim;
    using MemberInfo = MemberInfoSlim;
    using MethodInfo = MethodInfoSlim;
    using PropertyInfo = PropertyInfoSlim;
    using Type = TypeSlim;

    using Object = ObjectSlim;

    #endregion
#endif

    /// <summary>
    /// Expression visitor with visit methods for reflection objects.
    /// </summary>
#if USE_SLIM
    public class ExpressionSlimVisitorWithReflection : ScopedExpressionSlimVisitor<ParameterExpression>
#else
    public class ExpressionVisitorWithReflection : ScopedExpressionVisitor<ParameterExpression>
#endif
    {
        #region Constructor & fields

        private Dictionary<LabelTarget, LabelTarget> _labels;

        /// <summary>
        /// Creates a new expression visitor instance.
        /// </summary>
#if USE_SLIM
        public ExpressionSlimVisitorWithReflection()
#else
        public ExpressionVisitorWithReflection()
#endif
        {
        }

        #endregion

        #region Binary

        /// <summary>
        /// Visits the children of a binary expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitBinary(BinaryExpression node)
        {
            var left = Visit(node.Left);
            var conversion = VisitAndConvert(node.Conversion, nameof(VisitBinary));
            var right = Visit(node.Right);
            var method = VisitMethod(node.Method);

            if (left != node.Left || right != node.Right || conversion != node.Conversion || method != node.Method)
            {
                return MakeBinary(node.NodeType, left, right, node.IsLiftedToNull /* TODO: check */, method, conversion);
            }

            return node;
        }

        /// <summary>
        /// Creates a binary expression.
        /// </summary>
        /// <param name="binaryType">Binary expression node type.</param>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <param name="liftToNull">Indicates whether the operator is lifted.</param>
        /// <param name="method">Method implementing the operation.</param>
        /// <param name="conversion">Type conversion function for coalescing and compound assignment operations.</param>
        /// <returns>New binary expression node.</returns>
        protected virtual Expression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method, LambdaExpression conversion) => Expression.MakeBinary(binaryType, left, right, liftToNull, method, conversion);

        #endregion

        #region Block

        /// <summary>
        /// Visits the children of a block expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitBlockCore(BlockExpression node)
        {
            var type = VisitType(node.Type);
            var variables = VisitAndConvert(node.Variables, nameof(VisitBlockCore));
            var expressions = Visit(node.Expressions);

            if (type != node.Type || expressions != node.Expressions || variables != node.Variables)
            {
                return MakeBlock(type, variables, expressions);
            }

            return node;
        }

        /// <summary>
        /// Creates a block expression.
        /// </summary>
        /// <param name="type">Result type of the block expression.</param>
        /// <param name="variables">Variables in scope in the block expression.</param>
        /// <param name="expressions">Expressions in the body of the block expression.</param>
        /// <returns>New block expression node.</returns>
        protected virtual Expression MakeBlock(Type type, IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions) => Expression.Block(type, variables, expressions);

        #endregion

        #region CatchBlock

        /// <summary>
        /// Visits the children of a catch block node.
        /// </summary>
        /// <param name="node">The catch block to visit.</param>
        /// <returns>The modified catch block, if it or any subexpression was modified; otherwise, returns the original catch block.</returns>
        protected override CatchBlock VisitCatchBlockCore(CatchBlock node)
        {
            var test = VisitType(node.Test);
            var variable = VisitAndConvert(node.Variable, nameof(VisitCatchBlockCore));
            var body = Visit(node.Body);
            var filter = Visit(node.Filter);

            if (test != node.Test || variable != node.Variable || body != node.Body || filter != node.Filter)
            {
                return MakeCatchBlock(test, variable, body, filter);
            }

            return node;
        }

        /// <summary>
        /// Creates a catch block.
        /// </summary>
        /// <param name="test">Type of the exception to catch.</param>
        /// <param name="variable">Variable for the exception being handled by the catch block.</param>
        /// <param name="body">Body of the catch block.</param>
        /// <param name="filter">Filter condition for exceptions handled by the catch block.</param>
        /// <returns>New catch block node.</returns>
        protected virtual CatchBlock MakeCatchBlock(Type test, ParameterExpression variable, Expression body, Expression filter) => Expression.MakeCatchBlock(test, variable, body, filter);

        #endregion

        #region Conditional

        /// <summary>
        /// Visits the children of a conditional expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitConditional(ConditionalExpression node)
        {
            var test = Visit(node.Test);
            var ifTrue = Visit(node.IfTrue);
            var ifFalse = Visit(node.IfFalse);
            var type = VisitType(node.Type);

            if (test != node.Test || ifTrue != node.IfTrue || ifFalse != node.IfFalse || type != node.Type)
            {
                return MakeConditional(test, ifTrue, ifFalse, type);
            }

            return node;
        }

        /// <summary>
        /// Creates a conditional expression.
        /// </summary>
        /// <param name="test">Expression to test the condition.</param>
        /// <param name="ifTrue">Result expression when the condition evaluates to true.</param>
        /// <param name="ifFalse">Result expression when the condition evaluates to false.</param>
        /// <param name="type">Result type of the conditional expression.</param>
        /// <returns>New conditional expression node.</returns>
        protected virtual Expression MakeConditional(Expression test, Expression ifTrue, Expression ifFalse, Type type) => Expression.Condition(test, ifTrue, ifFalse, type);

        #endregion

        #region Constant

        /// <summary>
        /// Visits the constant expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitConstant(ConstantExpression node)
        {
            var type = VisitType(node.Type);

            if (type != node.Type)
            {
                return MakeConstant(node.Value, type);
            }

            return node;
        }

        /// <summary>
        /// Creates a constant expression.
        /// </summary>
        /// <param name="value">Value of the constant expression.</param>
        /// <param name="type">Type of the constant expression.</param>
        /// <returns>New constant expression node.</returns>
        protected virtual Expression MakeConstant(Object value, Type type) => Expression.Constant(value, type);

        #endregion

        #region Default

        /// <summary>
        /// Visits the default expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitDefault(DefaultExpression node)
        {
            var type = VisitType(node.Type);

            if (type != node.Type)
            {
                return MakeDefault(type);
            }

            return node;
        }

        /// <summary>
        /// Creates a default expression.
        /// </summary>
        /// <param name="type">Type of the default expression.</param>
        /// <returns>New default expression node.</returns>
        protected virtual Expression MakeDefault(Type type) => Expression.Default(type);

        #endregion

#if !USE_SLIM && !NO_DYNAMIC
        #region Dynamic

        /// <summary>
        /// Visits the children of a dynamic expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            var binder = VisitBinder(node.Binder);
            var returnType = VisitType(node.Type);
            var arguments = Visit(node.Arguments);

            if (binder != node.Binder || returnType != node.Type || arguments != node.Arguments)
            {
                return MakeDynamic(binder, returnType, arguments);
            }

            return node;
        }

        /// <summary>
        /// Creates a dynamic expression.
        /// </summary>
        /// <param name="binder">Binder to evaluate the dynamic expression.</param>
        /// <param name="returnType">Return type of the dynamic expression.</param>
        /// <param name="arguments">Arguments to pass to the dynamic expression.</param>
        /// <returns>New dynamic expression node.</returns>
        protected virtual Expression MakeDynamic(CallSiteBinder binder, Type returnType, IEnumerable<Expression> arguments) => Expression.Dynamic(binder, returnType, arguments);

        #endregion
#endif

        #region ElementInit

        /// <summary>
        /// Visits the children of an element initializer node.
        /// </summary>
        /// <param name="node">The element initializer to visit.</param>
        /// <returns>The modified element initializer, if it or any subexpression was modified; otherwise, returns the original element initializer.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override ElementInit VisitElementInit(ElementInit node)
        {
            var addMethod = VisitMethod(node.AddMethod);
            var arguments = Visit(node.Arguments);

            if (addMethod != node.AddMethod || arguments != node.Arguments)
            {
                return MakeElementInit(addMethod, arguments);
            }

            return node;
        }

        /// <summary>
        /// Creates an element initializer node.
        /// </summary>
        /// <param name="addMethod">Method to add the element to the collection.</param>
        /// <param name="arguments">Arguments to pass to the add method.</param>
        /// <returns>New element initializer node.</returns>
        protected virtual ElementInit MakeElementInit(MethodInfo addMethod, IEnumerable<Expression> arguments) => Expression.ElementInit(addMethod, arguments);

        #endregion

        #region Goto

        /// <summary>
        /// Visits the children of a goto expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitGoto(GotoExpression node)
        {
            var target = VisitIfNotNull(node.Target, VisitLabelTarget);
            var value = Visit(node.Value);
            var type = VisitType(node.Type);

            if (target != node.Target || value != node.Value || type != node.Type)
            {
                return MakeGoto(node.Kind, target, value, type);
            }

            return node;
        }

        /// <summary>
        /// Creates a goto expression.
        /// </summary>
        /// <param name="kind">Kind of the goto expression.</param>
        /// <param name="target">Label to jump to.</param>
        /// <param name="value">Value to transfer to the jump label.</param>
        /// <param name="type">Type of the value transferred to the jump label.</param>
        /// <returns>New goto expression node.</returns>
        protected virtual Expression MakeGoto(GotoExpressionKind kind, LabelTarget target, Expression value, Type type) => Expression.MakeGoto(kind, target, value, type);

        #endregion

        #region Index

        /// <summary>
        /// Visits the children of an index expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitIndex(IndexExpression node)
        {
            var @object = Visit(node.Object);
            var indexer = (PropertyInfo)VisitProperty(node.Indexer);
            var arguments = Visit(node.Arguments);

            if (@object != node.Object || indexer != node.Indexer || arguments != node.Arguments)
            {
                return MakeIndex(@object, indexer, arguments);
            }

            return node;
        }

        /// <summary>
        /// Creates an index expression.
        /// </summary>
        /// <param name="instance">Expression of the instance to index into.</param>
        /// <param name="indexer">Indexer to invoke.</param>
        /// <param name="arguments">Arguments to pass to the indexer.</param>
        /// <returns>New index expression node.</returns>
        protected virtual Expression MakeIndex(Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments) =>
#if USE_SLIM
            Expression.MakeIndex(instance, indexer, arguments.ToReadOnly());
#else
            Expression.MakeIndex(instance, indexer, arguments);
#endif

        #endregion

        #region Invocation

        /// <summary>
        /// Visits the children of an invocation expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitInvocation(InvocationExpression node)
        {
            var expression = Visit(node.Expression);
#if USE_SLIM
            var arguments = (IEnumerable<Expression>)VisitArguments(node);

            if (expression != node.Expression || arguments != null)
            {
                return MakeInvocation(expression, arguments ?? node.Arguments /* PERF: this may allocate; consider a way to return a ListArgumentProviderSlim */);
            }
#else
            var arguments = Visit(node.Arguments);

            if (expression != node.Expression || arguments != node.Arguments)
            {
                return MakeInvocation(expression, arguments);
            }
#endif
            return node;
        }

        /// <summary>
        /// Creates an invocation expression.
        /// </summary>
        /// <param name="expression">Expression of the function to invoke.</param>
        /// <param name="arguments">Arguments to pass to the invocation.</param>
        /// <returns>New invocation expression node.</returns>
        protected virtual Expression MakeInvocation(Expression expression, IEnumerable<Expression> arguments) => Expression.Invoke(expression, arguments);

        #endregion

        #region Label

        /// <summary>
        /// Visits the children of a label expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitLabel(LabelExpression node)
        {
            var target = VisitIfNotNull(node.Target, VisitLabelTarget);
            var defaultValue = Visit(node.DefaultValue);

            if (target != node.Target || defaultValue != node.DefaultValue)
            {
                return MakeLabel(target, defaultValue);
            }

            return node;
        }

        /// <summary>
        /// Creates a label expression.
        /// </summary>
        /// <param name="target">Target label associated with the label expression.</param>
        /// <param name="defaultValue">Default value of the label to use in case a goto expression doesn't carry a value.</param>
        /// <returns>New label expression node.</returns>
        protected virtual Expression MakeLabel(LabelTarget target, Expression defaultValue) => Expression.Label(target, defaultValue);

        #endregion

        #region LabelTarget

        /// <summary>
        /// Visits the children of a label target node.
        /// </summary>
        /// <param name="node">The label target to visit.</param>
        /// <returns>The modified label target, if it or any subexpression was modified; otherwise, returns the original label target.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            var type = VisitType(node.Type);

            if (type != node.Type)
            {
                if (_labels == null)
                {
                    _labels = new Dictionary<LabelTarget, LabelTarget>();
                }
                else if (_labels.TryGetValue(node, out LabelTarget res))
                {
                    return res;
                }

                var newLabel = MakeLabelTarget(type, node.Name);
                _labels[node] = newLabel;
                return newLabel;
            }

            return node;
        }

        /// <summary>
        /// Creates a label target.
        /// </summary>
        /// <param name="type">Type of values received by jumps to the label.</param>
        /// <param name="name">Name of the label.</param>
        /// <returns>New label target.</returns>
        protected virtual LabelTarget MakeLabelTarget(Type type, string name) => Expression.Label(type, name);

        #endregion

        #region Lambda

#if USE_SLIM
        /// <summary>
        /// Visits the children of a lambda expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitLambdaCore(LambdaExpression node)
        {
            var delegateType = VisitType(node.DelegateType);
            var body = Visit(node.Body);
            var parameters = VisitAndConvert(node.Parameters, nameof(VisitLambdaCore));

            if (delegateType != node.DelegateType || body != node.Body || parameters != node.Parameters)
            {
                return MakeLambda(delegateType, body, parameters);
            }

            return node;
        }
#else
        /// <summary>
        /// Visits the children of a lambda expression.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitLambdaCore<T>(Expression<T> node)
        {
            var delegateType = VisitType(node.Type);
            var body = Visit(node.Body);
            var parameters = VisitAndConvert(node.Parameters, nameof(VisitLambdaCore));

            if (delegateType != node.Type || body != node.Body || parameters != node.Parameters)
            {
                return MakeLambda(delegateType, body, node.Name, node.TailCall, parameters);
            }

            return node;
        }
#endif

#if USE_SLIM
        /// <summary>
        /// Creates a lambda expression.
        /// </summary>
        /// <param name="delegateType">Delegate type to use for lambda expression creation.</param>
        /// <param name="body">Body of the lambda expression.</param>
        /// <param name="parameters">Parameters of the lambda expression.</param>
        /// <returns>New lambda expression node.</returns>
        protected virtual Expression MakeLambda(Type delegateType, Expression body, IEnumerable<ParameterExpression> parameters)
        {
            return Expression.Lambda(delegateType, body, parameters);
        }
#else
        /// <summary>
        /// Creates a lambda expression.
        /// </summary>
        /// <param name="delegateType">Delegate type to use for lambda expression creation.</param>
        /// <param name="body">Body of the lambda expression.</param>
        /// <param name="name">Name of the lambda expression.</param>
        /// <param name="tailCall">Indicates whether the lambda expression should be optimized for tail calls.</param>
        /// <param name="parameters">Parameters of the lambda expression.</param>
        /// <returns>New lambda expression node.</returns>
        protected virtual Expression MakeLambda(Type delegateType, Expression body, string name, bool tailCall, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(delegateType, body, name, tailCall, parameters);
#endif

        #endregion

        #region ListInit

        /// <summary>
        /// Visits the children of a list initializer expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitListInit(ListInitExpression node)
        {
            var newExpression = VisitAndConvert(node.NewExpression, nameof(VisitListInit));

            var initializers = Visit(node.Initializers, VisitElementInit);

            if (newExpression != node.NewExpression || initializers != node.Initializers)
            {
                return MakeListInit(newExpression, initializers);
            }

            return node;
        }

        /// <summary>
        /// Creates a list initializer expression.
        /// </summary>
        /// <param name="newExpression">List instance creation expression.</param>
        /// <param name="initializers">Element initializers.</param>
        /// <returns>New list initializer expression node.</returns>
        protected virtual Expression MakeListInit(NewExpression newExpression, IEnumerable<ElementInit> initializers) => Expression.ListInit(newExpression, initializers);

        #endregion

        #region Loop

        /// <summary>
        /// Visits the children of a loop expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitLoop(LoopExpression node)
        {
            var body = Visit(node.Body);
            var @break = VisitIfNotNull(node.BreakLabel, VisitLabelTarget);
            var @continue = VisitIfNotNull(node.ContinueLabel, VisitLabelTarget);

            if (body != node.Body || @break != node.BreakLabel || @continue != node.ContinueLabel)
            {
                return MakeLoop(body, @break, @continue);
            }

            return node;
        }

        /// <summary>
        /// Creates a loop expression.
        /// </summary>
        /// <param name="body">Body of the loop.</param>
        /// <param name="break">Label to jump to upon breaking from the loop.</param>
        /// <param name="continue">Label to jump to upon continuing to the next loop iteration.</param>
        /// <returns>New loop expression node.</returns>
        protected virtual Expression MakeLoop(Expression body, LabelTarget @break, LabelTarget @continue) => Expression.Loop(body, @break, @continue);

        #endregion

        #region Member

        /// <summary>
        /// Visits the children of a member expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitMember(MemberExpression node)
        {
            var expression = Visit(node.Expression);
            var member = VisitMember(node.Member);

            if (expression != node.Expression || member != node.Member)
            {
                return MakeMember(expression, member);
            }

            return node;
        }

        /// <summary>
        /// Creates a member expression.
        /// </summary>
        /// <param name="expression">Expression to obtain the member value from.</param>
        /// <param name="member">Member to invoke.</param>
        /// <returns>New member expression node.</returns>
        protected virtual Expression MakeMember(Expression expression, MemberInfo member) => Expression.MakeMemberAccess(expression, member);

        #endregion

        #region MemberAssignment

        /// <summary>
        /// Visits the children of a member assignment node.
        /// </summary>
        /// <param name="node">The member assignment to visit.</param>
        /// <returns>The modified member assignment, if it or any subexpression was modified; otherwise, returns the original member assignment.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            var member = VisitMember(node.Member);
            var expression = Visit(node.Expression);

            if (member != node.Member || expression != node.Expression)
            {
                return MakeMemberAssignment(member, expression);
            }

            return node;
        }

        /// <summary>
        /// Creates a member assignment node.
        /// </summary>
        /// <param name="member">Member to assign to.</param>
        /// <param name="expression">Expression to assign to the member.</param>
        /// <returns>New member assignment node.</returns>
        protected virtual MemberAssignment MakeMemberAssignment(MemberInfo member, Expression expression) => Expression.Bind(member, expression);

        #endregion

        #region MemberInit

        /// <summary>
        /// Visits the children of a member initialization expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitMemberInit(MemberInitExpression node)
        {
            var newExpression = VisitAndConvert(node.NewExpression, nameof(VisitMemberInit));
            var bindings = Visit(node.Bindings, VisitMemberBinding);

            if (newExpression != node.NewExpression || bindings != node.Bindings)
            {
                return MakeMemberInit(newExpression, bindings);
            }

            return node;
        }

        /// <summary>
        /// Creates a member initialization expression.
        /// </summary>
        /// <param name="newExpression">Object instance creation expression.</param>
        /// <param name="bindings">Member bindings.</param>
        /// <returns>New member initialization expression node.</returns>
        protected virtual Expression MakeMemberInit(NewExpression newExpression, IEnumerable<MemberBinding> bindings) => Expression.MemberInit(newExpression, bindings);

        #endregion

        #region MemberListBinding

        /// <summary>
        /// Visits the children of a member list binding node.
        /// </summary>
        /// <param name="node">The list binding to visit.</param>
        /// <returns>The modified list binding, if it or any subexpression was modified; otherwise, returns the original list binding.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            var member = VisitMember(node.Member);
            var initializers = Visit(node.Initializers, VisitElementInit);

            if (member != node.Member || initializers != node.Initializers)
            {
                return MakeMemberListBinding(member, initializers);
            }

            return node;
        }

        /// <summary>
        /// Creates a member list binding node.
        /// </summary>
        /// <param name="member">Member to apply list bindings to.</param>
        /// <param name="initializers">Element initializers.</param>
        /// <returns>New member list binding node.</returns>
        protected virtual MemberListBinding MakeMemberListBinding(MemberInfo member, IEnumerable<ElementInit> initializers) => Expression.ListBind(member, initializers);

        #endregion

        #region MemberMemberBinding

        /// <summary>
        /// Visits the children of a member binding node.
        /// </summary>
        /// <param name="node">The member binding to visit.</param>
        /// <returns>The modified member binding, if it or any subexpression was modified; otherwise, returns the original member binding.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            var member = VisitMember(node.Member);
            var bindings = Visit(node.Bindings, VisitMemberBinding);

            if (member != node.Member || bindings != node.Bindings)
            {
                return MakeMemberMemberBinding(member, bindings);
            }

            return node;
        }

        /// <summary>
        /// Creates a member binding node.
        /// </summary>
        /// <param name="member">Member to apply member bindings to.</param>
        /// <param name="bindings">Member bindings.</param>
        /// <returns>New member binding node.</returns>
        protected virtual MemberMemberBinding MakeMemberMemberBinding(MemberInfo member, IEnumerable<MemberBinding> bindings) => Expression.MemberBind(member, bindings);

        #endregion

        #region MethodCall

        /// <summary>
        /// Visits the children of a method call expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitMethodCall(MethodCallExpression node)
        {
            var obj = Visit(node.Object);
            var method = VisitMethod(node.Method);
#if USE_SLIM
            var arguments = (IEnumerable<Expression>)VisitArguments(node);

            if (obj != node.Object || arguments != null || method != node.Method)
            {
                return MakeMethodCall(obj, method, arguments ?? node.Arguments /* PERF: this may allocate; consider a way to return a ListArgumentProviderSlim */);
            }
#else
            var arguments = Visit(node.Arguments);

            if (obj != node.Object || arguments != node.Arguments || method != node.Method)
            {
                return MakeMethodCall(obj, method, arguments);
            }
#endif
            return node;
        }

        /// <summary>
        /// Creates a method call expression.
        /// </summary>
        /// <param name="instance">Instance to call the method on.</param>
        /// <param name="method">Method to call.</param>
        /// <param name="arguments">Arguments to pass to the method call.</param>
        /// <returns>New method call expression node.</returns>
        protected virtual Expression MakeMethodCall(Expression instance, MethodInfo method, IEnumerable<Expression> arguments) => Expression.Call(instance, method, arguments);

        #endregion

        #region New

        /// <summary>
        /// Visits the children of a new expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitNew(NewExpression node)
        {
            var res = (Expression)node;

            if (node.Constructor != null)
            {
                var constructor = VisitConstructor(node.Constructor);
#if USE_SLIM
                var arguments = (IEnumerable<Expression>)VisitArguments(node);
#else
                var arguments = Visit(node.Arguments);
#endif
                var members = node.Members != null ? Visit(node.Members, VisitMember) : default;
#if USE_SLIM
                if (constructor != node.Constructor || arguments != null || members != node.Members)
                {
                    res = MakeNew(constructor, arguments ?? node.Arguments /* PERF: this may allocate; consider a way to return a ListArgumentProviderSlim */, members);
                }
#else
                if (constructor != node.Constructor || arguments != node.Arguments || members != node.Members)
                {
                    res = MakeNew(constructor, arguments, members);
                }
#endif
            }
            else
            {
                var type = VisitType(node.Type);

                if (type != node.Type)
                {
                    res = MakeNew(type);
                }
            }

            return res;
        }

        /// <summary>
        /// Creates a new expression.
        /// </summary>
        /// <param name="constructor">Constructor to call.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <param name="members">Members initialized by the arguments passed to the constructor.</param>
        /// <returns>New new expression node.</returns>
        protected virtual Expression MakeNew(ConstructorInfo constructor, IEnumerable<Expression> arguments, IEnumerable<MemberInfo> members)
        {
            if (members != null)
            {
                return Expression.New(constructor, arguments, members);
            }
            else
            {
                return Expression.New(constructor, arguments);
            }
        }

        /// <summary>
        /// Creates a new expression.
        /// </summary>
        /// <param name="type">The new type to use.</param>
        /// <returns>New new expression node.</returns>
        protected virtual Expression MakeNew(Type type) => Expression.New(type);

        #endregion

        #region NewArray

        /// <summary>
        /// Visits the children of a new array expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitNewArray(NewArrayExpression node)
        {
#if USE_SLIM
            var oldElementType = node.ElementType;
#else
            var oldElementType = node.Type.GetElementType();
#endif
            var newElementType = VisitType(oldElementType);

            var expressions = Visit(node.Expressions);

            if (newElementType != oldElementType || expressions != node.Expressions)
            {
                return MakeNewArray(node.NodeType, newElementType, expressions);
            }

            return node;
        }

        /// <summary>
        /// Creates a new array expression.
        /// </summary>
        /// <param name="newArrayType">Array creation expression node type.</param>
        /// <param name="type">Element type of the array.</param>
        /// <param name="expressions">Expression used for array bounds or element initializers.</param>
        /// <returns>>New new array expression node.</returns>
        protected virtual Expression MakeNewArray(ExpressionType newArrayType, Type type, IEnumerable<Expression> expressions)
        {
            return newArrayType switch
            {
                ExpressionType.NewArrayBounds => Expression.NewArrayBounds(type, expressions),
                ExpressionType.NewArrayInit => Expression.NewArrayInit(type, expressions),
                _ => throw new ArgumentException("Unknown array creation expression node type.", nameof(newArrayType)),
            };
        }

        #endregion

        #region Parameter

        /// <summary>
        /// Visits a parameter expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitParameter(ParameterExpression node)
        {
            if (TryLookup(node, out ParameterExpression res))
            {
                return res;
            }

            res = GetState(node);
            base.GlobalScope.Add(node, res);

            return res;
        }

        /// <summary>
        /// Maps a parameter declaration onto a new parameter expression if the visited type is different from the original type.
        /// </summary>
        /// <param name="parameter">Parameter declaration to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected sealed override ParameterExpression GetState(ParameterExpression parameter)
        {
            var type = VisitType(parameter.Type);

            if (type != parameter.Type)
            {
                return Expression.Parameter(type, parameter.Name);
            }

            return parameter;
        }

        #endregion

#if !USE_SLIM
        #region RuntimeVariables

        /// <summary>
        /// Visits the children of a runtime variables expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            var variables = VisitAndConvert(node.Variables, nameof(VisitRuntimeVariables));

            if (variables != node.Variables)
            {
                return MakeRuntimeVariables(variables);
            }

            return node;
        }

        /// <summary>
        /// Creates a runtime variables expression.
        /// </summary>
        /// <param name="variables">Variables to populate the runtime variables collection.</param>
        /// <returns>New runtime variables expression node.</returns>
        protected virtual Expression MakeRuntimeVariables(IEnumerable<ParameterExpression> variables) => Expression.RuntimeVariables(variables);

        #endregion
#endif

        #region Switch

        /// <summary>
        /// Visits the children of a switch expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitSwitch(SwitchExpression node)
        {
            var type = VisitType(node.Type);
            var switchValue = Visit(node.SwitchValue);
            var defaultBody = Visit(node.DefaultBody);
            var comparison = VisitMethod(node.Comparison);
            var cases = Visit(node.Cases, VisitSwitchCase);

            if (type != node.Type || switchValue != node.SwitchValue || defaultBody != node.DefaultBody || comparison != node.Comparison || cases != node.Cases)
            {
                return MakeSwitch(type, switchValue, defaultBody, comparison, cases);
            }

            return node;
        }

        /// <summary>
        /// Creates a switch expression.
        /// </summary>
        /// <param name="type">Result type of the switch expression.</param>
        /// <param name="switchValue">Value to switch on.</param>
        /// <param name="defaultBody">Body of the default label.</param>
        /// <param name="comparison">Method to use for comparison operations.</param>
        /// <param name="cases">Switch cases.</param>
        /// <returns>>New switch expression node.</returns>
        protected virtual Expression MakeSwitch(Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases) => Expression.Switch(type, switchValue, defaultBody, comparison, cases);

        #endregion

        #region SwitchCase

        /// <summary>
        /// Visits the children of a switch case node.
        /// </summary>
        /// <param name="node">The switch case to visit.</param>
        /// <returns>The modified switch case, if it or any subexpression was modified; otherwise, returns the original switch case.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            var body = Visit(node.Body);
            var testValues = Visit(node.TestValues);

            if (body != node.Body || testValues != node.TestValues)
            {
                return MakeSwitchCase(body, testValues);
            }

            return node;
        }

        /// <summary>
        /// Creates a switch case node.
        /// </summary>
        /// <param name="body">Body of the switch case.</param>
        /// <param name="testValues">Test values of the switch case.</param>
        /// <returns>>New switch case node.</returns>
        protected virtual SwitchCase MakeSwitchCase(Expression body, IEnumerable<Expression> testValues) => Expression.SwitchCase(body, testValues);

        #endregion

        #region Try

        /// <summary>
        /// Visits the children of a try expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitTry(TryExpression node)
        {
            var type = VisitType(node.Type);
            var body = Visit(node.Body);
            var @finally = Visit(node.Finally);
            var @fault = Visit(node.Fault);
            var handlers = Visit(node.Handlers, VisitCatchBlock);

            if (type != node.Type || body != node.Body || @finally != node.Finally || @fault != node.Fault || handlers != node.Handlers)
            {
                return MakeTry(type, body, @finally, @fault, handlers);
            }

            return node;
        }

        /// <summary>
        /// Creates a try expression.
        /// </summary>
        /// <param name="type">Result type of the try expression.</param>
        /// <param name="body">Body of the try expression.</param>
        /// <param name="finally">Finally block.</param>
        /// <param name="fault">Fault handler.</param>
        /// <param name="handlers">Catch blocks.</param>
        /// <returns>>New try expression node.</returns>
        protected virtual Expression MakeTry(Type type, Expression body, Expression @finally, Expression fault, IEnumerable<CatchBlock> handlers) => Expression.MakeTry(type, body, @finally, @fault, handlers);

        #endregion

        #region TypeBinary

        /// <summary>
        /// Visits the children of a type binary expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            var expression = Visit(node.Expression);
            var typeOperand = VisitType(node.TypeOperand);

            if (expression != node.Expression || typeOperand != node.TypeOperand)
            {
                return MakeTypeBinary(node.NodeType, expression, typeOperand);
            }

            return node;
        }

        /// <summary>
        /// Creates a type binary expression.
        /// </summary>
        /// <param name="typeBinaryType">Type binary expression node type.</param>
        /// <param name="expression">Expression to apply the type binary operation to.</param>
        /// <param name="typeOperand">Type operand of the type binary operation.</param>
        /// <returns>>New type binary expression node.</returns>
        protected virtual Expression MakeTypeBinary(ExpressionType typeBinaryType, Expression expression, Type typeOperand)
        {
            return typeBinaryType switch
            {
                ExpressionType.TypeIs => Expression.TypeIs(expression, typeOperand),
                ExpressionType.TypeEqual => Expression.TypeEqual(expression, typeOperand),
                _ => throw new ArgumentException("Unknown type binary expression node type.", nameof(typeBinaryType)),
            };
        }

        #endregion

        #region Unary

        /// <summary>
        /// Visits the children of a unary expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override Expression VisitUnary(UnaryExpression node)
        {
            var operand = Visit(node.Operand);
            var type = VisitType(node.Type);
            var method = VisitMethod(node.Method);

            if (operand != node.Operand || type != node.Type || method != node.Method)
            {
                return MakeUnary(node.NodeType, operand, type, method);
            }

            return node;
        }

        /// <summary>
        /// Creates a unary expression.
        /// </summary>
        /// <param name="unaryType">Unary expression node type.</param>
        /// <param name="operand">Operand of the unary expression.</param>
        /// <param name="type">Type used for conversion operations.</param>
        /// <param name="method">Method implementing the operation.</param>
        /// <returns>>New unary expression node.</returns>
        protected virtual Expression MakeUnary(ExpressionType unaryType, Expression operand, Type type, MethodInfo method) => Expression.MakeUnary(unaryType, operand, type, method);

        #endregion

        #region Reflection

        /// <summary>
        /// Visits a constructor. When overridden, other reflection object visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="constructor">Constructor to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual ConstructorInfo VisitConstructor(ConstructorInfo constructor) => constructor;

        /// <summary>
        /// Visits a member. When overridden, other reflection object visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="member">Member to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfo VisitMember(MemberInfo member) => member;

        /// <summary>
        /// Visits a method. When overridden, other reflection object visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MethodInfo VisitMethod(MethodInfo method) => method;

        /// <summary>
        /// Visits a property. When overridden, other reflection object visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="property">Property to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfo VisitProperty(PropertyInfo property) => property;

        /// <summary>
        /// Visits a type. When overridden, other reflection object visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual Type VisitType(Type type) => type;

#if !USE_SLIM
        /// <summary>
        /// Visits a call site binder.
        /// </summary>
        /// <param name="binder">Call site binder to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual CallSiteBinder VisitBinder(CallSiteBinder binder) => binder;
#endif

        #endregion

        #region Helpers

#if !USE_SLIM
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
#endif

        #endregion
    }
}
