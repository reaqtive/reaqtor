// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - April 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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

    using MemberInfo = MemberInfoSlim;
    using Type = TypeSlim;

    #endregion
#endif

    /// <summary>
    /// Base class for expression equality comparer implementations. Default behavior matches trees in a structural fashion.
    /// </summary>
#if USE_SLIM
    public class ExpressionSlimEqualityComparator
#else
    public class ExpressionEqualityComparator
#endif
        : IEqualityComparer<Expression>, IEqualityComparer<MemberBinding>, IEqualityComparer<ElementInit>, IEqualityComparer<CatchBlock>, IEqualityComparer<SwitchCase>
    {
        private readonly IEqualityComparer<Type> _typeComparer;
        private readonly IEqualityComparer<MemberInfo> _memberInfoComparer;
#if USE_SLIM
        private readonly IEqualityComparer<ObjectSlim> _constantExpressionValueComparer;
#else
        private readonly IEqualityComparer<object> _constantExpressionValueComparer;
#endif

        private readonly IEqualityComparer<CallSiteBinder> _callSiteBinderComparer;

        private readonly Stack<IReadOnlyList<ParameterExpression>> _environmentLeft;
        private readonly Stack<IReadOnlyList<ParameterExpression>> _environmentRight;

        /// <summary>
        /// Creates a new expression equality comparator with default comparers for types, members, objects, and call site binders.
        /// </summary>
#if USE_SLIM
        public ExpressionSlimEqualityComparator()
            : this(EqualityComparer<Type>.Default, EqualityComparer<MemberInfo>.Default, EqualityComparer<ObjectSlim>.Default, EqualityComparer<CallSiteBinder>.Default)
#else
        public ExpressionEqualityComparator()
            : this(EqualityComparer<Type>.Default, EqualityComparer<MemberInfo>.Default, EqualityComparer<object>.Default, EqualityComparer<CallSiteBinder>.Default)
#endif

        {
        }

        /// <summary>
        /// Creates a new expression equality comparator with the specified comparers for types, members, and objects.
        /// </summary>
        /// <param name="typeComparer">Comparer for types.</param>
        /// <param name="memberInfoComparer">Comparer for members.</param>
        /// <param name="constantExpressionValueComparer">Comparer for objects found in constant expression nodes.</param>
        /// <param name="callSiteBinderComparer">Comparer for callsite binders.</param>
#if USE_SLIM
        public ExpressionSlimEqualityComparator(IEqualityComparer<Type> typeComparer, IEqualityComparer<MemberInfo> memberInfoComparer, IEqualityComparer<ObjectSlim> constantExpressionValueComparer, IEqualityComparer<CallSiteBinder> callSiteBinderComparer)
#else
        public ExpressionEqualityComparator(IEqualityComparer<Type> typeComparer, IEqualityComparer<MemberInfo> memberInfoComparer, IEqualityComparer<object> constantExpressionValueComparer, IEqualityComparer<CallSiteBinder> callSiteBinderComparer)
#endif
        {
            _typeComparer = typeComparer;
            _memberInfoComparer = memberInfoComparer;
            _constantExpressionValueComparer = constantExpressionValueComparer;

            _environmentLeft = new Stack<IReadOnlyList<ParameterExpression>>();
            _environmentRight = new Stack<IReadOnlyList<ParameterExpression>>();

            _callSiteBinderComparer = callSiteBinderComparer;
        }

        #region Equals

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        public virtual bool Equals(Expression x, Expression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.NodeType != y.NodeType)
            {
                return false;
            }

            var res = false;

            switch (x.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AndAssign:
                case ExpressionType.Assign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.OrAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                    res = EqualsBinary((BinaryExpression)x, (BinaryExpression)y);
                    break;

                case ExpressionType.Conditional:
                    res = EqualsConditional((ConditionalExpression)x, (ConditionalExpression)y);
                    break;

                case ExpressionType.Constant:
                    res = EqualsConstant((ConstantExpression)x, (ConstantExpression)y);
                    break;

                case ExpressionType.Invoke:
                    res = EqualsInvocation((InvocationExpression)x, (InvocationExpression)y);
                    break;

                case ExpressionType.Lambda:
                    res = EqualsLambda((LambdaExpression)x, (LambdaExpression)y);
                    break;

                case ExpressionType.ListInit:
                    res = EqualsListInit((ListInitExpression)x, (ListInitExpression)y);
                    break;

                case ExpressionType.MemberAccess:
                    res = EqualsMember((MemberExpression)x, (MemberExpression)y);
                    break;

                case ExpressionType.MemberInit:
                    res = EqualsMemberInit((MemberInitExpression)x, (MemberInitExpression)y);
                    break;

                case ExpressionType.Call:
                    res = EqualsMethodCall((MethodCallExpression)x, (MethodCallExpression)y);
                    break;

                case ExpressionType.New:
                    res = EqualsNew((NewExpression)x, (NewExpression)y);
                    break;

                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                    res = EqualsNewArray((NewArrayExpression)x, (NewArrayExpression)y);
                    break;

                case ExpressionType.Parameter:
                    res = EqualsParameter((ParameterExpression)x, (ParameterExpression)y);
                    break;

                case ExpressionType.TypeIs:
                case ExpressionType.TypeEqual:
                    res = EqualsTypeBinary((TypeBinaryExpression)x, (TypeBinaryExpression)y);
                    break;

                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.OnesComplement:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Throw:
                case ExpressionType.Unbox:
                    res = EqualsUnary((UnaryExpression)x, (UnaryExpression)y);
                    break;

                case ExpressionType.Block:
                    res = EqualsBlock((BlockExpression)x, (BlockExpression)y);
                    break;

#if !USE_SLIM
                case ExpressionType.DebugInfo:
                    res = EqualsDebugInfo((DebugInfoExpression)x, (DebugInfoExpression)y);
                    break;
#endif

                case ExpressionType.Default:
                    res = EqualsDefault((DefaultExpression)x, (DefaultExpression)y);
                    break;

#if !USE_SLIM
                case ExpressionType.Dynamic:
                    res = EqualsDynamic((DynamicExpression)x, (DynamicExpression)y);
                    break;
#endif

                case ExpressionType.Extension:
                    res = EqualsExtension(x, y);
                    break;

                case ExpressionType.Goto:
                    res = EqualsGoto((GotoExpression)x, (GotoExpression)y);
                    break;

                case ExpressionType.Index:
                    res = EqualsIndex((IndexExpression)x, (IndexExpression)y);
                    break;

                case ExpressionType.Label:
                    res = EqualsLabel((LabelExpression)x, (LabelExpression)y);
                    break;

                case ExpressionType.Loop:
                    res = EqualsLoop((LoopExpression)x, (LoopExpression)y);
                    break;

#if !USE_SLIM
                case ExpressionType.RuntimeVariables:
                    res = EqualsRuntimeVariables((RuntimeVariablesExpression)x, (RuntimeVariablesExpression)y);
                    break;
#endif

                case ExpressionType.Switch:
                    res = EqualsSwitch((SwitchExpression)x, (SwitchExpression)y);
                    break;

                case ExpressionType.Try:
                    res = EqualsTry((TryExpression)x, (TryExpression)y);
                    break;
            }

            return res;
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsBinary(BinaryExpression x, BinaryExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Method, y.Method) &&
#if !USE_SLIM
                Equals(x.IsLifted, y.IsLifted) &&
#endif
                Equals(x.IsLiftedToNull, y.IsLiftedToNull) &&
                Equals(x.Left, y.Left) &&
                Equals(x.Right, y.Right) &&
                Equals(x.Conversion, y.Conversion);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsConditional(ConditionalExpression x, ConditionalExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Test, y.Test) &&
                Equals(x.IfTrue, y.IfTrue) &&
                Equals(x.IfFalse, y.IfFalse);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsConstant(ConstantExpression x, ConstantExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Type, y.Type) &&
                EqualsConstant(x.Value, y.Value);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsInvocation(InvocationExpression x, InvocationExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Expression, y.Expression) &&
                Equals(x.Arguments, y.Arguments);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsLambda(LambdaExpression x, LambdaExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Parameters.Count != y.Parameters.Count)
            {
                return false;
            }

            EqualsPush(x.Parameters, y.Parameters);

            var res =
#if !USE_SLIM
                Equals(x.Type, y.Type) &&
#endif
                Equals(x.Body, y.Body);

#if !USE_SLIM
            res = res && Equals(x.TailCall, y.TailCall);
#endif

            EqualsPop();

            return res;
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsListInit(ListInitExpression x, ListInitExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.NewExpression, y.NewExpression) &&
                Equals(x.Initializers, y.Initializers);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsMember(MemberExpression x, MemberExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Member, y.Member) &&
                Equals(x.Expression, y.Expression);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsMemberInit(MemberInitExpression x, MemberInitExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.NewExpression, y.NewExpression) &&
                Equals(x.Bindings, y.Bindings);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsMethodCall(MethodCallExpression x, MethodCallExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Method, y.Method) &&
                Equals(x.Object, y.Object) &&
                Equals(x.Arguments, y.Arguments);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (Name of expression tree node.)

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsNew(NewExpression x, NewExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Type, y.Type) && // Constructor can be null
                Equals(x.Constructor, y.Constructor) &&
                Equals(x.Arguments, y.Arguments) &&
                Equals(x.Members, y.Members);
        }

