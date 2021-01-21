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
using System.Linq.CompilerServices;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Expression visitor that converts expressions to expression slims.
    /// </summary>
    public class ExpressionToExpressionSlimConverter : ExpressionVisitorNarrow<ExpressionSlim, LambdaExpressionSlim, ParameterExpressionSlim, NewExpressionSlim, ElementInitSlim, MemberBindingSlim, MemberAssignmentSlim, MemberListBindingSlim, MemberMemberBindingSlim, CatchBlockSlim, SwitchCaseSlim, LabelTargetSlim>
    {
        #region Fields

        private readonly IExpressionSlimFactory _factory;

        // TODO Use memory pool for these dictionaries
        private readonly Dictionary<ParameterExpression, ParameterExpressionSlim> _parameters;
        private readonly Dictionary<LabelTarget, LabelTargetSlim> _labels;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an expression to expression slim converter with a fresh typespace.
        /// </summary>
        public ExpressionToExpressionSlimConverter()
            : this(new TypeSpace())
        {
        }

        /// <summary>
        /// Instantiates an expression to expression slim converter with a given typespace.
        /// </summary>
        /// <param name="typeSpace">The type space.</param>
        public ExpressionToExpressionSlimConverter(TypeSpace typeSpace)
            : this(typeSpace, ExpressionSlimFactory.Instance)
        {
        }

        /// <summary>
        /// Instantiates an expression to expression slim converter with a given typespace and slim expression factory.
        /// </summary>
        /// <param name="typeSpace">The type space.</param>
        /// <param name="factory">The slim expression factory.</param>
        public ExpressionToExpressionSlimConverter(TypeSpace typeSpace, IExpressionSlimFactory factory)
        {
            TypeSpace = typeSpace ?? throw new ArgumentNullException(nameof(typeSpace));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _parameters = new Dictionary<ParameterExpression, ParameterExpressionSlim>();
            _labels = new Dictionary<LabelTarget, LabelTargetSlim>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type space containing mapped types.
        /// </summary>
        protected TypeSpace TypeSpace { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Makes an expression slim representing a BinaryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="left">Left expression slim.</param>
        /// <param name="conversion">Conversion expression slim.</param>
        /// <param name="right">Right expression slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeBinary(BinaryExpression node, ExpressionSlim left, LambdaExpressionSlim conversion, ExpressionSlim right)
        {
            var method = default(MethodInfoSlim);

            if (node.Method != null)
            {
                method = MakeMethod(node.Method);
            }

            return _factory.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, method, conversion);
        }

        /// <summary>
        /// Makes an expression slim representing a BlockExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="variables">Variables in the block.</param>
        /// <param name="expressions">Expression slims in the block.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeBlock(BlockExpression node, ReadOnlyCollection<ParameterExpressionSlim> variables, ReadOnlyCollection<ExpressionSlim> expressions)
        {
            var type = MakeType(node.Type);
            return _factory.Block(type, variables, expressions);
        }

        /// <summary>
        /// Makes a catch block slim object representing a CatchBlock with the given children.
        /// </summary>
        /// <param name="node">Original catch block.</param>
        /// <param name="variable">Variable expression slim.</param>
        /// <param name="body">Body expression slim.</param>
        /// <param name="filter">Filter expression slim.</param>
        /// <returns>Slim representation of the original catch block.</returns>
        protected override CatchBlockSlim MakeCatchBlock(CatchBlock node, ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter)
        {
            var test = MakeType(node.Test);
            return _factory.MakeCatchBlock(test, variable, body, filter);
        }

        /// <summary>
        /// Makes an expression slim representing a ConditionalExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="test">Test expression slim.</param>
        /// <param name="ifTrue">True branch expression slim.</param>
        /// <param name="ifFalse">False branch expression slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeConditional(ConditionalExpression node, ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse)
        {
            if (node.IfTrue.Type != node.IfFalse.Type || node.IfTrue.Type != node.Type)
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
        /// Makes an expression slim representing a ConstantExpression.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeConstant(ConstantExpression node)
        {
            var type = MakeType(node.Type);
            var obj = ObjectSlim.Create(node.Value, type, node.Type);
            return _factory.Constant(obj, type);
        }

        /// <summary>
        /// Makes an expression slim representing a DefaultExpression.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeDefault(DefaultExpression node)
        {
            var type = MakeType(node.Type);
            return _factory.Default(type);
        }

        /// <summary>
        /// Makes an element initializer slim object representing a ElementInit object with the given children.
        /// </summary>
        /// <param name="node">Original element initializer.</param>
        /// <param name="arguments">Argument expression slims.</param>
        /// <returns>Slim representation of the original element initializer.</returns>
        protected override ElementInitSlim MakeElementInit(ElementInit node, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            var addMethod = MakeMethod(node.AddMethod);
            return _factory.ElementInit(addMethod, arguments);
        }

        /// <summary>
        /// Makes an expression slim representing a GotoExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="target">Target label slim.</param>
        /// <param name="value">Value expression slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeGoto(GotoExpression node, LabelTargetSlim target, ExpressionSlim value)
        {
            var type = MakeType(node.Type);
            return _factory.MakeGoto(node.Kind, target, value, type);
        }

        /// <summary>
        /// Makes an expression slim representing an IndexExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="object">The object to access indexed property.</param>
        /// <param name="arguments">The index arguments.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeIndex(IndexExpression node, ExpressionSlim @object, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            var indexer = MakeProperty(node.Indexer);
            return _factory.MakeIndex(@object, indexer, arguments);
        }

        /// <summary>
        /// Makes an expression slim representing a InvocationExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expression">Function expression slim.</param>
        /// <param name="arguments">Argument expression slims.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeInvocation(InvocationExpression node, ExpressionSlim expression, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            return _factory.Invoke(expression, arguments);
        }

        /// <summary>
        /// Makes an expression slim representing a LabelExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="target">Target label slim.</param>
        /// <param name="defaultValue">Default value expression slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeLabel(LabelExpression node, LabelTargetSlim target, ExpressionSlim defaultValue)
        {
            return _factory.Label(target, defaultValue);
        }

        /// <summary>
        /// Makes a label target slim object representing a LabelTarget.
        /// </summary>
        /// <param name="node">Original label target.</param>
        /// <returns>Slim representation of the label target.</returns>
        protected override LabelTargetSlim MakeLabelTarget(LabelTarget node)
        {
            if (_labels.TryGetValue(node, out LabelTargetSlim res))
            {
                return res;
            }

            var type = MakeType(node.Type);
            var l = _factory.Label(type, node.Name);
            _labels[node] = l;

            return l;
        }

        /// <summary>
        /// Makes an expression slim representing a LambdaExpression with the given children.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression slim.</param>
        /// <param name="parameters">Parameter expression slims.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override LambdaExpressionSlim MakeLambda<T>(Expression<T> node, ExpressionSlim body, ReadOnlyCollection<ParameterExpressionSlim> parameters)
        {
            // NB: This supports dynamically generated delegate types used by lambda expression
            //     with a parameter count that exceeds the number of Func<> and Action<> delegate
            //     types available in the BCL. When building such lambda expressions using the
            //     Expression.Lambda factory methods (that omit an explicit specification of a
            //     custom delegate type), the .NET Framework dynamically generates a delegate type
            //     at runtime in a dynamic assembly named "Snippets" and with type names starting
            //     with "Delegate". See System/Linq/Expressions/Compiler/DelegateHelpers.cs in the
            //     .NET Framework code for more information.
            //
            //     Note that it's technically possible for a user to obtain such a dynamically
            //     generated type and use it elsewhere in an expression tree in order to get some
            //     type equivalence property to hold when building a complex expression. In such
            //     a case, deserialization may still break when two separate lambda expressions
            //     result in the re-generation of two separate dynamic delegate types which are
            //     no longer nominally equal. We'll deem this case very unlikely to occur. Any use
            //     of dynamically generated types will need special handling at the side of the
            //     deserializer in order to make assembly and type loading succeed. This will need
            //     more work, for example by intercepting the (inverted) type space conversion
            //     operations. This will work as expected for any other dynamically generated type
            //     but will no longer involve type space operations for Snippets.Delegate*.

            if (node.Type.Assembly.IsDynamic && node.Type.FullName.StartsWith("Delegate", StringComparison.Ordinal) && node.Type.Assembly.GetName().Name == "Snippets")
            {
                return _factory.Lambda(body, parameters);
            }
            else
            {
                var delegateType = MakeType(node.Type);
                return _factory.Lambda(delegateType, body, parameters);
            }
        }

        /// <summary>
        /// Makes an expression slim representing a ListInitExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="newExpression">New expression slim.</param>
        /// <param name="initializers">Slim element initializers.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeListInit(ListInitExpression node, NewExpressionSlim newExpression, ReadOnlyCollection<ElementInitSlim> initializers)
        {
            return _factory.ListInit(newExpression, initializers);
        }

        /// <summary>
        /// Makes an expression slim representing a LoopExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression slim.</param>
        /// <param name="breakLabel">Break label slim.</param>
        /// <param name="continueLabel">Continue label slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeLoop(LoopExpression node, ExpressionSlim body, LabelTargetSlim breakLabel, LabelTargetSlim continueLabel)
        {
            return _factory.Loop(body, breakLabel, continueLabel);
        }

        /// <summary>
        /// Makes an expression slim representing a MemberExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expression">Object expression slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeMember(MemberExpression node, ExpressionSlim expression)
        {
            var member = MakeMember(node.Member);
            return _factory.MakeMemberAccess(expression, member);
        }

        /// <summary>
        /// Makes a member binding slim object representing a MemberAssignment with the given children.
        /// </summary>
        /// <param name="node">Original member assignment.</param>
        /// <param name="expression">Assigned expression slim.</param>
        /// <returns>Slim representation of the original member assignment.</returns>
        protected override MemberAssignmentSlim MakeMemberAssignment(MemberAssignment node, ExpressionSlim expression)
        {
            var member = MakeMember(node.Member);
            return _factory.Bind(member, expression);
        }

        /// <summary>
        /// Makes an expression slim representing a MemberInitExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="newExpression">New expression slim.</param>
        /// <param name="bindings">Slim member bindings.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeMemberInit(MemberInitExpression node, NewExpressionSlim newExpression, ReadOnlyCollection<MemberBindingSlim> bindings)
        {
            return _factory.MemberInit(newExpression, bindings);
        }

        /// <summary>
        /// Makes a member binding slim object representing a MemberListBinding with the given children.
        /// </summary>
        /// <param name="node">Original member list binding.</param>
        /// <param name="initializers">Slim element initializers.</param>
        /// <returns>Slim representation of the original member list binding.</returns>
        protected override MemberListBindingSlim MakeMemberListBinding(MemberListBinding node, ReadOnlyCollection<ElementInitSlim> initializers)
        {
            var member = MakeMember(node.Member);
            return _factory.ListBind(member, initializers);
        }

        /// <summary>
        /// Makes a member binding slim object representing a MemberMemberBinding with the given children.
        /// </summary>
        /// <param name="node">Original member member binding.</param>
        /// <param name="bindings">Slim member bindings.</param>
        /// <returns>Slim representation of the original member member binding.</returns>
        protected override MemberMemberBindingSlim MakeMemberMemberBinding(MemberMemberBinding node, ReadOnlyCollection<MemberBindingSlim> bindings)
        {
            var member = MakeMember(node.Member);
            return _factory.MemberBind(member, bindings);
        }

        /// <summary>
        /// Makes an expression slim representing a MethodCallExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="object">Object expression slim.</param>
        /// <param name="arguments">Argument expression slims.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeMethodCall(MethodCallExpression node, ExpressionSlim @object, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            var method = MakeMethod(node.Method);
            return _factory.Call(@object, method, arguments);
        }

        /// <summary>
        /// Makes an expression slim representing a NewExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="arguments">Argument expression slims.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeNew(NewExpression node, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            if (node.Constructor != null)
            {
                var constructor = MakeConstructor(node.Constructor);

                var richMembers = node.Members;
                var slimMembers = default(IList<MemberInfoSlim>);
                if (richMembers != null)
                {
                    var n = richMembers.Count;

                    slimMembers = new MemberInfoSlim[n];

                    for (var i = 0; i < n; i++)
                    {
                        slimMembers[i] = MakeMember(richMembers[i]);
                    }
                }

                return _factory.New(constructor, arguments, slimMembers);
            }
            else
            {
                var type = MakeType(node.Type);
                return _factory.New(type);
            }
        }

        /// <summary>
        /// Makes an expression slim representing a NewArrayExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expressions">Child expression slims.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeNewArray(NewArrayExpression node, ReadOnlyCollection<ExpressionSlim> expressions)
        {
            var elementType = MakeType(node.Type.GetElementType());

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
        /// Makes an expression slim representing a ParameterExpression.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ParameterExpressionSlim MakeParameter(ParameterExpression node)
        {
            if (!_parameters.TryGetValue(node, out ParameterExpressionSlim res))
            {
                var type = MakeType(node.Type);
                res = _factory.Parameter(type, node.Name);
                _parameters[node] = res;
            }

            return res;
        }

        /// <summary>
        /// Makes an expression slim representing a SwitchExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="switchValue">Switch value expression slim.</param>
        /// <param name="defaultBody">Default body expression slim.</param>
        /// <param name="cases">Switch case slims.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeSwitch(SwitchExpression node, ExpressionSlim switchValue, ExpressionSlim defaultBody, ReadOnlyCollection<SwitchCaseSlim> cases)
        {
            var t = MakeType(node.Type);
            var c = node.Comparison == null ? null : MakeMethod(node.Comparison);
            return _factory.Switch(t, switchValue, defaultBody, c, cases);
        }

        /// <summary>
        /// Makes a switch case slim object representing a SwitchCase with the given children.
        /// </summary>
        /// <param name="node">Original switch case.</param>
        /// <param name="body">Body expression slim.</param>
        /// <param name="testValues">Test value expression slims.</param>
        /// <returns>Slim representation of the original switch case.</returns>
        protected override SwitchCaseSlim MakeSwitchCase(SwitchCase node, ExpressionSlim body, ReadOnlyCollection<ExpressionSlim> testValues)
        {
            return _factory.SwitchCase(body, testValues);
        }

        /// <summary>
        /// Makes an slim expression representing a TryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression slim.</param>
        /// <param name="finally">Finally expression slim.</param>
        /// <param name="fault">Fault expression slim.</param>
        /// <param name="handlers">Catch handler slims.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeTry(TryExpression node, ExpressionSlim body, ExpressionSlim @finally, ExpressionSlim fault, ReadOnlyCollection<CatchBlockSlim> handlers)
        {
            var type = MakeType(node.Type);
            return _factory.MakeTry(type, body, @finally, fault, handlers);
        }

        /// <summary>
        /// Makes an expression slim representing a TypeBinaryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expression">Child expression slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeTypeBinary(TypeBinaryExpression node, ExpressionSlim expression)
        {
            var typeOperand = MakeType(node.TypeOperand);

            if (node.NodeType == ExpressionType.TypeIs)
            {
                return _factory.TypeIs(expression, typeOperand);
            }
            else
            {
                Debug.Assert(node.NodeType == ExpressionType.TypeEqual);
                return _factory.TypeEqual(expression, typeOperand);
            }
        }

        /// <summary>
        /// Makes an expression slim representing a UnaryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="operand">Operand expression slim.</param>
        /// <returns>Slim representation of the original expression.</returns>
        protected override ExpressionSlim MakeUnary(UnaryExpression node, ExpressionSlim operand)
        {
            var method = default(MethodInfoSlim);
            var type = default(TypeSlim);

            if (node.Method != null)
            {
                method = MakeMethod(node.Method);
            }

            switch (node.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Throw:
                case ExpressionType.TypeAs:
                case ExpressionType.Unbox:
                    type = MakeType(node.Type);
                    break;
            }

            return _factory.MakeUnary(node.NodeType, operand, type, method);
        }

        private TypeSlim MakeType(Type type)
        {
            return TypeSpace.ConvertType(type);
        }

        private MemberInfoSlim MakeMember(MemberInfo member)
        {
            var memberType = member.GetMemberType();

            Debug.Assert(memberType is MemberTypes.Property or MemberTypes.Field);

            var res = default(MemberInfoSlim);

            switch (memberType)
            {
                case MemberTypes.Property:
                    res = MakeProperty((PropertyInfo)member);
                    break;
                case MemberTypes.Field:
                    res = MakeField((FieldInfo)member);
                    break;
            }

            return res;
        }

        private PropertyInfoSlim MakeProperty(PropertyInfo property)
        {
            return TypeSpace.GetProperty(property);
        }

        private FieldInfoSlim MakeField(FieldInfo field)
        {
            return TypeSpace.GetField(field);
        }

        private ConstructorInfoSlim MakeConstructor(ConstructorInfo constructor)
        {
            return TypeSpace.GetConstructor(constructor);
        }

        private MethodInfoSlim MakeMethod(MethodInfo method)
        {
            return TypeSpace.GetMethod(method);
        }

        #endregion
    }
}
