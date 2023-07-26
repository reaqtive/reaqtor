// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class EmitterContext
    {
        private LateBoundSiteString _siteString;

#if !NO_IO
        private LateBoundSiteWriter _siteWriter;
#endif

        /// <summary>
        /// Gets the emitter action for late-bound serialization of arbitrary objects.
        /// </summary>
        public EmitStringAction<object> EmitAnyString
        {
            get
            {
                //
                // NB: We lazily allocate the late bound site because its quite heavy to allocate. This happens upon the first call
                //     to Emitter.EmitAny for a type that's not well-known.
                //

                _siteString ??= new LateBoundSiteString();

                return _siteString.Action;
            }
        }

#if !NO_IO
        /// <summary>
        /// Gets the emitter action for late-bound serialization of arbitrary objects.
        /// </summary>
        public EmitWriterAction<object> EmitAnyWriter
        {
            get
            {
                //
                // NB: We lazily allocate the late bound site because its quite heavy to allocate. This happens upon the first call
                //     to Emitter.EmitAny for a type that's not well-known.
                //

                _siteWriter ??= new LateBoundSiteWriter();

                return _siteWriter.Action;
            }
        }
#endif

        /// <summary>
        /// Call site for late bound emitters using a polymorphic inline cache to speed up repeated serialization of objects of the same runtime type.
        /// </summary>
        private sealed class LateBoundSiteString
        {
            private static readonly MethodInfo s_build = typeof(FastJsonSerializerFactory.EmitterStringBuilder).GetMethod(nameof(FastJsonSerializerFactory.EmitterStringBuilder.Build), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            private readonly ParameterExpression _builder = Expression.Parameter(typeof(StringBuilder), "builder");
            private readonly ParameterExpression _value = Expression.Parameter(typeof(object), "value");
            private readonly ParameterExpression _context = Expression.Parameter(typeof(EmitterContext), "context");

            private readonly ParameterExpression _type = Expression.Parameter(typeof(Type), "type");
            private readonly Expression _bind;
            private readonly ParameterExpression[] _locals;
            private readonly Expression _getType;

            private Expression _tests;

            /// <summary>
            /// Creates a new call site for late bound emitters. Invoking this constructor causes compilation of an initial empty call site.
            /// </summary>
            public LateBoundSiteString()
            {
                //
                // Cache a few expressions that will be used over and over again when refreshing the call site.
                //

                _bind = Expression.Call(Expression.Constant(this), typeof(LateBoundSiteString).GetMethod(nameof(Bind)), _type, _builder, _value, _context);
                _locals = new[] { _type };
                _getType = Expression.Assign(_type, Expression.Call(_value, typeof(object).GetMethod(nameof(GetType))));

                //
                // Build the initial call site which simply dispatches into the binder whose role it is to discover emit actions based
                // on the runtime type of the object being serialized. The binder will update the call site as it learns new cases.
                //

                _tests = _bind;
                Compile();
            }

            /// <summary>
            /// The delegate built by the call site.
            /// </summary>
            /// <remarks>
            /// For efficiency reasons, the delegate is exposed as a field. Use it in a read-only fashion only.
            /// </remarks>
            public EmitStringAction<object> Action;

            /// <summary>
            /// Compiles the call site with efficient polymorphic inline cache dispatch behavior.
            /// </summary>
            public void Compile()
            {
                var body =
                    Expression.Block(
                        _locals,
                        _getType,
                        _tests
                    );

                var expr = Expression.Lambda<EmitStringAction<object>>(body, _builder, _value, _context);

                Action = expr.Compile();
            }

            /// <summary>
            /// Performs binding and dispatch operations for a <paramref name="value"/> of the specified <paramref name="type"/> which does not yet occur in the polymorphic inline cache.
            /// </summary>
            /// <param name="type">The runtime type of the object to discover an emitter action for. When fond, the polymorphic inline cache is updated, causing the call site to be recompiled.</param>
            /// <param name="builder">The string builder to append serialization output to.</param>
            /// <param name="value">The object to serialize.</param>
            /// <param name="context">The emitter context.</param>
            public void Bind(Type type, StringBuilder builder, object value, EmitterContext context)
            {
                //
                // First, try to obtain an EmitAction<T> for the specified object runtime type by using the EmitterBuilder. This
                // late bound operation may fail, in which case no harm is done to our polymorphic inline cache.
                //
                // NB: We don't cache exceptions in the polymorphic inline cache at the moment; this could cause bloat with types
                //     that cannot be serialized.
                //

                Delegate emitAction;
                try
                {
                    emitAction = (Delegate)s_build.MakeGenericMethod(type).Invoke(context._builderString, ArrayBuilder<object>.Empty);
                }
                catch (TargetInvocationException ex)
                {
                    //
                    // NB: It's okay to reset the call stack here, pretending the error happens in the binder. No need to use EDI.
                    //

                    throw ex.InnerException;
                }

                //
                // Next, perform the task at hand and perform the serialization by making a late bound call to the retrieved emit
                // action. This call will be slow, but our goal is to speed up subsequent access.
                //

                emitAction.DynamicInvoke(new object[] { builder, value, context });

                //
                // Next, update the expression representing the polymorphic inline cache to include a type test for the discovered
                // emit action.
                //
                //   if (type == typeof(Foo))
                //   {
                //       EmitFoo(builder, (Foo)value, context);
                //   }
                //   else
                //   {
                //     .
                //       .
                //         .
                //           else
                //           {
                //               Bind(type, builder, value, context);
                //           }
                //         .
                //       .
                //     .
                //   }
                //
                // CONSIDER: Is it worth to add a cache eviction strategy? Currently, we rely on the scoping of the cache to be non-
                //           static and per serializer or invocation thereof. Contrast this to DLR call site caches which can be
                //           more invasive and have no eviction policy by default either. We'll go with the simple approach for now.
                //

                _tests =
                    Expression.IfThenElse(
                        Expression.Equal(
                            _type,
                            Expression.Constant(type, typeof(Type))
                        ),
                        Expression.Invoke(
                            Expression.Constant(emitAction),
                            _builder,
                            Expression.Convert(_value, type),
                            _context
                        ),
                        _tests
                    );

                //
                // Finally, compile the polymorphic inline cache. This will replace the Action delegate.
                //

                Compile();
            }
        }

#if !NO_IO
        /// <summary>
        /// Call site for late bound emitters using a polymorphic inline cache to speed up repeated serialization of objects of the same runtime type.
        /// </summary>
        private sealed class LateBoundSiteWriter
        {
            private static readonly MethodInfo s_build = typeof(FastJsonSerializerFactory.EmitterWriterBuilder).GetMethod(nameof(FastJsonSerializerFactory.EmitterWriterBuilder.Build), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            private readonly ParameterExpression _writer = Expression.Parameter(typeof(System.IO.TextWriter), "writer");
            private readonly ParameterExpression _value = Expression.Parameter(typeof(object), "value");
            private readonly ParameterExpression _context = Expression.Parameter(typeof(EmitterContext), "context");

            private readonly ParameterExpression _type = Expression.Parameter(typeof(Type), "type");
            private readonly Expression _bind;
            private readonly ParameterExpression[] _locals;
            private readonly Expression _getType;

            private Expression _tests;

            /// <summary>
            /// Creates a new call site for late bound emitters. Invoking this constructor causes compilation of an initial empty call site.
            /// </summary>
            public LateBoundSiteWriter()
            {
                //
                // Cache a few expressions that will be used over and over again when refreshing the call site.
                //

                _bind = Expression.Call(Expression.Constant(this), typeof(LateBoundSiteWriter).GetMethod(nameof(Bind)), _type, _writer, _value, _context);
                _locals = new[] { _type };
                _getType = Expression.Assign(_type, Expression.Call(_value, typeof(object).GetMethod(nameof(GetType))));

                //
                // Build the initial call site which simply dispatches into the binder whose role it is to discover emit actions based
                // on the runtime type of the object being serialized. The binder will update the call site as it learns new cases.
                //

                _tests = _bind;
                Compile();
            }

            /// <summary>
            /// The delegate built by the call site.
            /// </summary>
            /// <remarks>
            /// For efficiency reasons, the delegate is exposed as a field. Use it in a read-only fashion only.
            /// </remarks>
            public EmitWriterAction<object> Action;

            /// <summary>
            /// Compiles the call site with efficient polymorphic inline cache dispatch behavior.
            /// </summary>
            public void Compile()
            {
                var body =
                    Expression.Block(
                        _locals,
                        _getType,
                        _tests
                    );

                var expr = Expression.Lambda<EmitWriterAction<object>>(body, _writer, _value, _context);

                Action = expr.Compile();
            }

            /// <summary>
            /// Performs binding and dispatch operations for a <paramref name="value"/> of the specified <paramref name="type"/> which does not yet occur in the polymorphic inline cache.
            /// </summary>
            /// <param name="type">The runtime type of the object to discover an emitter action for. When fond, the polymorphic inline cache is updated, causing the call site to be recompiled.</param>
            /// <param name="writer">The text writer to append serialization output to.</param>
            /// <param name="value">The object to serialize.</param>
            /// <param name="context">The emitter context.</param>
            public void Bind(Type type, System.IO.TextWriter writer, object value, EmitterContext context)
            {
                //
                // First, try to obtain an EmitAction<T> for the specified object runtime type by using the EmitterBuilder. This
                // late bound operation may fail, in which case no harm is done to our polymorphic inline cache.
                //
                // NB: We don't cache exceptions in the polymorphic inline cache at the moment; this could cause bloat with types
                //     that cannot be serialized.
                //

                Delegate emitAction;
                try
                {
                    emitAction = (Delegate)s_build.MakeGenericMethod(type).Invoke(context._builderWriter, ArrayBuilder<object>.Empty);
                }
                catch (TargetInvocationException ex)
                {
                    //
                    // NB: It's okay to reset the call stack here, pretending the error happens in the binder. No need to use EDI.
                    //

                    throw ex.InnerException;
                }

                //
                // Next, perform the task at hand and perform the serialization by making a late bound call to the retrieved emit
                // action. This call will be slow, but our goal is to speed up subsequent access.
                //

                emitAction.DynamicInvoke(new object[] { writer, value, context });

                //
                // Next, update the expression representing the polymorphic inline cache to include a type test for the discovered
                // emit action.
                //
                //   if (type == typeof(Foo))
                //   {
                //       EmitFoo(writer, (Foo)value, context);
                //   }
                //   else
                //   {
                //     .
                //       .
                //         .
                //           else
                //           {
                //               Bind(type, writer, value, context);
                //           }
                //         .
                //       .
                //     .
                //   }
                //
                // CONSIDER: Is it worth to add a cache eviction strategy? Currently, we rely on the scoping of the cache to be non-
                //           static and per serializer or invocation thereof. Contrast this to DLR call site caches which can be
                //           more invasive and have no eviction policy by default either. We'll go with the simple approach for now.
                //

                _tests =
                    Expression.IfThenElse(
                        Expression.Equal(
                            _type,
                            Expression.Constant(type, typeof(Type))
                        ),
                        Expression.Invoke(
                            Expression.Constant(emitAction),
                            _writer,
                            Expression.Convert(_value, type),
                            _context
                        ),
                        _tests
                    );

                //
                // Finally, compile the polymorphic inline cache. This will replace the Action delegate.
                //

                Compile();
            }
        }
#endif
    }
}