#pragma warning restore CA1711
#pragma warning restore IDE0079

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsNewArray(NewArrayExpression x, NewArrayExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
#if USE_SLIM
                Equals(x.ElementType, y.ElementType) &&
#else
                Equals(x.Type, y.Type) &&
#endif
                Equals(x.Expressions, y.Expressions);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsParameter(ParameterExpression x, ParameterExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var l = Find(x, _environmentLeft);
            var r = Find(y, _environmentRight);

            if (l == null && r == null)
            {
                return EqualsGlobalParameter(x, y);
            }

            if (l == null || r == null)
            {
                return false;
            }

            var res = l.Value.Scope == r.Value.Scope && l.Value.Index == r.Value.Index;

#if !USE_SLIM
            res = res && Equals(x.IsByRef, y.IsByRef);
#endif

            return res;
        }

        /// <summary>
        /// Checks whether the two given global parameter expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y) => ReferenceEquals(x, y);

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsTypeBinary(TypeBinaryExpression x, TypeBinaryExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.TypeOperand, y.TypeOperand) &&
                Equals(x.Expression, y.Expression);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsUnary(UnaryExpression x, UnaryExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            switch (x.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.TypeAs:
                case ExpressionType.Throw:
                case ExpressionType.Unbox:
                    if (!Equals(x.Type, y.Type))
                    {
                        return false;
                    }
                    break;
            }

            return
                Equals(x.Method, y.Method) &&
#if !USE_SLIM
                Equals(x.IsLifted, y.IsLifted) &&
                Equals(x.IsLiftedToNull, y.IsLiftedToNull) &&
#endif
                Equals(x.Operand, y.Operand);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsBlock(BlockExpression x, BlockExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Variables.Count != y.Variables.Count)
            {
                return false;
            }

            EqualsPush(x.Variables, y.Variables);

            var res =
                Equals(x.Type, y.Type) &&
                Equals(x.Expressions, y.Expressions);

            EqualsPop();

            return res;
        }

#if !USE_SLIM
        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsDebugInfo(DebugInfoExpression x, DebugInfoExpression y)
        {
            throw new NotImplementedException("Equality of DebugInfoExpression to be supplied by derived types.");
        }
#endif

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsDefault(DefaultExpression x, DefaultExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Type, y.Type);
        }

