// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices.Tools.BURS
{
    internal abstract class NumTree : Tree<Num>, ITyped
    {
        public NumTree(NumKind kind)
            : base(new Num(kind))
        {
        }

        public NumTree(NumKind kind, params NumTree[] children)
            : base(new Num(kind), children)
        {
        }

        public abstract int Eval();

        IType ITyped.GetType() => NumType.Instance;
    }

    internal sealed class ConstNumTree : NumTree
    {
        public static readonly ConstNumTree Zero = new(0);
        public static readonly ConstNumTree One = new(1);

        public ConstNumTree(int value)
            : base(NumKind.Const)
        {
            Value = value;
        }

        public new int Value { get; }

        public override int Eval() => Value;

        protected override ITree<Num> UpdateCore(IEnumerable<ITree<Num>> children) => this;

        public override string ToStringFormat() => "Const(" + Value + ")";
    }

    internal sealed class BinaryNumTree : NumTree
    {
        public BinaryNumTree(NumKind kind, NumTree left, NumTree right)
            : base(kind, new[] { left, right })
        {
        }

        public override int Eval()
        {
            return Value.Kind switch
            {
                NumKind.Add => ((NumTree)Children[0]).Eval() + ((NumTree)Children[1]).Eval(),
                NumKind.Multiply => ((NumTree)Children[0]).Eval() * ((NumTree)Children[1]).Eval(),
                _ => throw new NotImplementedException(),
            };
        }

        protected override ITree<Num> UpdateCore(IEnumerable<ITree<Num>> children)
        {
            return new BinaryNumTree(Value.Kind, (NumTree)children.ElementAt(0), (NumTree)children.ElementAt(1));
        }
    }

    internal sealed class UnaryNumTree : NumTree
    {
        public UnaryNumTree(NumKind kind, NumTree operand)
            : base(kind, new[] { operand })
        {
        }

        public override int Eval()
        {
            return Value.Kind switch
            {
                NumKind.Negate => -((NumTree)Children[0]).Eval(),
                _ => throw new NotImplementedException(),
            };
        }

        protected override ITree<Num> UpdateCore(IEnumerable<ITree<Num>> children)
        {
            return new UnaryNumTree(Value.Kind, (NumTree)children.ElementAt(0));
        }
    }

    internal sealed class VariableNumTree : NumTree
    {
        public VariableNumTree(ParameterExpression variable)
            : base(NumKind.Variable)
        {
            Variable = variable;
        }

        public ParameterExpression Variable { get; }

        public override int Eval() => throw new NotImplementedException();

        protected override ITree<Num> UpdateCore(IEnumerable<ITree<Num>> children) => this;
    }

    internal sealed class NumType : IType
    {
        public static readonly NumType Instance = new();

        private NumType()
        {
        }

        public bool IsAssignableTo(IType type) => type == Instance;

#pragma warning disable CA1822 // Mark static
        public int CompareTo(IType other)
        {
            if (other == Instance)
                return 0;
            else
                throw new NotImplementedException();
        }
#pragma warning restore CA1822

        public bool Equals(IType other) => other == Instance;
    }

    internal sealed class Num : IEquatable<Num>
    {
        public Num(NumKind kind) => Kind = kind;

        public NumKind Kind { get; }

        public bool Equals(Num other)
        {
            if (other == null)
                return false;

            return Kind == other.Kind;
        }

        public override bool Equals(object obj) => obj is Num num && Equals(num);

        public override int GetHashCode() => Kind.GetHashCode();

        public override string ToString() => Kind.ToString();
    }

    internal enum NumKind
    {
        Const,
        Negate,
        Add,
        Multiply,
        Variable,
    }

    internal struct NumWildcards : IWildcardFactory<NumTree>
    {
        public NumTree CreateWildcard(ParameterExpression hole) => new VariableNumTree(hole);
    }
}
