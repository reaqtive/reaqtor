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

namespace SampleTrees.Arithmetic
{
    public class ArithNodeType(string name) : IEqualityComparer<ArithNodeType>
    {
        public static readonly ArithNodeType Const = new("Const");
        public static readonly ArithNodeType Lazy = new("Lazy");
        public static readonly ArithNodeType Add = new("Add");
        public static readonly ArithNodeType Mul = new("Mul");
        public static readonly ArithNodeType Negate = new("Neg");
        public static readonly ArithNodeType Absolute = new("Abs");
        public static readonly ArithNodeType Wildcard = new("Wildcard");

        public string Name { get; } = name;

        public bool Equals(ArithNodeType x, ArithNodeType y) => ReferenceEquals(x, y);

        public int GetHashCode(ArithNodeType obj) => obj.Name.GetHashCode();

        public override string ToString() => Name;
    }

    internal class ArithType : IType
    {
        public static readonly ArithType Instance = new();

        private ArithType()
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

        public bool Equals(IType other) => other is ArithType;

        public override string ToString() => "|N";
    }

    public abstract class ArithExpr(ArithNodeType type, params ArithExpr[] children) : Tree<ArithNodeType>(type, children), ITyped
    {
        public abstract int Eval();

        public new IType GetType() => ArithType.Instance;

        protected sealed override ITree<ArithNodeType> UpdateCore(IEnumerable<ITree<ArithNodeType>> children) => Update(children.Cast<ArithExpr>());

        protected abstract ArithExpr Update(IEnumerable<ArithExpr> children);
    }

    public class ArithWildcardFactory : IWildcardFactory<ArithExpr>
    {
        public ArithExpr CreateWildcard(ParameterExpression hole)
        {
            return new ArithWildcard(hole.Name);
        }
    }

    public class ArithWildcard(string name) : ArithExpr(ArithNodeType.Wildcard)
    {
        public string Name { get; } = name;

        public override string ToStringFormat() => Name;

        public override int Eval() => throw new NotImplementedException();

        protected override ArithExpr Update(IEnumerable<ArithExpr> children) => throw new NotImplementedException();
    }

    public class Const(int value) : ArithExpr(ArithNodeType.Const)
    {
        public new int Value { get; private set; } = value;

        public override string ToStringFormat() => "Const(" + Value + ")";

        public override int GetHashCode() => base.Value.GetHashCode() ^ Value;

        public override bool Equals(object obj) => obj is Const c && c.Value == Value;

        public override int Eval() => Value;

        protected override ArithExpr Update(IEnumerable<ArithExpr> children)
        {
            Debug.Assert(!children.Any());
            return new Const(Value);
        }
    }

    public class Lazy(Expression<Func<int>> eval) : ArithExpr(ArithNodeType.Lazy)
    {
        public Expression<Func<int>> EvalFunc { get; private set; } = eval;

        public override string ToStringFormat() => "Lazy(" + EvalFunc + ")";

        public override int GetHashCode() => Value.GetHashCode() ^ EvalFunc.GetHashCode();

        public override bool Equals(object obj) => false;

        public override int Eval() => EvalFunc.Compile()();

        protected override ArithExpr Update(IEnumerable<ArithExpr> children)
        {
            Debug.Assert(!children.Any());
            return new Lazy(EvalFunc);
        }
    }

    public abstract class UnaryArithExpr(ArithExpr operand, ArithNodeType type) : ArithExpr(type, operand)
    {
        public ArithExpr Operand => (ArithExpr)Children[0];

        public override string ToStringFormat() => Value.Name + "({0})";

        public override int GetHashCode() => Value.GetHashCode() ^ Operand.GetHashCode();

        public override bool Equals(object obj) => obj is UnaryArithExpr b && b.Value.Equals(Value) && b.Operand.Equals(Operand);
    }

    public class Neg(ArithExpr operand) : UnaryArithExpr(operand, ArithNodeType.Negate)
    {
        public override int Eval() => -Operand.Eval();

        protected override ArithExpr Update(IEnumerable<ArithExpr> children)
        {
            Debug.Assert(children.Count() == 1);
            return new Neg(children.Single());
        }
    }

    public class Abs(ArithExpr operand) : UnaryArithExpr(operand, ArithNodeType.Absolute)
    {
        public override int Eval() => Math.Abs(Operand.Eval());

        protected override ArithExpr Update(IEnumerable<ArithExpr> children)
        {
            Debug.Assert(children.Count() == 1);
            return new Abs(children.Single());
        }
    }

    public abstract class BinaryArithExpr(ArithExpr left, ArithExpr right, ArithNodeType type) : ArithExpr(type, left, right)
    {
        public ArithExpr Left => (ArithExpr)Children[0];

        public ArithExpr Right => (ArithExpr)Children[1];

        public override string ToStringFormat() => Value.Name + "({0}, {1})";

        public override int GetHashCode() => Value.GetHashCode() ^ Left.GetHashCode() ^ Right.GetHashCode();

        public override bool Equals(object obj) => obj is BinaryArithExpr b && b.Value.Equals(Value) && b.Left.Equals(Left) && b.Right.Equals(Right);
    }

    public class Add(ArithExpr left, ArithExpr right) : BinaryArithExpr(left, right, ArithNodeType.Add)
    {
        public override int Eval() => Left.Eval() + Right.Eval();

        protected override ArithExpr Update(IEnumerable<ArithExpr> children)
        {
            Debug.Assert(children.Count() == 2);
            return new Add(children.ElementAt(0), children.ElementAt(1));
        }
    }

    public class Mul(ArithExpr left, ArithExpr right) : BinaryArithExpr(left, right, ArithNodeType.Mul)
    {
        public override int Eval() => Left.Eval() * Right.Eval();

        protected override ArithExpr Update(IEnumerable<ArithExpr> children)
        {
            Debug.Assert(children.Count() == 2);
            return new Mul(children.ElementAt(0), children.ElementAt(1));
        }
    }
}