#if !USE_SLIM
        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsDynamic(DynamicExpression x, DynamicExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Type, y.Type) &&
                Equals(x.DelegateType, y.DelegateType) &&
                Equals(x.Binder, y.Binder) &&
                Equals(x.Arguments, y.Arguments);
        }
#endif

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsGoto(GotoExpression x, GotoExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                GotoLabelAndCheck(x.Target, y.Target) &&
                Equals(x.Kind, y.Kind) &&
                Equals(x.Value, y.Value);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsIndex(IndexExpression x, IndexExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Indexer, y.Indexer) &&
                Equals(x.Object, y.Object) &&
                Equals(x.Arguments, y.Arguments);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsLabel(LabelExpression x, LabelExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                DefineLabelAndCheck(x.Target, y.Target) &&
                Equals(x.DefaultValue, y.DefaultValue);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsLoop(LoopExpression x, LoopExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                DefineLabelAndCheck(x.BreakLabel, y.BreakLabel) &&
                DefineLabelAndCheck(x.ContinueLabel, y.ContinueLabel) &&
                Equals(x.Body, y.Body);
        }

#if !USE_SLIM
        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsRuntimeVariables(RuntimeVariablesExpression x, RuntimeVariablesExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return Equals(x.Variables, y.Variables);
        }
#endif

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsSwitch(SwitchExpression x, SwitchExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.SwitchValue, y.SwitchValue) &&
                Equals(x.Comparison, y.Comparison) &&
                Equals(x.Cases, y.Cases) &&
                Equals(x.DefaultBody, y.DefaultBody);
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsTry(TryExpression x, TryExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Body, y.Body) &&
                Equals(x.Handlers, y.Handlers) &&
                Equals(x.Fault, y.Fault) &&
                Equals(x.Finally, y.Finally);
        }

        /// <summary>
        /// Checks whether the two given switch cases are equal.
        /// </summary>
        /// <param name="x">First switch cases.</param>
        /// <param name="y">Second switch cases.</param>
        /// <returns>true if both switch cases are equal; otherwise, false.</returns>
        public virtual bool Equals(SwitchCase x, SwitchCase y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.TestValues, y.TestValues) &&
                Equals(x.Body, y.Body);
        }

        /// <summary>
        /// Checks whether the two given catch blocks are equal.
        /// </summary>
        /// <param name="x">First catch block.</param>
        /// <param name="y">Second catch block.</param>
        /// <returns>true if both catch blocks are equal; otherwise, false.</returns>
        public virtual bool Equals(CatchBlock x, CatchBlock y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            EqualsPush(new[] { x.Variable }, new[] { y.Variable });

            var res =
                Equals(x.Body, y.Body) &&
                Equals(x.Test, y.Test) &&
                Equals(x.Filter, y.Filter);

            EqualsPop();

            return res;
        }

        /// <summary>
        /// Checks whether the two given call site binders are equal.
        /// </summary>
        /// <param name="x">First call site binder.</param>
        /// <param name="y">Second call site binder.</param>
        /// <returns>true if both call site binders are equal; otherwise, false.</returns>
        protected bool Equals(CallSiteBinder x, CallSiteBinder y)
        {
            return _callSiteBinderComparer.Equals(x, y);
        }

        /// <summary>
        /// Checks whether the two given extension expressions are equal.
        /// </summary>
        /// <param name="x">First extension expression.</param>
        /// <param name="y">Second extension expression.</param>
        /// <returns>true if both extension expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsExtension(Expression x, Expression y)
        {
            throw new NotImplementedException("Equality of extension nodes to be supplied by derived types.");
        }

        /// <summary>
        /// Checks whether the two given member binders are equal.
        /// </summary>
        /// <param name="x">First member binder.</param>
        /// <param name="y">Second member binder.</param>
        /// <returns>true if both member binders are equal; otherwise, false.</returns>
        public virtual bool Equals(MemberBinding x, MemberBinding y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.BindingType != y.BindingType)
            {
                return false;
            }

            var res = false;

            switch (x.BindingType)
            {
                case MemberBindingType.Assignment:
                    res = EqualsMemberAssignment((MemberAssignment)x, (MemberAssignment)y);
                    break;
                case MemberBindingType.ListBinding:
                    res = EqualsMemberListBinding((MemberListBinding)x, (MemberListBinding)y);
                    break;
                case MemberBindingType.MemberBinding:
                    res = EqualsMemberMemberBinding((MemberMemberBinding)x, (MemberMemberBinding)y);
                    break;
            }

            return res;
        }

        /// <summary>
        /// Checks whether the two given member assignments are equal.
        /// </summary>
        /// <param name="x">First member assignment.</param>
        /// <param name="y">Second member assignment.</param>
        /// <returns>true if both member assignments are equal; otherwise, false.</returns>
        protected virtual bool EqualsMemberAssignment(MemberAssignment x, MemberAssignment y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Member, y.Member) &&
                Equals(x.Expression, y.Expression);
        }

        /// <summary>
        /// Checks whether the two given nested member bindings are equal.
        /// </summary>
        /// <param name="x">First nested member binding.</param>
        /// <param name="y">Second nested member binding.</param>
        /// <returns>true if both nested member bindings are equal; otherwise, false.</returns>
        protected virtual bool EqualsMemberMemberBinding(MemberMemberBinding x, MemberMemberBinding y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Member, y.Member) &&
                Equals(x.Bindings, y.Bindings);
        }

        /// <summary>
        /// Checks whether the two given member list bindings are equal.
        /// </summary>
        /// <param name="x">First member list binding.</param>
        /// <param name="y">Second member list binding.</param>
        /// <returns>true if both member list bindings are equal; otherwise, false.</returns>
        protected virtual bool EqualsMemberListBinding(MemberListBinding x, MemberListBinding y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Member, y.Member) &&
                Equals(x.Initializers, y.Initializers);
        }

        /// <summary>
        /// Checks whether the two given element initializers are equal.
        /// </summary>
        /// <param name="x">First element initializer.</param>
        /// <param name="y">Second element initializer.</param>
        /// <returns>true if both element initializers are equal; otherwise, false.</returns>
        public virtual bool Equals(ElementInit x, ElementInit y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.AddMethod, y.AddMethod) &&
                Equals(x.Arguments, y.Arguments);
        }

        /// <summary>
        /// Checks whether the two given members are equal.
        /// </summary>
        /// <param name="x">First member.</param>
        /// <param name="y">Second member.</param>
        /// <returns>true if both members are equal; otherwise, false.</returns>
        protected bool Equals(MemberInfo x, MemberInfo y) => _memberInfoComparer.Equals(x, y);

        /// <summary>
        /// Checks whether the two given member sequences are equal.
        /// </summary>
        /// <param name="x">First member sequence.</param>
        /// <param name="y">Second member sequence.</param>
        /// <returns>true if both member sequences are equal; otherwise, false.</returns>
        protected bool Equals(ReadOnlyCollection<MemberInfo> x, ReadOnlyCollection<MemberInfo> y) => SequenceEqual(x, y, _memberInfoComparer);

        /// <summary>
        /// Checks whether the two given types are equal.
        /// </summary>
        /// <param name="x">First type.</param>
        /// <param name="y">Second type.</param>
        /// <returns>true if both types are equal; otherwise, false.</returns>
        protected bool Equals(Type x, Type y) => _typeComparer.Equals(x, y);

        private static bool Equals(bool x, bool y) => x == y;

