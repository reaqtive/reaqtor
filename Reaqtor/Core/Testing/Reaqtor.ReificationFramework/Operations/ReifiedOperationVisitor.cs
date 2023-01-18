// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitting null checks for protected methods.)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Reflection;

using Reaqtor.TestingFramework;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Visitor for reified operations.
    /// </summary>
    public class ReifiedOperationVisitor
    {
        private static readonly MethodInfo s_catchGeneric =
            ((MethodInfo)ReflectionHelpers.InfoOf((ReifiedOperationVisitor v) => v.VisitCatch<Exception>(null))).GetGenericMethodDefinition();

        /// <summary>
        /// Visits a reified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        public virtual ReifiedOperation Visit(ReifiedOperation operation)
        {
            if (operation == null)
            {
                return null;
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
        protected virtual ReifiedOperation VisitAsync(Async operation)
        {
            var inner = Visit(operation.Operation);
            if (inner != operation.Operation)
            {
                return new Async(inner, operation.OnStart, operation.Token);
            }

            return operation;
        }

        private ReifiedOperation VisitCatch(ReifiedOperation operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(Catch<>));
            if (genericType != null)
            {
                var method = s_catchGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (ReifiedOperation)method.Invoke(this, new object[] { operation });
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
        protected virtual ReifiedOperation VisitCatch<T>(Catch<T> operation) where T : Exception
        {
            var inner = Visit(operation.Operation);
            if (inner != operation.Operation)
            {
                return new Catch<T>(inner, operation.Handler);
            }

            return operation;
        }

        /// <summary>
        /// Visits a chain operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual ReifiedOperation VisitChain(Chain operation)
        {
            var first = Visit(operation.Operation);
            var rest = default(List<ReifiedOperation>);
            var count = 0;
            foreach (var op in operation.Rest)
            {
                var result = Visit(op);
                if (result != op && rest == null)
                {
                    rest = new List<ReifiedOperation>(operation.Rest.Take(count));
                }

                rest?.Add(op);

                count++;
            }

            if (rest != null)
            {
                return new Chain(first, rest);
            }
            else if (first != operation.Operation)
            {
                return new Chain(first, operation.Rest);
            }

            return operation;
        }

        /// <summary>
        /// Visits an instrument operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual ReifiedOperation VisitInstrument(Instrument operation)
        {
            var inner = Visit(operation.Operation);
            if (inner != operation.Operation)
            {
                return new Instrument(inner, operation.OnEnter, operation.OnExit);
            }

            return operation;
        }

        /// <summary>
        /// Visits a lift wildcards operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual ReifiedOperation VisitLiftWildcards(LiftWildcards operation)
        {
            var inner = Visit(operation.Operation);
            if (inner != operation.Operation)
            {
                return new LiftWildcards(inner, operation.Generator);
            }

            return operation;
        }

        /// <summary>
        /// Visits a query engine operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual ReifiedOperation VisitQueryEngineOperation(QueryEngineOperation operation)
        {
            return operation;
        }

        /// <summary>
        /// Visits a repeat operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual ReifiedOperation VisitRepeat(Repeat operation)
        {
            var inner = Visit(operation.Operation);
            if (inner != operation.Operation)
            {
                return new Repeat(inner, operation.Count);
            }

            return operation;
        }

        /// <summary>
        /// Visits a repeat until operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual ReifiedOperation VisitRepeatUntil(RepeatUntil operation)
        {
            var inner = Visit(operation.Operation);
            if (inner != operation.Operation)
            {
                return new RepeatUntil(inner, operation.Token);
            }

            return operation;
        }

        /// <summary>
        /// Visits a service operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected virtual ReifiedOperation VisitServiceOperation(ServiceOperation operation)
        {
            return operation;
        }

        /// <summary>
        /// Visits an operation of an extension type.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        /// <exception cref="System.NotImplementedException">
        /// This method should be implemented in a derived visitor.
        /// </exception>
        protected virtual ReifiedOperation VisitExtension(ReifiedOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}
