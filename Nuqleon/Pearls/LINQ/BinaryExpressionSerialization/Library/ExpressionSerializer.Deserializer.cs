// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Collections.Generic;
using System.IO;
using System.Memory;
using System.Reflection;

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
#if USE_SLIM
    using Assembly = AssemblySlim;
    using BinaryExpression = BinaryExpressionSlim;
    using BlockExpression = BlockExpressionSlim;
    using CatchBlock = CatchBlockSlim;
    using ConditionalExpression = ConditionalExpressionSlim;
    using ConstantExpression = ConstantExpressionSlim;
    using ConstructorInfo = ConstructorInfoSlim;
    using DefaultExpression = DefaultExpressionSlim;
    using ElementInit = ElementInitSlim;
    using Expression = ExpressionSlim;
    using GotoExpression = GotoExpressionSlim;
    using IndexExpression = IndexExpressionSlim;
    using InvocationExpression = InvocationExpressionSlim;
    using LabelExpression = LabelExpressionSlim;
    using LabelTarget = LabelTargetSlim;
    using LambdaExpression = LambdaExpressionSlim;
    using ListInitExpression = ListInitExpressionSlim;
    using LoopExpression = LoopExpressionSlim;
    using MemberBinding = MemberBindingSlim;
    using MemberExpression = MemberExpressionSlim;
    using MemberInfo = MemberInfoSlim;
    using MemberInitExpression = MemberInitExpressionSlim;
    using MethodCallExpression = MethodCallExpressionSlim;
    using MethodInfo = MethodInfoSlim;
    using NewArrayExpression = NewArrayExpressionSlim;
    using NewExpression = NewExpressionSlim;
    using Object = ObjectSlim;
    using ParameterExpression = ParameterExpressionSlim;
    using PropertyInfo = PropertyInfoSlim;
    using SwitchCase = SwitchCaseSlim;
    using SwitchExpression = SwitchExpressionSlim;
    using TryExpression = TryExpressionSlim;
    using Type = TypeSlim;
    using TypeBinaryExpression = TypeBinaryExpressionSlim;
    using UnaryExpression = UnaryExpressionSlim;
