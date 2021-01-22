// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Linq.Expressions;

namespace System.Runtime.CompilerServices
{
    //
    // CONSIDER: Add more derived thunk types that implement different compilation policies, e.g. first use Compile(preferInterpretation: true),
    //           then keep invocation stats, and once it exceeds a threshold, replace it by using Compile(preferInterpretation: false). A more
    //           general mechanism to intercept compilation may be warranted, e.g. to support delegate caches. One option would be for the top-
    //           level Compile method to take in a factory for compilers (i.e. a `Func<Expression<T>, T>`).
    //

    /// <summary>
    /// Base class for thunks.
    /// </summary>
    /// <remarks>
    /// This type is used as a non-generic base class for thunks for purposes of having a convenient way to obtain the lambda expression describing the thunk's delegate.
    /// </remarks>
    public abstract class Thunk
    {
        /// <summary>
        /// Gets or sets the lambda expression representing the delegate that gets invoked through the thunk.
        /// </summary>
        /// <remarks>
        /// This expression will have a delegate type that has an additional closure parameter.
        /// When the thunk's target delegate gets invoked through the dispatcher, this parameter is supplied on behalf of the caller.
        /// </remarks>
        public abstract LambdaExpression Lambda { get; set; }
    }

    /// <summary>
    /// Base class for thunks.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate exposed by the thunk.</typeparam>
    /// <typeparam name="TClosure">The type of the closure parameter.</typeparam>
    /// <typeparam name="TInner">The type of the internal delegate used by the thunk. This delegate differs from <typeparamref name="TDelegate"/> by having an additional first parameter of type <typeparamref name="TClosure"/>.</typeparam>
    public abstract class Thunk<TDelegate, TClosure, TInner> : Thunk
    {
        //
        // NB: We adding constructors, don't forget to update the GetConstructors() logic in the runtime
        //     thunk type compiler to ensure it picks the right one when emitting IL code.
        //

        /// <summary>
        /// Creates a new instance of the thunk using the specified expression to implement the delegate.
        /// </summary>
        /// <param name="expression">Expression representing the implementation of the delegate.</param>
        protected Thunk(Expression<TInner> expression)
        {
            Expression = expression;

            //
            // NB: The constructor of derived types is responsible to set the Target field, either
            //     by immediate compilation of the given expression or by installing a JIT function
            //     whih also gets returned from the Compiler property.
            //
        }

        //
        // CONSIDER: Should we introduce another level in the class hierarchy to allow for thunks
        //           that don't require storage of the expression tree? This can fill a need for
        //           pre-compiled thunks that still have swappable delegate targets. One could also
        //           store the expression in an external map.
        //

#pragma warning disable CA1051 // Do not declare visible instance field. (Usage of field is by design.)

        /// <summary>
        /// Gets the expression implementing the thunk's internal delegate, i.e. the delegate that's parameterized on <typeparamref name="TClosure"/>.
        /// </summary>
        public Expression<TInner> Expression;

        /// <summary>
        /// Gets the target delegate invoked by dispatchers that supply the <typeparamref name="TClosure"/> object.
        /// </summary>
        public TInner Target;

#pragma warning restore CA1051

        /// <summary>
        /// Gets or sets the lambda expression of type <typeparamref name="TInner"/> representing the delegate that gets invoked through the thunk.
        /// </summary>
        /// <remarks>
        /// When assigning to this property, the expression gets replaced and the JIT compiler returned from <see cref="Compiler"/> is re-installed
        /// in the <see cref="Target"/> field. This causes the expression to get compiled upon the next delegate invocation through the thunk.
        /// For more information, see remarks on the base class definition.
        /// </remarks>
        public override LambdaExpression Lambda
        {
            get => Expression;

            set
            {
                Expression = (Expression<TInner>)value;
                Target = Compiler;
            }
        }

        /// <summary>
        /// Gets the JIT compiler function that can be re-installed in the <see cref="Target"/> field upon assigning <see cref="Lambda"/> with a
        /// new expression.
        /// </summary>
        public abstract TInner Compiler { get; }
    }
}
