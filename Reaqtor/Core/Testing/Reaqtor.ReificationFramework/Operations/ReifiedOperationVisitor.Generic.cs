// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitting null checks for protected methods.)

using Reaqtor.TestingFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Visitor for reified operations.
    /// </summary>
    /// <typeparam name="TResult">The visitor result type.</typeparam>
    public abstract class ReifiedOperationVisitor<TResult>
    {
        private static readonly MethodInfo s_catchGeneric =
            ((MethodInfo)ReflectionHelpers.InfoOf((ReifiedOperationVisitor<TResult> v) => v.VisitCatch<Exception>(null))).GetGenericMethodDefinition();

        /// <summary>
        /// Visits a reified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        public virtual TResult Visit(ReifiedOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            return operation.Kind switch
            {
                ReifiedOperationKind.Async => VisitAsync((Async)operation),
                ReifiedOperationKind.Catch => VisitCatch(operation),
                ReifiedOperationKind.Chain => VisitChain((Chain)operation),
                ReifiedOperationKind.Instrument => VisitInstrument((Instrument)operation),
                ReifiedOperationKind.LiftWildcards => VisitLiftWildcards((LiftWildcards)operation),
                ReifiedOperationKind.QueryEngineOperation => VisitQueryEngineOperation((QueryEngineOperation)operation),
                ReifiedOperationKind.Repeat => VisitRepeat((Repeat)operation),
                ReifiedOperationKind.RepeatUntil => VisitRepeatUntil((RepeatUntil)operation),
                ReifiedOperationKind.ServiceOperation => VisitServiceOperation((ServiceOperation)operation),
                _ => VisitExtension(operation),
            };
        }

        /// <summary>
        /// Visits an async operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual TResult VisitAsync(Async operation)
        {
            var inner = Visit(operation.Operation);
            return MakeAsync(inner, operation.OnStart, operation.Token);
        }

        /// <summary>
        /// Makes an async operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="onStart">
        /// A callback to return the task after the operation has been started.
        /// </param>
        /// <param name="token">
        /// A cancellation token to give to the task factory.
        /// </param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult MakeAsync(TResult operation, Action<Task> onStart, CancellationToken token);

        private TResult VisitCatch(ReifiedOperation operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(Catch<>));
            if (genericType != null)
            {
                var method = s_catchGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (TResult)method.Invoke(this, new object[] { operation });
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.InvariantCulture, "Expected operation with generic type definition '{0}'.", typeof(Catch<>)));
        }

        /// <summary>
        /// Visits a catch operation.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual TResult VisitCatch<T>(Catch<T> operation) where T : Exception
        {
            var inner = Visit(operation.Operation);
            return MakeCatch<T>(inner, operation.Handler);
        }

        /// <summary>
        /// Makes a catch operation.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="handler">The exception handler.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult MakeCatch<T>(TResult operation, Action<T> handler) where T : Exception;

        /// <summary>
        /// Visits a chain operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual TResult VisitChain(Chain operation)
        {
            var first = Visit(operation.Operation);
            var rest = new List<TResult>();
            foreach (var op in operation.Rest)
            {
                rest.Add(Visit(op));
            }

            return MakeChain(first, rest);
        }

        /// <summary>
        /// Makes a chain operation.
        /// </summary>
        /// <param name="first">The first operation.</param>
        /// <param name="rest">The remaining operations.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult MakeChain(TResult first, IEnumerable<TResult> rest);

        /// <summary>
        /// Visits an instrument operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual TResult VisitInstrument(Instrument operation)
        {
            var inner = Visit(operation.Operation);
            return MakeInstrument(inner, operation.OnEnter, operation.OnExit);
        }

        /// <summary>
        /// Makes an instrument operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="onEnter">
        /// Callback to invoke prior to evaluating the operation.
        /// </param>
        /// <param name="onExit">
        /// Callback to invoke after evaluating the operation.
        /// </param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult MakeInstrument(TResult operation, Action onEnter, Action onExit);

        /// <summary>
        /// Visits a lift wildcards operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual TResult VisitLiftWildcards(LiftWildcards operation)
        {
            var inner = Visit(operation.Operation);
            return MakeLiftWildcards(inner, operation.Generator);
        }

        /// <summary>
        /// Makes a lift wildcards operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="generator">The wildcard generator.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult MakeLiftWildcards(TResult operation, IWildcardGenerator generator);

        /// <summary>
        /// Visits a query engine operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult VisitQueryEngineOperation(QueryEngineOperation operation);

        /// <summary>
        /// Visits a repeat operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual TResult VisitRepeat(Repeat operation)
        {
            var inner = Visit(operation.Operation);
            return MakeRepeat(inner, operation.Count);
        }

        /// <summary>
        /// Makes a repeat operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="count">
        /// The number of times to repeat the operation.
        /// </param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult MakeRepeat(TResult operation, long count);

        /// <summary>
        /// Visits a repeat until operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual TResult VisitRepeatUntil(RepeatUntil operation)
        {
            var inner = Visit(operation.Operation);
            return MakeRepeatUntil(inner, operation.Token);
        }

        /// <summary>
        /// Makes a repeat until operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult MakeRepeatUntil(TResult operation, CancellationToken token);

        /// <summary>
        /// Visits a service operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult VisitServiceOperation(ServiceOperation operation);

        /// <summary>
        /// Visits an operation of an extension type.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult VisitExtension(ReifiedOperation operation);
    }
}
