// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    internal sealed class ExpressionTreeConversionWithDeBruijn : ExpressionTreeConversion
    {
        private readonly Dictionary<ParameterExpression, ExpressionTree<ParameterExpression>> _wildcardMap;
        private readonly Stack<IList<ParameterExpression>> _environment;

        public ExpressionTreeConversionWithDeBruijn() => _environment = new Stack<IList<ParameterExpression>>();

        public ExpressionTreeConversionWithDeBruijn(Dictionary<ParameterExpression, ExpressionTree<ParameterExpression>> wildcardMap)
            : this()
        {
            _wildcardMap = wildcardMap;
        }

        protected override ExpressionTree VisitLambda<T>(Expression<T> node)
        {
            _environment.Push(node.Parameters);

            var b = Visit(node.Body);

            _environment.Pop();

            var parameters = node.Parameters.Select(p => (ExpressionTree<ParameterExpression>)new ParameterDeclaration(p)).ToList().AsReadOnly();

            return MakeLambda<T>(node, b, parameters);
        }

        protected override ExpressionTree<ParameterExpression> MakeParameter(ParameterExpression node)
        {
            if (_wildcardMap != null && _wildcardMap.TryGetValue(node, out ExpressionTree<ParameterExpression> res))
            {
                return res;
            }

            var scope = 0;
            foreach (var frame in _environment)
            {
                var index = 0;

                foreach (var p in frame)
                {
                    if (p == node)
                    {
                        return new DeBruijnParameter(node, scope, index);
                    }

                    index++;
                }

                scope++;
            }

            return base.MakeParameter(node);
        }
    }

    internal sealed class DeBruijnParameter : ExpressionTree<ParameterExpression>
    {
        private readonly int _scope;
        private readonly int _index;

        public DeBruijnParameter(ParameterExpression parameter, int scope, int index)
            : base(parameter)
        {
            _scope = scope;
            _index = index;
        }

        public override bool Equals(object obj) => Equals(obj as ExpressionTreeBase);

        public override bool Equals(ExpressionTreeBase other) => other is DeBruijnParameter dbp && base.Expression.Type == dbp.Expression.Type && _scope == dbp._scope && _index == dbp._index;

        public override int GetHashCode() => base.Expression.Type.GetHashCode() << 16 + _scope << 8 + _index;

        public override string ToStringFormat() => "Parameter[@" + _scope + "." + _index + " : " + Expression.Type.ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false) + "]";
    }

    internal sealed class ParameterDeclaration : ExpressionTree<ParameterExpression>
    {
        public ParameterDeclaration(ParameterExpression parameter)
            : base(parameter)
        {
        }

        public override bool Equals(object obj) => Equals(obj as ExpressionTreeBase);

        public override bool Equals(ExpressionTreeBase other) => other is ParameterDeclaration pd && base.Expression.Type == pd.Expression.Type;

        public override int GetHashCode() => base.Expression.Type.GetHashCode();

        public override string ToStringFormat() => "Parameter[" + Expression.Type.ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false) + "]";
    }
}
