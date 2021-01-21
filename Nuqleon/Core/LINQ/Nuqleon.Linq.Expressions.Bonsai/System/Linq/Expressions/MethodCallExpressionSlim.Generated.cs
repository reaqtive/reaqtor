// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    internal sealed class InstanceMethodCallExpressionSlim1 : InstanceMethodCallExpressionSlim
    {
        private object _arg0;

        public InstanceMethodCallExpressionSlim1(ExpressionSlim @object, MethodInfoSlim method, ExpressionSlim arg0)
            : base(@object, method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == 1);

            if (args != null)
            {
                return ExpressionSlim.Call(instance, Method, args[0]);
            }
            else
            {
                return ExpressionSlim.Call(instance, Method, ReturnObject<ExpressionSlim>(_arg0));
            }
        }
    }

    internal sealed class StaticMethodCallExpressionSlim1 : StaticMethodCallExpressionSlim
    {
        private object _arg0;

        public StaticMethodCallExpressionSlim1(MethodInfoSlim method, ExpressionSlim arg0)
            : base(method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance == null);
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 1);

            // NB: We don't protect against a visitor that returns a non-null value for Visit(node.Object)
            //     when the original node.Object was null for a static call. This already breaks the assert
            //     higher up and will result in a NullReferenceException here (just like in LINQ ET).

            return ExpressionSlim.Call(Method, args[0]);
        }
    }

    internal sealed class InstanceMethodCallExpressionSlim2 : InstanceMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;

        public InstanceMethodCallExpressionSlim2(ExpressionSlim @object, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
            : base(@object, method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == 2);

            if (args != null)
            {
                return ExpressionSlim.Call(instance, Method, args[0], args[1]);
            }
            else
            {
                return ExpressionSlim.Call(instance, Method, ReturnObject<ExpressionSlim>(_arg0), _arg1);
            }
        }
    }

    internal sealed class StaticMethodCallExpressionSlim2 : StaticMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;

        public StaticMethodCallExpressionSlim2(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
            : base(method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance == null);
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 2);

            // NB: We don't protect against a visitor that returns a non-null value for Visit(node.Object)
            //     when the original node.Object was null for a static call. This already breaks the assert
            //     higher up and will result in a NullReferenceException here (just like in LINQ ET).

            return ExpressionSlim.Call(Method, args[0], args[1]);
        }
    }

    internal sealed class InstanceMethodCallExpressionSlim3 : InstanceMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;

        public InstanceMethodCallExpressionSlim3(ExpressionSlim @object, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
            : base(@object, method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == 3);

            if (args != null)
            {
                return ExpressionSlim.Call(instance, Method, args[0], args[1], args[2]);
            }
            else
            {
                return ExpressionSlim.Call(instance, Method, ReturnObject<ExpressionSlim>(_arg0), _arg1, _arg2);
            }
        }
    }

    internal sealed class StaticMethodCallExpressionSlim3 : StaticMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;

        public StaticMethodCallExpressionSlim3(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
            : base(method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance == null);
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 3);

            // NB: We don't protect against a visitor that returns a non-null value for Visit(node.Object)
            //     when the original node.Object was null for a static call. This already breaks the assert
            //     higher up and will result in a NullReferenceException here (just like in LINQ ET).

            return ExpressionSlim.Call(Method, args[0], args[1], args[2]);
        }
    }

    internal sealed class InstanceMethodCallExpressionSlim4 : InstanceMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;

        public InstanceMethodCallExpressionSlim4(ExpressionSlim @object, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
            : base(@object, method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == 4);

            if (args != null)
            {
                return ExpressionSlim.Call(instance, Method, args[0], args[1], args[2], args[3]);
            }
            else
            {
                return ExpressionSlim.Call(instance, Method, ReturnObject<ExpressionSlim>(_arg0), _arg1, _arg2, _arg3);
            }
        }
    }

    internal sealed class StaticMethodCallExpressionSlim4 : StaticMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;

        public StaticMethodCallExpressionSlim4(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
            : base(method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance == null);
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 4);

            // NB: We don't protect against a visitor that returns a non-null value for Visit(node.Object)
            //     when the original node.Object was null for a static call. This already breaks the assert
            //     higher up and will result in a NullReferenceException here (just like in LINQ ET).

            return ExpressionSlim.Call(Method, args[0], args[1], args[2], args[3]);
        }
    }

    internal sealed class InstanceMethodCallExpressionSlim5 : InstanceMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;
        private readonly ExpressionSlim _arg4;

        public InstanceMethodCallExpressionSlim5(ExpressionSlim @object, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
            : base(@object, method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == 5);

            if (args != null)
            {
                return ExpressionSlim.Call(instance, Method, args[0], args[1], args[2], args[3], args[4]);
            }
            else
            {
                return ExpressionSlim.Call(instance, Method, ReturnObject<ExpressionSlim>(_arg0), _arg1, _arg2, _arg3, _arg4);
            }
        }
    }

    internal sealed class StaticMethodCallExpressionSlim5 : StaticMethodCallExpressionSlim
    {
        private object _arg0;
        private readonly ExpressionSlim _arg1;
        private readonly ExpressionSlim _arg2;
        private readonly ExpressionSlim _arg3;
        private readonly ExpressionSlim _arg4;

        public StaticMethodCallExpressionSlim5(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
            : base(method)
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

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance == null);
            Debug.Assert(args != null);
            Debug.Assert(args.Count == 5);

            // NB: We don't protect against a visitor that returns a non-null value for Visit(node.Object)
            //     when the original node.Object was null for a static call. This already breaks the assert
            //     higher up and will result in a NullReferenceException here (just like in LINQ ET).

            return ExpressionSlim.Call(Method, args[0], args[1], args[2], args[3], args[4]);
        }
    }

