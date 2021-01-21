// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Memory;
using System.Reflection;

#if !USE_SLIM
using System.Linq.CompilerServices;
#endif

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
    using ExpressionVisitor = ExpressionSlimVisitor;
    using FieldInfo = FieldInfoSlim;
    using GotoExpression = GotoExpressionSlim;
    using IndexExpression = IndexExpressionSlim;
    using InvocationExpression = InvocationExpressionSlim;
    using LabelExpression = LabelExpressionSlim;
    using LabelTarget = LabelTargetSlim;
    using LambdaExpression = LambdaExpressionSlim;
    using ListInitExpression = ListInitExpressionSlim;
    using LoopExpression = LoopExpressionSlim;
    using MemberAssignment = MemberAssignmentSlim;
    using MemberBinding = MemberBindingSlim;
    using MemberExpression = MemberExpressionSlim;
    using MemberInfo = MemberInfoSlim;
    using MemberInitExpression = MemberInitExpressionSlim;
    using MemberListBinding = MemberListBindingSlim;
    using MemberMemberBinding = MemberMemberBindingSlim;
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
    using TypeVisitor = TypeSlimVisitor;
    using UnaryExpression = UnaryExpressionSlim;
#endif

    public partial class ExpressionSerializer
    {
        private sealed class Serializer : ExpressionVisitor
        {
            private readonly Stream _stream;
            private readonly SerializationContext _context;

            public Serializer(Stream stream, SerializationContext context)
            {
                _stream = stream;
                _context = context;
            }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    _stream.WriteByte((byte)((int)node.NodeType + 1));
                }
                else
                {
                    _stream.WriteByte(0x00);
                }

                return base.Visit(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.ArrayIndex:
                    case ExpressionType.Assign:
                        {
                            Visit(node.Left);
                            Visit(node.Right);
                        }
                        break;

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
                        {
                            if (node.Method != null)
                            {
                                _stream.WriteByte(Protocol.BINARY_FLAG_HASMETHOD);

                                SerializeMember(node.Method);
                            }
                            else
                            {
                                _stream.WriteByte(0x00);
                            }

                            Visit(node.Left);
                            Visit(node.Right);
                        }
                        break;

                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                        {
                            var flag = (byte)0x00;

                            if (node.Method != null)
                            {
                                flag |= Protocol.BINARY_FLAG_HASMETHOD;
                            }

                            if (node.IsLiftedToNull)
                            {
                                flag |= Protocol.BINARY_FLAG_ISLIFTED;
                            }

                            _stream.WriteByte(flag);

                            if (node.Method != null)
                            {
                                SerializeMember(node.Method);
                            }

                            Visit(node.Left);
                            Visit(node.Right);
                        }
                        break;

                    case ExpressionType.Coalesce:
                        {
                            if (node.Conversion != null)
                            {
                                _stream.WriteByte(Protocol.BINARY_FLAG_HASCONVERSION);

                                Visit(node.Conversion);
                            }
                            else
                            {
                                _stream.WriteByte(0x00);
                            }

                            Visit(node.Left);
                            Visit(node.Right);
                        }
                        break;

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
                        {
                            var flag = (byte)0x00;

                            if (node.Method != null)
                            {
                                flag |= Protocol.BINARY_FLAG_HASMETHOD;
                            }

                            if (node.Conversion != null)
                            {
                                flag |= Protocol.BINARY_FLAG_HASCONVERSION;
                            }

                            _stream.WriteByte(flag);

                            if (node.Method != null)
                            {
                                SerializeMember(node.Method);
                            }

                            if (node.Conversion != null)
                            {
                                Visit(node.Conversion);
                            }

                            Visit(node.Left);
                            Visit(node.Right);
                        }
                        break;
                }

                return node;
            }

            protected override Expression VisitBlock(BlockExpression node)
            {
                var flags = (byte)0x00;

#if !USE_SLIM
                if (node.Type != node.Result.Type)
#endif
                {
                    flags |= Protocol.BLOCK_HASTYPE;
                }

                _stream.WriteByte(flags);

#if !USE_SLIM
                if (node.Type != node.Result.Type)
#endif
                {
                    SerializeType(node.Type);
                }

                _stream.WriteUInt32Compact((uint)node.Variables.Count);
                foreach (var v in node.Variables)
                {
                    VisitParameter(v);
                }

                _stream.WriteUInt32Compact((uint)node.Expressions.Count);
                foreach (var e in node.Expressions)
                {
                    Visit(e);
                }

                return node;
            }

            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                var flags = (byte)0x00;

                if (node.Variable != null)
                {
                    flags |= Protocol.CATCHBLOCK_FLAG_HASVARIABLE;
                }

                if (node.Filter != null)
                {
                    flags |= Protocol.CATCHBLOCK_FLAG_HASFILTER;
                }

                _stream.WriteByte(flags);

                if (node.Variable != null)
                {
                    VisitParameter(node.Variable);
                }
                else
                {
                    SerializeType(node.Test);
                }

                if (node.Filter != null)
                {
                    Visit(node.Filter);
                }

                Visit(node.Body);

                return node;
            }

            protected override Expression VisitConditional(ConditionalExpression node)
            {
#if USE_SLIM
                var flags = (byte)0x00;

                if (node.Type != null)
                {
                    flags |= Protocol.CONDITIONAL_HASTYPE;
                }

                _stream.WriteByte(flags);

                if (node.Type != null)
                {
                    SerializeType(node.Type);
                }
#else
                // REVIEW: Can we apply this optimization for "fat" expressions too?
                SerializeType(node.Type);
#endif

                Visit(node.Test);
                Visit(node.IfTrue);
                Visit(node.IfFalse);

                return node;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                SerializeType(node.Type);

                var value = _context.AddConstant(node.Value);
                _stream.WriteUInt32Compact(value);

                return node;
            }

