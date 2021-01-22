// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Provides a table with built-in precompiled closure types.
    /// </summary>
    internal static class Closures
    {
        /// <summary>
        /// A list of built-in precompiled closure types. The index corresponds to the generic arity of the closure type and hence
        /// the number of available slots for hoisted locals.
        /// </summary>
        /// <remarks>
        /// This field should be used in a read-only manner; we don't put a read-only wrapper or use a read-only interface to avoid any
        /// overheads in accessing it (considering that this field is only used internally).
        /// </remarks>
        public static readonly List<Type> Types = new List<Type>(17)
        {
            { typeof(Empty) }, // NB: Closure parameter to use when no variables are hoisted; simplifies code-gen.
            { typeof(Closure<>) },
            { typeof(Closure<,>) },
            { typeof(Closure<,,>) },
            { typeof(Closure<,,,>) },
            { typeof(Closure<,,,,>) },
            { typeof(Closure<,,,,,>) },
            { typeof(Closure<,,,,,,>) },
            { typeof(Closure<,,,,,,,>) },
            { typeof(Closure<,,,,,,,,>) },
            { typeof(Closure<,,,,,,,,,>) },
            { typeof(Closure<,,,,,,,,,,>) },
            { typeof(Closure<,,,,,,,,,,,>) },
            { typeof(Closure<,,,,,,,,,,,,>) },
            { typeof(Closure<,,,,,,,,,,,,,>) },
            { typeof(Closure<,,,,,,,,,,,,,,>) },
            { typeof(Closure<,,,,,,,,,,,,,,,>) },
        };
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 1;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 2;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 3;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 4;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 5;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 6;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 7;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 8;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 9;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Field holding the tenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T10 Item10;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 10;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    case 9:
                        return Item10;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    case 9:
                        Item10 = (T10)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Field holding the tenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T10 Item10;

        /// <summary>
        /// Field holding the eleventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T11 Item11;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 11;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    case 9:
                        return Item10;
                    case 10:
                        return Item11;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    case 9:
                        Item10 = (T10)value;
                        break;
                    case 10:
                        Item11 = (T11)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Field holding the tenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T10 Item10;

        /// <summary>
        /// Field holding the eleventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T11 Item11;

        /// <summary>
        /// Field holding the twelfth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T12 Item12;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 12;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    case 9:
                        return Item10;
                    case 10:
                        return Item11;
                    case 11:
                        return Item12;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    case 9:
                        Item10 = (T10)value;
                        break;
                    case 10:
                        Item11 = (T11)value;
                        break;
                    case 11:
                        Item12 = (T12)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Field holding the tenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T10 Item10;

        /// <summary>
        /// Field holding the eleventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T11 Item11;

        /// <summary>
        /// Field holding the twelfth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T12 Item12;

        /// <summary>
        /// Field holding the thirteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T13 Item13;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 13;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    case 9:
                        return Item10;
                    case 10:
                        return Item11;
                    case 11:
                        return Item12;
                    case 12:
                        return Item13;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    case 9:
                        Item10 = (T10)value;
                        break;
                    case 10:
                        Item11 = (T11)value;
                        break;
                    case 11:
                        Item12 = (T12)value;
                        break;
                    case 12:
                        Item13 = (T13)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Field holding the tenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T10 Item10;

        /// <summary>
        /// Field holding the eleventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T11 Item11;

        /// <summary>
        /// Field holding the twelfth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T12 Item12;

        /// <summary>
        /// Field holding the thirteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T13 Item13;

        /// <summary>
        /// Field holding the fourteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T14 Item14;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 14;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    case 9:
                        return Item10;
                    case 10:
                        return Item11;
                    case 11:
                        return Item12;
                    case 12:
                        return Item13;
                    case 13:
                        return Item14;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    case 9:
                        Item10 = (T10)value;
                        break;
                    case 10:
                        Item11 = (T11)value;
                        break;
                    case 11:
                        Item12 = (T12)value;
                        break;
                    case 12:
                        Item13 = (T13)value;
                        break;
                    case 13:
                        Item14 = (T14)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Field holding the tenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T10 Item10;

        /// <summary>
        /// Field holding the eleventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T11 Item11;

        /// <summary>
        /// Field holding the twelfth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T12 Item12;

        /// <summary>
        /// Field holding the thirteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T13 Item13;

        /// <summary>
        /// Field holding the fourteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T14 Item14;

        /// <summary>
        /// Field holding the fifteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T15 Item15;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 15;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    case 9:
                        return Item10;
                    case 10:
                        return Item11;
                    case 11:
                        return Item12;
                    case 12:
                        return Item13;
                    case 13:
                        return Item14;
                    case 14:
                        return Item15;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    case 9:
                        Item10 = (T10)value;
                        break;
                    case 10:
                        Item11 = (T11)value;
                        break;
                    case 11:
                        Item12 = (T12)value;
                        break;
                    case 12:
                        Item13 = (T13)value;
                        break;
                    case 13:
                        Item14 = (T14)value;
                        break;
                    case 14:
                        Item15 = (T15)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Closure<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IRuntimeVariables
    {
        /// <summary>
        /// Field holding the first piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T1 Item1;

        /// <summary>
        /// Field holding the second piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T2 Item2;

        /// <summary>
        /// Field holding the third piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T3 Item3;

        /// <summary>
        /// Field holding the fourth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T4 Item4;

        /// <summary>
        /// Field holding the fifth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T5 Item5;

        /// <summary>
        /// Field holding the sixth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T6 Item6;

        /// <summary>
        /// Field holding the seventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T7 Item7;

        /// <summary>
        /// Field holding the eighth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T8 Item8;

        /// <summary>
        /// Field holding the ninth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T9 Item9;

        /// <summary>
        /// Field holding the tenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T10 Item10;

        /// <summary>
        /// Field holding the eleventh piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T11 Item11;

        /// <summary>
        /// Field holding the twelfth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T12 Item12;

        /// <summary>
        /// Field holding the thirteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T13 Item13;

        /// <summary>
        /// Field holding the fourteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T14 Item14;

        /// <summary>
        /// Field holding the fifteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T15 Item15;

        /// <summary>
        /// Field holding the sixteenth piece of state.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public T16 Item16;

        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 16;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Item1;
                    case 1:
                        return Item2;
                    case 2:
                        return Item3;
                    case 3:
                        return Item4;
                    case 4:
                        return Item5;
                    case 5:
                        return Item6;
                    case 6:
                        return Item7;
                    case 7:
                        return Item8;
                    case 8:
                        return Item9;
                    case 9:
                        return Item10;
                    case 10:
                        return Item11;
                    case 11:
                        return Item12;
                    case 12:
                        return Item13;
                    case 13:
                        return Item14;
                    case 14:
                        return Item15;
                    case 15:
                        return Item16;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Item1 = (T1)value;
                        break;
                    case 1:
                        Item2 = (T2)value;
                        break;
                    case 2:
                        Item3 = (T3)value;
                        break;
                    case 3:
                        Item4 = (T4)value;
                        break;
                    case 4:
                        Item5 = (T5)value;
                        break;
                    case 5:
                        Item6 = (T6)value;
                        break;
                    case 6:
                        Item7 = (T7)value;
                        break;
                    case 7:
                        Item8 = (T8)value;
                        break;
                    case 8:
                        Item9 = (T9)value;
                        break;
                    case 9:
                        Item10 = (T10)value;
                        break;
                    case 10:
                        Item11 = (T11)value;
                        break;
                    case 11:
                        Item12 = (T12)value;
                        break;
                    case 12:
                        Item13 = (T13)value;
                        break;
                    case 13:
                        Item14 = (T14)value;
                        break;
                    case 14:
                        Item15 = (T15)value;
                        break;
                    case 15:
                        Item16 = (T16)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

}