#if USE_SLIM
        private bool EqualsConstant(ObjectSlim x, ObjectSlim y) => _constantExpressionValueComparer.Equals(x, y);
#else
        private bool EqualsConstant(object x, object y) => _constantExpressionValueComparer.Equals(x, y);
#endif

        #region Helpers

        /// <summary>
        /// Checks whether the two given expression sequences are equal.
        /// </summary>
        /// <param name="x">First expression sequence.</param>
        /// <param name="y">Second expression sequence.</param>
        /// <returns>true if both expression sequences are equal; otherwise, false.</returns>
        protected bool Equals(ReadOnlyCollection<Expression> x, ReadOnlyCollection<Expression> y) => SequenceEqual(x, y, this);

        /// <summary>
        /// Checks whether the two given parameter expression sequences are equal.
        /// </summary>
        /// <param name="x">First parameter expression sequence.</param>
        /// <param name="y">Second parameter expression sequence.</param>
        /// <returns>true if both parameter expression sequences are equal; otherwise, false.</returns>
        protected bool Equals(ReadOnlyCollection<ParameterExpression> x, ReadOnlyCollection<ParameterExpression> y) => SequenceEqual(x, y, this);

        /// <summary>
        /// Checks whether the two given member binding sequences are equal.
        /// </summary>
        /// <param name="x">First member binding sequence.</param>
        /// <param name="y">Second member binding sequence.</param>
        /// <returns>true if both member binding sequences are equal; otherwise, false.</returns>
        protected bool Equals(ReadOnlyCollection<MemberBinding> x, ReadOnlyCollection<MemberBinding> y) => SequenceEqual(x, y, this);

        /// <summary>
        /// Checks whether the two given element initializer sequences are equal.
        /// </summary>
        /// <param name="x">First element initializer sequence.</param>
        /// <param name="y">Second element initializer sequence.</param>
        /// <returns>true if both element initializer sequences are equal; otherwise, false.</returns>
        protected bool Equals(ReadOnlyCollection<ElementInit> x, ReadOnlyCollection<ElementInit> y) => SequenceEqual(x, y, this);

        /// <summary>
        /// Checks whether the two given sequences are equal.
        /// </summary>
        /// <typeparam name="T">Element type of the sequences.</typeparam>
        /// <param name="first">First sequence to compare.</param>
        /// <param name="second">Second sequence to compare.</param>
        /// <param name="comparer">Equality comparer for elements.</param>
        /// <returns>true if both sequences are equal; otherwise, false.</returns>
        private static bool SequenceEqual<T>(ReadOnlyCollection<T> first, ReadOnlyCollection<T> second, IEqualityComparer<T> comparer)
        {
            if (first == null && second == null)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            var n = first.Count;

            if (n != second.Count)
            {
                return false;
            }

            for (var i = 0; i < n; i++)
            {
                if (!comparer.Equals(first[i], second[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private Dictionary<LabelTarget, HashSet<LabelTarget>> _labelDefinitionsLeft;
        private Dictionary<LabelTarget, HashSet<LabelTarget>> _labelDefinitionsRight;
        private Dictionary<LabelTarget, HashSet<LabelTarget>> _labelGotosLeft;
        private Dictionary<LabelTarget, HashSet<LabelTarget>> _labelGotosRight;

        private void EnsureBranchTrackingDataStructures()
        {
            if (_labelDefinitionsLeft == null)
            {
                _labelDefinitionsLeft = new Dictionary<LabelTarget, HashSet<LabelTarget>>();
                _labelDefinitionsRight = new Dictionary<LabelTarget, HashSet<LabelTarget>>();
                _labelGotosLeft = new Dictionary<LabelTarget, HashSet<LabelTarget>>();
                _labelGotosRight = new Dictionary<LabelTarget, HashSet<LabelTarget>>();
            }
        }

        /// <summary>
        /// Checks whether the two given switch case sequences are equal.
        /// </summary>
        /// <param name="x">First switch case sequence.</param>
        /// <param name="y">Second switch case sequence.</param>
        /// <returns>true if both switch case sequences are equal; otherwise, false.</returns>
        protected bool Equals(ReadOnlyCollection<SwitchCase> x, ReadOnlyCollection<SwitchCase> y) => SequenceEqual(x, y, this);

        /// <summary>
        /// Checks whether the two given catch block sequences are equal.
        /// </summary>
        /// <param name="x">First catch block sequence.</param>
        /// <param name="y">Second catch block sequence.</param>
        /// <returns>true if both catch block sequences are equal; otherwise, false.</returns>
        protected bool Equals(ReadOnlyCollection<CatchBlock> x, ReadOnlyCollection<CatchBlock> y) => SequenceEqual(x, y, this);

        /// <summary>
        /// Push parameters into the expression environments for equality checks.
        /// </summary>
        /// <param name="x">The left parameters.</param>
        /// <param name="y">The right parameters.</param>
        protected void EqualsPush(IReadOnlyList<ParameterExpression> x, IReadOnlyList<ParameterExpression> y)
        {
            _environmentLeft.Push(x);
            _environmentRight.Push(y);
        }

        /// <summary>
        /// Pop parameters from the expression environments for equality checks.
        /// </summary>
        protected void EqualsPop()
        {
            _environmentRight.Pop();
            _environmentLeft.Pop();
        }

        private static bool Equals(GotoExpressionKind x, GotoExpressionKind y) => x == y;

        private bool DefineLabelAndCheck(LabelTarget x, LabelTarget y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            EnsureBranchTrackingDataStructures();

            UpdateLabelMap(_labelDefinitionsLeft, x, y);
            UpdateLabelMap(_labelDefinitionsRight, y, x);

            if (!CheckLabelMap(_labelGotosLeft, x, y) || !CheckLabelMap(_labelGotosRight, y, x))
            {
                return false;
            }

            return true;
        }

        private bool GotoLabelAndCheck(LabelTarget x, LabelTarget y)
        {
            EnsureBranchTrackingDataStructures();

            UpdateLabelMap(_labelGotosLeft, x, y);
            UpdateLabelMap(_labelGotosRight, y, x);

            if (!CheckLabelMap(_labelDefinitionsLeft, x, y) || !CheckLabelMap(_labelDefinitionsRight, y, x))
            {
                return false;
            }

            return true;
        }

        private static void UpdateLabelMap(Dictionary<LabelTarget, HashSet<LabelTarget>> labelMap, LabelTarget source, LabelTarget target)
        {
            if (!labelMap.TryGetValue(source, out HashSet<LabelTarget> labels))
            {
                labelMap[source] = labels = new HashSet<LabelTarget>();
            }

            labels.Add(target);
        }

        private static bool CheckLabelMap(Dictionary<LabelTarget, HashSet<LabelTarget>> labelMap, LabelTarget source, LabelTarget target)
        {
            if (labelMap.TryGetValue(source, out HashSet<LabelTarget> labels) && !labels.Contains(target))
            {
                return false;
            }

            return true;
        }

        private static ParameterIndex? Find(ParameterExpression p, Stack<IReadOnlyList<ParameterExpression>> parameters)
        {
            var scope = 0;
            foreach (var frame in parameters)
            {
                var index = 0;
                for (int i = 0, n = frame.Count; i < n; i++)
                {
                    if (frame[i] == p)
                    {
                        return new ParameterIndex { Scope = scope, Index = index };
                    }

                    index++;
                }

                scope++;
            }

            return null;
        }

        private struct ParameterIndex
        {
            public int Scope;
            public int Index;
        }

        #endregion

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        public virtual int GetHashCode(Expression obj)
        {
            if (obj == null)
            {
                return 17;
            }

            switch (obj.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AndAssign:
                case ExpressionType.Assign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.OrAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                    return GetHashCodeBinary((BinaryExpression)obj);

                case ExpressionType.Conditional:
                    return GetHashCodeConditional((ConditionalExpression)obj);

                case ExpressionType.Constant:
                    return GetHashCodeConstant((ConstantExpression)obj);

                case ExpressionType.Invoke:
                    return GetHashCodeInvocation((InvocationExpression)obj);

                case ExpressionType.Lambda:
                    return GetHashCodeLambda((LambdaExpression)obj);

                case ExpressionType.ListInit:
                    return GetHashCodeListInit((ListInitExpression)obj);

                case ExpressionType.MemberAccess:
                    return GetHashCodeMember((MemberExpression)obj);

                case ExpressionType.MemberInit:
                    return GetHashCodeMemberInit((MemberInitExpression)obj);

                case ExpressionType.Call:
                    return GetHashCodeMethodCall((MethodCallExpression)obj);

                case ExpressionType.New:
                    return GetHashCodeNew((NewExpression)obj);

                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                    return GetHashCodeNewArray((NewArrayExpression)obj);

                case ExpressionType.Parameter:
                    return GetHashCodeParameter((ParameterExpression)obj);

                case ExpressionType.TypeIs:
                case ExpressionType.TypeEqual:
                    return GetHashCodeTypeBinary((TypeBinaryExpression)obj);

                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.OnesComplement:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Throw:
                case ExpressionType.Unbox:
                    return GetHashCodeUnary((UnaryExpression)obj);

                case ExpressionType.Block:
                    return GetHashCodeBlock((BlockExpression)obj);

#if !USE_SLIM
                case ExpressionType.DebugInfo:
                    return GetHashCodeDebugInfo((DebugInfoExpression)obj);
#endif

                case ExpressionType.Default:
                    return GetHashCodeDefault((DefaultExpression)obj);

#if !USE_SLIM
                case ExpressionType.Dynamic:
                    return GetHashCodeDynamic((DynamicExpression)obj);
#endif

                case ExpressionType.Extension:
                    return GetHashCodeExtension(obj);

                case ExpressionType.Goto:
                    return GetHashCodeGoto((GotoExpression)obj);

                case ExpressionType.Index:
                    return GetHashCodeIndex((IndexExpression)obj);

                case ExpressionType.Label:
                    return GetHashCodeLabel((LabelExpression)obj);

                case ExpressionType.Loop:
                    return GetHashCodeLoop((LoopExpression)obj);

#if !USE_SLIM
                case ExpressionType.RuntimeVariables:
                    return GetHashCodeRuntimeVariables((RuntimeVariablesExpression)obj);
#endif

                case ExpressionType.Switch:
                    return GetHashCodeSwitch((SwitchExpression)obj);

                case ExpressionType.Try:
                    return GetHashCodeTry((TryExpression)obj);
            }

            return 1979;
        }

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeBinary(BinaryExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.NodeType),
                GetHashCode(obj.Left),
                GetHashCode(obj.Right),
                Hash(
                    GetHashCode(obj.Conversion),
                    GetHashCode(obj.Method),
#if !USE_SLIM
                    GetHashCode(obj.IsLifted),
#endif
                    GetHashCode(obj.IsLiftedToNull)
                )
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeConditional(ConditionalExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Test),
                GetHashCode(obj.IfTrue),
                GetHashCode(obj.IfFalse)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeConstant(ConstantExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Type),
                GetHashCodeConstant(obj.Value)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeInvocation(InvocationExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Expression),
                GetHashCode(obj.Arguments)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeLambda(LambdaExpression obj)
        {
            if (obj == null)
            {
                return 17;
            }

            GetHashCodePush(obj.Parameters);

#if USE_SLIM
            var res = GetHashCode(obj.Body);
#else
            var res = Hash(
                GetHashCode(obj.Body),
                GetHashCode(obj.Type)
            );
#endif

#if !USE_SLIM
            res = Hash(res, obj.TailCall.GetHashCode());
#endif

            GetHashCodePop();

            return res;
        }

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeListInit(ListInitExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.NewExpression),
                GetHashCode(obj.Initializers)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeMember(MemberExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Expression),
                GetHashCode(obj.Member)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeMemberInit(MemberInitExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.NewExpression),
                GetHashCode(obj.Bindings)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeMethodCall(MethodCallExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Object),
                GetHashCode(obj.Method),
                GetHashCode(obj.Arguments)
            );

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (Name of expression tree node.)

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeNew(NewExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Type),
                GetHashCode(obj.Constructor),
                GetHashCode(obj.Arguments),
                GetHashCode(obj.Members)
            );

#pragma warning restore CA1711
#pragma warning restore IDE0079

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeNewArray(NewArrayExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.NodeType),
#if USE_SLIM
                GetHashCode(obj.ElementType),
#else
                GetHashCode(obj.Type),
#endif
                GetHashCode(obj.Expressions)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeParameter(ParameterExpression obj)
        {
            if (obj == null)
            {
                return 17;
            }

            var i = 0;
            foreach (var frame in _environmentLeft)
            {
                for (int j = 0, n = frame.Count; j < n; j++)
                {
                    if (frame[j] == obj)
                    {
                        return i * 37 + j;
                    }
                }

                i++;
            }

            return GetHashCodeGlobalParameter(obj);
        }

        /// <summary>
        /// Gets a hash code for a global parameter.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeGlobalParameter(ParameterExpression obj) =>
            obj == null ? 17 : obj.GetHashCode();

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeTypeBinary(TypeBinaryExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.NodeType),
                GetHashCode(obj.Expression),
                GetHashCode(obj.TypeOperand)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeUnary(UnaryExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.NodeType),
                GetHashCode(obj.Operand),
#if USE_SLIM
                GetHashCode(obj.Method)
#else
                Hash(
                    GetHashCode(obj.Method),
                    obj.IsLifted.GetHashCode(),
                    obj.IsLiftedToNull.GetHashCode()
                )
#endif
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeBlock(BlockExpression obj)
        {
            if (obj == null)
            {
                return 17;
            }

            GetHashCodePush(obj.Variables);

            var res = Hash(
                GetHashCode(obj.Expressions),
                GetHashCode(obj.Type)
            );

            GetHashCodePop();

            return res;
        }

#if !USE_SLIM
        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeDebugInfo(DebugInfoExpression obj)
        {
            throw new NotImplementedException("Hash code of DebugInfoExpression to be supplied by derived types.");
        }
#endif

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeDefault(DefaultExpression obj) =>
            obj == null ? 17 : GetHashCode(obj.Type);

#if !USE_SLIM
        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeDynamic(DynamicExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Arguments),
                GetHashCode(obj.Type),
                GetHashCode(obj.DelegateType),
                GetHashCode(obj.Binder)
            );
#endif

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeGoto(GotoExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Value),
                GetHashCode(obj.Target),
                (int)obj.Kind
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeIndex(IndexExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Object),
                GetHashCode(obj.Indexer),
                GetHashCode(obj.Arguments)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeLabel(LabelExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.DefaultValue),
                GetHashCode(obj.Target)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeLoop(LoopExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.ContinueLabel),
                GetHashCode(obj.BreakLabel),
                GetHashCode(obj.Body)
            );

        private static int GetHashCode(LabelTarget obj) =>
            obj == null ? 17 : obj.GetHashCode();

#if !USE_SLIM
        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeRuntimeVariables(RuntimeVariablesExpression obj) =>
            obj == null ? 17 : GetHashCode(obj.Variables);
#endif

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeSwitch(SwitchExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.SwitchValue),
                GetHashCode(obj.Comparison),
                GetHashCode(obj.Cases),
                GetHashCode(obj.DefaultBody)
            );

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeTry(TryExpression obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Body),
                GetHashCode(obj.Handlers),
                GetHashCode(obj.Fault),
                GetHashCode(obj.Finally)
            );

        /// <summary>
        /// Gets a hash code for the given switch case.
        /// </summary>
        /// <param name="obj">Switch case to compute a hash code for.</param>
        /// <returns>Hash code for the given switch case.</returns>
        public virtual int GetHashCode(SwitchCase obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.TestValues),
                GetHashCode(obj.Body)
            );

        /// <summary>
        /// Gets a hash code for the given catch block.
        /// </summary>
        /// <param name="obj">Catch block to compute a hash code for.</param>
        /// <returns>Hash code for the given catch block.</returns>
        public virtual int GetHashCode(CatchBlock obj)
        {
            if (obj == null)
            {
                return 17;
            }

            GetHashCodePush(new[] { obj.Variable });

            var res = Hash(
                GetHashCode(obj.Body),
                GetHashCode(obj.Test),
                GetHashCode(obj.Filter)
            );

            GetHashCodePop();

            return res;
        }

        /// <summary>
        /// Gets a hash code for the given call site binder.
        /// </summary>
        /// <param name="obj">Call site binder to compute a hash code for.</param>
        /// <returns>Hash code for the given call site binder.</returns>
        protected int GetHashCode(CallSiteBinder obj) => _callSiteBinderComparer.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given extension expression.
        /// </summary>
        /// <param name="obj">Extension expression to compute a hash code for.</param>
        /// <returns>Hash code for the given extension expression.</returns>
        protected virtual int GetHashCodeExtension(Expression obj)
        {
            throw new NotImplementedException("Hash code of extension nodes to be supplied by derived types.");
        }

        /// <summary>
        /// Gets a hash code for the given member binding.
        /// </summary>
        /// <param name="obj">Member binding to compute a hash code for.</param>
        /// <returns>Hash code for the given member binding.</returns>
        public virtual int GetHashCode(MemberBinding obj)
        {
            var res = 17;

            if (obj == null)
            {
                return res;
            }

            res = Hash(res, (int)obj.BindingType);

            switch (obj.BindingType)
            {
                case MemberBindingType.Assignment:
                    res = Hash(res, GetHashCodeMemberAssignment((MemberAssignment)obj));
                    break;
                case MemberBindingType.ListBinding:
                    res = Hash(res, GetHashCodeMemberListBinding((MemberListBinding)obj));
                    break;
                case MemberBindingType.MemberBinding:
                    res = Hash(res, GetHashCodeMemberMemberBinding((MemberMemberBinding)obj));
                    break;
            }

            return res;
        }

        /// <summary>
        /// Gets a hash code for the given member assignment.
        /// </summary>
        /// <param name="obj">Member assignment to compute a hash code for.</param>
        /// <returns>Hash code for the given member assignment.</returns>
        protected virtual int GetHashCodeMemberAssignment(MemberAssignment obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Member),
                GetHashCode(obj.Expression)
            );

        /// <summary>
        /// Gets a hash code for the given deep member binding.
        /// </summary>
        /// <param name="obj">Deep member binding to compute a hash code for.</param>
        /// <returns>Hash code for the given deep member binding.</returns>
        protected virtual int GetHashCodeMemberMemberBinding(MemberMemberBinding obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Member),
                GetHashCode(obj.Bindings)
            );

        /// <summary>
        /// Gets a hash code for the given member list binding.
        /// </summary>
        /// <param name="obj">Member list binding to compute a hash code for.</param>
        /// <returns>Hash code for the given member list binding.</returns>
        protected virtual int GetHashCodeMemberListBinding(MemberListBinding obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.Member),
                GetHashCode(obj.Initializers)
            );

        /// <summary>
        /// Gets a hash code for the given element initializer.
        /// </summary>
        /// <param name="obj">Element initializer to compute a hash code for.</param>
        /// <returns>Hash code for the given element initializer.</returns>
        public virtual int GetHashCode(ElementInit obj) =>
            obj == null ? 17 : Hash(
                GetHashCode(obj.AddMethod),
                GetHashCode(obj.Arguments)
            );

        /// <summary>
        /// Gets a hash code for the given member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected int GetHashCode(MemberInfo obj) => _memberInfoComparer.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given member sequence.
        /// </summary>
        /// <param name="obj">Member sequence to compute a hash code for.</param>
        /// <returns>Hash code for the given member sequence.</returns>
        protected int GetHashCode(ReadOnlyCollection<MemberInfo> obj)
        {
            unchecked
            {
                var h = 17;

                if (obj != null)
                {
                    for (int i = 0, n = obj.Count; i < n; i++)
                    {
                        h = h * 23 + GetHashCode(obj[i]);
                    }
                }

                return h;
            }
        }

        /// <summary>
        /// Gets a hash code for the given type.
        /// </summary>
        /// <param name="obj">Type to compute a hash code for.</param>
        /// <returns>Hash code for the given type.</returns>
        protected int GetHashCode(Type obj) => _typeComparer.GetHashCode(obj);

        /// <summary>
        /// Push parameters into the expression environment for hash code computation.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        protected void GetHashCodePush(IReadOnlyList<ParameterExpression> parameters) => _environmentLeft.Push(parameters);

        /// <summary>
        /// Pop parameters from the expression environments for hash code computation.
        /// </summary>
        protected void GetHashCodePop() => _environmentLeft.Pop();

        private static int GetHashCode(ExpressionType obj) => (int)obj;

        private static int GetHashCode(bool obj) => obj.GetHashCode();

