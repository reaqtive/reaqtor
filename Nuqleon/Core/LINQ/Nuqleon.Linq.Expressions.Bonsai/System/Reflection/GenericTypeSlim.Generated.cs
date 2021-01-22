// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Reflection
{
    internal sealed class GenericTypeSlim1 : GenericTypeSlimN
    {

        internal GenericTypeSlim1(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1)
            : base(typeDefinition, argument1)
        {
        }

        public override int GenericArgumentCount => 1;

        public override TypeSlim GetGenericArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return Argument1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override TypeSlim[] Pack(TypeSlim argument1) => new TypeSlim[] { argument1 };

        internal override GenericTypeSlim Rewrite(GenericDefinitionTypeSlim typeDefinition, TypeSlim[] arguments)
        {
            Debug.Assert(typeDefinition != null);
            Debug.Assert(arguments == null || arguments.Length == 1);

            if (arguments == null)
            {
                return TypeSlim.Generic(typeDefinition, Argument1);
            }
            else
            {
                return TypeSlim.Generic(typeDefinition, arguments[0]);
            }
        }
    }

    internal sealed class GenericTypeSlim2 : GenericTypeSlimN
    {
        private readonly TypeSlim _argument2;

        internal GenericTypeSlim2(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1, TypeSlim argument2)
            : base(typeDefinition, argument1)
        {
            _argument2 = argument2;
        }

        public override int GenericArgumentCount => 2;

        public override TypeSlim GetGenericArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return Argument1;
                case 1:
                    return _argument2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override TypeSlim[] Pack(TypeSlim argument1) => new TypeSlim[] { argument1, _argument2 };

        internal override GenericTypeSlim Rewrite(GenericDefinitionTypeSlim typeDefinition, TypeSlim[] arguments)
        {
            Debug.Assert(typeDefinition != null);
            Debug.Assert(arguments == null || arguments.Length == 2);

            if (arguments == null)
            {
                return TypeSlim.Generic(typeDefinition, Argument1, _argument2);
            }
            else
            {
                return TypeSlim.Generic(typeDefinition, arguments[0], arguments[1]);
            }
        }
    }

    internal sealed class GenericTypeSlim3 : GenericTypeSlimN
    {
        private readonly TypeSlim _argument2;
        private readonly TypeSlim _argument3;

        internal GenericTypeSlim3(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1, TypeSlim argument2, TypeSlim argument3)
            : base(typeDefinition, argument1)
        {
            _argument2 = argument2;
            _argument3 = argument3;
        }

        public override int GenericArgumentCount => 3;

        public override TypeSlim GetGenericArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return Argument1;
                case 1:
                    return _argument2;
                case 2:
                    return _argument3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override TypeSlim[] Pack(TypeSlim argument1) => new TypeSlim[] { argument1, _argument2, _argument3 };

        internal override GenericTypeSlim Rewrite(GenericDefinitionTypeSlim typeDefinition, TypeSlim[] arguments)
        {
            Debug.Assert(typeDefinition != null);
            Debug.Assert(arguments == null || arguments.Length == 3);

            if (arguments == null)
            {
                return TypeSlim.Generic(typeDefinition, Argument1, _argument2, _argument3);
            }
            else
            {
                return TypeSlim.Generic(typeDefinition, arguments[0], arguments[1], arguments[2]);
            }
        }
    }

    internal sealed class GenericTypeSlim4 : GenericTypeSlimN
    {
        private readonly TypeSlim _argument2;
        private readonly TypeSlim _argument3;
        private readonly TypeSlim _argument4;

        internal GenericTypeSlim4(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1, TypeSlim argument2, TypeSlim argument3, TypeSlim argument4)
            : base(typeDefinition, argument1)
        {
            _argument2 = argument2;
            _argument3 = argument3;
            _argument4 = argument4;
        }

        public override int GenericArgumentCount => 4;

        public override TypeSlim GetGenericArgument(int index)
        {
            switch (index)
            {
                case 0:
                    return Argument1;
                case 1:
                    return _argument2;
                case 2:
                    return _argument3;
                case 3:
                    return _argument4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override TypeSlim[] Pack(TypeSlim argument1) => new TypeSlim[] { argument1, _argument2, _argument3, _argument4 };

        internal override GenericTypeSlim Rewrite(GenericDefinitionTypeSlim typeDefinition, TypeSlim[] arguments)
        {
            Debug.Assert(typeDefinition != null);
            Debug.Assert(arguments == null || arguments.Length == 4);

            if (arguments == null)
            {
                return TypeSlim.Generic(typeDefinition, Argument1, _argument2, _argument3, _argument4);
            }
            else
            {
                return TypeSlim.Generic(typeDefinition, arguments[0], arguments[1], arguments[2], arguments[3]);
            }
        }
    }

}