#endif

    public partial class ExpressionSerializer
    {
        private sealed class Deserializer
        {
            private readonly ExpressionSerializer _parent;
            private readonly Stream _stream;
            private readonly DeserializationContext _context;

            public Deserializer(ExpressionSerializer parent, Stream stream, DeserializationContext context)
            {
                _parent = parent;
                _stream = stream;
                _context = context;
            }

            public Expression Deserialize()
            {
                var b = ReadSingleByte();

                if (b == 0x00)
                {
                    return null;
                }

                var nodeType = (ExpressionType)(b - 1);

                switch (nodeType)
                {
                    case ExpressionType.Conditional:
                        return DeserializeConditional();

                    case ExpressionType.Constant:
                        return DeserializeConstant();

                    case ExpressionType.Default:
                        return DeserializeDefault();

                    case ExpressionType.TypeIs:
                    case ExpressionType.TypeEqual:
                        return DeserializeTypeBinary(nodeType);

                    case ExpressionType.ArrayLength:
                    case ExpressionType.Quote:
                        return DeserializeUnary1(nodeType);

                    case ExpressionType.Throw:
                    case ExpressionType.TypeAs:
                    case ExpressionType.Unbox:
                        return DeserializeUnary2(nodeType);

                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                    case ExpressionType.IsFalse:
                    case ExpressionType.IsTrue:
                    case ExpressionType.OnesComplement:
                    case ExpressionType.UnaryPlus:
                    case ExpressionType.Increment:
                    case ExpressionType.Decrement:
                    case ExpressionType.PreIncrementAssign:
                    case ExpressionType.PreDecrementAssign:
                    case ExpressionType.PostIncrementAssign:
                    case ExpressionType.PostDecrementAssign:
                        return DeserializeUnary3(nodeType);

                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        return DeserializeUnary4(nodeType);

                    case ExpressionType.ArrayIndex:
                    case ExpressionType.Assign:
                        return DeserializeBinary1(nodeType);

                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.Power:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                    case ExpressionType.ExclusiveOr:
                    case ExpressionType.RightShift:
                    case ExpressionType.LeftShift:
                        return DeserializeBinary2(nodeType);

                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                        return DeserializeBinary3(nodeType);

                    case ExpressionType.Coalesce:
                        return DeserializeBinary4(nodeType);

                    case ExpressionType.AddAssign:
                    case ExpressionType.AddAssignChecked:
                    case ExpressionType.SubtractAssign:
                    case ExpressionType.SubtractAssignChecked:
                    case ExpressionType.MultiplyAssign:
                    case ExpressionType.MultiplyAssignChecked:
                    case ExpressionType.DivideAssign:
                    case ExpressionType.ModuloAssign:
                    case ExpressionType.PowerAssign:
                    case ExpressionType.AndAssign:
                    case ExpressionType.OrAssign:
                    case ExpressionType.ExclusiveOrAssign:
                    case ExpressionType.RightShiftAssign:
                    case ExpressionType.LeftShiftAssign:
                        return DeserializeBinary5(nodeType);

                    case ExpressionType.Block:
                        return DeserializeBlock();

                    case ExpressionType.Goto:
                        return DeserializeGoto();

                    case ExpressionType.Index:
                        return DeserializeIndex();

                    case ExpressionType.Invoke:
                        return DeserializeInvocation();

                    case ExpressionType.Lambda:
                        return DeserializeLambda();

                    case ExpressionType.ListInit:
                        return DeserializeListInit();

                    case ExpressionType.Loop:
                        return DeserializeLoop();

                    case ExpressionType.MemberAccess:
                        return DeserializeMember();

                    case ExpressionType.MemberInit:
                        return DeserializeMemberInit();

                    case ExpressionType.Call:
                        return DeserializeMethodCall();

                    case ExpressionType.New:
                        return DeserializeNew();

                    case ExpressionType.NewArrayBounds:
                    case ExpressionType.NewArrayInit:
                        return DeserializeNewArray(nodeType);

                    case ExpressionType.Parameter:
                        return DeserializeParameter();

#if !USE_SLIM
                    case ExpressionType.RuntimeVariables:
                        return DeserializeRuntimeVariables();
#endif

                    case ExpressionType.Switch:
                        return DeserializeSwitch();

                    case ExpressionType.Try:
                        return DeserializeTry();

                    case ExpressionType.Label:
                        return DeserializeLabel();
                }

                throw new NotImplementedException(nodeType.ToString());
            }

            private BinaryExpression DeserializeBinary1(ExpressionType nodeType)
            {
                var left = Deserialize();
                var right = Deserialize();

                return _parent._factory.MakeBinary(nodeType, left, right);
            }

            private BinaryExpression DeserializeBinary2(ExpressionType nodeType)
            {
                var flag = ReadSingleByte();

                var method = default(MethodInfo);

                if (flag == Protocol.BINARY_FLAG_HASMETHOD)
                {
                    method = DeserializeMethodInfo();
                }

                var left = Deserialize();
                var right = Deserialize();

                return _parent._factory.MakeBinary(nodeType, left, right, liftToNull: false, method);
            }

            private BinaryExpression DeserializeBinary3(ExpressionType nodeType)
            {
                var flag = ReadSingleByte();

                var method = default(MethodInfo);

                if ((flag & Protocol.BINARY_FLAG_HASMETHOD) == Protocol.BINARY_FLAG_HASMETHOD)
                {
                    method = DeserializeMethodInfo();
                }

                var liftToNull = (flag & Protocol.BINARY_FLAG_ISLIFTED) == Protocol.BINARY_FLAG_ISLIFTED;

                var left = Deserialize();
                var right = Deserialize();

                return _parent._factory.MakeBinary(nodeType, left, right, liftToNull, method);
            }

            private BinaryExpression DeserializeBinary4(ExpressionType nodeType)
            {
                var flag = ReadSingleByte();

                var conversion = default(LambdaExpression);

                if ((flag & Protocol.BINARY_FLAG_HASCONVERSION) == Protocol.BINARY_FLAG_HASCONVERSION)
                {
                    conversion = (LambdaExpression)Deserialize();
                }

                var left = Deserialize();
                var right = Deserialize();

                return _parent._factory.MakeBinary(nodeType, left, right, liftToNull: false, method: null, conversion);
            }

            private BinaryExpression DeserializeBinary5(ExpressionType nodeType)
            {
                var flag = ReadSingleByte();

                var method = default(MethodInfo);

                if ((flag & Protocol.BINARY_FLAG_HASMETHOD) == Protocol.BINARY_FLAG_HASMETHOD)
                {
                    method = DeserializeMethodInfo();
                }

                var conversion = default(LambdaExpression);

                if ((flag & Protocol.BINARY_FLAG_HASCONVERSION) == Protocol.BINARY_FLAG_HASCONVERSION)
                {
                    conversion = (LambdaExpression)Deserialize();
                }

                var left = Deserialize();
                var right = Deserialize();

                return _parent._factory.MakeBinary(nodeType, left, right, liftToNull: false, method, conversion);
            }

            private BlockExpression DeserializeBlock()
            {
                var flag = ReadSingleByte();

                var type = default(Type);

                if (flag == Protocol.BLOCK_HASTYPE)
                {
                    type = DeserializeType();
                }

                var variableCount = _stream.ReadUInt32Compact();

                var variables = new ParameterExpression[(int)variableCount];

                for (var i = 0; i < variableCount; i++)
                {
                    variables[i] = DeserializeParameter();
                }

                var expressionCount = _stream.ReadUInt32Compact();

                var expressions = new Expression[(int)expressionCount];

                for (var i = 0; i < expressionCount; i++)
                {
                    expressions[i] = Deserialize();
                }

                if (type != null)
                {
                    return _parent._factory.Block(type, variables, expressions);
                }
                else
                {
                    return _parent._factory.Block(variables, expressions);
                }
            }

            private ParameterExpression DeserializeParameter()
            {
                var parameterIndex = _stream.ReadUInt32Compact();
                return _context.LookupParameter(parameterIndex);
            }

            private CatchBlock DeserializeCatchBlock()
            {
                var flag = ReadSingleByte();

                var variable = default(ParameterExpression);
                var type = default(Type);

                if ((flag & Protocol.CATCHBLOCK_FLAG_HASVARIABLE) == Protocol.CATCHBLOCK_FLAG_HASVARIABLE)
                {
                    variable = DeserializeParameter();
                }
                else
                {
                    type = DeserializeType();
                }

                var filter = default(Expression);

                if ((flag & Protocol.CATCHBLOCK_FLAG_HASFILTER) == Protocol.CATCHBLOCK_FLAG_HASFILTER)
                {
                    filter = Deserialize();
                }

                var body = Deserialize();

                if (type == null)
                {
                    return _parent._factory.MakeCatchBlock(variable.Type, variable, body, filter);
                }
                else
                {
                    return _parent._factory.MakeCatchBlock(type, variable, body, filter);
                }
            }

            private ConditionalExpression DeserializeConditional()
            {
#if USE_SLIM
                var type = default(Type);

                var flag = ReadSingleByte();

                if ((flag & Protocol.CONDITIONAL_HASTYPE) == Protocol.CONDITIONAL_HASTYPE)
                {
                    type = DeserializeType();
                }
#else
                var type = DeserializeType();
#endif

                var test = Deserialize();
                var ifTrue = Deserialize();
                var ifFalse = Deserialize();

                return _parent._factory.Condition(test, ifTrue, ifFalse, type);
            }

            private ConstantExpression DeserializeConstant()
            {
                var type = DeserializeType();

                var valueIndex = _stream.ReadUInt32Compact();
                var value = _context.GetConstant(valueIndex, type);

                return _parent._factory.Constant(value, type);
            }

            private DefaultExpression DeserializeDefault()
            {
                var type = DeserializeType();

                return _parent._factory.Default(type);
            }

            private ElementInit DeserializeElementInit()
            {
                var addMethod = DeserializeMethodInfo();

#if USE_SLIM
                var n = addMethod.ParameterTypes.Count;
#else
                var n = addMethod.GetParameters().Length;
#endif

                var arguments = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    arguments[i] = Deserialize();
                }

                return _parent._factory.ElementInit(addMethod, arguments);
            }

            private GotoExpression DeserializeGoto()
            {
                var flags = _stream.ReadByte();

                var kind = (GotoExpressionKind)_stream.ReadByte();

                var type = DeserializeType();

                var target = DeserializeLabelTarget();

                var value = default(Expression);

                if ((flags & Protocol.GOTO_HASVALUE) == Protocol.GOTO_HASVALUE)
                {
                    value = Deserialize();
                }

                return _parent._factory.MakeGoto(kind, target, value, type);
            }

            private IndexExpression DeserializeIndex()
            {
                var indexer = DeserializePropertyInfo();

                var obj = Deserialize();

#if USE_SLIM
                var n = indexer.IndexParameterTypes.Count;
#else
                var n = indexer.GetGetMethod().GetParameters().Length;
#endif

                var arguments = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    arguments[i] = Deserialize();
                }

                return _parent._factory.MakeIndex(obj, indexer, arguments);
            }

            private InvocationExpression DeserializeInvocation()
            {
                var expression = Deserialize();

#if USE_SLIM
                var n = (int)_stream.ReadUInt32Compact();
#else
                var n = expression.Type.GetMethod("Invoke").GetParameters().Length;
#endif

                var arguments = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    arguments[i] = Deserialize();
                }

                return _parent._factory.Invoke(expression, arguments);
            }

            private LambdaExpression DeserializeLambda()
            {
                var flags = _stream.ReadByte();

#if !USE_SLIM
                var tailCall = (flags & Protocol.LAMBDA_ISTAILCALL) == Protocol.LAMBDA_ISTAILCALL;

                var name = default(string);

                if ((flags & Protocol.LAMBDA_HASNAME) == Protocol.LAMBDA_HASNAME)
                {
                    name = _stream.ReadString();
                }
#endif

                var delegateType = default(Type);
                var n = 0;

                if ((flags & Protocol.LAMBDA_HASTYPE) == Protocol.LAMBDA_HASTYPE)
                {
                    delegateType = DeserializeType();
#if !USE_SLIM
                    n = delegateType.GetMethod("Invoke").GetParameters().Length;
#endif
                }
#if !USE_SLIM
                else
#endif
                {
                    n = (int)_stream.ReadUInt32Compact();
                }

                var parameters = new ParameterExpression[n];

                for (var i = 0; i < n; i++)
                {
                    parameters[i] = DeserializeParameter();
                }

                var body = Deserialize();

                if (delegateType != null)
                {
                    return _parent._factory.Lambda(
                        delegateType,
                        body,
#if !USE_SLIM
                        name,
                        tailCall,
#endif
                        parameters
                    );
                }
                else
                {
                    return _parent._factory.Lambda(
                        body,
#if !USE_SLIM
                        name,
                        tailCall,
#endif
                        parameters
                    );
                }
            }

            private ListInitExpression DeserializeListInit()
            {
                var newExpr = (NewExpression)Deserialize();

                var n = _stream.ReadUInt32Compact();

                var initializers = new ElementInit[n];

                for (var i = 0; i < n; i++)
                {
                    initializers[i] = DeserializeElementInit();
                }

                return _parent._factory.ListInit(newExpr, initializers);
            }

            private LoopExpression DeserializeLoop()
            {
                var flags = _stream.ReadByte();

                var @break = default(LabelTarget);
                var @continue = default(LabelTarget);

                if ((flags & Protocol.LOOP_HASBREAK) == Protocol.LOOP_HASBREAK)
                {
                    @break = DeserializeLabelTarget();
                }

                if ((flags & Protocol.LOOP_HASCONTINUE) == Protocol.LOOP_HASCONTINUE)
                {
                    @continue = DeserializeLabelTarget();
                }

                var body = Deserialize();

                return _parent._factory.Loop(body, @break, @continue);
            }

            private MemberExpression DeserializeMember()
            {
#if USE_SLIM
                var flags = _stream.ReadByte();
#endif

                var member = DeserializeMemberInfo();

                var expression = default(Expression);

#if USE_SLIM
                if ((flags & Protocol.MEMBER_HASOBJ) == Protocol.MEMBER_HASOBJ)
#else
                if (!IsStatic(member))
#endif
                {
                    expression = Deserialize();
                }

                return _parent._factory.MakeMemberAccess(expression, member);
            }

            private MemberInitExpression DeserializeMemberInit()
            {
                var newExpr = (NewExpression)Deserialize();

                var n = (int)_stream.ReadUInt32Compact();

                var bindings = new MemberBinding[n];

                for (var i = 0; i < n; i++)
                {
                    bindings[i] = DeserializeMemberBinding();
                }

                return _parent._factory.MemberInit(newExpr, bindings);
            }

            private MemberBinding DeserializeMemberBinding()
            {
                var bindingType = (MemberBindingType)_stream.ReadByte();

                var member = DeserializeMemberInfo();

                var res = default(MemberBinding);

                switch (bindingType)
                {
                    case MemberBindingType.Assignment:
                        res = DeserializeMemberAssignment(member);
                        break;
                    case MemberBindingType.ListBinding:
                        res = DeserializeMemberListBinding(member);
                        break;
                    case MemberBindingType.MemberBinding:
                        res = DeserializeMemberMemberBinding(member);
                        break;
                }

                return res;
            }

            private MemberBinding DeserializeMemberAssignment(MemberInfo member)
            {
                var expression = Deserialize();

                return _parent._factory.Bind(member, expression);
            }

            private MemberBinding DeserializeMemberListBinding(MemberInfo member)
            {
                var n = (int)_stream.ReadUInt32Compact();

                var initializers = new ElementInit[n];

                for (var i = 0; i < n; i++)
                {
                    initializers[i] = DeserializeElementInit();
                }

                return _parent._factory.ListBind(member, initializers);
            }

            private MemberBinding DeserializeMemberMemberBinding(MemberInfo member)
            {
                var n = (int)_stream.ReadUInt32Compact();

                var bindings = new MemberBinding[n];

                for (var i = 0; i < n; i++)
                {
                    bindings[i] = DeserializeMemberBinding();
                }

                return _parent._factory.MemberBind(member, bindings);
            }

            private MethodCallExpression DeserializeMethodCall()
            {
#if USE_SLIM
                var flags = _stream.ReadByte();
#endif

                var method = DeserializeMethodInfo();

                var obj = default(Expression);

#if USE_SLIM
                if ((flags & Protocol.CALL_HASOBJ) == Protocol.CALL_HASOBJ)
#else
                if (!method.IsStatic)
#endif
                {
                    obj = Deserialize();
                }

#if USE_SLIM
                var n = method.ParameterTypes.Count;
#else
                var n = method.GetParameters().Length;
#endif

                var arguments = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    arguments[i] = Deserialize();
                }

                return _parent._factory.Call(obj, method, arguments);
            }

            private NewExpression DeserializeNew()
            {
                var flags = _stream.ReadByte();

                if ((flags & Protocol.NEW_NOCTOR) == Protocol.NEW_NOCTOR)
                {
                    var type = DeserializeType();

                    return _parent._factory.New(type);
                }

                var ctor = DeserializeConstructorInfo();

#if USE_SLIM
                var n = ctor.ParameterTypes.Count;
#else
                var n = ctor.GetParameters().Length;
#endif

                var arguments = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    arguments[i] = Deserialize();
                }

                if ((flags & Protocol.NEW_HASMEMBERS) == Protocol.NEW_HASMEMBERS)
                {
                    var members = new MemberInfo[n];

                    for (var i = 0; i < n; i++)
                    {
                        members[i] = DeserializeMemberInfo();
                    }

                    return _parent._factory.New(ctor, arguments, members);
                }
                else
                {
                    return _parent._factory.New(ctor, arguments);
                }
            }

            private NewArrayExpression DeserializeNewArray(ExpressionType nodeType)
            {
                var res = default(NewArrayExpression);

                var type = DeserializeType();

                var n = (int)_stream.ReadUInt32Compact();

                var expressions = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    expressions[i] = Deserialize();
                }

                switch (nodeType)
                {
                    case ExpressionType.NewArrayBounds:
                        res = _parent._factory.NewArrayBounds(type, expressions);
                        break;
                    case ExpressionType.NewArrayInit:
                        res = _parent._factory.NewArrayInit(type, expressions);
                        break;
                }

                return res;
            }

