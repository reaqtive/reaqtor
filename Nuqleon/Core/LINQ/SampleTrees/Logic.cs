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

namespace SampleTrees.Logic
{
    public class LogicNodeType : IEqualityComparer<LogicNodeType>
    {
        public static readonly LogicNodeType Const = new("Const");
        public static readonly LogicNodeType Symbol = new("Symbol");
        public static readonly LogicNodeType Not = new("Not");
        public static readonly LogicNodeType And = new("And");
        public static readonly LogicNodeType Or = new("Or");
        public static readonly LogicNodeType Wildcard = new("Wildcard");

        public LogicNodeType(string name) => Name = name;

        public string Name { get; }

        public bool Equals(LogicNodeType x, LogicNodeType y) => ReferenceEquals(x, y);

        public int GetHashCode(LogicNodeType obj) => obj.Name.GetHashCode();

        public override string ToString() => Name;
    }

    public class LogicType : IType
    {
        public static readonly LogicType Instance = new();

        private LogicType()
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

        public bool Equals(IType other) => other is LogicType;

        public override string ToString() => "|B";
    }

    public abstract class LogicExpr : Tree<LogicNodeType>, ITyped
    {
        public LogicExpr(LogicNodeType type, params LogicExpr[] children)
            : base(type, children)
        {
        }

        public abstract bool Eval(Dictionary<BoolSymbol, BoolConst> bindings);

        public bool Eval() => Eval(new Dictionary<BoolSymbol, BoolConst>());

        public static Not operator !(LogicExpr expr) => new(expr);

        public static And operator &(LogicExpr left, LogicExpr right) => new(left, right);

        public static Or operator |(LogicExpr left, LogicExpr right) => new(left, right);

        public new IType GetType() => LogicType.Instance;

        public override int GetHashCode() => base.Value.GetHashCode();

        protected sealed override ITree<LogicNodeType> UpdateCore(IEnumerable<ITree<LogicNodeType>> children) => Update(children.Cast<LogicExpr>());

        protected abstract LogicExpr Update(IEnumerable<LogicExpr> children);
    }

    public class LogicWildcardFactory : IWildcardFactory<LogicExpr>
    {
        public LogicExpr CreateWildcard(ParameterExpression hole) => new LogicWildcard(hole.Name);
    }

    public class LogicWildcard : LogicExpr
    {
        public LogicWildcard(string name)
            : base(LogicNodeType.Wildcard)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToStringFormat() => Name;

        public override bool Eval(Dictionary<BoolSymbol, BoolConst> bindings) => throw new NotImplementedException();

        protected override LogicExpr Update(IEnumerable<LogicExpr> children) => throw new NotImplementedException();
    }

    public class BoolConst : LogicExpr
    {
        public static readonly BoolConst True = new(true);
        public static readonly BoolConst False = new(false);

        private BoolConst(bool value)
            : base(LogicNodeType.Const)
        {
            Value = value;
        }

        public new bool Value { get; }

        public override int GetHashCode() => base.GetHashCode() ^ (Value ? 1 : 0);

        public override bool Equals(object obj) => obj is BoolConst other && other.Value == Value;

        public override string ToStringFormat() => Value.ToString();

        public override bool Eval(Dictionary<BoolSymbol, BoolConst> bindings) => Value;

        protected override LogicExpr Update(IEnumerable<LogicExpr> children)
        {
            Debug.Assert(!children.Any());
            return new BoolConst(Value);
        }
    }

    public class BoolSymbol : LogicExpr
    {
        private BoolSymbol(string name)
            : base(LogicNodeType.Symbol)
        {
            Name = name;
        }

        public string Name { get; }

        public override int GetHashCode() => base.GetHashCode() ^ Name.GetHashCode();

        public override bool Equals(object obj) => obj is BoolSymbol other && other.Name == Name;

        public override string ToStringFormat() => Name;

        public override bool Eval(Dictionary<BoolSymbol, BoolConst> bindings)
        {
            if (!bindings.TryGetValue(this, out BoolConst value))
                throw new InvalidOperationException("Boolean expression contains unknown symbol: " + Name);

            return value.Eval(bindings);
        }

        protected override LogicExpr Update(IEnumerable<LogicExpr> children)
        {
            Debug.Assert(!children.Any());
            return new BoolSymbol(Name);
        }
    }

    public class Not : LogicExpr
    {
        public Not(LogicExpr operand)
            : base(LogicNodeType.Not, operand)
        {
        }

        public LogicExpr Operand => (LogicExpr)Children[0];

        public override int GetHashCode() => base.GetHashCode() ^ Operand.GetHashCode();

        public override bool Equals(object obj) => obj is Not other && other.Operand.Equals(Operand);

        public override bool Eval(Dictionary<BoolSymbol, BoolConst> bindings) => !Operand.Eval(bindings);

        protected override LogicExpr Update(IEnumerable<LogicExpr> children)
        {
            Debug.Assert(children.Count() == 1);
            return new Not(children.Single());
        }
    }

    public abstract class BinaryLogicExpr : LogicExpr
    {
        public BinaryLogicExpr(LogicExpr left, LogicExpr right, LogicNodeType type)
            : base(type, left, right)
        {
        }

        public LogicExpr Left => (LogicExpr)Children[0];

        public LogicExpr Right => (LogicExpr)Children[1];

        public override int GetHashCode() => base.GetHashCode() ^ Left.GetHashCode() ^ Right.GetHashCode();

        public override bool Equals(object obj) => obj is BinaryLogicExpr other && other.Value.Equals(Value) && other.Left.Equals(Left) && other.Right.Equals(Right);
    }

    public class And : BinaryLogicExpr
    {
        public And(LogicExpr left, LogicExpr right)
            : base(left, right, LogicNodeType.And)
        {
        }

        public override bool Eval(Dictionary<BoolSymbol, BoolConst> bindings) => Left.Eval(bindings) && Right.Eval(bindings);

        protected override LogicExpr Update(IEnumerable<LogicExpr> children)
        {
            Debug.Assert(children.Count() == 2);
            return new And(children.ElementAt(0), children.ElementAt(1));
        }
    }

    public class Or : BinaryLogicExpr
    {
        public Or(LogicExpr left, LogicExpr right)
            : base(left, right, LogicNodeType.Or)
        {
        }

        public override bool Eval(Dictionary<BoolSymbol, BoolConst> bindings) => Left.Eval(bindings) || Right.Eval(bindings);

        protected override LogicExpr Update(IEnumerable<LogicExpr> children)
        {
            Debug.Assert(children.Count() == 2);
            return new Or(children.ElementAt(0), children.ElementAt(1));
        }
    }
}