#if !USE_SLIM
            protected override Expression VisitDebugInfo(DebugInfoExpression node)
            {
                throw new NotSupportedException();
            }
#endif

            protected override Expression VisitDefault(DefaultExpression node)
            {
                SerializeType(node.Type);

                return node;
            }

#if !USE_SLIM
            protected override Expression VisitDynamic(DynamicExpression node)
            {
                throw new NotSupportedException();
            }
#endif

            protected override ElementInit VisitElementInit(ElementInit node)
            {
                SerializeMember(node.AddMethod);

                foreach (var a in node.Arguments) // # known from reflection info
                {
                    Visit(a);
                }

                return node;
            }

#if !USE_SLIM
            protected override Expression VisitExtension(Expression node)
            {
                throw new NotSupportedException();
            }
#endif

            protected override Expression VisitGoto(GotoExpression node)
            {
                var flags = (byte)0x00;

                if (node.Value != null)
                {
                    flags |= Protocol.GOTO_HASVALUE;
                }

                _stream.WriteByte(flags);

                _stream.WriteByte((byte)node.Kind);

                SerializeType(node.Type);

                VisitLabelTarget(node.Target);

                if (node.Value != null)
                {
                    Visit(node.Value);
                }

                return node;
            }

            protected override Expression VisitIndex(IndexExpression node)
            {
                SerializeMember(node.Indexer);

                Visit(node.Object);

                foreach (var a in node.Arguments) // # known from reflection info
                {
                    Visit(a);
                }

                return node;
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                Visit(node.Expression);

#if USE_SLIM
                var n = node.ArgumentCount;

                _stream.WriteUInt32Compact((uint)n);

                for (var i = 0; i < n; i++)
                {
                    Visit(node.GetArgument(i));
                }
#else
                foreach (var a in node.Arguments) // # known from reflection info
                {
                    Visit(a);
                }
#endif

                return node;
            }

            protected override Expression VisitLabel(LabelExpression node)
            {
                if (node.DefaultValue != null)
                {
                    _stream.WriteByte(Protocol.LABEL_HASDEFAULTVALUE);
                }
                else
                {
                    _stream.WriteByte(0x00);
                }

                VisitLabelTarget(node.Target);

                if (node.DefaultValue != null)
                {
                    Visit(node.DefaultValue);
                }

                return node;
            }

            protected override LabelTarget VisitLabelTarget(LabelTarget node)
            {
                var labelReference = GetLabelReference(node);
                _stream.WriteUInt32Compact(labelReference);

                return node;
            }

#if USE_SLIM
            protected override Expression VisitLambda(LambdaExpression node)
#else
            protected override Expression VisitLambda<T>(Expression<T> node)
