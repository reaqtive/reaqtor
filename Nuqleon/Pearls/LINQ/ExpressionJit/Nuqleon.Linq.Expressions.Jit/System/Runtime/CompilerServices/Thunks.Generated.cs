// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Provides a table with built-in precompiled thunk types that will compile expression trees using dynamic IL code generation.
    /// </summary>
    internal static class CompilingThunks
    {
        /// <summary>
        /// A map of built-in precompiled thunk types. The key represents the type of the delegate; the value represents the thunk type.
        /// For generic delegate types, the key and value are generic type definitions. In order to close the open generic thunk type
        /// using the type arguments of the generic delegate type, the closure type parameter has to be prepended.
        /// </summary>
        /// <remarks>
        /// This field should be used in a read-only manner; we don't put a read-only wrapper or use a read-only interface to avoid any
        /// overheads in accessing it (considering that this field is only used internally).
        /// </remarks>
        public static readonly Dictionary<Type, Type> TypeMap = new Dictionary<Type, Type>(32)
        {
            { typeof(Func<>), typeof(CompilingFuncThunk<,>) },
            { typeof(Func<,>), typeof(CompilingFuncThunk<,,>) },
            { typeof(Func<,,>), typeof(CompilingFuncThunk<,,,>) },
            { typeof(Func<,,,>), typeof(CompilingFuncThunk<,,,,>) },
            { typeof(Func<,,,,>), typeof(CompilingFuncThunk<,,,,,>) },
            { typeof(Func<,,,,,>), typeof(CompilingFuncThunk<,,,,,,>) },
            { typeof(Func<,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,>) },
            { typeof(Func<,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,>) },
            { typeof(Func<,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,,,>), typeof(CompilingFuncThunk<,,,,,,,,,,,,,,,,>) },
            { typeof(Action), typeof(CompilingActionThunk<>) },
            { typeof(Action<>), typeof(CompilingActionThunk<,>) },
            { typeof(Action<,>), typeof(CompilingActionThunk<,,>) },
            { typeof(Action<,,>), typeof(CompilingActionThunk<,,,>) },
            { typeof(Action<,,,>), typeof(CompilingActionThunk<,,,,>) },
            { typeof(Action<,,,,>), typeof(CompilingActionThunk<,,,,,>) },
            { typeof(Action<,,,,,>), typeof(CompilingActionThunk<,,,,,,>) },
            { typeof(Action<,,,,,,>), typeof(CompilingActionThunk<,,,,,,,>) },
            { typeof(Action<,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,>) },
            { typeof(Action<,,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,,,>), typeof(CompilingActionThunk<,,,,,,,,,,,,,,,>) },
        };
    }

    /// <summary>
    /// Provides a table with built-in precompiled thunk types that will compile expression trees using an interpreter execution target.
    /// </summary>
    internal static class InterpretingThunks
    {
        /// <summary>
        /// A map of built-in precompiled thunk types. The key represents the type of the delegate; the value represents the thunk type.
        /// For generic delegate types, the key and value are generic type definitions. In order to close the open generic thunk type
        /// using the type arguments of the generic delegate type, the closure type parameter has to be prepended.
        /// </summary>
        /// <remarks>
        /// This field should be used in a read-only manner; we don't put a read-only wrapper or use a read-only interface to avoid any
        /// overheads in accessing it (considering that this field is only used internally).
        /// </remarks>
        public static readonly Dictionary<Type, Type> TypeMap = new Dictionary<Type, Type>(32)
        {
            { typeof(Func<>), typeof(InterpretingFuncThunk<,>) },
            { typeof(Func<,>), typeof(InterpretingFuncThunk<,,>) },
            { typeof(Func<,,>), typeof(InterpretingFuncThunk<,,,>) },
            { typeof(Func<,,,>), typeof(InterpretingFuncThunk<,,,,>) },
            { typeof(Func<,,,,>), typeof(InterpretingFuncThunk<,,,,,>) },
            { typeof(Func<,,,,,>), typeof(InterpretingFuncThunk<,,,,,,>) },
            { typeof(Func<,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,>) },
            { typeof(Func<,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,>) },
            { typeof(Func<,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,,,>), typeof(InterpretingFuncThunk<,,,,,,,,,,,,,,,,>) },
            { typeof(Action), typeof(InterpretingActionThunk<>) },
            { typeof(Action<>), typeof(InterpretingActionThunk<,>) },
            { typeof(Action<,>), typeof(InterpretingActionThunk<,,>) },
            { typeof(Action<,,>), typeof(InterpretingActionThunk<,,,>) },
            { typeof(Action<,,,>), typeof(InterpretingActionThunk<,,,,>) },
            { typeof(Action<,,,,>), typeof(InterpretingActionThunk<,,,,,>) },
            { typeof(Action<,,,,,>), typeof(InterpretingActionThunk<,,,,,,>) },
            { typeof(Action<,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,>) },
            { typeof(Action<,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,>) },
            { typeof(Action<,,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,,,>), typeof(InterpretingActionThunk<,,,,,,,,,,,,,,,>) },
        };
    }

    /// <summary>
    /// Provides a table with built-in precompiled thunk types that will first compile expression trees using an interpreter execution
    /// target, and recompile expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    internal static class TieredCompliationThunks
    {
        /// <summary>
        /// A map of built-in precompiled thunk types. The key represents the type of the delegate; the value represents the thunk type.
        /// For generic delegate types, the key and value are generic type definitions. In order to close the open generic thunk type
        /// using the type arguments of the generic delegate type, the closure type parameter has to be prepended.
        /// </summary>
        /// <remarks>
        /// This field should be used in a read-only manner; we don't put a read-only wrapper or use a read-only interface to avoid any
        /// overheads in accessing it (considering that this field is only used internally).
        /// </remarks>
        public static readonly Dictionary<Type, Type> TypeMap = new Dictionary<Type, Type>(32)
        {
            { typeof(Func<>), typeof(TieredCompilationFuncThunk<,>) },
            { typeof(Func<,>), typeof(TieredCompilationFuncThunk<,,>) },
            { typeof(Func<,,>), typeof(TieredCompilationFuncThunk<,,,>) },
            { typeof(Func<,,,>), typeof(TieredCompilationFuncThunk<,,,,>) },
            { typeof(Func<,,,,>), typeof(TieredCompilationFuncThunk<,,,,,>) },
            { typeof(Func<,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,>) },
            { typeof(Func<,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,>) },
            { typeof(Func<,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,>) },
            { typeof(Func<,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,,,,,,,>) },
            { typeof(Func<,,,,,,,,,,,,,,,>), typeof(TieredCompilationFuncThunk<,,,,,,,,,,,,,,,,>) },
            { typeof(Action), typeof(TieredCompilationActionThunk<>) },
            { typeof(Action<>), typeof(TieredCompilationActionThunk<,>) },
            { typeof(Action<,>), typeof(TieredCompilationActionThunk<,,>) },
            { typeof(Action<,,>), typeof(TieredCompilationActionThunk<,,,>) },
            { typeof(Action<,,,>), typeof(TieredCompilationActionThunk<,,,,>) },
            { typeof(Action<,,,,>), typeof(TieredCompilationActionThunk<,,,,,>) },
            { typeof(Action<,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,>) },
            { typeof(Action<,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,>) },
            { typeof(Action<,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,>) },
            { typeof(Action<,,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,,,,,,,>) },
            { typeof(Action<,,,,,,,,,,,,,,>), typeof(TieredCompilationActionThunk<,,,,,,,,,,,,,,,>) },
        };
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, TResult> : Thunk<Func<TResult>, TClosure, Func<TClosure, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, TResult> CompilerCore => (closure) =>
        {
            Compile();

            return Target(closure);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<TResult>, TClosure, Func<TClosure, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke() => Parent.Target(Closure);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, TResult> : FuncThunk<TClosure, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, TResult> : FuncThunk<TClosure, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, TResult> : FuncThunk<TClosure, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, TResult> : Thunk<Func<T1, TResult>, TClosure, Func<TClosure, T1, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, TResult> CompilerCore => (closure, p1) =>
        {
            Compile();

            return Target(closure, p1);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, TResult>, TClosure, Func<TClosure, T1, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1) => Parent.Target(Closure, p1);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, TResult> : FuncThunk<TClosure, T1, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, TResult> : FuncThunk<TClosure, T1, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, TResult> : FuncThunk<TClosure, T1, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, TResult> : Thunk<Func<T1, T2, TResult>, TClosure, Func<TClosure, T1, T2, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, TResult> CompilerCore => (closure, p1, p2) =>
        {
            Compile();

            return Target(closure, p1, p2);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, TResult>, TClosure, Func<TClosure, T1, T2, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2) => Parent.Target(Closure, p1, p2);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, TResult> : FuncThunk<TClosure, T1, T2, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, TResult> : FuncThunk<TClosure, T1, T2, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, TResult> : FuncThunk<TClosure, T1, T2, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, TResult> : Thunk<Func<T1, T2, T3, TResult>, TClosure, Func<TClosure, T1, T2, T3, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, TResult> CompilerCore => (closure, p1, p2, p3) =>
        {
            Compile();

            return Target(closure, p1, p2, p3);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, TResult>, TClosure, Func<TClosure, T1, T2, T3, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3) => Parent.Target(Closure, p1, p2, p3);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, TResult> : FuncThunk<TClosure, T1, T2, T3, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, TResult> : FuncThunk<TClosure, T1, T2, T3, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, TResult> : FuncThunk<TClosure, T1, T2, T3, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, TResult> : Thunk<Func<T1, T2, T3, T4, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, TResult> CompilerCore => (closure, p1, p2, p3, p4) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4) => Parent.Target(Closure, p1, p2, p3, p4);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, TResult> : Thunk<Func<T1, T2, T3, T4, T5, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5) => Parent.Target(Closure, p1, p2, p3, p4, p5);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            /// <param name="p13">The thirteenth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            /// <param name="p13">The thirteenth caller's parameter passed to the target delegate.</param>
            /// <param name="p14">The fourteenth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal abstract class FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : Thunk<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public FuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) =>
        {
            Compile();

            return Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, TClosure, Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            /// <param name="p13">The thirteenth caller's parameter passed to the target delegate.</param>
            /// <param name="p14">The fourteenth caller's parameter passed to the target delegate.</param>
            /// <param name="p15">The fifteenth caller's parameter passed to the target delegate.</param>
            /// <returns>The result of invoking the target delegate.</returns>
            public TResult Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class CompilingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class InterpretingFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    /// <typeparam name="TResult">Type returned by the function.</typeparam>
    internal sealed class TieredCompilationFuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : FuncThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationFuncThunk(Expression<Func<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    return target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action"/> delegate.
    /// </summary>
    internal abstract class ActionThunk<TClosure> : Thunk<Action, TClosure, Action<TClosure>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure> CompilerCore => closure =>
        {
            Compile();

            Target(closure);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action, TClosure, Action<TClosure>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            public void Invoke() => Parent.Target(Closure);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    internal sealed class CompilingActionThunk<TClosure> : ActionThunk<TClosure>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    internal sealed class InterpretingActionThunk<TClosure> : ActionThunk<TClosure>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    internal sealed class TieredCompilationActionThunk<TClosure> : ActionThunk<TClosure>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1> : Thunk<Action<T1>, TClosure, Action<TClosure, T1>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1> CompilerCore => (closure, p1) =>
        {
            Compile();

            Target(closure, p1);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1>, TClosure, Action<TClosure, T1>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1) => Parent.Target(Closure, p1);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1> : ActionThunk<TClosure, T1>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1> : ActionThunk<TClosure, T1>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1> : ActionThunk<TClosure, T1>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2> : Thunk<Action<T1, T2>, TClosure, Action<TClosure, T1, T2>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2> CompilerCore => (closure, p1, p2) =>
        {
            Compile();

            Target(closure, p1, p2);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2>, TClosure, Action<TClosure, T1, T2>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2) => Parent.Target(Closure, p1, p2);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2> : ActionThunk<TClosure, T1, T2>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2> : ActionThunk<TClosure, T1, T2>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2> : ActionThunk<TClosure, T1, T2>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3> : Thunk<Action<T1, T2, T3>, TClosure, Action<TClosure, T1, T2, T3>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3> CompilerCore => (closure, p1, p2, p3) =>
        {
            Compile();

            Target(closure, p1, p2, p3);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3>, TClosure, Action<TClosure, T1, T2, T3>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3) => Parent.Target(Closure, p1, p2, p3);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3> : ActionThunk<TClosure, T1, T2, T3>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3> : ActionThunk<TClosure, T1, T2, T3>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3> : ActionThunk<TClosure, T1, T2, T3>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4> : Thunk<Action<T1, T2, T3, T4>, TClosure, Action<TClosure, T1, T2, T3, T4>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4> CompilerCore => (closure, p1, p2, p3, p4) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4>, TClosure, Action<TClosure, T1, T2, T3, T4>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4) => Parent.Target(Closure, p1, p2, p3, p4);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4> : ActionThunk<TClosure, T1, T2, T3, T4>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4> : ActionThunk<TClosure, T1, T2, T3, T4>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4> : ActionThunk<TClosure, T1, T2, T3, T4>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5> : Thunk<Action<T1, T2, T3, T4, T5>, TClosure, Action<TClosure, T1, T2, T3, T4, T5>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5> CompilerCore => (closure, p1, p2, p3, p4, p5) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5>, TClosure, Action<TClosure, T1, T2, T3, T4, T5>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5) => Parent.Target(Closure, p1, p2, p3, p4, p5);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5> : ActionThunk<TClosure, T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5> : ActionThunk<TClosure, T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5> : ActionThunk<TClosure, T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6> : Thunk<Action<T1, T2, T3, T4, T5, T6>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6> CompilerCore => (closure, p1, p2, p3, p4, p5, p6) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            /// <param name="p13">The thirteenth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            /// <param name="p13">The thirteenth caller's parameter passed to the target delegate.</param>
            /// <param name="p14">The fourteenth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                };

                Expression = null;
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> delegate.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    internal abstract class ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : Thunk<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public ActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression)
            : base(expression)
        {
            //
            // NB: We use a non-virtual property in the derived class rather than reading from the
            //     virtual Compiler property in the base class constructor. This avoids trickiness
            //     around calling virtual overridable members from a constructor (cf. CA2214).
            //
            Target = CompilerCore;
        }

        /// <summary>
        /// Gets the compilation function used to convert the expression to a delegate.
        /// </summary>
        public sealed override Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Compiler => CompilerCore;

        /// <summary>
        /// Property returning the compilation function.
        /// </summary>
        /// <remarks>
        /// Note we use a property to avoid having to store the compiler in a field. Access to this
        /// property should be rare: once during construction of the thunk and once every time the
        /// thunk gets invalidated when replacing the expression.
        /// </remarks>
        private Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CompilerCore => (closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) =>
        {
            Compile();

            Target(closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
        };

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected abstract void Compile();

        /// <summary>
        /// Creates an instance of the delegate represented by the thunk using the specified closure object.
        /// When invoked, delegates returned by this method will invoke the thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
        /// <returns>A delegate instance of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> using the specified closure as the first parameter for the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.</returns>
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateDelegate(TClosure closure) => new Dispatcher(this, closure).Invoke;

        /// <summary>
        /// Dispatcher implementation used to hand out delegates of type <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> that route invocations through the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate.
        /// </summary>
        private sealed class Dispatcher : Dispatcher<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TClosure, Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
        {
            /// <summary>
            /// Creates a new instance of the dispatcher using the specified parent thunk and closure object.
            /// </summary>
            /// <param name="parent">The parent thunk to route delegate invocations through.</param>
            /// <param name="closure">The closure object to pass to the <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate as the first parameter.</param>
            public Dispatcher(ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> parent, TClosure closure)
            {
                Parent = parent;
                Closure = closure;
            }

            /// <summary>
            /// Invokes the parent thunk's <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/> delegate using the specified parameters.
            /// This method matches the signature of the <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> delegate type in order to use it as the target method when creating such a delegate through <see cref="CreateDelegate"/>.
            /// </summary>
            /// <param name="p1">The first caller's parameter passed to the target delegate.</param>
            /// <param name="p2">The second caller's parameter passed to the target delegate.</param>
            /// <param name="p3">The third caller's parameter passed to the target delegate.</param>
            /// <param name="p4">The fourth caller's parameter passed to the target delegate.</param>
            /// <param name="p5">The fifth caller's parameter passed to the target delegate.</param>
            /// <param name="p6">The sixth caller's parameter passed to the target delegate.</param>
            /// <param name="p7">The seventh caller's parameter passed to the target delegate.</param>
            /// <param name="p8">The eighth caller's parameter passed to the target delegate.</param>
            /// <param name="p9">The ninth caller's parameter passed to the target delegate.</param>
            /// <param name="p10">The tenth caller's parameter passed to the target delegate.</param>
            /// <param name="p11">The eleventh caller's parameter passed to the target delegate.</param>
            /// <param name="p12">The twelfth caller's parameter passed to the target delegate.</param>
            /// <param name="p13">The thirteenth caller's parameter passed to the target delegate.</param>
            /// <param name="p14">The fourteenth caller's parameter passed to the target delegate.</param>
            /// <param name="p15">The fifteenth caller's parameter passed to the target delegate.</param>
            public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15) => Parent.Target(Closure, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> delegate. A thunk of this type compiles expression trees using dynamic IL code generation.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    internal sealed class CompilingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public CompilingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            //
            // Avoid concurrent compilations which are expensive and can bloat the code heap.
            //
            // NB: We lock on `this` in order to avoid having to allocate another object; users of the
            //     thunk reaching into the guts of the JIT compiler shall not lock on it as well.
            //
            lock (this)
            {
                if (Expression != null)
                {
                    Target = Expression.Compile();
                    Expression = null;
                }
            }
        }
    }

    /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> delegate. A thunk of this type compiles expression trees using an interpreter execution target.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    internal sealed class InterpretingActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public InterpretingActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                Target = expression.Compile(preferInterpretation: true);
                Expression = null;
            }
        }
    }

        /// <summary>
    /// Thunk for a <see cref="System.Action{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> delegate. A thunk of this type first compiles expression trees using an interpreter execution target,
    /// and recompiles expression trees using dynamic IL code generation if sufficient invocations are made.
    /// </summary>
    /// <typeparam name="TClosure">Type of the closure.</typeparam>
    /// <typeparam name="T1">Type of the first parameter of the function.</typeparam>
    /// <typeparam name="T2">Type of the second parameter of the function.</typeparam>
    /// <typeparam name="T3">Type of the third parameter of the function.</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter of the function.</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter of the function.</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter of the function.</typeparam>
    /// <typeparam name="T7">Type of the seventh parameter of the function.</typeparam>
    /// <typeparam name="T8">Type of the eighth parameter of the function.</typeparam>
    /// <typeparam name="T9">Type of the ninth parameter of the function.</typeparam>
    /// <typeparam name="T10">Type of the tenth parameter of the function.</typeparam>
    /// <typeparam name="T11">Type of the eleventh parameter of the function.</typeparam>
    /// <typeparam name="T12">Type of the twelfth parameter of the function.</typeparam>
    /// <typeparam name="T13">Type of the thirteenth parameter of the function.</typeparam>
    /// <typeparam name="T14">Type of the fourteenth parameter of the function.</typeparam>
    /// <typeparam name="T15">Type of the fifteenth parameter of the function.</typeparam>
    internal sealed class TieredCompilationActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : ActionThunk<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        public TieredCompilationActionThunk(Expression<Action<TClosure, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Compiles the expression tree and stores it in <see cref="Thunk{TDelegate, TClosure, TInner}.Target"/>.
        /// </summary>
        protected override void Compile()
        {
            var expression = Expression;

            if (expression != null)
            {
                var target = expression.Compile(preferInterpretation: true);

                int hitCount = 0;

                Target = (TClosure closure, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
                {
                    //
                    // NB: We use Interlocked.Increment to avoid compiling more than once, which can be costly.
                    //     On the flip side, it makes interpretation a tiny bit slower, and more invocations
                    //     may be running the interpreted code while the compilation is ongoing and Target has
                    //     not yet been reassigned.
                    //
                    if (Interlocked.Increment(ref hitCount) == JitConstants.TieredCompilationThreshold)
                    {
                        Target = target = expression.Compile();
                    }

                    target(closure, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
                };

                Expression = null;
            }
        }
    }

}
