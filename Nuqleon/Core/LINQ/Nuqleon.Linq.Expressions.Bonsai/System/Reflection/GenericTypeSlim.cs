// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Threading;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a closed generic type.
    /// </summary>
    public abstract class GenericTypeSlim : TypeSlim
    {
        #region Constructors

        /// <summary>
        /// Creates a new closed generic type representation object.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        internal GenericTypeSlim(GenericDefinitionTypeSlim typeDefinition)
        {
            GenericTypeDefinition = typeDefinition;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the kind of the type.
        /// </summary>
        public override TypeSlimKind Kind => TypeSlimKind.Generic;

        /// <summary>
        /// Gets the generic type definition.
        /// </summary>
        public GenericDefinitionTypeSlim GenericTypeDefinition { get; }

        /// <summary>
        /// Gets the generic type arguments.
        /// </summary>
        public abstract ReadOnlyCollection<TypeSlim> GenericArguments { get; }

        /// <summary>
        /// Gets the number of generic type arguments.
        /// </summary>
        public abstract int GenericArgumentCount { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the generic type argument at the specified index.
        /// </summary>
        /// <param name="index">The index of the generic type argument to get.</param>
        /// <returns>The generic type argument at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified index is out of range.</exception>
        public abstract TypeSlim GetGenericArgument(int index);

        /// <summary>
        /// Called by the visitor to update the node.
        /// </summary>
        /// <param name="typeDefinition">The new generic type definition.</param>
        /// <param name="arguments">The new generic type arguments, or null if the current type arguments have to be copied.</param>
        /// <returns>The updated node.</returns>
        internal abstract GenericTypeSlim Rewrite(GenericDefinitionTypeSlim typeDefinition, TypeSlim[] arguments);

        #endregion
    }

    internal sealed class GenericTypeSlimMany : GenericTypeSlim
    {
        /// <summary>
        /// Creates a new closed generic type representation object.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="arguments">Generic type arguments.</param>
        internal GenericTypeSlimMany(GenericDefinitionTypeSlim typeDefinition, ReadOnlyCollection<TypeSlim> arguments)
            : base(typeDefinition)
        {
            GenericArguments = arguments;
        }

        /// <summary>
        /// Gets the generic type arguments.
        /// </summary>
        public override ReadOnlyCollection<TypeSlim> GenericArguments { get; }

        /// <summary>
        /// Gets the number of generic type arguments.
        /// </summary>
        public override int GenericArgumentCount => GenericArguments.Count;

        /// <summary>
        /// Gets the generic type argument at the specified index.
        /// </summary>
        /// <param name="index">The index of the generic type argument to get.</param>
        /// <returns>The generic type argument at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified index is out of range.</exception>
        public override TypeSlim GetGenericArgument(int index) => GenericArguments[index];

        internal override GenericTypeSlim Rewrite(GenericDefinitionTypeSlim typeDefinition, TypeSlim[] arguments)
        {
            Debug.Assert(typeDefinition != null);
            Debug.Assert(arguments == null || arguments.Length == GenericArgumentCount);

            // CONSIDER: Add a factory method overload with IEnumerable<TypeSlim>.

            if (arguments != null)
            {
                return TypeSlim.Generic(typeDefinition, arguments);
            }
            else
            {
                return TypeSlim.Generic(typeDefinition, GenericArguments);
            }
        }
    }

    internal abstract class GenericTypeSlimN : GenericTypeSlim
    {
        private object _argument1;

        public GenericTypeSlimN(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1)
            : base(typeDefinition)
        {
            _argument1 = argument1;
        }

        public override ReadOnlyCollection<TypeSlim> GenericArguments
        {
            get
            {
                var argument1 = _argument1;

                if (argument1 is not ReadOnlyCollection<TypeSlim> collection)
                {
                    var arguments = new TrueReadOnlyCollection<TypeSlim>(Pack((TypeSlim)argument1) /* transfer ownership */);

                    Interlocked.CompareExchange(ref _argument1, arguments, argument1);

                    collection = (ReadOnlyCollection<TypeSlim>)_argument1;
                }

                return collection;
            }
        }

        protected TypeSlim Argument1
        {
            get
            {
                var argument1 = _argument1;

                var type = argument1 as TypeSlim;
                if (type != null)
                {
                    return type;
                }

                return ((ReadOnlyCollection<TypeSlim>)argument1)[0];
            }
        }

        protected abstract TypeSlim[] Pack(TypeSlim argument1);
    }

    public partial class TypeSlim
    {
        /// <summary>
        /// Creates a new lightweight representation of a closed generic type.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="arguments">Generic type arguments.</param>
        /// <returns>A new lightweight representation of a closed generic type.</returns>
        public static GenericTypeSlim Generic(GenericDefinitionTypeSlim typeDefinition, ReadOnlyCollection<TypeSlim> arguments)
        {
            RequireNotNull(typeDefinition, nameof(typeDefinition));
            RequireNotNull(arguments, nameof(arguments));

            return arguments.Count switch
            {
                1 => Generic(typeDefinition, arguments[0]),
                2 => Generic(typeDefinition, arguments[0], arguments[1]),
                3 => Generic(typeDefinition, arguments[0], arguments[1], arguments[2]),
                4 => Generic(typeDefinition, arguments[0], arguments[1], arguments[2], arguments[3]),
                _ => new GenericTypeSlimMany(typeDefinition, arguments),
            };
        }

        /// <summary>
        /// Creates a new lightweight representation of a closed generic type.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="arguments">Generic type arguments.</param>
        /// <returns>A new lightweight representation of a closed generic type.</returns>
        public static GenericTypeSlim Generic(GenericDefinitionTypeSlim typeDefinition, params TypeSlim[] arguments)
        {
            RequireNotNull(typeDefinition, nameof(typeDefinition));
            RequireNotNull(arguments, nameof(arguments));

            return arguments.Length switch
            {
                1 => Generic(typeDefinition, arguments[0]),
                2 => Generic(typeDefinition, arguments[0], arguments[1]),
                3 => Generic(typeDefinition, arguments[0], arguments[1], arguments[2]),
                4 => Generic(typeDefinition, arguments[0], arguments[1], arguments[2], arguments[3]),
                _ => new GenericTypeSlimMany(typeDefinition, arguments.ToReadOnly()),
            };
        }

        /// <summary>
        /// Creates a new lightweight representation of a closed generic type with one type argument.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="argument1">The first type argument.</param>
        /// <returns>A new lightweight representation of a closed generic type.</returns>
        public static GenericTypeSlim Generic(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1)
        {
            RequireNotNull(typeDefinition, nameof(typeDefinition));
            RequireNotNull(argument1, nameof(argument1));

            return new GenericTypeSlim1(typeDefinition, argument1);
        }

        /// <summary>
        /// Creates a new lightweight representation of a closed generic type with two type arguments.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="argument1">The first type argument.</param>
        /// <param name="argument2">The second type argument.</param>
        /// <returns>A new lightweight representation of a closed generic type.</returns>
        public static GenericTypeSlim Generic(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1, TypeSlim argument2)
        {
            RequireNotNull(typeDefinition, nameof(typeDefinition));
            RequireNotNull(argument1, nameof(argument1));
            RequireNotNull(argument2, nameof(argument2));

            return new GenericTypeSlim2(typeDefinition, argument1, argument2);
        }

        /// <summary>
        /// Creates a new lightweight representation of a closed generic type with three type arguments.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="argument1">The first type argument.</param>
        /// <param name="argument2">The second type argument.</param>
        /// <param name="argument3">The third type argument.</param>
        /// <returns>A new lightweight representation of a closed generic type.</returns>
        public static GenericTypeSlim Generic(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1, TypeSlim argument2, TypeSlim argument3)
        {
            RequireNotNull(typeDefinition, nameof(typeDefinition));
            RequireNotNull(argument1, nameof(argument1));
            RequireNotNull(argument2, nameof(argument2));
            RequireNotNull(argument3, nameof(argument3));

            return new GenericTypeSlim3(typeDefinition, argument1, argument2, argument3);
        }

        /// <summary>
        /// Creates a new lightweight representation of a closed generic type with four type arguments.
        /// </summary>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="argument1">The first type argument.</param>
        /// <param name="argument2">The second type argument.</param>
        /// <param name="argument3">The third type argument.</param>
        /// <param name="argument4">The fourth type argument.</param>
        /// <returns>A new lightweight representation of a closed generic type.</returns>
        public static GenericTypeSlim Generic(GenericDefinitionTypeSlim typeDefinition, TypeSlim argument1, TypeSlim argument2, TypeSlim argument3, TypeSlim argument4)
        {
            RequireNotNull(typeDefinition, nameof(typeDefinition));
            RequireNotNull(argument1, nameof(argument1));
            RequireNotNull(argument2, nameof(argument2));
            RequireNotNull(argument3, nameof(argument3));
            RequireNotNull(argument4, nameof(argument4));

            return new GenericTypeSlim4(typeDefinition, argument1, argument2, argument3, argument4);
        }
    }
}
