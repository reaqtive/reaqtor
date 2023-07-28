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
    internal abstract class StrTree : Tree<StrKind>
    {
        public StrTree(StrKind kind)
            : base(kind)
        {
        }

        public StrTree(StrKind kind, params StrTree[] children)
            : base(kind, children)
        {
        }

        public abstract string Eval();
    }

    internal sealed class ConstStrTree : StrTree
    {
        public static readonly ConstStrTree Empty = new("");

        public ConstStrTree(string value)
            : base(StrKind.Const)
        {
            Value = value;
        }

        public new string Value { get; }

        public override string Eval() => Value;

        protected override ITree<StrKind> UpdateCore(IEnumerable<ITree<StrKind>> children) => this;

        public override string ToStringFormat() => "Const(" + Value + ")";
    }

    internal sealed class UnaryStrTree : StrTree
    {
        public UnaryStrTree(StrKind kind, StrTree operand)
            : base(kind, new[] { operand })
        {
        }

        public override string Eval()
        {
            return Value switch
            {
                StrKind.ToLower => ((StrTree)Children[0]).Eval().ToLower(),
                StrKind.ToUpper => ((StrTree)Children[0]).Eval().ToUpper(),
                _ => throw new NotImplementedException(),
            };
        }

        protected override ITree<StrKind> UpdateCore(IEnumerable<ITree<StrKind>> children)
        {
            return new UnaryStrTree(Value, (StrTree)children.ElementAt(0));
        }
    }

    internal sealed class ConcatStrTree : StrTree
    {
        public ConcatStrTree(params StrTree[] children)
            : base(StrKind.Concat, children)
        {
        }

        public override string Eval()
        {
            return string.Concat(Children.Cast<StrTree>().Select(c => c.Eval()));
        }

        protected override ITree<StrKind> UpdateCore(IEnumerable<ITree<StrKind>> children)
        {
            return new ConcatStrTree(children.Cast<StrTree>().ToArray());
        }
    }

    internal sealed class VariableStrTree : StrTree
    {
        public VariableStrTree(ParameterExpression variable)
            : base(StrKind.Variable)
        {
            Variable = variable;
        }

        public ParameterExpression Variable { get; }

        public override string Eval() => throw new NotImplementedException();

        protected override ITree<StrKind> UpdateCore(IEnumerable<ITree<StrKind>> children) => this;
    }

    internal enum StrKind
    {
        Const,
        ToLower,
        ToUpper,
        Concat,
        Variable,
    }

    internal readonly struct StrWildcards : IWildcardFactory<StrTree>
    {
        public StrTree CreateWildcard(ParameterExpression hole) => new VariableStrTree(hole);
    }
}
