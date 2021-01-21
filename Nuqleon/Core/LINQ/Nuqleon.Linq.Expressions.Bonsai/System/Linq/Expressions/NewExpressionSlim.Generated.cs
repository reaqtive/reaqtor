// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    internal sealed class NewReferenceTypeExpressionSlim1 : NewReferenceTypeExpressionSlim
    {
        private object _arg0;

        public NewReferenceTypeExpressionSlim1(ConstructorInfoSlim constructor, ExpressionSlim arg0)
            : base(constructor)
        {
            _arg0 = arg0;
        }

        public override int ArgumentCount => 1;

        public override ExpressionSlim GetArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return ReturnObject<ExpressionSlim>(_arg0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 1);

            return ExpressionSlim.New(Constructor, args[0]);
        }
    }

    internal sealed class NewReferenceTypeExpressionSlim2 : NewReferenceTypeExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;

        public NewReferenceTypeExpressionSlim2(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1)
            : base(constructor)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }

        public override int ArgumentCount => 2;

        public override ExpressionSlim GetArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return ReturnObject<ExpressionSlim>(_arg0);
                case 1:
                    return _arg1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 2);

            return ExpressionSlim.New(Constructor, args[0], args[1]);
        }
    }

    internal sealed class NewReferenceTypeExpressionSlim3 : NewReferenceTypeExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;

        public NewReferenceTypeExpressionSlim3(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
            : base(constructor)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        public override int ArgumentCount => 3;

        public override ExpressionSlim GetArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return ReturnObject<ExpressionSlim>(_arg0);
                case 1:
                    return _arg1;
                case 2:
                    return _arg2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 3);

            return ExpressionSlim.New(Constructor, args[0], args[1], args[2]);
        }
    }

    internal sealed class NewReferenceTypeExpressionSlim4 : NewReferenceTypeExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;

        public NewReferenceTypeExpressionSlim4(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
            : base(constructor)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
        }

        public override int ArgumentCount => 4;

        public override ExpressionSlim GetArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return ReturnObject<ExpressionSlim>(_arg0);
                case 1:
                    return _arg1;
                case 2:
                    return _arg2;
                case 3:
                    return _arg3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 4);

            return ExpressionSlim.New(Constructor, args[0], args[1], args[2], args[3]);
        }
    }

    internal sealed class NewReferenceTypeExpressionSlim5 : NewReferenceTypeExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;
        private readonly ExpressionSlim _arg4;

        public NewReferenceTypeExpressionSlim5(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
            : base(constructor)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
        }

        public override int ArgumentCount => 5;

        public override ExpressionSlim GetArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return ReturnObject<ExpressionSlim>(_arg0);
                case 1:
                    return _arg1;
                case 2:
                    return _arg2;
                case 3:
                    return _arg3;
                case 4:
                    return _arg4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 5);

            return ExpressionSlim.New(Constructor, args[0], args[1], args[2], args[3], args[4]);
        }
    }

#pragma warning disable 1591
    partial class ExpressionSlim
    {
        public static NewExpressionSlim New(ConstructorInfoSlim constructor)
        {
            Require(constructor, nameof(constructor));

            return new NewReferenceTypeExpressionSlim0(constructor);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0)
        {
            Require(constructor, nameof(constructor));
            Require(arg0, nameof(arg0));

            return new NewReferenceTypeExpressionSlim1(constructor, arg0);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            Require(constructor, nameof(constructor));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));

            return new NewReferenceTypeExpressionSlim2(constructor, arg0, arg1);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            Require(constructor, nameof(constructor));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));

            return new NewReferenceTypeExpressionSlim3(constructor, arg0, arg1, arg2);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            Require(constructor, nameof(constructor));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));

            return new NewReferenceTypeExpressionSlim4(constructor, arg0, arg1, arg2, arg3);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            Require(constructor, nameof(constructor));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));
            Require(arg4, nameof(arg4));

            return new NewReferenceTypeExpressionSlim5(constructor, arg0, arg1, arg2, arg3, arg4);
        }

    }

    partial class ExpressionSlimUnsafe
    {
        public static NewExpressionSlim New(ConstructorInfoSlim constructor)
        {
            return new NewReferenceTypeExpressionSlim0(constructor);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0)
        {
            return new NewReferenceTypeExpressionSlim1(constructor, arg0);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            return new NewReferenceTypeExpressionSlim2(constructor, arg0, arg1);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            return new NewReferenceTypeExpressionSlim3(constructor, arg0, arg1, arg2);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            return new NewReferenceTypeExpressionSlim4(constructor, arg0, arg1, arg2, arg3);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            return new NewReferenceTypeExpressionSlim5(constructor, arg0, arg1, arg2, arg3, arg4);
        }

    }
#pragma warning restore
}
