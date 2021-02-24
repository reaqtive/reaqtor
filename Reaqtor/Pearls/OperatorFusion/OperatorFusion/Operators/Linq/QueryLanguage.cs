// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Exposes query operators in a fluent interface pattern as a stepping stone
// towards an approach based on proper binding.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;

namespace OperatorFusion
{
    static class QueryLanguage
    {
        public static Operator<T> Source<T>(string name)
        {
            _ = name; // placeholder

            return new Operator<T>
            {
                Inputs = Array.Empty<Operator>(),
                Factory = null
            };
        }

        public static SinkOperator<T> Sink<T>(this Operator<T> source, string name)
        {
            _ = name; // placeholder

            return new SinkOperator<T>
            {
                Inputs = new[] { source },
                Factory = new SinkFactory
                {
                    OutputType = typeof(T) // TODO: remove
                }
            };
        }

        public static Operator<int> Count<T>(this Operator<T> source)
        {
            return new Operator<int>
            {
                Inputs = new[] { source },
                Factory = new CountFactory()
            };
        }

        public static Operator<int> Count<T>(this Operator<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Where(predicate).Count();
        }

        public static Operator<T> DistinctUntilChanged<T>(this Operator<T> source)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new DistinctUntilChangedFactory
                {
                    OutputType = typeof(T) // TODO: remove
                }
            };
        }

        public static Operator<T> First<T>(this Operator<T> source)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new FirstFactory
                {
                    OutputType = typeof(T) // TODO: remove
                }
            };
        }

        public static Operator<T> First<T>(this Operator<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Where(predicate).First();
        }

        public static Operator<T> FirstOrDefault<T>(this Operator<T> source)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new FirstOrDefaultFactory
                {
                    OutputType = typeof(T) // TODO: remove
                }
            };
        }

        public static Operator<T> FirstOrDefault<T>(this Operator<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Where(predicate).FirstOrDefault();
        }

        public static Operator<T> IgnoreElements<T>(this Operator<T> source)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new IgnoreElementsFactory
                {
                    OutputType = typeof(T) // TODO: remove
                }
            };
        }

        public static Operator<T> Last<T>(this Operator<T> source)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new LastFactory
                {
                    OutputType = typeof(T) // TODO: remove
                }
            };
        }

        public static Operator<T> Last<T>(this Operator<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Where(predicate).Last();
        }

        public static Operator<long> LongCount<T>(this Operator<T> source)
        {
            return new Operator<long>
            {
                Inputs = new[] { source },
                Factory = new LongCountFactory()
            };
        }

        public static Operator<long> LongCount<T>(this Operator<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Where(predicate).LongCount();
        }

        public static Operator<R> Select<T, R>(this Operator<T> source, Expression<Func<T, R>> selector)
        {
            return new Operator<R>
            {
                Inputs = new[] { source },
                Factory = new SelectFactory
                {
                    Selector = selector
                }
            };
        }

        public static Operator<R> Select<T, R>(this Operator<T> source, Expression<Func<T, int, R>> selector)
        {
            return new Operator<R>
            {
                Inputs = new[] { source },
                Factory = new SelectIndexedFactory
                {
                    Selector = selector
                }
            };
        }

        public static Operator<int> Sum(this Operator<int> source)
        {
            return new Operator<int>
            {
                Inputs = new[] { source },
                Factory = new SumFactory(typeof(int))
            };
        }

        public static Operator<int> Sum<T>(this Operator<T> source, Expression<Func<T, int>> selector)
        {
            return source.Select(selector).Sum();
        }

        public static Operator<T> Take<T>(this Operator<T> source, int count)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new TakeFactory
                {
                    Count = count,
                    OutputType = typeof(T) // TODO: remove
                }
            };
        }

        public static Operator<T> Where<T>(this Operator<T> source, Expression<Func<T, bool>> predicate)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new WhereFactory
                {
                    Predicate = predicate
                }
            };
        }

        public static Operator<T> Where<T>(this Operator<T> source, Expression<Func<T, int, bool>> predicate)
        {
            return new Operator<T>
            {
                Inputs = new[] { source },
                Factory = new WhereIndexedFactory
                {
                    Predicate = predicate
                }
            };
        }
    }
}