#if !USE_SLIM
            private RuntimeVariablesExpression DeserializeRuntimeVariables()
            {
                var n = (int)_stream.ReadUInt32Compact();

                var variables = new ParameterExpression[n];

                for (var i = 0; i < n; i++)
                {
                    variables[i] = (ParameterExpression)Deserialize();
                }

                return _parent._factory.RuntimeVariables(variables);
            }
#endif

            private SwitchExpression DeserializeSwitch()
            {
                var flags = _stream.ReadByte();

                var type = default(Type);

                if ((flags & Protocol.SWITCH_HASTYPE) == Protocol.SWITCH_HASTYPE)
                {
                    type = DeserializeType();
                }

                var switchValue = Deserialize();

                var comparison = default(MethodInfo);

                if ((flags & Protocol.SWITCH_HASCOMPARISON) == Protocol.SWITCH_HASCOMPARISON)
                {
                    comparison = DeserializeMethodInfo();
                }

                var n = (int)_stream.ReadUInt32Compact();

                var cases = new SwitchCase[n];

                for (var i = 0; i < n; i++)
                {
                    cases[i] = DeserializeSwitchCase();
                }

                var defaultBody = default(Expression);

                if ((flags & Protocol.SWITCH_HASDEFAULT) == Protocol.SWITCH_HASDEFAULT)
                {
                    defaultBody = Deserialize();
                }

                return _parent._factory.Switch(type, switchValue, defaultBody, comparison, cases);
            }

            private SwitchCase DeserializeSwitchCase()
            {
                var n = (int)_stream.ReadUInt32Compact();

                var testValues = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    testValues[i] = Deserialize();
                }

                var body = Deserialize();

                return _parent._factory.SwitchCase(body, testValues);
            }

            private TryExpression DeserializeTry()
            {
                var flags = _stream.ReadByte();

                var type = default(Type);

                if ((flags & Protocol.TRY_HASTYPE) == Protocol.TRY_HASTYPE)
                {
                    type = DeserializeType();
                }

                var body = Deserialize();

                var handlers = default(CatchBlock[]);

                if ((flags & Protocol.TRY_HASCATCH) == Protocol.TRY_HASCATCH)
                {
                    var n = (int)_stream.ReadUInt32Compact();

                    handlers = new CatchBlock[n];

                    for (var i = 0; i < n; i++)
                    {
                        handlers[i] = DeserializeCatchBlock();
                    }
                }

                var fault = default(Expression);

                if ((flags & Protocol.TRY_HASFAULT) == Protocol.TRY_HASFAULT)
                {
                    fault = Deserialize();
                }

                var @finally = default(Expression);

                if ((flags & Protocol.TRY_HASFINALLY) == Protocol.TRY_HASFINALLY)
                {
                    @finally = Deserialize();
                }

                return _parent._factory.MakeTry(type, body, @finally, fault, handlers);
            }

            private TypeBinaryExpression DeserializeTypeBinary(ExpressionType nodeType)
            {
                var type = DeserializeType();
                var expression = Deserialize();

                var res = default(TypeBinaryExpression);

                switch (nodeType)
                {
                    case ExpressionType.TypeEqual:
                        res = _parent._factory.TypeEqual(expression, type);
                        break;
                    case ExpressionType.TypeIs:
                        res = _parent._factory.TypeIs(expression, type);
                        break;
                }

                return res;
            }

            private UnaryExpression DeserializeUnary1(ExpressionType nodeType)
            {
                var operand = Deserialize();

                return _parent._factory.MakeUnary(nodeType, operand, type: null);
            }

            private UnaryExpression DeserializeUnary2(ExpressionType nodeType)
            {
                var type = DeserializeType();

                var operand = Deserialize();

                return _parent._factory.MakeUnary(nodeType, operand, type);
            }

            private UnaryExpression DeserializeUnary3(ExpressionType nodeType)
            {
                var flag = ReadSingleByte();

                var method = default(MethodInfo);

                if (flag == Protocol.UNARY_FLAG_HASMETHOD)
                {
                    method = DeserializeMethodInfo();
                }

                var operand = Deserialize();

                return _parent._factory.MakeUnary(nodeType, operand, type: null, method);
            }

            private UnaryExpression DeserializeUnary4(ExpressionType nodeType)
            {
                var flag = ReadSingleByte();

                var type = DeserializeType();

                var method = default(MethodInfo);

                if (flag == Protocol.UNARY_FLAG_HASMETHOD)
                {
                    method = DeserializeMethodInfo();
                }

                var operand = Deserialize();

                return _parent._factory.MakeUnary(nodeType, operand, type, method);
            }

            private Type DeserializeType()
            {
                var typeIndex = _stream.ReadUInt32Compact();

                var type = _context.LookupType(typeIndex);

                return type.ToType();
            }

            private LabelExpression DeserializeLabel()
            {
                var flag = ReadSingleByte();

                var target = DeserializeLabelTarget();

                var defaultValue = default(Expression);

                if ((flag & Protocol.LABEL_HASDEFAULTVALUE) == Protocol.LABEL_HASDEFAULTVALUE)
                {
                    defaultValue = Deserialize();
                }

                return _parent._factory.Label(target, defaultValue);
            }

            private LabelTarget DeserializeLabelTarget()
            {
                var labelIndex = _stream.ReadUInt32Compact();
                return _context.LookupLabelTarget(labelIndex);
            }

            private ConstructorInfo DeserializeConstructorInfo()
            {
                return (ConstructorInfo)DeserializeMemberInfo();
            }

            private MethodInfo DeserializeMethodInfo()
            {
                return (MethodInfo)DeserializeMemberInfo();
            }

            private PropertyInfo DeserializePropertyInfo()
            {
                return (PropertyInfo)DeserializeMemberInfo();
            }

            private MemberInfo DeserializeMemberInfo()
            {
                var memberIndex = _stream.ReadUInt32Compact();

                return _context.LookupMember(memberIndex).ToMember(EmptyArray<Epyt>.Instance);
            }