#pragma warning disable 1591
    partial class ExpressionSlim
    {
        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method)
        {
            Require(method, nameof(method));

            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim0(method);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim0(instance, method);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));

            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim1(method, arg0);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim1(instance, method, arg0);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));

            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim2(method, arg0, arg1);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim2(instance, method, arg0, arg1);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));

            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim3(method, arg0, arg1, arg2);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim3(instance, method, arg0, arg1, arg2);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));

            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim4(method, arg0, arg1, arg2, arg3);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim4(instance, method, arg0, arg1, arg2, arg3);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));
            Require(arg4, nameof(arg4));

            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim5(method, arg0, arg1, arg2, arg3, arg4);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim5(instance, method, arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method)
        {
            Require(method, nameof(method));

            return new StaticMethodCallExpressionSlim0(method);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));

            return new StaticMethodCallExpressionSlim1(method, arg0);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));

            return new StaticMethodCallExpressionSlim2(method, arg0, arg1);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));

            return new StaticMethodCallExpressionSlim3(method, arg0, arg1, arg2);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));

            return new StaticMethodCallExpressionSlim4(method, arg0, arg1, arg2, arg3);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            Require(method, nameof(method));
            Require(arg0, nameof(arg0));
            Require(arg1, nameof(arg1));
            Require(arg2, nameof(arg2));
            Require(arg3, nameof(arg3));
            Require(arg4, nameof(arg4));

            return new StaticMethodCallExpressionSlim5(method, arg0, arg1, arg2, arg3, arg4);
        }

    }

    partial class ExpressionSlimUnsafe
    {
        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method)
        {
            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim0(method);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim0(instance, method);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0)
        {
            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim1(method, arg0);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim1(instance, method, arg0);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim2(method, arg0, arg1);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim2(instance, method, arg0, arg1);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim3(method, arg0, arg1, arg2);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim3(instance, method, arg0, arg1, arg2);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim4(method, arg0, arg1, arg2, arg3);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim4(instance, method, arg0, arg1, arg2, arg3);
            }
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            if (instance == null)
            {
                return new StaticMethodCallExpressionSlim5(method, arg0, arg1, arg2, arg3, arg4);
            }
            else
            {
                return new InstanceMethodCallExpressionSlim5(instance, method, arg0, arg1, arg2, arg3, arg4);
            }
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method)
        {
            return new StaticMethodCallExpressionSlim0(method);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0)
        {
            return new StaticMethodCallExpressionSlim1(method, arg0);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            return new StaticMethodCallExpressionSlim2(method, arg0, arg1);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            return new StaticMethodCallExpressionSlim3(method, arg0, arg1, arg2);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            return new StaticMethodCallExpressionSlim4(method, arg0, arg1, arg2, arg3);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            return new StaticMethodCallExpressionSlim5(method, arg0, arg1, arg2, arg3, arg4);
        }

    }
#pragma warning restore
}
