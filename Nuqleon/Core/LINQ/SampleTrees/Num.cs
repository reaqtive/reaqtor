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
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace SampleTrees.Numerical
{
    public class NumNodeType : IEqualityComparer<NumNodeType>
    {
        public static readonly NumNodeType Val = new("Val");
        public static readonly NumNodeType Plus = new("Plus");
        public static readonly NumNodeType Times = new("Times");
        public static readonly NumNodeType Inc = new("Inc");
        public static readonly NumNodeType TimesPlus = new("TimesPlus");
        public static readonly NumNodeType Wildcard = new("Wildcard");

        public NumNodeType(string name) => Name = name;

        public string Name { get; }

        public bool Equals(NumNodeType x, NumNodeType y) => ReferenceEquals(x, y);

        public int GetHashCode(NumNodeType obj) => obj.Name.GetHashCode();

        public override string ToString() => Name;
    }

    public abstract class NumExpr : Tree<NumNodeType>
    {
        protected NumExpr(NumNodeType type, params NumExpr[] children)
            : base(type, children)
        {
        }

        public abstract int Eval();
    }

    public class NumWildcardFactory : IWildcardFactory<NumExpr>
    {
        public NumExpr CreateWildcard(ParameterExpression hole) => new NumWildcard(hole.Name);
    }

    public class NumWildcard : NumExpr
    {
        public NumWildcard(string name)
            : base(NumNodeType.Wildcard)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToStringFormat() => Name;

        public override int Eval() => throw new NotImplementedException();
    }

    public class Val : NumExpr
    {
        public static readonly Val Zero = new(0);
        public static readonly Val One = new(1);

        public Val(int value)
            : base(NumNodeType.Val)
        {
            Value = value;
        }

        public new int Value { get; }

        public override string ToStringFormat() => Value.ToString();

        public override int Eval() => Value;
    }

    public class Inc : NumExpr
    {
        public Inc(NumExpr operand)
            : base(NumNodeType.Inc, operand)
        {
            Operand = operand;
        }

        public NumExpr Operand { get; }

        public override string ToStringFormat() => "Inc({0})";

        public override int Eval() => Operand.Eval() + 1;
    }

    public abstract class BinNumExpr : NumExpr
    {
        public BinNumExpr(NumNodeType type, NumExpr left, NumExpr right)
            : base(type, left, right)
        {
        }

        public NumExpr Left => (NumExpr)Children[0];

        public NumExpr Right => (NumExpr)Children[1];
    }

    public class Plus : BinNumExpr
    {
        public Plus(NumExpr left, NumExpr right)
            : base(NumNodeType.Plus, left, right)
        {
        }

        public override string ToStringFormat() => "Plus({0}, {1})";

        public override int Eval() => Left.Eval() + Right.Eval();
    }

    public class Times : BinNumExpr
    {
        public Times(NumExpr left, NumExpr right)
            : base(NumNodeType.Times, left, right)
        {
        }

        public override string ToStringFormat() => "Times({0}, {1})";

        public override int Eval() => Left.Eval() * Right.Eval();
    }

    public class TimesPlus : NumExpr
    {
        public TimesPlus(NumExpr first, NumExpr second, NumExpr third)
            : base(NumNodeType.TimesPlus, first, second, third)
        {
        }

        public NumExpr First => (NumExpr)Children[0];

        public NumExpr Second => (NumExpr)Children[1];

        public NumExpr Third => (NumExpr)Children[2];

        public override string ToStringFormat() => "TimesPlus({0}, {1}, {2})";

        public override int Eval() => First.Eval() * Second.Eval() + Third.Eval();
    }
}
