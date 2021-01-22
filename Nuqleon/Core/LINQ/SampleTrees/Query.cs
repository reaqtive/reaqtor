// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace SampleTrees.Query
{
    public class QueryNodeType : IEqualityComparer<QueryNodeType>
    {
        public static readonly QueryNodeType Empty = new("Empty");
        public static readonly QueryNodeType Quote = new("Quote");
        public static readonly QueryNodeType Constant = new("Constant");
        public static readonly QueryNodeType Lazy = new("Lazy");
        public static readonly QueryNodeType Source = new("Source");
        public static readonly QueryNodeType Filter = new("Filter");
        public static readonly QueryNodeType Projection = new("Projection");
        public static readonly QueryNodeType Top = new("Top");
        public static readonly QueryNodeType Wildcard = new("Wildcard");

        public QueryNodeType(string name) => Name = name;

        public string Name { get; }

        public bool Equals(QueryNodeType x, QueryNodeType y) => ReferenceEquals(x, y);

        public int GetHashCode(QueryNodeType obj) => obj.Name.GetHashCode();

        public override string ToString() => Name;
    }

    internal class QueryType : IType
    {
        public static readonly QueryType Instance = new();

        private QueryType()
        {
        }

        public bool IsAssignableTo(IType type) => CompareTo(type) >= 0;

        public int CompareTo(IType other)
        {
            if (Equals(other))
                return 0;
            else
                throw new InvalidOperationException("Type check across type systems.");
        }

        public bool Equals(IType other) => other is QueryType;

        public override string ToString() => "|N";
    }

    public abstract class QueryExpr : Tree<QueryNodeType>, ITyped
    {
        public QueryExpr(QueryNodeType type, params ITree<QueryNodeType>[] children)
            : base(type, children)
        {
        }

        public QueryExpr(QueryNodeType type, IEnumerable<ITree<QueryNodeType>> children)
            : base(type, children)
        {
        }

        public new IType GetType() => QueryType.Instance;

        protected sealed override ITree<QueryNodeType> UpdateCore(IEnumerable<ITree<QueryNodeType>> children) => Update(children.Cast<QueryExpr>());

        protected abstract QueryExpr Update(IEnumerable<QueryExpr> children);
    }

    public class QueryWildcardFactory : IWildcardFactory<QueryExpr>
    {
        public QueryExpr CreateWildcard(ParameterExpression hole)
        {
            return new QueryWildcard(hole.Name);
        }
    }

    public class QueryWildcard : QueryExpr
    {
        public QueryWildcard(string name)
            : base(QueryNodeType.Wildcard)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToStringFormat() => Name;

        protected override QueryExpr Update(IEnumerable<QueryExpr> children) => throw new NotImplementedException();
    }

    public class NullaryQueryExpr : QueryExpr
    {
        public NullaryQueryExpr(QueryNodeType nodeType)
            : base(nodeType)
        {
        }

        protected override QueryExpr Update(IEnumerable<QueryExpr> children) => this;
    }

    public class Empty : NullaryQueryExpr
    {
        public static readonly Empty Instance = new();

        public Empty()
            : base(QueryNodeType.Empty)
        {
        }

        public override string ToStringFormat() => "Empty";
    }

    public class Source : NullaryQueryExpr
    {
        public Source(IQueryable queryable)
            : base(QueryNodeType.Source)
        {
            Queryable = queryable;
        }

        public IQueryable Queryable { get; }

        public override string ToStringFormat() => "Source(" + (Queryable != null ? Queryable.ToString() : "<unbound>") + ")";
    }

    public class Constant : NullaryQueryExpr
    {
        public Constant(ConstantExpression expression)
            : base(QueryNodeType.Constant)
        {
            Expression = expression;
        }

        public ConstantExpression Expression { get; }

        public override string ToStringFormat() => Expression.ToString();
    }

    public class Funclet : QueryExpr
    {
        public Funclet(Expression<Func<object>> eval)
            : base(QueryNodeType.Lazy)
        {
            EvalFunc = eval;
        }

        public Expression<Func<object>> EvalFunc { get; }

        public override string ToStringFormat() => "Lazy(" + EvalFunc + ")";

        public override int GetHashCode() => Value.GetHashCode() ^ EvalFunc.GetHashCode();

        public override bool Equals(object obj) => false;

        protected override QueryExpr Update(IEnumerable<QueryExpr> children)
        {
            Debug.Assert(!children.Any());
            return new Funclet(EvalFunc);
        }
    }

    public class Quote : NullaryQueryExpr
    {
        public Quote(LambdaExpression lambda)
            : base(QueryNodeType.Quote)
        {
            Lambda = lambda;
        }

        public LambdaExpression Lambda { get; }

        public override string ToStringFormat() => Lambda.ToString();
    }

    public abstract class UnaryQueryExpr : QueryExpr
    {
        public UnaryQueryExpr(QueryNodeType nodeType, QueryExpr source, params QueryExpr[] arguments)
            : base(nodeType, new[] { source }.Concat(arguments).ToArray())
        {
            Source = source;
        }

        public QueryExpr Source { get; }

        public override string ToStringFormat() => Value.ToString() + "(" + string.Join(", ", Enumerable.Range(0, Children.Count).Select(i => "{" + i + "}").ToArray()) + ")";

        public override string ToString() => string.Format("{0}.{1}({2})", Children.ElementAt(0), Value, string.Join(", ", Children.Skip(1).Select(c => c.ToString()).ToArray()));

        protected abstract IEnumerable<string> GetParametersToString();
    }

    public class Filter : UnaryQueryExpr
    {
        public Filter(QueryExpr source, QueryExpr predicate)
            : base(QueryNodeType.Filter, source, predicate)
        {
        }

        public LambdaExpression Predicate => ((Quote)Children.ElementAt(1)).Lambda;

        protected override QueryExpr Update(IEnumerable<QueryExpr> children)
        {
            Debug.Assert(children.Count() == 2);
            return new Filter(children.ElementAt(0), (Quote)children.ElementAt(1));
        }

        protected override IEnumerable<string> GetParametersToString()
        {
            yield return Predicate.ToString();
        }
    }

    public class Projection : UnaryQueryExpr
    {
        public Projection(QueryExpr source, QueryExpr selector)
            : base(QueryNodeType.Projection, source, selector)
        {
        }

        public LambdaExpression Selector => ((Quote)Children.ElementAt(1)).Lambda;

        protected override QueryExpr Update(IEnumerable<QueryExpr> children)
        {
            Debug.Assert(children.Count() == 2);
            return new Filter(children.ElementAt(0), (Quote)children.ElementAt(1));
        }

        protected override IEnumerable<string> GetParametersToString()
        {
            yield return Selector.ToString();
        }
    }

    public class Top : UnaryQueryExpr
    {
        public Top(QueryExpr source, Constant count)
            : base(QueryNodeType.Top, source, count)
        {
        }

        public int Count => (int)((Constant)Children.ElementAt(1)).Expression.Value;

        protected override QueryExpr Update(IEnumerable<QueryExpr> children)
        {
            Debug.Assert(children.Count() == 2);
            return new Top(children.ElementAt(0), (Constant)children.ElementAt(1));
        }

        protected override IEnumerable<string> GetParametersToString()
        {
            yield return Count.ToString();
        }
    }
}
