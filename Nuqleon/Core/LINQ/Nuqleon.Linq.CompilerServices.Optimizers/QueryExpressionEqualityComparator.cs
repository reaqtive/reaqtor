// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Base class for query expression equality comparer implementations. Default behavior matches trees in a structural fashion.
    /// </summary>
    public class QueryExpressionEqualityComparator : ExpressionEqualityComparator, IEqualityComparer<QueryTree>
    {
        private readonly Stack<IEnumerable<AbstractedParameter>> _abstractedParametersLeft;
        private readonly Stack<IEnumerable<AbstractedParameter>> _abstractedParametersRight;

        /// <summary>
        /// Creates a new query expression equality comparator with default comparers for expressions, types, members, objects, and call site binders.
        /// </summary>
        public QueryExpressionEqualityComparator()
        {
            _abstractedParametersLeft = new Stack<IEnumerable<AbstractedParameter>>();
            _abstractedParametersRight = new Stack<IEnumerable<AbstractedParameter>>();
        }

        #region Equals

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        public virtual bool Equals(QueryTree x, QueryTree y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.QueryNodeType != y.QueryNodeType)
            {
                return false;
            }

            return x.QueryNodeType switch
            {
                QueryNodeType.Lambda => EqualsLambdaAbstraction((LambdaAbstraction)x, (LambdaAbstraction)y),
                QueryNodeType.MonadAbstraction => EqualsMonadAbstraction((MonadAbstraction)x, (MonadAbstraction)y),
                QueryNodeType.Operator => EqualsQueryOperator((QueryOperator)x, (QueryOperator)y),
                _ => EqualsExtension(x, y),
            };
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsExtension(QueryTree x, QueryTree y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsFirst(FirstOperator x, FirstOperator y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.ElementType, y.ElementType) &&
                Equals(x.Source, y.Source);
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsFirstPredicate(FirstPredicateOperator x, FirstPredicateOperator y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.ElementType, y.ElementType) &&
                Equals(x.Source, y.Source) &&
                Equals(x.Predicate, y.Predicate);
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsLambdaAbstraction(LambdaAbstraction x, LambdaAbstraction y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Parameters.Count != y.Parameters.Count)
            {
                return false;
            }

            _abstractedParametersLeft.Push(EnumerateAbstractedParameters(x.Body.Parameters, x.Parameters));
            _abstractedParametersRight.Push(EnumerateAbstractedParameters(y.Body.Parameters, y.Parameters));

            var res = Equals(x.Body.Body, y.Body.Body);

            _abstractedParametersLeft.Pop();
            _abstractedParametersRight.Pop();

            return res;
        }

        private static IEnumerable<AbstractedParameter> EnumerateAbstractedParameters(ReadOnlyCollection<ParameterExpression> parameters, ReadOnlyCollection<QueryTree> abstractions)
        {
            Debug.Assert(parameters != null);
            Debug.Assert(abstractions != null);
            Debug.Assert(parameters.Count == abstractions.Count);

            var cnt = parameters.Count;
            for (var i = 0; i < cnt; i++)
            {
                yield return new AbstractedParameter { Parameter = parameters[i], Value = abstractions[i] };
            }
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsMonadAbstraction(MonadAbstraction x, MonadAbstraction y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.ElementType, y.ElementType) &&
                Equals(x.Inner, y.Inner);
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsQueryOperator(QueryOperator x, QueryOperator y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.NodeType != y.NodeType || !Equals(x.ElementType, y.ElementType))
            {
                return false;
            }

            return x.NodeType switch
            {
                OperatorType.First => EqualsFirst((FirstOperator)x, (FirstOperator)y),
                OperatorType.FirstPredicate => EqualsFirstPredicate((FirstPredicateOperator)x, (FirstPredicateOperator)y),
                OperatorType.Where => EqualsWhere((WhereOperator)x, (WhereOperator)y),
                OperatorType.Select => EqualsSelect((SelectOperator)x, (SelectOperator)y),
                OperatorType.Take => EqualsTake((TakeOperator)x, (TakeOperator)y),
                _ => EqualsQueryOperatorExtension(x, y),
            };
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsQueryOperatorExtension(QueryOperator x, QueryOperator y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsSelect(SelectOperator x, SelectOperator y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.ElementType, y.ElementType) &&
                Equals(x.InputElementType, y.InputElementType) &&
                Equals(x.Source, y.Source) &&
                Equals(x.Selector, y.Selector);
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsTake(TakeOperator x, TakeOperator y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.ElementType, y.ElementType) &&
                Equals(x.Source, y.Source) &&
                Equals(x.Count, y.Count);
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsWhere(WhereOperator x, WhereOperator y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.ElementType, y.ElementType) &&
                Equals(x.Source, y.Source) &&
                Equals(x.Predicate, y.Predicate);
        }

        /// <summary>
        /// Checks whether the two given global parameter expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected sealed override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
        {
            if (x == null || y == null)
            {
                return EqualsGlobalParameterCore(x, y);
            }

            var l = Find(x, _abstractedParametersLeft);
            var r = Find(y, _abstractedParametersRight);

            if (l == null || r == null)
            {
                return EqualsGlobalParameterCore(x, y);
            }

            if (l.Value.Scope != 0 || r.Value.Scope != 0)
                throw new InvalidOperationException("Parameter is not in closest enclosing scope."); // TODO if we support this we would need some cycle detection

            return
                l.Value.Index == r.Value.Index &&
                Equals(l.Value.Value, r.Value.Value);
        }

        /// <summary>
        /// Checks whether the two given global parameter expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        protected virtual bool EqualsGlobalParameterCore(ParameterExpression x, ParameterExpression y)
        {
            return base.EqualsGlobalParameter(x, y);
        }

        private static ParameterIndex? Find(ParameterExpression p, Stack<IEnumerable<AbstractedParameter>> parameters)
        {
            var scope = 0;
            foreach (var frame in parameters)
            {
                var index = 0;
                foreach (var parameter in frame)
                {
                    if (object.ReferenceEquals(parameter.Parameter, p))
                    {
                        return new ParameterIndex { Scope = scope, Index = index, Value = parameter.Value };
                    }

                    index++;
                }

                scope++;
            }

            return null;
        }

        #endregion

        #region HashCode

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        public virtual int GetHashCode(QueryTree obj)
        {
            if (obj == null)
                return 17;

            return obj.QueryNodeType switch
            {
                QueryNodeType.Lambda => GetHashCodeLambdaAbstraction((LambdaAbstraction)obj),
                QueryNodeType.MonadAbstraction => GetHashCodeMonadAbstraction((MonadAbstraction)obj),
                QueryNodeType.Operator => GetHashCodeQueryOperator((QueryOperator)obj),
                _ => GetHashCodeExtension(obj),
            };
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeExtension(QueryTree obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeFirst(FirstOperator obj)
        {
            return obj == null ? 17 : Hash(
                GetHashCode(obj.ElementType),
                GetHashCode(obj.Source)
            );
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeFirstPredicate(FirstPredicateOperator obj)
        {
            return obj == null ? 17 : Hash(
                GetHashCode(obj.ElementType),
                GetHashCode(obj.Source),
                GetHashCode(obj.Predicate)
            );
        }

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected sealed override int GetHashCodeGlobalParameter(ParameterExpression obj)
        {
            if (obj == null)
                return 17;

            var i = 0;
            foreach (var frame in _abstractedParametersLeft)
            {
                var j = 0;
                foreach (var parameter in frame)
                {
                    if (parameter.Parameter == obj)
                    {
                        if (i != 0)
                        {
                            // TODO this is for consistency with Equals, which would require some cycle detection
                            throw new InvalidOperationException("Parameter is not in closest enclosing scope.");
                        }

                        return Hash(
                            i * 37 + j,
                            GetHashCode(parameter.Value)
                        );
                    }

                    j++;
                }

                i++;
            }

            return GetHashCodeGlobalParameterCore(obj);
        }

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        protected virtual int GetHashCodeGlobalParameterCore(ParameterExpression obj)
        {
            return base.GetHashCodeGlobalParameter(obj);
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeLambdaAbstraction(LambdaAbstraction obj)
        {
            if (obj == null)
            {
                return 17;
            }

            _abstractedParametersLeft.Push(EnumerateAbstractedParameters(obj.Body.Parameters, obj.Parameters));

            var res = GetHashCode(obj.Body.Body);

            _abstractedParametersLeft.Pop();

            return res;
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeMonadAbstraction(MonadAbstraction obj)
        {
            return obj == null ? 17 : GetHashCode(obj.Inner);
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeQueryOperator(QueryOperator obj)
        {
            if (obj == null)
                return 17;

            return obj.NodeType switch
            {
                OperatorType.First => GetHashCodeFirst((FirstOperator)obj),
                OperatorType.FirstPredicate => GetHashCodeFirstPredicate((FirstPredicateOperator)obj),
                OperatorType.Where => GetHashCodeWhere((WhereOperator)obj),
                OperatorType.Select => GetHashCodeSelect((SelectOperator)obj),
                OperatorType.Take => GetHashCodeTake((TakeOperator)obj),
                _ => GetHashCodeQueryOperatorExtension(obj),
            };
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeQueryOperatorExtension(QueryOperator obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeSelect(SelectOperator obj)
        {
            return obj == null ? 17 : Hash(
                GetHashCode(obj.ElementType),
                GetHashCode(obj.InputElementType),
                GetHashCode(obj.Source),
                GetHashCode(obj.Selector)
            );
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeTake(TakeOperator obj)
        {
            return obj == null ? 17 : Hash(
                GetHashCode(obj.ElementType),
                GetHashCode(obj.Source),
                GetHashCode(obj.Count)
            );
        }

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        protected virtual int GetHashCodeWhere(WhereOperator obj)
        {
            return obj == null ? 17 : Hash(
                GetHashCode(obj.ElementType),
                GetHashCode(obj.Source),
                GetHashCode(obj.Predicate)
            );
        }

        #endregion

        private struct ParameterIndex
        {
            public int Scope;
            public int Index;
            public QueryTree Value;
        }

        private struct AbstractedParameter
        {
            public ParameterExpression Parameter;
            public QueryTree Value;
        }
    }
}