#endif
            {
                var flags = (byte)0x00;

#if !USE_SLIM
                if (node.Name != null)
                {
                    flags |= Protocol.LAMBDA_HASNAME;
                }

                if (node.TailCall)
                {
                    flags |= Protocol.LAMBDA_ISTAILCALL;
                }
#endif

#if USE_SLIM
                var type = node.DelegateType;

                var hasType = false;

                if (type != null)
                {
                    hasType = true;

                    // REVIEW: Check if LambdaExpressionSlim deals correctly with high arity funcs and actions.

                    if (type.Kind == TypeSlimKind.Generic)
                    {
                        var gen = (GenericTypeSlim)type;
                        var def = gen.GenericTypeDefinition;

                        if (Types.GenericDelegateTypes.Contains(def))
                        {
                            hasType = false;
                        }
                    }
                }
#else
                var type = node.Type;

                var hasType = false;

                if (node.ReturnType == typeof(void))
                {
                    var n = node.Parameters.Count;

                    var types = new Type[n];

                    for (var i = 0; i < n; i++)
                    {
                        types[i] = node.Parameters[i].Type;
                    }

                    hasType = !Expression.TryGetActionType(types, out Type delegateType) || delegateType != node.Type;
                }
                else
                {
                    var n = node.Parameters.Count;

                    var types = new Type[n + 1];

                    for (var i = 0; i < n; i++)
                    {
                        types[i] = node.Parameters[i].Type;
                    }

                    types[n] = node.ReturnType;

                    hasType = !Expression.TryGetFuncType(types, out Type delegateType) || delegateType != node.Type;
                }
#endif

#if !USE_SLIM
                if (type.Assembly.IsDynamic)
                {
                    hasType = false;
                }
#endif

                if (hasType)
                {
                    flags |= Protocol.LAMBDA_HASTYPE;
                }

                _stream.WriteByte(flags);

#if !USE_SLIM
                if (node.Name != null)
                {
                    _stream.WriteString(node.Name);
                }
#endif

                if (hasType)
                {
                    SerializeType(type);
                }

#if !USE_SLIM
                if (!hasType)
#endif
                {
                    _stream.WriteUInt32Compact((uint)node.Parameters.Count);
                }

                foreach (var p in node.Parameters)
                {
                    VisitParameter(p);
                }

                Visit(node.Body);

                return node;
            }

            protected override Expression VisitListInit(ListInitExpression node)
            {
                Visit(node.NewExpression);

                _stream.WriteUInt32Compact((uint)node.Initializers.Count);

                foreach (var i in node.Initializers)
                {
                    VisitElementInit(i);
                }

                return node;
            }

            protected override Expression VisitLoop(LoopExpression node)
            {
                var flags = (byte)0x00;

                if (node.BreakLabel != null)
                {
                    flags |= Protocol.LOOP_HASBREAK;
                }

                if (node.ContinueLabel != null)
                {
                    flags |= Protocol.LOOP_HASCONTINUE;
                }

                _stream.WriteByte(flags);

                if (node.BreakLabel != null)
                {
                    VisitLabelTarget(node.BreakLabel);
                }

                if (node.ContinueLabel != null)
                {
                    VisitLabelTarget(node.ContinueLabel);
                }

                Visit(node.Body);

                return node;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
#if USE_SLIM
                var flags = (byte)0x00;

                if (node.Expression != null)
                {
                    flags |= Protocol.MEMBER_HASOBJ;
                }

                _stream.WriteByte(flags);
#endif

                SerializeMember(node.Member);

                if (node.Expression != null)
                {
                    Visit(node.Expression);
                }

                return node;
            }

            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                Visit(node.NewExpression);

                _stream.WriteUInt32Compact((uint)node.Bindings.Count);

                foreach (var b in node.Bindings)
                {
                    VisitMemberBinding(b);
                }

                return node;
            }

            protected override MemberBinding VisitMemberBinding(MemberBinding node)
            {
                _stream.WriteByte((byte)node.BindingType);

                SerializeMember(node.Member);

                return base.VisitMemberBinding(node);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
            {
                Visit(node.Expression);

                return node;
            }

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
            {
                _stream.WriteUInt32Compact((uint)node.Initializers.Count);

                foreach (var i in node.Initializers)
                {
                    VisitElementInit(i);
                }

                return node;
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
            {
                _stream.WriteUInt32Compact((uint)node.Bindings.Count);

                foreach (var b in node.Bindings)
                {
                    VisitMemberBinding(b);
                }

                return node;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
#if USE_SLIM
                var flags = (byte)0x00;

                if (node.Object != null)
                {
                    flags |= Protocol.CALL_HASOBJ;
                }

                _stream.WriteByte(flags);
#endif

                SerializeMember(node.Method);

                if (node.Object != null)
                {
                    Visit(node.Object);
                }

                foreach (var a in node.Arguments) // # known from reflection info
                {
                    Visit(a);
                }

                return node;
            }

            protected override Expression VisitNew(NewExpression node)
            {
                var flags = (byte)0x00;

                if (node.Constructor == null)
                {
                    flags |= Protocol.NEW_NOCTOR;
                }

                if (node.Members != null)
                {
                    flags |= Protocol.NEW_HASMEMBERS;
                }

                _stream.WriteByte(flags);

                if (node.Constructor != null)
                {
                    SerializeMember(node.Constructor);

                    foreach (var a in node.Arguments) // # known from reflection info
                    {
                        Visit(a);
                    }
                }
                else
                {
                    SerializeType(node.Type);
                }

                if (node.Members != null)
                {
                    foreach (var m in node.Members) // # known from reflection info
                    {
                        SerializeMember(m);
                    }
                }

                return node;
            }

            protected override Expression VisitNewArray(NewArrayExpression node)
            {
#if USE_SLIM
                var elementType = node.ElementType;
#else
                var elementType = node.Type.GetElementType();
#endif
                SerializeType(elementType);

                _stream.WriteUInt32Compact((uint)node.Expressions.Count);

                foreach (var a in node.Expressions)
                {
                    Visit(a);
                }

                return node;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                var parameterIndex = GetParameterReference(node);
                _stream.WriteUInt32Compact(parameterIndex);

                return node;
            }

#if !USE_SLIM
            protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
            {
                _stream.WriteUInt32Compact((uint)node.Variables.Count);

                foreach (var v in node.Variables)
                {
                    Visit(v);
                }

                return node;
            }
#endif

            protected override Expression VisitSwitch(SwitchExpression node)
            {
                var flags = (byte)0x00;

#if !USE_SLIM
                if (node.Type != node.Cases[0].Body.Type)
#endif
                {
                    flags |= Protocol.SWITCH_HASTYPE;
                }

                if (node.Comparison != null)
                {
                    flags |= Protocol.SWITCH_HASCOMPARISON;
                }

                if (node.DefaultBody != null)
                {
                    flags |= Protocol.SWITCH_HASDEFAULT;
                }

                _stream.WriteByte(flags);

#if !USE_SLIM
                if (node.Type != node.Cases[0].Body.Type)
#endif
                {
                    SerializeType(node.Type);
                }

                Visit(node.SwitchValue);

                if (node.Comparison != null)
                {
                    SerializeMember(node.Comparison);
                }

                _stream.WriteUInt32Compact((uint)node.Cases.Count);

                foreach (var c in node.Cases)
                {
                    VisitSwitchCase(c);
                }

                if (node.DefaultBody != null)
                {
                    Visit(node.DefaultBody);
                }

                return node;
            }

            protected override SwitchCase VisitSwitchCase(SwitchCase node)
            {
                _stream.WriteUInt32Compact((uint)node.TestValues.Count);

                foreach (var v in node.TestValues)
                {
                    Visit(v);
                }

                Visit(node.Body);

                return node;
            }

            protected override Expression VisitTry(TryExpression node)
            {
                var flags = (byte)0x00;

#if !USE_SLIM
                if (node.Type != node.Body.Type)
#endif
                {
                    flags |= Protocol.TRY_HASTYPE;
                }

                if (node.Handlers.Count != 0)
                {
                    flags |= Protocol.TRY_HASCATCH;
                }

                if (node.Fault != null)
                {
                    flags |= Protocol.TRY_HASFAULT;
                }

                if (node.Finally != null)
                {
                    flags |= Protocol.TRY_HASFINALLY;
                }

                _stream.WriteByte(flags);

#if !USE_SLIM
                if (node.Type != node.Body.Type)
#endif
                {
                    SerializeType(node.Type);
                }

                Visit(node.Body);

                if (node.Handlers.Count != 0)
                {
                    _stream.WriteUInt32Compact((uint)node.Handlers.Count);

                    foreach (var h in node.Handlers)
                    {
                        VisitCatchBlock(h);
                    }
                }

                if (node.Fault != null)
                {
                    Visit(node.Fault);
                }

                if (node.Finally != null)
                {
                    Visit(node.Finally);
                }

                return node;
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                SerializeType(node.TypeOperand);

                Visit(node.Expression);

                return node;
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.ArrayLength:
                    case ExpressionType.Quote:
                        {
                            Visit(node.Operand);
                        }
                        break;

                    case ExpressionType.Throw:
                    case ExpressionType.TypeAs:
                    case ExpressionType.Unbox:
                        {
                            SerializeType(node.Type);

                            Visit(node.Operand);
                        }
                        break;

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
                        {
                            if (node.Method != null)
                            {
                                _stream.WriteByte(Protocol.UNARY_FLAG_HASMETHOD);

                                SerializeMember(node.Method);
                            }
                            else
                            {
                                _stream.WriteByte(0x00);
                            }

                            Visit(node.Operand);
                        }
                        break;

                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        {
                            if (node.Method != null)
                            {
                                _stream.WriteByte(Protocol.UNARY_FLAG_HASMETHOD);
                            }
                            else
                            {
                                _stream.WriteByte(0x00);
                            }

                            SerializeType(node.Type);

                            if (node.Method != null)
                            {
                                SerializeMember(node.Method);
                            }

                            Visit(node.Operand);
                        }
                        break;
                }

                return node;
            }

            private uint GetLabelReference(LabelTarget label)
            {
                return _context.Register(label);
            }

            private uint GetParameterReference(ParameterExpression parameter)
            {
#if !USE_SLIM
                if (parameter.IsByRef)
                {
                    throw new NotImplementedException();
                }
#endif

                return _context.Register(parameter);
            }

            private void SerializeType(Type type)
            {
                var typeIndex = _context.Register(type);
                _stream.WriteUInt32Compact(typeIndex);
            }

            private void SerializeMember(MemberInfo member)
            {
                var memberIndex = _context.Register(member);
                _stream.WriteUInt32Compact(memberIndex);
            }
        }

        private sealed class SerializationContext : IClearable, IFreeable
        {
#if USE_SLIM
            private static readonly IEqualityComparer<Type> s_typeComparer = PooledTypeSlimEqualityComparer.Instance;
            private static readonly IEqualityComparer<MemberInfo> s_memberComparer = new MemberInfoSlimEqualityComparer(s_typeComparer);
#else
            private static readonly IEqualityComparer<Type> s_typeComparer = EqualityComparer<Type>.Default;
            private static readonly IEqualityComparer<MemberInfo> s_memberComparer = EqualityComparer<MemberInfo>.Default;
#endif

            private readonly ExpressionSerializer _parent;
            private readonly TypeExpander _expander;

            // TODO: pool
            private readonly Dictionary<Object, int> _constantsMap = new();
            private readonly List<Object> _constants = new();

            private readonly Dictionary<ParameterExpression, int> _parameterMap = new();
            private readonly List<TypedParameter> _parameters = new();

            private readonly Dictionary<LabelTarget, int> _labelMap = new();
            private readonly List<TypedLabel> _labels = new();

            private readonly Dictionary<Assembly, int> _assemblyMap = new();
            private readonly List<Assembly> _assemblies = new();

            private readonly Dictionary<Type, int> _typeMap = new(s_typeComparer);
            private readonly List<Type> _types = new();

            private readonly Dictionary<MemberInfo, int> _memberMap = new(s_memberComparer);
            private readonly List<MemberInfo> _members = new();

#if USE_SLIM
            private IList<Type> _genericArguments;
#else
            private Type[] _genericArguments;
#endif

            private static readonly Dictionary<Type, byte> s_typeCodes = new()
            {
                { Types.Void, Protocol.TYPE_VOID },
                { Types.Object, Protocol.TYPE_OBJECT },
                { Types.Guid, Protocol.TYPE_GUID },
                { Types.DateTimeOffset, Protocol.TYPE_DATETIMEOFFSET },
                { Types.TimeSpan, Protocol.TYPE_TIMESPAN },
                { Types.Uri, Protocol.TYPE_URI },
            };

            public SerializationContext(ExpressionSerializer parent)
            {
                _expander = new TypeExpander(this);
                _parent = parent;
            }

            private uint Register(Assembly assembly)
            {
                if (!_assemblyMap.TryGetValue(assembly, out int res))
                {
                    _assemblies.Add(assembly);

                    res = _assemblies.Count;
                    _assemblyMap[assembly] = res;
                }

                return (uint)res;
            }

            public bool ContainsTypeRegistration(Type type)
            {
                return _typeMap.ContainsKey(type);
            }

            public uint Register(Type type, bool skipExpand = false)
            {
                if (type.IsGenericParameter())
                {
                    if (!_typeMap.TryGetValue(type, out int res))
                    {
#if USE_SLIM
                        var i = _genericArguments.IndexOf(type);
#else
                        var i = Array.IndexOf(_genericArguments, type);
#endif
                        if (i >= 0)
                        {
                            var index = int.MaxValue - i;
                            res = index;
                        }
                        else
                        {
                            throw new InvalidOperationException(); // TODO
                        }

                        _typeMap[type] = res;
                    }

                    return (uint)res;
                }

                var t = type;

                var isArray = t.IsArray();
                var isNullable = false;

                if (isArray)
                {
                    t = t.GetElementType();
                }

                if (t.IsGenericType() && !t.IsGenericTypeDefinition() && s_typeComparer.Equals(t.GetGenericTypeDefinition(), Types.NullableOfT))
                {
                    isNullable = true;
                    t = t.GetGenericArguments()[0];
                }

                var typeCode = default(byte?);

                switch (t.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        typeCode = Protocol.TYPE_BOOLEAN;
                        break;
                    case TypeCode.Byte:
                        typeCode = Protocol.TYPE_UINT8;
                        break;
                    case TypeCode.Char:
                        typeCode = Protocol.TYPE_CHAR;
                        break;
                    case TypeCode.DateTime:
                        typeCode = Protocol.TYPE_DATETIME;
                        break;
                    case TypeCode.Decimal:
                        typeCode = Protocol.TYPE_DECIMAL;
                        break;
                    case TypeCode.Double:
                        typeCode = Protocol.TYPE_FLOAT8;
                        break;
                    case TypeCode.Int16:
                        typeCode = Protocol.TYPE_INT16;
                        break;
                    case TypeCode.Int32:
                        typeCode = Protocol.TYPE_INT32;
                        break;
                    case TypeCode.Int64:
                        typeCode = Protocol.TYPE_INT64;
                        break;
                    case TypeCode.SByte:
                        typeCode = Protocol.TYPE_INT8;
                        break;
                    case TypeCode.Single:
                        typeCode = Protocol.TYPE_FLOAT4;
                        break;
                    case TypeCode.String:
                        typeCode = Protocol.TYPE_STRING;
                        break;
                    case TypeCode.UInt16:
                        typeCode = Protocol.TYPE_UINT16;
                        break;
                    case TypeCode.UInt32:
                        typeCode = Protocol.TYPE_UINT32;
                        break;
                    case TypeCode.UInt64:
                        typeCode = Protocol.TYPE_UINT64;
                        break;
                    default:
                        {
                            if (s_typeCodes.TryGetValue(t, out byte typeCodeValue))
                            {
                                typeCode = typeCodeValue;
                            }
                        }
                        break;
                }

                if (typeCode == null)
                {
                    var asm = type.GetAssembly();

                    if (asm != null)
                    {
                        Register(asm);
                    }

                    if (!skipExpand)
                    {
                        _expander.Visit(type);
                    }

                    if (!_typeMap.TryGetValue(type, out int res))
                    {
                        _types.Add(type);

                        res = _types.Count;
                        _typeMap[type] = res;
                    }

                    if (res < Protocol.TYPE_SHORT_INDEX_MAX)
                    {
                        var value = (byte)(Protocol.TYPE_FLAG_INDEXED | res); // 10xx xxxx
                        return value;
                    }
                    else
                    {
                        var value = Protocol.TYPE_LONG_INDEX_FLAG | (uint)res;
                        return value;
                    }
                }
                else
                {
                    var value = typeCode.Value;

                    if (isArray)
                    {
                        value |= Protocol.TYPE_FLAG_ARRAY;
                    }

                    if (isNullable)
                    {
                        value |= Protocol.TYPE_FLAG_NULLABLE;
                    }

                    return value;
                }
            }

            public uint Register(MemberInfo member)
            {
                if (!_memberMap.TryGetValue(member, out int res))
                {
                    Register(member.DeclaringType);

                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            var field = (FieldInfo)member;
                            Register(field.FieldType);
                            break;
                        case MemberTypes.Property:
                            var property = (PropertyInfo)member;
                            Register(property.PropertyType);
#if USE_SLIM
                            foreach (var indexParameterType in property.IndexParameterTypes)
                            {
                                Register(indexParameterType);
                            }
#else
                            foreach (var p in property.GetIndexParameters())
                            {
                                Register(p.ParameterType);
                            }
#endif
                            break;
                        case MemberTypes.Method:
                            var method = (MethodInfo)member;

                            if (method.IsGenericMethod && !method.IsGenericMethodDefinition())
                            {
                                Register(method.GetGenericMethodDefinition());

                                foreach (var arg in method.GetGenericArguments())
                                {
                                    Register(arg);
                                }
                            }

                            if (method.IsGenericMethodDefinition())
                            {
                                PushGenericArguments(method.GetGenericArguments());
                            }

                            try
                            {
                                Register(method.ReturnType);
#if USE_SLIM
                                foreach (var parameterType in method.ParameterTypes)
                                {
                                    Register(parameterType);
                                }
#else
                                foreach (var p in method.GetParameters())
                                {
                                    Register(p.ParameterType);
                                }
#endif
                            }
                            finally
                            {
                                if (method.IsGenericMethodDefinition())
                                {
                                    PopGenericArguments();
                                }
                            }

                            break;
                        case MemberTypes.Constructor:
                            var ctor = (ConstructorInfo)member;
#if USE_SLIM
                            foreach (var parameterType in ctor.ParameterTypes)
                            {
                                Register(parameterType);
                            }
#else
                            foreach (var p in ctor.GetParameters())
                            {
                                Register(p.ParameterType);
                            }
#endif
                            break;
                    }

                    _members.Add(member);

                    res = _members.Count;
                    _memberMap[member] = res;
                }

                return (uint)res;
            }

            public uint Register(LabelTarget label)
            {
                if (!_labelMap.TryGetValue(label, out int res))
                {
                    _labels.Add(new TypedLabel { Label = label, Type = Register(label.Type) });

                    res = _labels.Count;
                    _labelMap[label] = res;
                }

                return (uint)res;
            }

            public uint Register(ParameterExpression parameter)
            {
                Register(parameter.Type);

                if (!_parameterMap.TryGetValue(parameter, out int res))
                {
                    _parameters.Add(new TypedParameter { Parameter = parameter, Type = Register(parameter.Type) });

                    res = _parameters.Count;
                    _parameterMap[parameter] = res;
                }

                return (uint)res;
            }