#if !USE_SLIM
            private static bool IsStatic(MemberInfo member)
            {
                if (member is PropertyInfo p)
                {
                    return p.GetGetMethod(true).IsStatic;
                }
                else
                {
                    return ((FieldInfo)member).IsStatic;
                }
            }
#endif

            private byte ReadSingleByte()
            {
                var i = _stream.ReadByte();

                if (i < 0)
                {
                    throw new InvalidOperationException(); // TODO
                }

                var flag = (byte)i;

                return flag;
            }
        }

        private sealed class DeserializationContext : IClearable, IFreeable
        {
            private readonly ExpressionSerializer _parent;

            // TODO: pool
            private readonly List<Object> _constants = new();
            private readonly List<TypedParameter> _parameters = new();
            private readonly List<TypedLabel> _labels = new();
            private readonly List<Assembly> _assemblies = new();
            private readonly List<Epyt> _types = new();
            private readonly List<MemberOfni> _members = new();

            public DeserializationContext(ExpressionSerializer parent)
            {
                _parent = parent;
            }

            public Assembly LookupAssembly(uint assemblyIndex)
            {
                return _assemblies[(int)assemblyIndex - 1];
            }

            public Epyt LookupType(uint typeIndex)
            {
                if (typeIndex <= 0x80 /* CHECK */)
                {
                    var nullable = (typeIndex & Protocol.TYPE_FLAG_NULLABLE) != 0;
                    var array = (typeIndex & Protocol.TYPE_FLAG_ARRAY) != 0;

                    var typeCode = (byte)typeIndex & Protocol.TYPE_KNOWN_MASK;

                    var type = typeCode switch
                    {
                        Protocol.TYPE_BOOLEAN => Types.Boolean,
                        Protocol.TYPE_UINT8 => Types.Byte,
                        Protocol.TYPE_INT8 => Types.SByte,
                        Protocol.TYPE_UINT16 => Types.UInt16,
                        Protocol.TYPE_INT16 => Types.Int16,
                        Protocol.TYPE_UINT32 => Types.UInt32,
                        Protocol.TYPE_INT32 => Types.Int32,
                        Protocol.TYPE_UINT64 => Types.UInt64,
                        Protocol.TYPE_INT64 => Types.Int64,
                        Protocol.TYPE_FLOAT4 => Types.Single,
                        Protocol.TYPE_FLOAT8 => Types.Double,
                        Protocol.TYPE_CHAR => Types.Char,
                        Protocol.TYPE_STRING => Types.String,
                        Protocol.TYPE_DATETIME => Types.DateTime,
                        Protocol.TYPE_DECIMAL => Types.Decimal,
                        Protocol.TYPE_OBJECT => Types.Object,
                        Protocol.TYPE_VOID => Types.Void,
                        Protocol.TYPE_GUID => Types.Guid,
                        Protocol.TYPE_DATETIMEOFFSET => Types.DateTimeOffset,
                        Protocol.TYPE_TIMESPAN => Types.TimeSpan,
                        Protocol.TYPE_URI => Types.Uri,
                        _ => throw new NotSupportedException(),
                    };
                    if (nullable)
                    {
                        type = Types.NullableOfT.MakeGenericType(type);
                    }

                    if (array)
                    {
                        type = type.MakeArrayType();
                    }

                    return new SimpleEpyt(type);
                }
                else
                {
                    if (typeIndex <= byte.MaxValue)
                    {
                        var ti = (byte)typeIndex;

                        if ((ti & Protocol.TYPE_FLAG_INDEXED) == Protocol.TYPE_FLAG_INDEXED)
                        {
                            var index = ti & Protocol.TYPE_SHORT_INDEX_MASK;
                            return _types[index - 1];
                        }
                        else
                        {
                            throw new NotImplementedException(); // TODO: assert invalid data
                        }
                    }
                    else
                    {
                        var index = typeIndex & Protocol.TYPE_LONG_INDEX_MASK;

                        if (index - 1 >= _types.Count)
                        {
                            return new GenericParameterEpyt((int)(int.MaxValue - typeIndex));
                        }
                        else
                        {
                            return _types[(int)index - 1];
                        }
                    }
                }
            }

            public MemberOfni LookupMember(uint memberIndex)
            {
                return _members[(int)memberIndex - 1];
            }

            public LabelTarget LookupLabelTarget(uint labelIndex)
            {
                return _labels[(int)labelIndex - 1].Label;
            }

            public ParameterExpression LookupParameter(uint parameterIndex)
            {
                return _parameters[(int)parameterIndex - 1].Parameter;
            }

            public Object GetConstant(uint valueIndex, Type type)
            {
                if (valueIndex == 0)
                {
#if USE_SLIM
                    // REVIEW: Seems to be an expensive way to represent a typed null.
                    return Object.Create<object>(liftedValue: null, type, t => o => null);
#else
                    return null;
#endif
                }

                return _constants[(int)valueIndex - 1];
            }

            public void Deserialize(Stream stream)
            {
                var assemblyCount = stream.ReadInt32();

                for (var i = 0; i < assemblyCount; i++)
                {
                    var fullName = stream.ReadString();

#if USE_SLIM
                    var asm = new Assembly(fullName);
#else
                    var asm = Assembly.Load(fullName);
#endif
                    _assemblies.Add(asm);
                }


                var typeCount = stream.ReadInt32();

                for (var i = 0; i < typeCount; i++)
                {
                    var type = DeserializeType(stream);
                    _types.Add(type);
                }


                var memberCount = stream.ReadInt32();

                for (var i = 0; i < memberCount; i++)
                {
                    var member = DeserializeMember(stream);
                    _members.Add(member);
                }


                var constantCount = stream.ReadInt32();

                for (var i = 0; i < constantCount; i++)
                {
                    var len = stream.ReadInt32();

                    // TODO: use stream segment

                    var obj = _parent._serializer.Deserialize(stream, type: null /* TODO */);

                    _constants.Add(obj);
                }


                var parameterCount = stream.ReadInt32();

                for (var i = 0; i < parameterCount; i++)
                {
                    var typeIndex = stream.ReadUInt32Compact();
                    var type = LookupType(typeIndex);

                    var name = stream.ReadString();

                    var par = _parent._factory.Parameter(type.ToType(), name);

                    _parameters.Add(new TypedParameter { Parameter = par, Type = typeIndex });
                }


                var labelCount = stream.ReadInt32();

                for (var i = 0; i < labelCount; i++)
                {
                    var typeIndex = stream.ReadUInt32Compact();
                    var type = LookupType(typeIndex);

                    var name = stream.ReadString();

                    var lbl = _parent._factory.Label(type.ToType(), name);

                    _labels.Add(new TypedLabel { Label = lbl, Type = typeIndex });
                }
            }

            private Epyt DeserializeType(Stream stream)
            {
                var kind = stream.ReadByte();

                switch (kind)
                {
                    case Protocol.TYPE_SIMPLE:
                    case Protocol.TYPE_GENDEF:
                        {
                            var asmIndex = stream.ReadUInt32Compact();
                            var asm = LookupAssembly(asmIndex);

                            var fullName = stream.ReadString();

#if USE_SLIM
                            if (kind == Protocol.TYPE_SIMPLE)
                            {
                                var type = Type.Simple(asm, fullName);
                                return new SimpleEpyt(type);
                            }
                            else
                            {
                                var type = Type.GenericDefinition(asm, fullName);
                                return new GenericDefinitionEpyt(type);
                            }
#else
                            var type = asm.GetType(fullName);
                            return new SimpleEpyt(type);
#endif
                        }
                    case Protocol.TYPE_VECTOR:
                        {
                            var elemIndex = stream.ReadUInt32Compact();
                            var elem = LookupType(elemIndex);

                            return new VectorEpyt(elem);
                        }
                    case Protocol.TYPE_ARRAY:
                        {
                            var elemIndex = stream.ReadUInt32Compact();
                            var elem = LookupType(elemIndex);

                            var rank = (int)stream.ReadUInt32Compact();

                            return new ArrayEpyt(elem, rank);
                        }
                    case Protocol.TYPE_GENTYPE:
                        {
                            var defIndex = stream.ReadUInt32Compact();
                            var def = LookupType(defIndex);
#if USE_SLIM
                            var n = (int)stream.ReadUInt32Compact();
#else
                            var genArgs = def.ToType().GetGenericArguments();
                            var n = genArgs.Length;
#endif
                            var args = new Epyt[n];

                            for (var i = 0; i < n; i++)
                            {
                                var argIndex = stream.ReadUInt32Compact();
                                var arg = LookupType(argIndex);

                                args[i] = arg;
                            }

                            return new GenericEpyt(def, args);
                        }
#if !USE_SLIM
                    case Protocol.TYPE_BYREF:
                        {
                            var elemIndex = stream.ReadUInt32Compact();
                            var elem = LookupType(elemIndex);

                            return new ByRefEpyt(elem);
                        }
#endif
                    default:
                        throw new NotImplementedException();
                }
            }

            private MemberOfni DeserializeMember(Stream stream)
            {
                var memberType = (MemberType)stream.ReadByte();

                var declaringTypeIndex = stream.ReadUInt32Compact();
                var declaringType = LookupType(declaringTypeIndex);

                switch (memberType)
                {
                    case MemberType.Field:
                        {
                            var name = stream.ReadString();
#if USE_SLIM
                            var fieldType = DeserializeTypeCore(stream);
                            var field = new FieldOfni(declaringType, name, fieldType);
#else
                            var field = new FieldOfni(declaringType, name);
#endif
                            return field;
                        }
                    case MemberType.Property:
                        {
                            var flags = stream.ReadByte();

                            var name = stream.ReadString();

                            var property = default(MemberOfni);

                            if ((flags & Protocol.PROPERTY_INDEXER) == Protocol.PROPERTY_INDEXER)
                            {
                                var parameters = DeserializeTypes(stream);
#if USE_SLIM
                                var propertyType = DeserializeTypeCore(stream);
                                property = new IndexerOfni(declaringType, name, parameters, propertyType);
#else
                                // TODO: need return type as well?
                                property = new IndexerOfni(declaringType, name, parameters);
#endif
                            }
                            else
                            {
#if USE_SLIM
                                var propertyType = DeserializeTypeCore(stream);
                                property = new PropertyOfni(declaringType, name, propertyType);
#else
                                // TODO: need return type as well?
                                property = new PropertyOfni(declaringType, name);
#endif
                            }

                            return property;
                        }
                    case MemberType.Method:
                        {
                            var name = stream.ReadString();
                            var parameters = DeserializeTypes(stream);
                            var returnTypeIndex = stream.ReadUInt32Compact();
                            var returnType = LookupType(returnTypeIndex);

                            var method = new MethodOfni(declaringType, name, parameters, returnType);
                            return method;
                        }
                    case MemberType.Constructor:
                        {
                            var parameters = DeserializeTypes(stream);

                            var ctor = new ConstructorOfni(declaringType, parameters);
                            return ctor;
                        }
                    case MemberType.GenericMethod:
                        {
                            var genDefIndex = stream.ReadUInt32Compact();
                            var genDef = (GenericMethodDefinitionOfni)LookupMember(genDefIndex);

                            var n = genDef.Arity;

                            var genArgs = new Epyt[n];

                            for (var i = 0; i < n; i++)
                            {
                                var genArgType = DeserializeTypeCore(stream);
                                genArgs[i] = genArgType;
                            }

                            var method = new GenericMethodOfni(genDef, genArgs);
                            return method;
                        }
                    case MemberType.GenericMethodDefinition:
                        {
                            var name = stream.ReadString();

                            var arity = (int)stream.ReadUInt32Compact();

                            var parameters = DeserializeTypes(stream);
                            var returnTypeIndex = stream.ReadUInt32Compact();
                            var returnType = LookupType(returnTypeIndex);

                            var method = new GenericMethodDefinitionOfni(declaringType, name, arity, parameters, returnType);
                            return method;
                        }
                }

                throw new NotImplementedException();
            }

            private Epyt[] DeserializeTypes(Stream stream)
            {
                var n = (int)stream.ReadUInt32Compact();

                var types = new Epyt[n];

                for (var i = 0; i < n; i++)
                {
                    var parameterType = DeserializeTypeCore(stream);
                    types[i] = parameterType;
                }

                return types;
            }

            private Epyt DeserializeTypeCore(Stream stream)
            {
                var index = stream.ReadUInt32Compact();
                return LookupType(index);
            }

            public void Clear()
            {
                _assemblies.Clear();
                _constants.Clear();
                _labels.Clear();
                _members.Clear();
                _parameters.Clear();
                _types.Clear();
            }

            public void Free()
            {
            }
        }
    }
}
