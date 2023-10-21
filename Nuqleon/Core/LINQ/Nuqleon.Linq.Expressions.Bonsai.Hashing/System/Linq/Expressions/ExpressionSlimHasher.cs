// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks. (Protected methods are supposed to be called with non-null arguments.)

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Memory;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
    using static HashHelpers;

    /// <summary>
    /// Base class for expression tree hash code computations.
    /// </summary>
    public class ExpressionSlimHasher
    {
        private readonly ObjectPool<ExpressionHashingVisitor> _pool;
        private readonly IMemoizer _memoizer;

        /// <summary>
        /// Creates a new expression hasher.
        /// </summary>
        public ExpressionSlimHasher()
            : this(Environment.ProcessorCount, MemoizationCacheFactory.Unbounded)
        {
        }

        /// <summary>
        /// Creates a new expression hasher with the specified object pool capacity and memoization cache factory.
        /// </summary>
        /// <param name="poolCapacity">The size of the internal object pool to keep data structures that are required to compute a hash code.</param>
        /// <param name="cacheFactory"></param>
        /// <remarks>
        /// The <paramref name="poolCapacity"/> parameter can be used to adjust the internal pool size when it's known that the
        /// hasher is used in specific configurations. For example, if the hasher is only used from a single thread, a pool size
        /// of <c>1</c> could suffice. In case one expects the overridden <see cref="GetHashCode(ObjectSlim)"/> method to reenter
        /// the hasher, the pool size could be adjusted to allow for such reentrancy. If possible, it is recommended to make such
        /// overridden <see cref="GetHashCode(ObjectSlim)"/> implementations call <see cref="GetHashCode(ExpressionSlim)"/> directly
        /// rather than causing the creation of a new hasher instance.
        ///
        /// Note that a pool size that's too small does not cause a functional regression; it merely can result in degraded hashing
        /// performance due to the allocation of internal data structures that cannot be reused.
        /// </remarks>
        public ExpressionSlimHasher(int poolCapacity, IMemoizationCacheFactory cacheFactory)
        {
            _pool = new ObjectPool<ExpressionHashingVisitor>(CreateHashingVisitor, poolCapacity);
            _memoizer = Memoizer.Create(cacheFactory);
        }

        /// <summary>
        /// Gets a hash code for the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The expression tree to compute a hash code for.</param>
        /// <returns>A hash code for the specified <paramref name="expression"/>.</returns>
        public int GetHashCode(ExpressionSlim expression)
        {
            using var pooledHasher = _pool.New();

            return pooledHasher.Object.Visit(expression);
        }

        /// <summary>
        /// Gets the hash code for a global <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter">The global parameter to get the hash code for.</param>
        /// <returns>A hash code for the specified global <paramref name="parameter"/>.</returns>
        /// <remarks>
        /// The expression type (<see cref="ExpressionType.Parameter"/>) and the type of the expression
        /// (see <see cref="ParameterExpressionSlim.Type"/> will be added to the hash code returned by
        /// the overridden method. It is recommended to base the hash code of a global parameter either
        /// on object identity or the name. The default implementation uses the name.
        /// </remarks>
        protected virtual int GetHashCode(ParameterExpressionSlim parameter) => GetHashCode(parameter.Name);

        /// <summary>
        /// Gets the hash code for a <paramref name="label"/>.
        /// </summary>
        /// <param name="label">The label to get the hash code for.</param>
        /// <returns>A hash code for the specified global <paramref name="label"/>.</returns>
        /// <remarks>
        /// The type of the label target (see <see cref="ParameterExpressionSlim.Type"/> will be added
        /// to the hash code returned by the overridden method. It is recommended to base the hash code
        /// of a label either on object identity or the name. The default implementation uses the name.
        /// </remarks>
        protected virtual int GetHashCode(LabelTargetSlim label) => GetHashCode(label.Name);

        /// <summary>
        /// Gets the hash code for the specified object <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object value to get the hash code for.</param>
        /// <returns>A hash code for the specified object <paramref name="value"/>.</returns>
        protected virtual int GetHashCode(ObjectSlim value) => value?.Value?.GetHashCode() ?? 0;

        /// <summary>
        /// Gets the hash code for the specified string <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The string value to get the hash code for.</param>
        /// <returns>A hash code for the specified string <paramref name="value"/>.</returns>
        protected virtual int GetHashCode(string value) =>
#if NET8_0 || NETSTANDARD2_1
            value?.GetHashCode(StringComparison.Ordinal) ?? 0
#else
            value?.GetHashCode() ?? 0
#endif
            ;

        /// <summary>
        /// Gets the hash code for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the hash code for.</param>
        /// <returns>A hash code for the specified <paramref name="assembly"/>.</returns>
        protected virtual int GetHashCode(AssemblySlim assembly) => GetHashCode(assembly?.Name);

        private ExpressionHashingVisitor CreateHashingVisitor() => new(this, _memoizer);

        private sealed class ExpressionHashingVisitor : ExpressionSlimVisitor<int, int, int, int, int, int, int, int, int, int, int, int>, IClearable
        {
            private readonly ExpressionSlimHasher _parent;

            private readonly Stack<IList<ParameterExpressionSlim>> _environment = new();

            private readonly TypeSlimHashingVisitor _typeHasher;
            private readonly MemberInfoSlimHashingVisitor _memberHasher;

            private readonly IMemoizedDelegate<Func<ExpressionSlim, int>> _visitCache;

            public ExpressionHashingVisitor(ExpressionSlimHasher parent, IMemoizer memoizer)
            {
                _parent = parent;

                _typeHasher = new TypeSlimHashingVisitor(parent, memoizer);
                _memberHasher = new MemberInfoSlimHashingVisitor(parent, _typeHasher, memoizer);

                // NB: We use reference equality for memoization in order to prevent the computation of another
                //     hash code through the overridden GetHashCode implementation on ExpressionSlim. Note that
                //     this memoization has relatively low overhead and is only effective when many expression
                //     trees from a shared expression forest are visited, so reference equal nodes could occur.

                _visitCache = memoizer.Memoize(base.Visit, MemoizationOptions.None, ReferenceEqualityComparer<ExpressionSlim>.Instance);
            }

            public void Clear()
            {
                // CONSIDER: Add a mode where the memoization caches are cleared conditionally.

                _visitCache.Cache.Clear();
                _environment.Clear();
                _typeHasher.Clear();
                _memberHasher.Clear();
            }

            public override int Visit(ExpressionSlim node)
            {
                // NB: We don't cache the result of computing a hash code for an expression tree in case it could
                //     be dependent on the binding of variables. Note that all labels are treated as global and
                //     don't get tracked in an environment. This is done because we don't perform "label scope"
                //     tracking because the rules for binding to label targets are quite complex and definitions
                //     of labels can occur after their use sites. We choose the conservative route here and decide
                //     not to cache in case an expression *could* be dependent on bound variables. Note that global
                //     variables are always hashed by their name of identity, so these result in stable context-
                //     independent hashes. We will only consult the cache or add items to the cache in a context-
                //     free environment.

                if (_environment.Count == 0)
                {
                    return _visitCache.Delegate(node);
                }
                else
                {
                    return base.Visit(node);
                }
            }

            private int MakeParameterDeclarations(IList<ParameterExpressionSlim> parameters)
            {
                var res = 0;

                for (int i = 0, n = parameters.Count; i < n; i++)
                {
                    res = Combine(res, MakeParameterDeclaration(parameters[i]));
                }

                return res;
            }

            private int MakeParameterDeclaration(ParameterExpressionSlim parameter) => Combine((int)ExpressionType.Parameter, Visit(parameter?.Type)); /* NB: omitting name for declaration sites of bound parameters */

            protected override int MakeBinary(BinaryExpressionSlim node, int left, int conversion, int right) => Combine((int)node.NodeType, left, conversion, right, Visit(node.Method), node.IsLiftedToNull ? 1 : 0);

            protected override int VisitBlock(BlockExpressionSlim node)
            {
                if (node.Variables.Count > 0)
                {
                    _environment.Push(node.Variables);
                }

                var variables = MakeParameterDeclarations(node.Variables);
                var expressions = Visit(node.Expressions);

                if (node.Variables.Count > 0)
                {
                    _environment.Pop();
                }

                return Combine((int)node.NodeType, variables, expressions, Visit(node.Type));
            }

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeBlock(BlockExpressionSlim node, ReadOnlyCollection<int> variables, ReadOnlyCollection<int> expressions) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitCatchBlock(CatchBlockSlim node)
            {
                if (node.Variable != null)
                {
                    _environment.Push(new[] { node.Variable });
                }

                var variable = MakeParameterDeclaration(node.Variable);
                var body = Visit(node.Body);
                var filter = Visit(node.Filter);

                if (node.Variable != null)
                {
                    _environment.Pop();
                }

                return MakeCatchBlock(node, variable, body, filter);
            }

            protected override int MakeCatchBlock(CatchBlockSlim node, int variable, int body, int filter) => Combine(variable, Visit(node.Test), body, filter);

            protected override int MakeConditional(ConditionalExpressionSlim node, int test, int ifTrue, int ifFalse) => Combine((int)node.NodeType, test, ifTrue, ifFalse, Visit(node.Type));

            protected override int MakeConstant(ConstantExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Type), _parent.GetHashCode(node.Value));

            protected override int MakeDefault(DefaultExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Type));

            protected override int VisitElementInit(ElementInitSlim node) => Combine(Visit(node.AddMethod), Visit(node.Arguments));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeElementInit(ElementInitSlim node, ReadOnlyCollection<int> arguments) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int MakeGoto(GotoExpressionSlim node, int target, int value) => Combine((int)node.NodeType, (int)node.Kind, target, value, Visit(node.Type));

            protected override int VisitIndex(IndexExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Object), Visit(node.Arguments), Visit(node.Indexer));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeIndex(IndexExpressionSlim node, int @object, ReadOnlyCollection<int> arguments) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitInvocation(InvocationExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Expression), Visit(node.Arguments));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeInvocation(InvocationExpressionSlim node, int expression, ReadOnlyCollection<int> arguments) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int MakeLabel(LabelExpressionSlim node, int target, int defaultValue) => Combine((int)node.NodeType, target, defaultValue);

            protected override int MakeLabelTarget(LabelTargetSlim node) => Combine(Visit(node.Type), _parent.GetHashCode(node));

            protected override int VisitLambda(LambdaExpressionSlim node)
            {
                if (node.Parameters.Count > 0)
                {
                    _environment.Push(node.Parameters);
                }

                var body = Visit(node.Body);
                var parameters = MakeParameterDeclarations(node.Parameters);

                if (node.Parameters.Count > 0)
                {
                    _environment.Pop();
                }

                return Combine((int)node.NodeType, body, parameters, Visit(node.DelegateType));
            }

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeLambda(LambdaExpressionSlim node, int body, ReadOnlyCollection<int> parameters) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitListInit(ListInitExpressionSlim node) => Combine((int)node.NodeType, Visit(node.NewExpression), Visit(node.Initializers, VisitElementInit));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeListInit(ListInitExpressionSlim node, int newExpression, ReadOnlyCollection<int> initializers) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int MakeLoop(LoopExpressionSlim node, int body, int breakLabel, int continueLabel) => Combine((int)node.NodeType, body, breakLabel, continueLabel);

            protected override int MakeMember(MemberExpressionSlim node, int expression) => Combine((int)node.NodeType, expression, Visit(node.Member));

            protected override int MakeMemberAssignment(MemberAssignmentSlim node, int expression) => Combine((int)node.BindingType, expression, Visit(node.Member));

            protected override int VisitMemberInit(MemberInitExpressionSlim node) => Combine((int)node.NodeType, Visit(node.NewExpression), Visit(node.Bindings, VisitMemberBinding));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeMemberInit(MemberInitExpressionSlim node, int newExpression, ReadOnlyCollection<int> bindings) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitMemberListBinding(MemberListBindingSlim node) => Combine((int)node.BindingType, Visit(node.Member), Visit(node.Initializers, VisitElementInit));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeMemberListBinding(MemberListBindingSlim node, ReadOnlyCollection<int> initializers) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitMemberMemberBinding(MemberMemberBindingSlim node) => Combine((int)node.BindingType, Visit(node.Member), Visit(node.Bindings, VisitMemberBinding));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeMemberMemberBinding(MemberMemberBindingSlim node, ReadOnlyCollection<int> bindings) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitMethodCall(MethodCallExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Object), Visit(node.Arguments), Visit(node.Method));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeMethodCall(MethodCallExpressionSlim node, int @object, ReadOnlyCollection<int> arguments) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitNew(NewExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Arguments), Visit(node.Constructor), Visit(node.Type), Visit(node.Members, Visit));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeNew(NewExpressionSlim node, ReadOnlyCollection<int> arguments) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitNewArray(NewArrayExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Expressions), Visit(node.ElementType));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeNewArray(NewArrayExpressionSlim node, ReadOnlyCollection<int> expressions) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int MakeParameter(ParameterExpressionSlim node)
            {
                var i = 0;
                foreach (var scope in _environment)
                {
                    for (int j = 0, n = scope.Count; j < n; j++)
                    {
                        if (node == scope[j])
                        {
                            return Combine(i, j);
                        }
                    }

                    i++;
                }

                return Combine((int)node.NodeType, Visit(node.Type), _parent.GetHashCode(node));
            }

            protected override int VisitSwitch(SwitchExpressionSlim node) => Combine((int)node.NodeType, Visit(node.SwitchValue), Visit(node.DefaultBody), Visit(node.Cases, VisitSwitchCase), Visit(node.Type), Visit(node.Comparison));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeSwitch(SwitchExpressionSlim node, int switchValue, int defaultBody, ReadOnlyCollection<int> cases) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitSwitchCase(SwitchCaseSlim node) => Combine(Visit(node.Body), Visit(node.TestValues));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeSwitchCase(SwitchCaseSlim node, int body, ReadOnlyCollection<int> testValues) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitTry(TryExpressionSlim node) => Combine((int)node.NodeType, Visit(node.Body), Visit(node.Finally), Visit(node.Fault), Visit(node.Handlers, VisitCatchBlock), Visit(node.Type));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeTry(TryExpressionSlim node, int body, int @finally, int fault, ReadOnlyCollection<int> handlers) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int MakeTypeBinary(TypeBinaryExpressionSlim node, int expression) => Combine((int)node.NodeType, expression, Visit(node.TypeOperand));

            protected override int MakeUnary(UnaryExpressionSlim node, int operand) => Combine((int)node.NodeType, operand, Visit(node.Method), Visit(node.Type));

            private new int Visit<T>(ReadOnlyCollection<T> elements)
                where T : ExpressionSlim
            {
                var res = 0;

                if (elements != null)
                {
                    for (int i = 0, n = elements.Count; i < n; i++)
                    {
                        res = Combine(res, Visit(elements[i]));
                    }
                }

                return res;
            }

            private static int Visit<T>(ReadOnlyCollection<T> elements, Func<T, int> visit)
            {
                var res = 0;

                if (elements != null)
                {
                    for (int i = 0, n = elements.Count; i < n; i++)
                    {
                        res = Combine(res, visit(elements[i]));
                    }
                }

                return res;
            }

            private int Visit(TypeSlim type) => type == null ? 0 : _typeHasher.Visit(type);

            private int Visit(MemberInfoSlim member) => member == null ? 0 : _memberHasher.Visit(member);
        }

        private sealed class MemberInfoSlimHashingVisitor : MemberInfoSlimVisitor<int, int, int, int, int, int, int, int>
        {
            private readonly ExpressionSlimHasher _parent;

            private readonly TypeSlimHashingVisitor _typeHasher;
            private readonly IMemoizedDelegate<Func<MemberInfoSlim, int>> _visitCache;

            public MemberInfoSlimHashingVisitor(ExpressionSlimHasher parent, TypeSlimHashingVisitor typeHasher, IMemoizer memoizer)
            {
                _parent = parent;
                _typeHasher = typeHasher;

                // NB: We use reference equality for memoization in order to prevent the computation of another
                //     hash code through the overridden GetHashCode implementation on MemberInfoSlim. The use
                //     of reference equality is very effective within expressions and for reflection objects that
                //     come from a shared reflection context where reference-equal objects are returned.

                _visitCache = memoizer.Memoize(base.Visit, MemoizationOptions.None, ReferenceEqualityComparer<MemberInfoSlim>.Instance);
            }

            public void Clear()
            {
                // CONSIDER: Add a mode where the memoization caches are cleared conditionally.

                _visitCache.Cache.Clear();
            }

            // NB: Hashing of members is independent of any context. This assumption would change in case members
            //     could have references to generic parameters of defining types. However, we don't deal with open
            //     generic declaring types for members in the context of expression trees, so we can memoize just
            //     fine over here.

            public override int Visit(MemberInfoSlim member) => _visitCache.Delegate(member);

            protected override int MakeGenericMethod(GenericMethodInfoSlim method, int methodDefinition) => Combine((int)method.MemberType, methodDefinition, Visit(method.GenericArguments));

            protected override int VisitConstructor(ConstructorInfoSlim constructor) => Combine((int)constructor.MemberType, Visit(constructor.DeclaringType), Visit(constructor.ParameterTypes));

            protected override int VisitField(FieldInfoSlim field) => Combine((int)field.MemberType, Visit(field.DeclaringType), Visit(field.FieldType), Visit(field.Name));

            protected override int VisitGenericDefinitionMethod(GenericDefinitionMethodInfoSlim method)
            {
                _typeHasher.Push(method.GenericParameterTypes);

                var arity = method.GenericParameterTypes.Count;

                var res = Combine((int)method.MemberType, Visit(method.DeclaringType), Visit(method.ReturnType), Visit(method.Name), arity, Visit(method.ParameterTypes));

                _typeHasher.Pop();

                return res;
            }

            protected override int VisitProperty(PropertyInfoSlim property) => Combine((int)property.MemberType, Visit(property.DeclaringType), Visit(property.PropertyType), Visit(property.Name), Visit(property.IndexParameterTypes));

            protected override int VisitSimpleMethod(SimpleMethodInfoSlim method) => Combine((int)method.MemberType, Visit(method.DeclaringType), Visit(method.ReturnType), Visit(method.Name), Visit(method.ParameterTypes));

            private int Visit(TypeSlim type) => type == null ? 0 : _typeHasher.Visit(type);

            private int Visit(ReadOnlyCollection<TypeSlim> types) => _typeHasher.Visit(types);

            private int Visit(string s) => _parent.GetHashCode(s);
        }

        private sealed class TypeSlimHashingVisitor : TypeSlimVisitor<int, int, int, int, int, int, int>, IClearable
        {
            private readonly ExpressionSlimHasher _parent;

            private readonly StrongBox<ReadOnlyCollection<TypeSlim>> _environment = new();

            private readonly IMemoizedDelegate<Func<TypeSlim, int>> _visitCache;

            public TypeSlimHashingVisitor(ExpressionSlimHasher parent, IMemoizer memoizer)
            {
                _parent = parent;
                _visitCache = memoizer.Memoize(base.Visit, MemoizationOptions.None, ReferenceEqualityComparer<TypeSlim>.Instance);
            }

            public void Clear()
            {
                // CONSIDER: Add a mode where the memoization caches are cleared conditionally.

                _visitCache.Cache.Clear();
                _environment.Value = null;
            }

            public override int Visit(TypeSlim type)
            {
                // NB: Hashing of types can be dependent on bound generic parameters. We take a conservative approach
                //     and decide not to cache in case any such bound parameter is present. For use with expression
                //     trees, this only occurs for closed generic methods when visiting their parameter types and their
                //     return type.

                if (_environment.Value == null)
                {
                    return _visitCache.Delegate(type);
                }
                else
                {
                    return base.Visit(type);
                }
            }

            public void Push(ReadOnlyCollection<TypeSlim> typeParameters)
            {
                Debug.Assert(_environment.Value == null);
                _environment.Value = typeParameters;
            }

            public void Pop()
            {
                _environment.Value = null;
            }

            protected override int MakeArrayType(ArrayTypeSlim type, int elementType, int? rank) => Combine((int)type.Kind, elementType, rank ?? -1);

            protected override int VisitGeneric(GenericTypeSlim type) => Combine((int)type.Kind, Visit(type.GenericTypeDefinition), Visit(type.GenericArguments));

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeGeneric(GenericTypeSlim type, int typeDefinition, ReadOnlyCollection<int> arguments) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int MakeGenericDefinition(GenericDefinitionTypeSlim type) => Combine((int)type.Kind, Visit(type.Name), MakeAssembly(type.Assembly));

            protected override int VisitStructural(StructuralTypeSlim type)
            {
                var res = Combine((int)type.Kind, (int)type.StructuralKind);

                var properties = type.Properties;

                for (int i = 0, n = properties.Count; i < n; i++)
                {
                    var property = properties[i];

                    res = Combine(res, Visit(property.Name), Visit(property.PropertyType), Visit(property.IndexParameterTypes));
                }

                return res;
            }

            [ExcludeFromCodeCoverage] // Unreachable code
            protected override int MakeStructuralType(StructuralTypeSlim type, IEnumerable<KeyValuePair<PropertyInfoSlim, int>> propertyTypes, IEnumerable<KeyValuePair<PropertyInfoSlim, ReadOnlyCollection<int>>> propertyIndexParameters) { throw new NotImplementedException("More efficient implementation provided through Visit method."); }

            protected override int VisitGenericParameter(GenericParameterTypeSlim type)
            {
                var scope = _environment.Value;

                if (scope != null)
                {
                    for (int j = 0, n = scope.Count; j < n; j++)
                    {
                        if (type == scope[j])
                        {
                            return Combine(0, j); // NB: Using a De Bruijn scheme to encode a scope and an index.
                        }
                    }
                }

                return Combine((int)type.Kind, _parent.GetHashCode(type.Name));
            }

            protected override int VisitSimple(SimpleTypeSlim type) => Combine((int)type.Kind, Visit(type.Name), MakeAssembly(type.Assembly));

            private int MakeAssembly(AssemblySlim assembly) => _parent.GetHashCode(assembly);

            public new int Visit(ReadOnlyCollection<TypeSlim> elements)
            {
                var res = 0;

                if (elements != null)
                {
                    for (int i = 0, n = elements.Count; i < n; i++)
                    {
                        res = Combine(res, Visit(elements[i]));
                    }
                }

                return res;
            }

            private int Visit(string s) => _parent.GetHashCode(s);
        }
    }
}