#if USE_SLIM
            private void PushGenericArguments(IList<Type> types)
#else
            private void PushGenericArguments(Type[] types)
#endif
            {
                Debug.Assert(_genericArguments == null);
                _genericArguments = types;
            }

            private void PopGenericArguments()
            {
                Debug.Assert(_genericArguments != null);
                _genericArguments = null;
            }

            public uint AddConstant(Object value)
            {
#if USE_SLIM
                if (value?.Value == null)
#else
                if (value == null)
#endif
                {
                    return 0;
                }

                if (!_constantsMap.TryGetValue(value, out int res))
                {
                    _constants.Add(value);

                    res = _constants.Count;
                    _constantsMap[value] = res;
                }

                return (uint)res;
            }

            public void Serialize(Stream stream)
            {
                stream.WriteInt32(_assemblies.Count);

                foreach (var asm in _assemblies)
                {
                    // TODO: known assemblies
#if USE_SLIM
                    stream.WriteString(asm.Name);
#else
                    stream.WriteString(asm.FullName);
#endif
                }


                stream.WriteInt32(_types.Count);

                foreach (var type in _types)
                {
                    SerializeType(stream, type);
                }


                stream.WriteInt32(_members.Count);

                foreach (var member in _members)
                {
                    SerializeMember(stream, member);
                }


                stream.WriteInt32(_constants.Count);

                foreach (var cst in _constants)
                {
                    var begin = stream.Position;

                    stream.Position += 4;

                    _parent._serializer.Serialize(stream, type: null /* TODO */, cst);

                    var end = stream.Position;

                    stream.Position = begin;
                    stream.WriteInt32(checked((int)(end - /* TODO: subtract not needed under checked */ begin)));

                    stream.Position = end;
                }


                stream.WriteInt32(_parameters.Count);

                foreach (var par in _parameters)
                {
                    stream.WriteUInt32Compact(par.Type);

                    stream.WriteString(par.Parameter.Name);
                }


                stream.WriteInt32(_labels.Count);

                foreach (var lbl in _labels)
                {
                    stream.WriteUInt32Compact(lbl.Type);

                    stream.WriteString(lbl.Label.Name);
                }
            }

            private void SerializeType(Stream stream, Type type)
            {
                if (type.IsArray())
                {
                    var rank = type.GetArrayRank();

                    if (rank == 1)
                    {
                        stream.WriteByte(Protocol.TYPE_VECTOR);

                        var elemIndex = Register(type.GetElementType());
                        stream.WriteUInt32Compact(elemIndex);
                    }
                    else
                    {
                        stream.WriteByte(Protocol.TYPE_ARRAY);

                        var elemIndex = Register(type.GetElementType());
                        stream.WriteUInt32Compact(elemIndex);

                        stream.WriteUInt32Compact((uint)rank);
                    }
                }
                else if (type.IsGenericTypeDefinition())
                {
                    stream.WriteByte(Protocol.TYPE_GENDEF);

                    type.GetAssemblyAndName(out var asm, out var name);

                    var asmIndex = Register(asm);
                    stream.WriteUInt32Compact(asmIndex);

                    stream.WriteString(name);
                }
                else if (type.IsGenericParameter())
                {
                    stream.WriteByte(Protocol.TYPE_GENPAR);

                    stream.WriteString(type.GetName());
                }
                else if (type.IsGenericType())
                {
                    stream.WriteByte(Protocol.TYPE_GENTYPE);

                    var defIndex = Register(type.GetGenericTypeDefinition());
                    stream.WriteUInt32Compact(defIndex);

                    var args = type.GetGenericArguments();

#if USE_SLIM
                    stream.WriteUInt32Compact((uint)args.Count); // REVIEW: Can we have the arity stored with the definition?
#endif

                    foreach (var arg in args)
                    {
                        var argIndex = Register(arg);
                        stream.WriteUInt32Compact(argIndex);
                    }
                }
#if !USE_SLIM
                else if (type.IsByRef)
                {
                    stream.WriteByte(Protocol.TYPE_BYREF);

                    var elemIndex = Register(type.GetElementType());
                    stream.WriteUInt32Compact(elemIndex);
                }
#endif
                else
                {
                    stream.WriteByte(Protocol.TYPE_SIMPLE);

                    type.GetAssemblyAndName(out var asm, out var name);

                    var asmIndex = Register(asm);
                    stream.WriteUInt32Compact(asmIndex);

                    stream.WriteString(name);
                }
            }

            private void SerializeMember(Stream stream, MemberInfo member)
            {
                var kind = default(MemberType);

                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        kind = MemberType.Field;
                        break;
                    case MemberTypes.Property:
                        kind = MemberType.Property;
                        break;
                    case MemberTypes.Constructor:
                        kind = MemberType.Constructor;
                        break;
                    case MemberTypes.Method:
                        {
                            var method = (MethodInfo)member;
                            if (method.IsGenericMethod)
                            {
                                if (method.IsGenericMethodDefinition())
                                {
                                    kind = MemberType.GenericMethodDefinition;
                                }
                                else
                                {
                                    kind = MemberType.GenericMethod;
                                }
                            }
                            else
                            {
                                kind = MemberType.Method;
                            }
                        }
                        break;
                }

                stream.WriteByte((byte)kind);

                var declaringTypeIndex = Register(member.DeclaringType);
                stream.WriteUInt32Compact(declaringTypeIndex);

                switch (kind)
                {
                    case MemberType.Field:
                        {
                            var field = (FieldInfo)member;

                            stream.WriteString(field.Name);
#if USE_SLIM
                            SerializeTypeCore(stream, field.FieldType);
#endif
                        }
                        break;
                    case MemberType.Property:
                        {
                            var property = (PropertyInfo)member;

#if USE_SLIM
                            var parameterTypes = property.IndexParameterTypes;
                            var n = parameterTypes.Count;
#else
                            var parameters = property.GetIndexParameters();
                            var n = parameters.Length;
#endif

                            var flag = (byte)0x00;

                            if (n != 0)
                            {
                                flag |= Protocol.PROPERTY_INDEXER;
                            }

                            stream.WriteByte(flag);

                            stream.WriteString(property.Name);

                            if (n != 0)
                            {
#if USE_SLIM
                                SerializeParameterTypes(stream, parameterTypes);
#else
                                SerializeParameterTypes(stream, parameters);
#endif
                            }

#if USE_SLIM
                            SerializeTypeCore(stream, property.PropertyType);
#else
                            // TODO: need return type as well?
#endif
                        }
                        break;
                    case MemberType.Method:
                        {
                            var method = (MethodInfo)member;

                            stream.WriteString(method.GetName());

#if USE_SLIM
                            SerializeParameterTypes(stream, method.ParameterTypes);
#else
                            SerializeParameterTypes(stream, method.GetParameters());
#endif

                            var returnTypeIndex = Register(method.ReturnType); // TODO: avoid if not needed? e.g. only for op_Implicit and op_Explicit?
                            stream.WriteUInt32Compact(returnTypeIndex);
                        }
                        break;
                    case MemberType.GenericMethod:
                        {
                            var method = (MethodInfo)member;

                            var genDef = method.GetGenericMethodDefinition();

                            var genDefIndex = Register(genDef);
                            stream.WriteUInt32Compact(genDefIndex);

                            var genArgs = method.GetGenericArguments();

                            foreach (var genArg in genArgs)
                            {
                                var genArgTypeIndex = Register(genArg);
                                stream.WriteUInt32Compact(genArgTypeIndex);
                            }
                        }
                        break;
                    case MemberType.GenericMethodDefinition:
                        {
                            var method = (MethodInfo)member;

                            stream.WriteString(method.GetName());

                            var genArgs = method.GetGenericArguments();

#if USE_SLIM
                            var arity = genArgs.Count;
#else
                            var arity = genArgs.Length;
#endif

                            stream.WriteUInt32Compact((uint)arity);

                            PushGenericArguments(genArgs);

                            try
                            {
#if USE_SLIM
                                SerializeParameterTypes(stream, method.ParameterTypes);
#else
                                SerializeParameterTypes(stream, method.GetParameters());
#endif

                                var returnTypeIndex = Register(method.ReturnType); // TODO: avoid if not needed? e.g. only for op_Implicit and op_Explicit?
                                stream.WriteUInt32Compact(returnTypeIndex);
                            }
                            finally
                            {
                                PopGenericArguments();
                            }
                        }
                        break;
                    case MemberType.Constructor:
                        {
                            var ctor = (ConstructorInfo)member;

#if USE_SLIM
                            SerializeParameterTypes(stream, ctor.ParameterTypes);
#else
                            SerializeParameterTypes(stream, ctor.GetParameters());
#endif
                        }
                        break;
                }
            }

