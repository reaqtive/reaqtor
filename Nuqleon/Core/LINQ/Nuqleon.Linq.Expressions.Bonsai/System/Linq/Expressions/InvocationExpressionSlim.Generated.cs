// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Linq.Expressions
{
    internal sealed class InvocationExpressionSlim1 : InvocationExpressionSlim
    {
        private object _arg0;

        public InvocationExpressionSlim1(ExpressionSlim expression, ExpressionSlim arg0)
            : base(expression)
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

        internal override InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args)
        {
            Debug.Assert(expression != null);
            Debug.Assert(args == null || args.Count == 1);

            if (args != null)
            {
                return ExpressionSlim.Invoke(expression, args[0]);
            }
            else
            {
                return ExpressionSlim.Invoke(expression, ReturnObject<ExpressionSlim>(_arg0));
            }
        }
    }

    internal sealed class InvocationExpressionSlim2 : InvocationExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;

        public InvocationExpressionSlim2(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1)
            : base(expression)
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

        internal override InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args)
        {
            Debug.Assert(expression != null);
            Debug.Assert(args == null || args.Count == 2);

            if (args != null)
            {
                return ExpressionSlim.Invoke(expression, args[0], args[1]);
            }
            else
            {
                return ExpressionSlim.Invoke(expression, ReturnObject<ExpressionSlim>(_arg0), _arg1);
            }
        }
    }

    internal sealed class InvocationExpressionSlim3 : InvocationExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;

        public InvocationExpressionSlim3(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
            : base(expression)
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

        internal override InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args)
        {
            Debug.Assert(expression != null);
            Debug.Assert(args == null || args.Count == 3);

            if (args != null)
            {
                return ExpressionSlim.Invoke(expression, args[0], args[1], args[2]);
            }
            else
            {
                return ExpressionSlim.Invoke(expression, ReturnObject<ExpressionSlim>(_arg0), _arg1, _arg2);
            }
        }
    }

    internal sealed class InvocationExpressionSlim4 : InvocationExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;

        public InvocationExpressionSlim4(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
            : base(expression)
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

        internal override InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args)
        {
            Debug.Assert(expression != null);
            Debug.Assert(args == null || args.Count == 4);

            if (args != null)
            {
                return ExpressionSlim.Invoke(expression, args[0], args[1], args[2], args[3]);
            }
            else
            {
                return ExpressionSlim.Invoke(expression, ReturnObject<ExpressionSlim>(_arg0), _arg1, _arg2, _arg3);
            }
        }
    }

    internal sealed class InvocationExpressionSlim5 : InvocationExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;
        private readonly ExpressionSlim _arg4;

        public InvocationExpressionSlim5(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
            : base(expression)
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

        internal override InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args)
        {
            Debug.Assert(expression != null);
            Debug.Assert(args == null || args.Count == 5);

            if (args != null)
            {
                return ExpressionSlim.Invoke(expression, args[0], args[1], args[2], args[3], args[4]);
            }
            else
            {
                return ExpressionSlim.Invoke(expression, ReturnObject<ExpressionSlim>(_arg0), _arg1, _arg2, _arg3, _arg4);
            }
        }
    }

#pragma warning disable 1591
    partial class ExpressionSlim
    {
        public static InvocationExpressionSlim Invoke(ExpressionSlim expression)
        {
            Require(expression, nameof(expression));

            return new InvocationExpressionSlim0(expression);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0)
        {
            Require(expression, nameof(expression));
            Require(arg0, nameof(arg0));

            return new InvocationExpressionSlim1(expression, arg0);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            Require(expression, nameof(expression));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));

            return new InvocationExpressionSlim2(expression, arg0, arg1);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            Require(expression, nameof(expression));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));

            return new InvocationExpressionSlim3(expression, arg0, arg1, arg2);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            Require(expression, nameof(expression));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));

            return new InvocationExpressionSlim4(expression, arg0, arg1, arg2, arg3);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            Require(expression, nameof(expression));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));
            Require(arg4, nameof(arg4));

            return new InvocationExpressionSlim5(expression, arg0, arg1, arg2, arg3, arg4);
        }

    }

    partial class ExpressionSlimUnsafe
    {
        public static InvocationExpressionSlim Invoke(ExpressionSlim expression)
        {
            return new InvocationExpressionSlim0(expression);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0)
        {
            return new InvocationExpressionSlim1(expression, arg0);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            return new InvocationExpressionSlim2(expression, arg0, arg1);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            return new InvocationExpressionSlim3(expression, arg0, arg1, arg2);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            return new InvocationExpressionSlim4(expression, arg0, arg1, arg2, arg3);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            return new InvocationExpressionSlim5(expression, arg0, arg1, arg2, arg3, arg4);
        }

    }
#pragma warning restore
}