#if USE_SLIM
        private int GetHashCodeConstant(ObjectSlim obj) => _constantExpressionValueComparer.GetHashCode(obj);
#else
        private int GetHashCodeConstant(object obj) => _constantExpressionValueComparer.GetHashCode(obj);
#endif

        #region Helpers

        /// <summary>
        /// Gets a hash code for the given expression sequence.
        /// </summary>
        /// <param name="obj">Expression sequence to compute a hash code for.</param>
        /// <returns>Hash code for the given expression sequence.</returns>
        protected int GetHashCode(ReadOnlyCollection<Expression> obj)
        {
            unchecked
            {
                var h = 17;

                if (obj != null)
                {
                    for (int i = 0, n = obj.Count; i < n; i++)
                    {
                        h = h * 23 + GetHashCode(obj[i]);
                    }
                }

                return h;
            }
        }

        /// <summary>
        /// Gets a hash code for the given parameter expression sequence.
        /// </summary>
        /// <param name="obj">Parameter expression sequence to compute a hash code for.</param>
        /// <returns>Hash code for the given parameter expression sequence.</returns>
        protected int GetHashCode(ReadOnlyCollection<ParameterExpression> obj)
        {
            unchecked
            {
                var h = 17;

                if (obj != null)
                {
                    for (int i = 0, n = obj.Count; i < n; i++)
                    {
                        h = h * 23 + GetHashCode(obj[i]);
                    }
                }

                return h;
            }
        }

        /// <summary>
        /// Gets a hash code for the given member binding sequence.
        /// </summary>
        /// <param name="obj">Member binding sequence to compute a hash code for.</param>
        /// <returns>Hash code for the given member binding sequence.</returns>
        protected int GetHashCode(ReadOnlyCollection<MemberBinding> obj)
        {
            unchecked
            {
                var h = 17;

                if (obj != null)
                {
                    for (int i = 0, n = obj.Count; i < n; i++)
                    {
                        h = h * 23 + GetHashCode(obj[i]);
                    }
                }

                return h;
            }
        }

        /// <summary>
        /// Gets a hash code for the given element initializer sequence.
        /// </summary>
        /// <param name="obj">Member sequence to compute a hash code for.</param>
        /// <returns>Hash code for the given element initializer sequence.</returns>
        protected int GetHashCode(ReadOnlyCollection<ElementInit> obj)
        {
            unchecked
            {
                var h = 17;

                if (obj != null)
                {
                    for (int i = 0, n = obj.Count; i < n; i++)
                    {
                        h = h * 23 + GetHashCode(obj[i]);
                    }
                }

                return h;
            }
        }

        /// <summary>
        /// Gets a hash code for the given switch case sequence.
        /// </summary>
        /// <param name="obj">Switch case sequence to compute a hash code for.</param>
        /// <returns>Hash code for the given switch case sequence.</returns>
        protected int GetHashCode(ReadOnlyCollection<SwitchCase> obj)
        {
            unchecked
            {
                var h = 17;

                if (obj != null)
                {
                    for (int i = 0, n = obj.Count; i < n; i++)
                    {
                        h = h * 23 + GetHashCode(obj[i]);
                    }
                }

                return h;
            }
        }

        /// <summary>
        /// Gets a hash code for the given catch block sequence.
        /// </summary>
        /// <param name="obj">Catch block sequence to compute a hash code for.</param>
        /// <returns>Hash code for the given catch block sequence.</returns>
        protected int GetHashCode(ReadOnlyCollection<CatchBlock> obj)
        {
            unchecked
            {
                var h = 17;

                if (obj != null)
                {
                    for (int i = 0, n = obj.Count; i < n; i++)
                    {
                        h = h * 23 + GetHashCode(obj[i]);
                    }
                }

                return h;
            }
        }

        /// <summary>
        /// Helper method to create a hash code from the specified values.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Hash code composed from the specified values.</returns>
        protected static int Hash(int a, int b)
        {
            unchecked
            {
                var h = 17;
                h = h * 23 + a;
                h = h * 23 + b;
                return h;
            }
        }

        /// <summary>
        /// Helper method to create a hash code from the specified values.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="c">Third value.</param>
        /// <returns>Hash code composed from the specified values.</returns>
        protected static int Hash(int a, int b, int c)
        {
            unchecked
            {
                var h = 17;
                h = h * 23 + a;
                h = h * 23 + b;
                h = h * 23 + c;
                return h;
            }
        }

        /// <summary>
        /// Helper method to create a hash code from the specified values.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="c">Third value.</param>
        /// <param name="d">Fourth value.</param>
        /// <returns>Hash code composed from the specified values.</returns>
        protected static int Hash(int a, int b, int c, int d)
        {
            unchecked
            {
                var h = 17;
                h = h * 23 + a;
                h = h * 23 + b;
                h = h * 23 + c;
                h = h * 23 + d;
                return h;
            }
        }

        #endregion

        #endregion
    }
}
