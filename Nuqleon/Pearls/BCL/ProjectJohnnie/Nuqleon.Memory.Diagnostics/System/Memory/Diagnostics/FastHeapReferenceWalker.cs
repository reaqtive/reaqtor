// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/07/2017 - Created this type.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Provides a fast walker for an object heap by traversing object references in a way similar to the
    /// reachability analysis for live objects performed by the garbage collector. This walker is considered
    /// fast given that it doesn't provide any detailed information about the edges traversed. Instead, it
    /// only provides callbacks in the <see cref="Walk(object, Func{object, bool})"/> method, enabling a user
    /// to inspect all reachable objects, for example to collect them in a set.
    /// </summary>
    public sealed class FastHeapReferenceWalker
    {
        /// <summary>
        /// Singleton instance of a fast walker that doesn't continue the walk.
        /// </summary>
        private static readonly FastWalker s_nop = (obj, enqueue) => { };

        /// <summary>
        /// Singleton instance of <see cref="WalkerInfo"/> using <see cref="s_nop"/>. This instance is used
        /// to bail out from object graph walks and reference equality checks with this field are made to
        /// determine whether a recursively obtained fast walker is a no-op walker, allowing for propagation
        /// of optimizations.
        /// </summary>
        private static readonly WalkerInfo s_nopInfo = new(s_nop);

        //
        // CONSIDER: The following fields are global statics and could benefit from being user-supplied.
        //

        /// <summary>
        /// Table mapping types onto heap walker information entries.
        /// </summary>
        /// <remarks>
        /// A <see cref="ConditionalWeakTable{TKey, TValue}"/> is used to ensure we don't keep collectible
        /// types alive when a walker for such a type exists.
        /// </remarks>
        private static readonly ConditionalWeakTable<Type, WalkerInfo> s_walkers = new();

        /// <summary>
        /// A pool for <see cref="Queue{T}"/> instances that can be used to maintain the work list for
        /// heap walking in <see cref="Walk"/>. The initial capacity for queues is set to 1K to support non-
        /// trivial heap traversals. Given that a heap walk is non-reentrant, a maximum number of queues equal
        /// to the number of CPUs is reasonable. The maximum capacity is set to 128K, which results in a maximum
        /// memory use per queue of 1MB on 64 bit machines, which is rather small and enables scanning quite
        /// big heaps (e.g. containing an array of several thousands of elements).
        /// </summary>
        private static readonly QueuePool<object> s_queuePool = QueuePool<object>.Create(size: Environment.ProcessorCount, capacity: 1024, maxCapacity: 128 * 1024);

        /// <summary>
        /// A pool for <see cref="Dictionary{Type, FastWalker}"/> instances that can be used to maintain a local
        /// cache of walkers during the execution of the <see cref="Walk"/> algorithm. The use of such a cache
        /// can avoid the cost associated with performing a lookup in <see cref="s_walkers"/> which is subject
        /// to acquiring a lock. The capacities are chosen based on the expected numbers of distinct types on a
        /// heap, with a maximum of 32K (which is approximately 25% of the total number of types in mscorlib,
        /// System, and System.Core, excluding constructed types such as arrays and generics, thus very generous).
        /// </summary>
        /// <remarks>
        /// Note that the equality comparer for the dictionaries is the reference equality comparer. This matches
        /// the behavior of <see cref="ConditionalWeakTable{TKey, TValue}"/> used by <see cref="s_walkers"/>.
        /// </remarks>
        private static readonly DictionaryPool<Type, FastWalker> s_typeToWalkerPool = DictionaryPool<Type, FastWalker>.Create(comparer: ReferenceEqualityComparer<Type>.Instance, size: Environment.ProcessorCount, capacity: 1024, maxCapacity: 32 * 1024);

#pragma warning disable CA1822 // Mark members as static (support future config at instance level)
        /// <summary>
        /// Walks the specified object, following all reachable reference typed objects.
        /// </summary>
        /// <param name="obj">
        /// The object to start the walk from.
        /// </param>
        /// <param name="fence">
        /// Fence predicate representing a condition evaluated for every object encountered during the heap
        /// walk in order to determine whether further traversal is needed. When the predicate is applied to
        /// an object and returns <c>true</c>, the traversal continues. It's the caller's responsibility to
        /// supply a callback that detects object cycles in order to avoid infinite looping of the walk. The
        /// fence predicate does also get applied to the initial object <paramref name="obj"/>.
        /// </param>
        public void Walk(object obj, Func<object, bool> fence)
        {
            if (fence == null)
                throw new ArgumentNullException(nameof(fence));

            if (obj == null)
                return;

            //
            // CONSIDER: It's imagineable for some use cases of heap walking to take place in a resource-
            //           constrained environment. It may be worth to provide an overload with a YieldToken,
            //           allowing for the caller to pause and resume the algorithm (knowing that an increase
            //           in pause time reduces the confidence of the returned result given that the already
            //           traversed portion of the heap may change, and knowing that the paused algorithm can
            //           keep objects rooted for a prolonged time).
            //

            //
            // REVIEW: Evaluate the use of a stack instead of a queue, and determine whether the performance
            //         comparison is significant to warrant either a change of data structure or to offer
            //         different options to the caller.
            //

            using var queueHolder = s_queuePool.New();
            var queue = queueHolder.Queue;

            using var dicHolder = s_typeToWalkerPool.New();
            var walkerCache = dicHolder.Dictionary;

#pragma warning disable 1587
            /// <summary>
            /// Local helper function to obtain a walker from the local cache, falling back to the
            /// <see cref="GetWalker"/> method if no entry is found. This helps to avoid the cost
            /// associated with accessing the <see cref="s_walkers"/> shared collection.
            /// </summary>
            /// <param name="type">The type to get a walker for.</param>
            /// <returns>A walker delegate for the specified type.</returns>
#pragma warning restore 1587
            FastWalker GetCachedWalker(Type type)
            {
                if (!walkerCache.TryGetValue(type, out var walker))
                {
                    walker = GetWalker(type);
                    walkerCache[type] = walker;
                }

                return walker;
            }

            //
            // REVIEW: Use a delegate or make a call to a method?
            //

            var enqueue = new Action<object>(o =>
            {
                if (o != null)
                {
                    queue.Enqueue(o);
                }
            });

            queue.Enqueue(obj);

            while (queue.Count > 0)
            {
                var next = queue.Dequeue(); // NB: No null references can be enqueued, so the following is safe.

                if (fence(next))
                {
                    var type = next.GetType();

                    GetCachedWalker(type)(next, enqueue);
                }
            }
        }
#pragma warning restore CA1822

        /// <summary>
        /// Gets a walker for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get a walker for.</param>
        /// <returns>A <see cref="FastWalker"/> delegate representing the walker.</returns>
        private static FastWalker GetWalker(Type type)
        {
            if (type.IsPrimitive || type.IsEnum)
            {
                return s_nop;
            }

            return GetOrCreateWalkerInfo(type).Delegate;
        }

        /// <summary>
        /// Gets or creates a walker for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get a walker for.</param>
        /// <returns>A <see cref="WalkerInfo"/> object holding information about the walker.</returns>
        private static WalkerInfo GetOrCreateWalkerInfo(Type type)
        {
            if (!s_walkers.TryGetValue(type, out var walker))
            {
                walker = GetOrCreateWalkerInfoSlow(type);
            }

            return walker;
        }

        /// <summary>
        /// Gets or creates a walker for the specified <paramref name="type"/>, after a check determined that
        /// no walker is available yet. Note that the <see cref="s_walkers"/> collection is concurrency safe,
        /// so it's possible that a concurrent call also prepares a walker, which may get returned by the "get
        /// or add" operation within this method.
        /// </summary>
        /// <param name="type">The type to get a walker for.</param>
        /// <returns>A <see cref="WalkerInfo"/> object holding information about the walker.</returns>
        /// <remarks>
        /// This method allocates a delegate and is kept separate from <see cref="GetOrCreateWalkerInfo"/>
        /// in order to avoid this allocation in the common case where we already have a walker.
        /// </remarks>
        private static WalkerInfo GetOrCreateWalkerInfoSlow(Type type)
        {
            return s_walkers.GetValue(type, t =>
            {
                if (t.IsArray)
                {
                    return GetArrayWalkerInfo(t);
                }
                else
                {
                    return GetFieldWalkerInfo(t);
                }
            });
        }

        /// <summary>
        /// Gets a walker that traverses into reference typed fields of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get a walker for.</param>
        /// <returns>A walker for the specified <paramref name="type"/>.</returns>
        private static WalkerInfo GetFieldWalkerInfo(Type type)
        {
            //
            // We'll build a list with traversal patterns using the Access type. For a deeply nested struct
            // type, we can end up with deep traversals to reach all reference typed objects. E.g.:
            //
            //     struct Bar { Foo f; string s; }
            //     struct Foo { Qux q; string s; }
            //     struct Qux { int y; string s; }
            //
            // will yield the following traversal patterns:
            //
            //     .f.s
            //     .f.q.s
            //     .s
            //
            // By using the Access type, we can apply the access operation many times to different expressions,
            // which is beneficial when we want to inline the walking logic in a bigger walker, e.g. to walk
            // the elements of an array.
            //

            var traverse = new List<Access>();

#pragma warning disable 1587
            /// <summary>
            /// Recursive helper function to build the traversal patterns.
            /// </summary>
            /// <param name="access">The current access pattern (<c>null</c> for the initial call).</param>
            /// <param name="field">The field being accessed.</param>
#pragma warning restore 1587
            void ChaseReferences(Access access, FieldInfo field)
            {
                var fieldType = field.FieldType;

                if (!fieldType.IsByRef && !fieldType.IsPointer)
                {
                    var fieldAccess = Access.Field(field);

                    //
                    // PERF: Note that deeply nested field accesses can result in the repeated lookup of common
                    //       subexpressions (e.g. obj.x.y.z1 and obj.x.y.z2 both look up obj.x.y). Given that
                    //       we're dealing with value types, there's little we can do to reduce this, because
                    //       the expression tree APIs don't support ref locals.
                    //

                    var nextAccess = access == null ? (Access)fieldAccess : Access.Composite(access, fieldAccess);

                    if (fieldType.IsValueType)
                    {
                        foreach (var structField in fieldType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                        {
                            if (structField.FieldType != fieldType) // NB: Primitive types have this seemingly cyclic structure.
                            {
                                ChaseReferences(nextAccess, structField);
                            }
                        }
                    }
                    else
                    {
                        traverse.Add(nextAccess);
                    }
                }
            }

            //
            // Get all reference typed fields, including those inherited from base types. We perform this
            // analysis in the order of the base types, with least derived type first (cf. the step that reverses
            // the list of the types in the hierarchy). This improves locality because a walker will access the
            // fields in the order they typically occur in the object layout.
            //

            var types = new List<Type>();

            for (var t = type; t != null; t = t.BaseType)
            {
                types.Add(t);
            }

            types.Reverse();

            foreach (var t in types)
            {
                foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    ChaseReferences(access: null, field);
                }
            }

            //
            // If the type has no reference typed fields, we can simply return the the no-op walker. Note that
            // this optimization can be propagated, e.g. to array types whose element types have a no-op walker.
            //

            if (traverse.Count == 0)
            {
                return s_nopInfo;
            }
            else
            {
                //
                // We're gonna keep the traverse collection alive in a static, so we better be space efficient.
                // Given that we only calculate walkers once per type, we can afford to spend a few more cycles
                // to compact the representation by using an appropriately sized array. A nice side-effect is
                // that the expression factory built below can loop over an array rather than a list.
                //

                var traverseArray = traverse.ToArray();

                //
                // For not-trivial types, we'll build an expression factory. This will enable use to inline the
                // walker elsewhere, e.g. to walk the elements of an array.
                //

                var createExpression = new CreateFastWalker(/* [ref traverseArray] */ (objExpr, enqueueExpr) =>
                {
                    var exprs = new Expression[traverseArray.Length];

                    for (var i = 0; i < traverseArray.Length; i++)
                    {
                        exprs[i] = Expression.Invoke(enqueueExpr, traverseArray[i].ToExpression(objExpr));
                    }

                    return Expression.Block(typeof(void), exprs);
                });

                //
                // Convert the object being walked to the most derived type used for the walk and store it in
                // a local in order to avoid performing this conversion over and over again for each field.
                //

                var converted = Expression.Parameter(type);
                var convert = Expression.Assign(converted, Expression.Convert(Expressions.s_objParameter, type));

                //
                // Given that the expression factory returns a BlockExpression and we need to insert an
                // assignment statement, we'd end up with nested blocks if we were to use a naive implementation
                // here. Instead, we'll merge the blocks.
                //

                var fieldWalker = createExpression(converted, Expressions.s_enqueueParameter);

                var bodyExprs = new Expression[fieldWalker.Expressions.Count + 1];
                bodyExprs[0] = convert;
                fieldWalker.Expressions.CopyTo(bodyExprs, 1);

                var body = Expression.Block(typeof(void), new[] { converted }, bodyExprs);

                //
                // Finally, build a lambda expression representing the FastWalker delegate, compile it, and
                // return it together with the expression factory, so it can be reused for inlining purposes.
                //

                var lambda = Expression.Lambda<FastWalker>(body, Expressions.s_walkerParameters);
                var walker = lambda.Compile();
                return new WalkerInfo(walker, createExpression);
            }
        }

        /// <summary>
        /// Gets a walker for an array <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The array type to get a walker for.</param>
        /// <returns>A walker for the specified array <paramref name="type"/>.</returns>
        private static WalkerInfo GetArrayWalkerInfo(Type type)
        {
            Debug.Assert(type.IsArray);

            var elemType = type.GetElementType();

            //
            // First, get a walker for the element type. If there's nothing to walk for the elements of the
            // array, simply return the no-op walker here as well. Note that the array itself will still be
            // walked (that's how we got here in the first place), so it can be accounted for. ALso, reference
            // typed elements can't be skipped.
            //

            var elemWalker = GetOrCreateWalkerInfo(elemType);

            if (elemType.IsValueType && elemWalker == s_nopInfo)
            {
                return s_nopInfo;
            }

            //
            // Differentiate between the different kinds of arrays and dispatch to a specialized helper
            // method to build the appropriate walker.
            //

            if (elemType.MakeArrayType() == type)
            {
                return GetVectorWalkerInfo(type, elemType, elemWalker);
            }
            else
            {
                return GetMultidimensionalArrayWalkerInfo(type, elemType, elemWalker);
            }
        }

        /// <summary>
        /// Gets a walker for a single-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the single-dimensional array to get a walker for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemWalker">The walker to be used for the elements.</param>
        /// <returns>A walker for a single-dimensional array of the specified <paramref name="type"/>.</returns>
        private static WalkerInfo GetVectorWalkerInfo(Type type, Type elemType, WalkerInfo elemWalker)
        {
            Debug.Assert(type.IsArray && type.GetElementType() == elemType);

            if (elemType.IsValueType)
            {
                //
                // PERF: We want to avoid the allocation that comes from enumerating over the vector using a cast to
                //       the System.Array type. If we didn't bother about this, we could get away with a piece of
                //       statically compiled code:
                //
                //           return (obj, enqueue) =>
                //           {
                //               foreach (var element in (Array)obj) // PERF: This allocates an enumerator.
                //               {
                //                   elemWalker(element, enqueue);
                //               }
                //           };
                //
                //       If we end up generating a lot of code, it may be worth revisiting this to introduce a thunk
                //       that switches between the statically compiled code and the dynamically generated code once
                //       a type becomes popular. The main concern is with generic types which can lead to an increase
                //       in the number of distinct array types (e.g. KeyValuePair<,>[]).
                //

                //
                // First, check if we ever had an expression to walk objects of the array's element type. Given that
                // we're already post the check for a no-op element walker and we're dealing with value typed elements,
                // it is expected we can get a non-trivial expresssion to walk the elements.
                //

                Debug.Assert(elemWalker.ExpressionFactory != null);

                return GetVectorWalkerInfo(type, elemType, elemWalker.ExpressionFactory);
            }
            else
            {
                //
                // If the elements of the array are not value types, we can safely cast the array to an object[]
                // due to array covariance. That makes it trivial (and fast) to loop over the elements.
                //

                return new WalkerInfo(new FastWalker((obj, enqueue) =>
                {
                    foreach (var element in (object[])obj)
                    {
                        enqueue(element);
                    }
                }));
            }
        }

        /// <summary>
        /// Gets a walker for a single-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the single-dimensional array to get a walker for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemWalkerFactory">The factory to generate a walker expression to walk elements of the array.</param>
        /// <returns>A walker for a single-dimensional array of the specified <paramref name="type"/>.</returns>
        private static WalkerInfo GetVectorWalkerInfo(Type type, Type elemType, CreateFastWalker elemWalkerFactory)
        {
            //
            // Generate code that's equivalent to this:
            //
            //     (object obj, Action<object> enqueue) =>
            //     {
            //         var arr = (T[])obj;
            //
            //         for (int i = 0; i < arr.Length; ++i)
            //         {
            //             walk(arr[i]);
            //         }
            //     }
            //

            //
            // Variables for the loop over the array elements.
            //

            var arr = Expression.Parameter(type, "array");
            var i = Expressions.s_i;
            var len = Expressions.s_len;

            //
            // Evaluate the element type walker factory in order to achieve inlining here. By default,
            // we avoid creating a copy of the array element to a local which can cause a lot of copying
            // which is unnecessary. This behavior can be tweaked using USE_LOCAL.
            //

#if USE_LOCAL
            var elem = Expression.Parameter(elemType, "elem");
#else
            var elem = Expression.ArrayIndex(arr, i);
#endif
            var walkElemExpr = elemWalkerFactory(elem, Expressions.s_enqueueParameter);

            //
            // Prepare the loop body by inlining the element walker expressions, to avoid ending up with
            // nested block expressions which adds a bit of unnecessary cost.
            //

            var loopBodyExprsCount =
                1 /* break condition */ +
#if USE_LOCAL
                1 /* assign to local */ +
#endif
                1 /* index increment */ +
                walkElemExpr.Expressions.Count;

            var loopBodyExprs = new Expression[loopBodyExprsCount];

            loopBodyExprs[0] = Expressions.s_breakIndexEqualLength;
#if USE_LOCAL
            loopBodyExprs[1] =
                Expression.Assign(
                    elem,
                    Expression.ArrayIndex(arr, i)
                );
            walkElemExpr.Expressions.CopyTo(loopBodyExprs, 2);
#else
            walkElemExpr.Expressions.CopyTo(loopBodyExprs, 1);
#endif
            loopBodyExprs[loopBodyExprsCount - 1] = Expressions.s_preIncrementAssignIndex;

            var loopBody = Expression.Block(typeof(void), loopBodyExprs);

            //
            // Build the for (int i = 0; i < array.Length; i++) loop using the LoopExpression construct,
            // which requires some plumbing with break labels.
            //

            var exprs = new Expression[]
            {
                Expression.Assign(arr, Expression.Convert(Expressions.s_objParameter, type)),
                Expressions.s_indexAssign0,
                Expression.Assign(len, Expression.ArrayLength(arr)),

                Expression.Loop(
                    loopBody,
                    Expressions.s_brk
                )
            };

            var body = Expression.Block(
                typeof(void),
#if USE_LOCAL
                new[] { arr, i, len, elem },
#else
                new[] { arr, i, len },
#endif
                exprs
            );

            //
            // Construct and compile the lambda expression.
            //

            var lambda = Expression.Lambda<FastWalker>(body, Expressions.s_walkerParameters);

            var walker = lambda.Compile();

            //
            // We only return the walker delegate, without an expression factory. The reason is that
            // arrays are reference types, so we'll never be interested in inlining the expression in
            // another bigger walker.
            //

            return new WalkerInfo(walker);
        }

        /// <summary>
        /// Gets a walker for a multi-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the multi-dimensional array to get a walker for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemWalker">The walker to be used for the elements.</param>
        /// <returns>A walker for a multi-dimensional array of the specified <paramref name="type"/>.</returns>
        private static WalkerInfo GetMultidimensionalArrayWalkerInfo(Type type, Type elemType, WalkerInfo elemWalker)
        {
            Debug.Assert(type.IsArray && type.GetElementType() == elemType);

            if (elemType.IsValueType)
            {
#if NO_MD_EXPR
                //
                // NB: We consider multi-dimensional arrays to be relatively rare. In fact, in all of
                //     mscorlib, System, and System.Core, there are only four fields that are multi-
                //     dimensional arrays, at the time of writing (August 2017). These types include
                //     ListChunk <Pair<,>>[,] and are all found in System.Linq.Parallel. As such, we
                //     don't (yet) bother generating expression trees for these types.
                //

                var walkElem = elemWalker.Delegate;

                return new WalkerInfo((obj, enqueue) =>
                {
                    foreach (var element in (Array)obj) // NB: Causes an allocation of an enumerator.
                    {
                        walkElem(element, enqueue);
                    }
                });
#else
                //
                // NB: See remarks in GetVectorArrayWalkerInfo for the rationale on using expressions.
                //

                //
                // First, check if we ever had an expression to walk objects of the array's element type. Given that
                // we're already post the check for a no-op element walker and we're dealing with value typed elements,
                // it is expected we can get a non-trivial expresssion to walk the elements.
                //

                Debug.Assert(elemWalker.ExpressionFactory != null);

                return GetMultidimensionalArrayWalkerInfo(type, elemType, elemWalker.ExpressionFactory);
#endif
            }
            else
            {
                return new WalkerInfo((obj, enqueue) =>
                {
                    foreach (var element in (Array)obj) // NB: Causes an allocation of an enumerator.
                    {
                        enqueue(element);
                    }
                });
            }
        }

        /// <summary>
        /// Gets a walker for a multi-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the multi-dimensional array to get a walker for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemWalkerFactory">The factory to generate a walker expression to walk elements of the array.</param>
        /// <returns>A walker for a multi-dimensional array of the specified <paramref name="type"/>.</returns>
        private static WalkerInfo GetMultidimensionalArrayWalkerInfo(Type type, Type elemType, CreateFastWalker elemWalkerFactory)
        {
            _ = elemType;

            //
            // Generate code that's equivalent to this:
            //
            //     (object obj, Action<object> enqueue) =>
            //     {
            //         var arr = (T[,...,])obj;
            //
            //         for (int i1 = arr.GetLowerBound(0); i1 < arr.GetUpperBound(0); ++i0)
            //         {
            //          .
            //           .
            //            .
            //             for (int in = arr.GetLowerBound(n - 1); i1 < arr.GetUpperBound(n - 1); ++in)
            //             {
            //                 walk(arr[i0, ..., in]);
            //             }
            //            .
            //           .
            //          .
            //         }
            //     }
            //

            //
            // Use the rank of the array, which can be obtained from the static type, in order
            // to generate all the variables needed.
            //

            var rank = type.GetArrayRank();

            //
            // Variables for the loop over the array elements.
            //

            var arr = Expression.Parameter(type, "array");
            var indexes = new ParameterExpression[rank];
            var lowerBounds = new ParameterExpression[rank];
            var upperBounds = new ParameterExpression[rank];

            var variableCount =
                1 /* arr */ +
                3 * rank /* indexes, lowerBounds, upperBounds */
                ;

            var variables = new ParameterExpression[variableCount];
            var varIndex = 0;

            //
            // Expressions for the body of the walker.
            //

            var exprCount =
                1 /* assign array variable */ +
                2 * rank /* assign lowerBounds, and upperBounds */ +
                1 /* loop */
                ;

            var exprs = new Expression[exprCount];
            var exprIndex = 0;

            //
            // First, add the assignment of the arr variable.
            //

            variables[varIndex++] = arr;
            exprs[exprIndex++] = Expression.Assign(arr, Expression.Convert(Expressions.s_objParameter, type));

            //
            // Second, add the assignments of the bounds.
            //

            for (var i = 0; i < rank; i++)
            {
                var index = Expression.Parameter(typeof(int), "i" + i);
                var lowerBound = Expression.Parameter(typeof(int), "l" + i);
                var upperBound = Expression.Parameter(typeof(int), "u" + i);

                indexes[i] = index;
                lowerBounds[i] = lowerBound;
                upperBounds[i] = upperBound;

                variables[varIndex++] = index;
                variables[varIndex++] = lowerBound;
                variables[varIndex++] = upperBound;

                var dimension = Expression.Constant(i);

                exprs[exprIndex++] = Expression.Assign(lowerBound, Expression.Call(arr, Expressions.s_getLowerBound, dimension));
                exprs[exprIndex++] = Expression.Assign(upperBound, Expression.Call(arr, Expressions.s_getUpperBound, dimension));
            }

            //
            // Third, prepare the body of the innermost loop.
            //

            var elem = Expression.ArrayAccess(arr, indexes);

            var walkElemExpr = elemWalkerFactory(elem, Expressions.s_enqueueParameter);

            var loopBody = walkElemExpr;

            //
            // Fourth, generate the nested loops.
            //

            Debug.Assert(exprIndex == exprs.Length - 1);

            for (var i = rank - 1; i >= 0; i--)
            {
                //
                // Get the variables used in the loop.
                //

                var index = indexes[i];
                var lowerBound = lowerBounds[i];
                var upperBound = upperBounds[i];

                //
                // Generate a break label for exiting the current loop.
                //

                var brk = Expression.Label();

                //
                // Create the loop body and flatten the block of the previous iteration.
                //

                var loopBodyExprs = loopBody.Expressions;

                var newLoopBodyExprCount =
                    1 /* condition */ +
                    loopBodyExprs.Count /* flatten */ +
                    1 /* increment*/;

                var newLoopBodyExprs = new Expression[newLoopBodyExprCount];

                newLoopBodyExprs[0] =
                    Expression.IfThen(
                        Expression.GreaterThan(index, upperBound),
                        Expression.Break(brk)
                    );

                loopBodyExprs.CopyTo(newLoopBodyExprs, 1);

                newLoopBodyExprs[newLoopBodyExprCount - 1] =
                    Expression.PostIncrementAssign(
                        index
                    );

                var newLoopBody = Expression.Block(newLoopBodyExprs);

                //
                // Build the loop.
                //

                loopBody =
                    Expression.Block(
                        typeof(void),
                        Expression.Assign(index, lowerBound),
                        Expression.Loop(
                            newLoopBody,
                            brk
                        )
                    );
            }

            exprs[exprIndex++] = loopBody; // NB: We could flatten this block.

            //
            // Finally, generate the block.
            //

            var body =
                Expression.Block(
                    typeof(void),
                    variables,
                    exprs
                );

            //
            // Construct and compile the lambda expression.
            //

            var lambda = Expression.Lambda<FastWalker>(body, Expressions.s_walkerParameters);

            var walker = lambda.Compile();

            //
            // We only return the walker delegate, without an expression factory. The reason is that
            // arrays are reference types, so we'll never be interested in inlining the expression in
            // another bigger walker.
            //

            return new WalkerInfo(walker);
        }

        /// <summary>
        /// Delegate type for fast object heap walkers.
        /// </summary>
        /// <param name="obj">The object being visited.</param>
        /// <param name="enqueue">The callback used to enqueue the next objects to walk.</param>
        private delegate void FastWalker(object obj, Action<object> enqueue);

        /// <summary>
        /// Delegate type for code generation of fast walker delegates.
        /// </summary>
        /// <param name="obj">The expression representing the object being visited.</param>
        /// <param name="enqueue">The expression representing the callback used to the enqueue the next objects to walk.</param>
        /// <returns>An expression representing a <see cref="FastWalker"/>.</returns>
        private delegate BlockExpression CreateFastWalker(Expression obj, Expression enqueue);

        /// <summary>
        /// Class holding information about a walker for a type. Besides holding a <see cref="FastWalker"/> delegate, a value
        /// of this type may also hold an expression factory which can be used to inline the logic for a walker in a bigger
        /// walker, e.g. for walking the elements of an array whose element type is a value type.
        /// </summary>
        private sealed class WalkerInfo
        {
            /// <summary>
            /// Creates a new walker information instance for a walker without an expression factory.
            /// </summary>
            /// <param name="walker">The walker delegate.</param>
            public WalkerInfo(FastWalker walker)
            {
                Delegate = walker;
                ExpressionFactory = null;
            }

            /// <summary>
            /// Creates a new walker information instance for a walker that has a corresponding expression factory.
            /// </summary>
            /// <param name="walker">The walker delegate.</param>
            /// <param name="expressionFactory">The expression factory that can be used to create a walker delegate.</param>
            public WalkerInfo(FastWalker walker, CreateFastWalker expressionFactory)
            {
                Delegate = walker;
                ExpressionFactory = expressionFactory;
            }

            /// <summary>
            /// Gets the walker delegate. This property is guaranteed to be non-<c>null</c>.
            /// </summary>
            public FastWalker Delegate { get; }

            /// <summary>
            /// Gets the factory that can be used to create a walker delegate. This property can be <c>null</c>.
            /// </summary>
            public CreateFastWalker ExpressionFactory { get; }
        }

        /// <summary>
        /// Provides a set of shared expression tree fragments in order to reduce memory allocations.
        /// </summary>
        private static class Expressions
        {
            /// <summary>
            /// The first parameter of an expression representing a <see cref="FastWalker"/> delegate.
            /// </summary>
            public static readonly ParameterExpression s_objParameter = Expression.Parameter(typeof(object), "obj");

            /// <summary>
            /// The second parameter of an expression representing a <see cref="FastWalker"/> delegate.
            /// </summary>
            public static readonly ParameterExpression s_enqueueParameter = Expression.Parameter(typeof(Action<object>), "enqueue");

            /// <summary>
            /// The collection of parameters to use for an expression representing a <see cref="FastWalker"/> delegate.
            /// </summary>
            public static readonly ReadOnlyCollection<ParameterExpression> s_walkerParameters = Expression.Lambda(Expression.Empty(), s_objParameter, s_enqueueParameter).Parameters;

            /// <summary>
            /// A variable of type <see cref="int"/> used to represent an index variable for a loop over an array.
            /// </summary>
            public static readonly ParameterExpression s_i = Expression.Parameter(typeof(int), "i");

            /// <summary>
            /// An expression representing the assignment of a constant with an <see cref="int"/> value of <c>0</c> to <see cref="s_i"/>.
            /// </summary>
            public static readonly Expression s_indexAssign0 = Expression.Assign(s_i, Expression.Constant(0));

            /// <summary>
            /// A variable of type <see cref="int"/> used to represent the length of an array.
            /// </summary>
            public static readonly ParameterExpression s_len = Expression.Parameter(typeof(int), "len");

            /// <summary>
            /// Label target representing the break label of a loop.
            /// </summary>
            public static readonly LabelTarget s_brk = Expression.Label();

            /// <summary>
            /// An expression representing the break condition for an index-based loop using <see cref="s_i"/>, <see cref="s_len"/>, and <see cref="s_brk"/>.
            /// </summary>
            public static readonly Expression s_breakIndexEqualLength = Expression.IfThen(Expression.Equal(s_i, s_len), Expression.Break(s_brk));

            /// <summary>
            /// An expression representing the index increment step for an index-based loop using <see cref="s_i"/>.
            /// </summary>
            public static readonly Expression s_preIncrementAssignIndex = Expression.PreIncrementAssign(s_i); // NB: This avoids allocation of a local compared to PostIncrementAssign.;

            /// <summary>
            /// A <see cref="MethodInfo"/> object representing the <see cref="Array.GetLowerBound(int)"/> method.
            /// </summary>
            public static readonly MethodInfo s_getLowerBound = typeof(Array).GetMethod(nameof(Array.GetLowerBound));

            /// <summary>
            /// A <see cref="MethodInfo"/> object representing the <see cref="Array.GetUpperBound(int)"/> method.
            /// </summary>
            public static readonly MethodInfo s_getUpperBound = typeof(Array).GetMethod(nameof(Array.GetUpperBound));
        }
    }
}