#if USE_SLIM
            private void SerializeParameterTypes(Stream stream, IList<Type> parameterTypes)
            {
                stream.WriteUInt32Compact((uint)parameterTypes.Count);

                foreach (var parameterType in parameterTypes)
                {
                    SerializeTypeCore(stream, parameterType);
                }
            }
#else
            private void SerializeParameterTypes(Stream stream, ParameterInfo[] parameters)
            {
                stream.WriteUInt32Compact((uint)parameters.Length);

                foreach (var parameter in parameters)
                {
                    SerializeTypeCore(stream, parameter.ParameterType);
                }
            }
#endif

            private void SerializeTypeCore(Stream stream, Type type)
            {
                var parameterTypeIndex = Register(type);
                stream.WriteUInt32Compact(parameterTypeIndex);
            }

            private sealed class TypeExpander : TypeVisitor
            {
                private readonly SerializationContext _context;

                public TypeExpander(SerializationContext context)
                {
                    _context = context;
                }

                public override Type Visit(Type type)
                {
                    if (_context.ContainsTypeRegistration(type))
                    {
                        return type;
                    }

                    var res = base.Visit(type); // depth first

                    _context.Register(type, skipExpand: true);

                    return res;
                }
            }

            public void Clear()
            {
                _assemblies.Clear();
                _assemblyMap.Clear();
                _constants.Clear();
                _constantsMap.Clear();
                _labels.Clear();
                _labelMap.Clear();
                _members.Clear();
                _memberMap.Clear();
                _parameters.Clear();
                _parameterMap.Clear();
                _types.Clear();
                _typeMap.Clear();
            }

            public void Free()
            {
            }
        }
    }
}
