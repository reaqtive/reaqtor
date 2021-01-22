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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// An expression slim visitor to produce an expression from an expression slim.
    /// </summary>
    public class ExpressionSlimToExpressionConverter : ExpressionSlimVisitor<Expression, LambdaExpression, ParameterExpression, NewExpression, ElementInit, MemberBinding, MemberAssignment, MemberListBinding, MemberMemberBinding, CatchBlock, SwitchCase, LabelTarget>
    {
        #region Fields

        // TODO Use memory pool for these dictionaries
        private readonly Dictionary<ParameterExpressionSlim, ParameterExpression> _variables;
        private readonly Dictionary<LabelTargetSlim, LabelTarget> _labels;
        private readonly IExpressionFactory _factory;

        private Func<MethodInfoSlim, MethodInfo> _makeMethod;
        private Func<TypeSlim, Type> _makeType;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an expression slim to expression converter using a fresh type space.
        /// </summary>
        public ExpressionSlimToExpressionConverter()
            : this(new InvertedTypeSpace())
        {
        }

        /// <summary>
        /// Creates an expression slim to expression converter using the provided type space.
        /// </summary>
        /// <param name="typeSpace">A type space, potentially containing pre-mapped types.</param>
        public ExpressionSlimToExpressionConverter(InvertedTypeSpace typeSpace)
            : this(typeSpace, ExpressionFactory.Instance)
        {
        }

        /// <summary>
        /// Creates an expression slim to expression converter using the provided type space.
        /// </summary>
        /// <param name="typeSpace">A type space, potentially containing pre-mapped types.</param>
        /// <param name="factory">The expression factory to use for expression creation.</param>
        public ExpressionSlimToExpressionConverter(InvertedTypeSpace typeSpace, IExpressionFactory factory)
        {
            TypeSpace = typeSpace ?? throw new ArgumentNullException(nameof(typeSpace));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _variables = new Dictionary<ParameterExpressionSlim, ParameterExpression>();
            _labels = new Dictionary<LabelTargetSlim, LabelTarget>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type space containing mapped types.
        /// </summary>
        protected InvertedTypeSpace TypeSpace { get; }

        private Func<MethodInfoSlim, MethodInfo> MakeMethodCachedDelegate => _makeMethod ??= MakeMethod;

        private Func<TypeSlim, Type> MakeTypeCachedDelegate => _makeType ??= MakeType;

        #endregion

        #region Methods

        /// <summary>
        /// Visits a binary expression slim tree node, produces a binary expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="left">Left operand.</param>
        /// <param name="conversion">Conversion operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>The binary expression represented by the expression slim node.</returns>
        protected override Expression MakeBinary(BinaryExpressionSlim node, Expression left, LambdaExpression conversion, Expression right)
        {
            var method = VisitIfNotNull(node.Method, MakeMethodCachedDelegate);

            var res = default(Expression);

            if (method == null && conversion == null && !left.Type.IsValueType && !right.Type.IsValueType)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Equal:
                        res = _factory.ReferenceEqual(left, right);
                        break;
                    case ExpressionType.NotEqual:
                        res = _factory.ReferenceNotEqual(left, right);
                        break;
                }
            }

            if (res == null)
            {
                res = _factory.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, method, conversion);
            }

            return res;
        }

        /// <summary>
        /// Visits a block expression slim tree node, produces a block expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="variables">Variables in the block.</param>
        /// <param name="expressions">Expression slims in the block.</param>
        /// <returns>The block expression represented by the expression slim node.</returns>
        protected override Expression MakeBlock(BlockExpressionSlim node, ReadOnlyCollection<ParameterExpression> variables, ReadOnlyCollection<Expression> expressions)
        {
            if (node.Type != null)
            {
                var type = MakeType(node.Type);
                return _factory.Block(type, variables, expressions);
            }
            else
            {
                return _factory.Block(variables, expressions);
            }
        }

        /// <summary>
        /// Visits a catch block slim tree node, produces a catch block.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="variable">Variable expressions.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="filter">Filter expressions.</param>
        /// <returns>The catch block represented by the catch block slim node.</returns>
        protected override CatchBlock MakeCatchBlock(CatchBlockSlim node, ParameterExpression variable, Expression body, Expression filter)
        {
            return _factory.MakeCatchBlock(VisitIfNotNull(node.Test, MakeTypeCachedDelegate), variable, body, filter);
        }

        /// <summary>
        /// Visits a conditional expression slim tree node, produces a conditional expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="test">Test operand.</param>
        /// <param name="ifTrue">True operand.</param>
        /// <param name="ifFalse">False operand.</param>
        /// <returns>The conditional expression represented by the expression slim node.</returns>
        protected override Expression MakeConditional(ConditionalExpressionSlim node, Expression test, Expression ifTrue, Expression ifFalse)
        {
            if (node.Type != null)
            {
                var type = MakeType(node.Type);
                return _factory.Condition(test, ifTrue, ifFalse, type);
            }
            else
            {
                return _factory.Condition(test, ifTrue, ifFalse);
            }
        }

        /// <summary>
        /// Visits a constant expression slim tree node, produces a constant expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>The constant expression represented by the expression slim node.</returns>
        protected override Expression MakeConstant(ConstantExpressionSlim node)
        {
            var type = MakeType(node.Type);

            var reduced = default(object);

            if (node.Value != null)
            {
                reduced = node.Value.Reduce(type);
            }

            return _factory.Constant(reduced, type);
        }

        /// <summary>
        /// Visits a default expression slim tree node, produces a default expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>The default expression represented by the expression slim node.</returns>
        protected override Expression MakeDefault(DefaultExpressionSlim node)
        {
            var type = MakeType(node.Type);
            return _factory.Default(type);
        }

        /// <summary>
        /// Visits an element init slim tree node, produces an element init.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>The element init represented by the element init slim node.</returns>
        protected override ElementInit MakeElementInit(ElementInitSlim node, ReadOnlyCollection<Expression> arguments)
        {
            var method = MakeMethod(node.AddMethod);
            return _factory.ElementInit(method, arguments);
        }

        /// <summary>
        /// Visits a goto expression slim tree node, produces a goto expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="target">Target label.</param>
        /// <param name="value">Value expression.</param>
        /// <returns>The goto expression represented by the expression slim node.</returns>
        protected override Expression MakeGoto(GotoExpressionSlim node, LabelTarget target, Expression value)
        {
            var t = VisitIfNotNull(node.Type, MakeTypeCachedDelegate);
            return _factory.MakeGoto(node.Kind, target, value, t);
        }

        /// <summary>
        /// Visits an index expression slim tree node, produces an index expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="object">Object expression.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>The index expression represented by the expression slim node.</returns>
        protected override Expression MakeIndex(IndexExpressionSlim node, Expression @object, ReadOnlyCollection<Expression> arguments)
        {
            if (node.Indexer != null)
            {
                var indexer = MakeProperty(node.Indexer);
                return _factory.MakeIndex(@object, indexer, arguments);
            }
            else
            {
                return _factory.MakeIndex(@object, indexer: null, arguments);
            }
        }

        /// <summary>
        /// Visits an invocation expression slim tree node, produces an invocation expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="expression">Function expression.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>The invocation expression represented by the expression slim node.</returns>
        protected override Expression MakeInvocation(InvocationExpressionSlim node, Expression expression, ReadOnlyCollection<Expression> arguments)
        {
            return _factory.Invoke(expression, arguments);
        }

        /// <summary>
        /// Visits a label expression slim tree node, produces a label expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="target">Target label.</param>
        /// <param name="defaultValue">Default value expression.</param>
        /// <returns>The label expression represented by the expression slim node.</returns>
        protected override Expression MakeLabel(LabelExpressionSlim node, LabelTarget target, Expression defaultValue)
        {
            return _factory.Label(target, defaultValue);
        }

        /// <summary>
        /// Visits a label target slim tree node, produces a label target.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>The label target represented by the label target slim node.</returns>
        protected override LabelTarget MakeLabelTarget(LabelTargetSlim node)
        {
            if (_labels.TryGetValue(node, out LabelTarget res))
            {
                return res;
            }

            var type = MakeType(node.Type);
            var l = _factory.Label(type, node.Name);
            _labels[node] = l;

            return l;
        }

        /// <summary>
        /// Visits a lambda expression slim tree node, produces a lambda expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="body">Lambda body expression.</param>
        /// <param name="parameters">Lambda parameter expressions.</param>
        /// <returns>The lambda expression represented by the expression slim node.</returns>
        protected override LambdaExpression MakeLambda(LambdaExpressionSlim node, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
        {
            if (node.DelegateType != null)
            {
                var delegateType = MakeType(node.DelegateType);
                return _factory.Lambda(delegateType, body, parameters);
            }
            else
            {
                return _factory.Lambda(body, parameters);
            }
        }

        /// <summary>
        /// Visits a list init expression slim tree node, produces a list init expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="newExpression">New expression.</param>
        /// <param name="initializers">Initializer expressions.</param>
        /// <returns>The list init expression represented by the expression slim node.</returns>
        protected override Expression MakeListInit(ListInitExpressionSlim node, NewExpression newExpression, ReadOnlyCollection<ElementInit> initializers)
        {
            return _factory.ListInit(newExpression, initializers);
        }

        /// <summary>
        /// Visits a loop expression slim tree node, produces a loop expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="breakLabel">Break label.</param>
        /// <param name="continueLabel">Continue label.</param>
        /// <returns>The loop expression represented by the expression slim node.</returns>
        protected override Expression MakeLoop(LoopExpressionSlim node, Expression body, LabelTarget breakLabel, LabelTarget continueLabel)
        {
            return _factory.Loop(body, breakLabel, continueLabel);
        }

        /// <summary>
        /// Visits a member assignment slim tree node, produces a member assignment.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="expression">Operand expression.</param>
        /// <returns>The member assignment represented by the member assignment slim node.</returns>
        protected override MemberAssignment MakeMemberAssignment(MemberAssignmentSlim node, Expression expression)
        {
            var member = MakeMember(node.Member);
            return _factory.Bind(member, expression);
        }

        /// <summary>
        /// Visits a member expression slim tree node, produces a member expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="expression">Operand expression.</param>
        /// <returns>The member expression represented by the expression slim node.</returns>
        protected override Expression MakeMember(MemberExpressionSlim node, Expression expression)
        {
            var member = MakeMember(node.Member);
            return _factory.MakeMemberAccess(expression, member);
        }

        /// <summary>
        /// Visits a member init expression slim tree node, produces a member init expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="newExpression">New expression.</param>
        /// <param name="bindings">Binding expressions.</param>
        /// <returns>The member init expression represented by the expression slim node.</returns>
        protected override Expression MakeMemberInit(MemberInitExpressionSlim node, NewExpression newExpression, ReadOnlyCollection<MemberBinding> bindings)
        {
            return _factory.MemberInit(newExpression, bindings);
        }

        /// <summary>
        /// Visits a member list binding slim tree node, produces a member list binding.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="initializers">Element initializers.</param>
        /// <returns>The member list binding represented by the member binding slim node.</returns>
        protected override MemberListBinding MakeMemberListBinding(MemberListBindingSlim node, ReadOnlyCollection<ElementInit> initializers)
        {
            var member = MakeMember(node.Member);
            return _factory.ListBind(member, initializers);
        }

        /// <summary>
        /// Visits a member member binding slim tree node, produces a member member binding.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="bindings">Member bindings.</param>
        /// <returns>The member member binding represented by the member binding slim node.</returns>
        protected override MemberMemberBinding MakeMemberMemberBinding(MemberMemberBindingSlim node, ReadOnlyCollection<MemberBinding> bindings)
        {
            var member = MakeMember(node.Member);
            return _factory.MemberBind(member, bindings);
        }

        /// <summary>
        /// Visits a method call expression slim tree node, produces a method call expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="object">Object operand.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>The method call expression represented by the expression slim node.</returns>
        protected override Expression MakeMethodCall(MethodCallExpressionSlim node, Expression @object, ReadOnlyCollection<Expression> arguments)
        {
            var method = MakeMethod(node.Method);
            return _factory.Call(@object, method, arguments);
        }

        /// <summary>
        /// Visits a new expression slim tree node, produces a new expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>The new expression represented by the expression slim node.</returns>
        protected override Expression MakeNew(NewExpressionSlim node, ReadOnlyCollection<Expression> arguments)
        {
            if (node.Constructor != null)
            {
                var constructor = MakeConstructor(node.Constructor);

                var slimMembers = node.Members;

                if (slimMembers != null)
                {
                    var count = slimMembers.Count;
                    var members = new MemberInfo[count];
                    for (var i = 0; i < count; i++)
                    {
                        members[i] = MakeMember(slimMembers[i]);
                    }

                    return _factory.New(constructor, arguments, members);
                }
                else
                {
                    return _factory.New(constructor, arguments);
                }
            }
            else
            {
                var type = MakeType(node.Type);
                return _factory.New(type);
            }
        }

        /// <summary>
        /// Visits a new array expression slim tree node, produces a new array expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="expressions">Operand expressions.</param>
        /// <returns>The new array expression represented by the expression slim node.</returns>
        protected override Expression MakeNewArray(NewArrayExpressionSlim node, ReadOnlyCollection<Expression> expressions)
        {
            var elementType = MakeType(node.ElementType);

            if (node.NodeType == ExpressionType.NewArrayBounds)
            {
                return _factory.NewArrayBounds(elementType, expressions);
            }
            else
            {
                Debug.Assert(node.NodeType == ExpressionType.NewArrayInit);
                return _factory.NewArrayInit(elementType, expressions);
            }
        }

        /// <summary>
        /// Visits a parameter expression slim tree node, produces a parameter expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>The parameter expression represented by the expression slim node.</returns>
        protected override ParameterExpression MakeParameter(ParameterExpressionSlim node)
        {
            if (!_variables.TryGetValue(node, out ParameterExpression res))
            {
                var type = MakeType(node.Type);
                res = _factory.Parameter(type, node.Name);
                _variables[node] = res;
            }

            return res;
        }

        /// <summary>
        /// Visits a switch expression slim tree node, produces a switch expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="switchValue">Switch value expression.</param>
        /// <param name="defaultBody">Default body expression.</param>
        /// <param name="cases">Switch cases.</param>
        /// <returns>The switch expression represented by the expression slim node.</returns>
        protected override Expression MakeSwitch(SwitchExpressionSlim node, Expression switchValue, Expression defaultBody, ReadOnlyCollection<SwitchCase> cases)
        {
            return _factory.Switch(
                VisitIfNotNull(node.Type, MakeTypeCachedDelegate),
                switchValue,
                defaultBody,
                VisitIfNotNull(node.Comparison, MakeMethodCachedDelegate),
                cases
            );
        }

        /// <summary>
        /// Visits a switch case slim tree node, produces a switch case.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="body">Body expressions.</param>
        /// <param name="testValues">Test value expressions.</param>
        /// <returns>The switch case represented by the switch case slim node.</returns>
        protected override SwitchCase MakeSwitchCase(SwitchCaseSlim node, Expression body, ReadOnlyCollection<Expression> testValues)
        {
            return _factory.SwitchCase(body, testValues);
        }

        /// <summary>
        /// Visits a try expression slim tree node, produces a try expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="finally">Finally expression.</param>
        /// <param name="fault">Fault expression.</param>
        /// <param name="handlers">Handler expressions.</param>
        /// <returns>The try expression represented by the expression slim node.</returns>
        protected override Expression MakeTry(TryExpressionSlim node, Expression body, Expression @finally, Expression fault, ReadOnlyCollection<CatchBlock> handlers)
        {
            return _factory.MakeTry(VisitIfNotNull(node.Type, MakeTypeCachedDelegate), body, @finally, fault, handlers);
        }

        /// <summary>
        /// Visits a type binary expression slim tree node, produces a type binary expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="expression">Expression operand.</param>
        /// <returns>The type binary expression represented by the expression slim node.</returns>
        protected override Expression MakeTypeBinary(TypeBinaryExpressionSlim node, Expression expression)
        {
            var typeOperand = MakeType(node.TypeOperand);

            return node.NodeType switch
            {
                ExpressionType.TypeIs => _factory.TypeIs(expression, typeOperand),
                ExpressionType.TypeEqual => _factory.TypeEqual(expression, typeOperand),
                _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Type expression type '{0}' not supported.", node.NodeType)),
            };
        }

        /// <summary>
        /// Visits a unary expression slim tree node, produces a unary expression.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <param name="operand">Unary operand.</param>
        /// <returns>The unary expression represented by the expression slim node.</returns>
        protected override Expression MakeUnary(UnaryExpressionSlim node, Expression operand)
        {
            var type = VisitIfNotNull(node.Type, MakeTypeCachedDelegate);
            var method = VisitIfNotNull(node.Method, MakeMethodCachedDelegate);

            if (node.NodeType == ExpressionType.Throw)
            {
                return _factory.Throw(operand, type ?? typeof(void));
            }

            return _factory.MakeUnary(node.NodeType, operand, type, method);
        }

        private ConstructorInfo MakeConstructor(ConstructorInfoSlim constructor)
        {
            return TypeSpace.GetConstructor(constructor);
        }

        private MemberInfo MakeMember(MemberInfoSlim member)
        {
            return TypeSpace.GetMember(member);
        }

        private MethodInfo MakeMethod(MethodInfoSlim method)
        {
            return TypeSpace.GetMethod(method);
        }

        private PropertyInfo MakeProperty(PropertyInfoSlim property)
        {
            return TypeSpace.GetProperty(property);
        }

        private Type MakeType(TypeSlim type)
        {
            return TypeSpace.ConvertType(type);
        }

        #endregion
    }
}
