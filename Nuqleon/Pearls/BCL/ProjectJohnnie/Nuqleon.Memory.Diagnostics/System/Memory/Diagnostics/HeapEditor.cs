// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Utility to make edits to object graphs.
    /// </summary>
    public class HeapEditor
    {
        /// <summary>
        /// Singleton instance of an editor that doesn't continue walking the object graph.
        /// </summary>
        private static readonly Editor s_nop = (obj, edit, enqueue) => { };

        /// <summary>
        /// Singleton instance of <see cref="EditorInfo"/> using <see cref="s_nop"/>. This instance is used
        /// to bail out from object graph walks and reference equality checks with this field are made to
        /// determine whether a recursively obtained editor is a no-op edutir, allowing for propagation of
        /// optimizations.
        /// </summary>
        private static readonly EditorInfo s_nopInfo = new(s_nop);

        //
        // CONSIDER: The following fields are global statics and could benefit from being user-supplied.
        //

        /// <summary>
        /// Table mapping types onto heap editor information entries.
        /// </summary>
        /// <remarks>
        /// A <see cref="ConditionalWeakTable{TKey, TValue}"/> is used to ensure we don't keep collectible
        /// types alive when an editor for such a type exists.
        /// </remarks>
        private static readonly ConditionalWeakTable<Type, EditorInfo> s_editors = new();

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
        /// A pool for <see cref="ObjectSet"/> instances that can be used to perform cycle detection during
        /// heap walking in<see cref="Walk"/>.
        /// </summary>
        private static readonly ObjectPool<ObjectSet> s_setPool = new(() => new ObjectSet(), size: Environment.ProcessorCount);

        /// <summary>
        /// A pool for <see cref="Dictionary{Type, Editor}"/> instances that can be used to maintain a local
        /// cache of editors during the execution of the <see cref="Walk"/> algorithm. The use of such a cache
        /// can avoid the cost associated with performing a lookup in <see cref="s_editors"/> which is subject
        /// to acquiring a lock. The capacities are chosen based on the expected numbers of distinct types on a
        /// heap, with a maximum of 32K (which is approximately 25% of the total number of types in mscorlib,
        /// System, and System.Core, excluding constructed types such as arrays and generics, thus very generous).
        /// </summary>
        /// <remarks>
        /// Note that the equality comparer for the dictionaries is the reference equality comparer. This matches
        /// the behavior of <see cref="ConditionalWeakTable{TKey, TValue}"/> used by <see cref="s_editors"/>.
        /// </remarks>
        private static readonly DictionaryPool<Type, Editor> s_typeToEditorPool = DictionaryPool<Type, Editor>.Create(comparer: ReferenceEqualityComparer<Type>.Instance, size: Environment.ProcessorCount, capacity: 1024, maxCapacity: 32 * 1024);

        /// <summary>
        /// Walks the specified object, following all reachable reference typed objects, which will be passed
        /// in calls to the <see cref="Edit"/> method, allowing for edits to the object graph.
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

            using var setHolder = s_setPool.New();
            var set = setHolder.Object;

            using var queueHolder = s_queuePool.New();
            var queue = queueHolder.Queue;

            using var dicHolder = s_typeToEditorPool.New();
            var editorCache = dicHolder.Dictionary;

#pragma warning disable 1587
            /// <summary>
            /// Local helper function to obtain an editor from the local cache, falling back to the
            /// <see cref="GetEditor"/> method if no entry is found. This helps to avoid the cost
            /// associated with accessing the <see cref="s_editors"/> shared collection.
            /// </summary>
            /// <param name="type">The type to get an editor for.</param>
            /// <returns>An editor delegate for the specified type.</returns>
#pragma warning restore 1587
            Editor GetCachedEditor(Type type)
            {
                if (!editorCache.TryGetValue(type, out var editor))
                {
                    editor = GetEditor(type);
                    editorCache[type] = editor;
                }

                return editor;
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

            var edit = new Func<object, object>(Edit);

            queue.Enqueue(obj);

            while (queue.Count > 0)
            {
                var next = queue.Dequeue(); // NB: No null references can be enqueued, so the following is safe.

                //
                // NB: Unlike the FastHeapReferenceWalker, we're doing cycling detection ourselves
                //     here. We should consider making this behavior consistent in both places.
                //

                if (set.Add(next) && fence(next))
                {
                    var type = next.GetType();

                    GetCachedEditor(type)(next, edit, enqueue);
                }
            }
        }

        /// <summary>
        /// Edits the specified object found during the heap walk.
        /// </summary>
        /// <param name="obj">The object to edit.</param>
        /// <returns>The original object or a substitute for the object.</returns>
        public virtual object Edit(object obj)
        {
            return obj;
        }

        /// <summary>
        /// Gets an editor for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get an editor for.</param>
        /// <returns>An <see cref="Editor"/> delegate representing the editor.</returns>
        private static Editor GetEditor(Type type)
        {
            if (type.IsPrimitive || type.IsEnum)
            {
                return s_nop;
            }

            return GetOrCreateEditorInfo(type).Delegate;
        }

        /// <summary>
        /// Gets or creates an editor for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get an editor for.</param>
        /// <returns>An <see cref="EditorInfo"/> object holding information about the editor.</returns>
        private static EditorInfo GetOrCreateEditorInfo(Type type)
        {
            if (!s_editors.TryGetValue(type, out var editor))
            {
                editor = GetOrCreateEditorInfoSlow(type);
            }

            return editor;
        }

        /// <summary>
        /// Gets or creates an editor for the specified <paramref name="type"/>, after a check determined that
        /// no editor is available yet. Note that the <see cref="s_editors"/> collection is concurrency safe,
        /// so it's possible that a concurrent call also prepares an editor, which may get returned by the "get
        /// or add" operation within this method.
        /// </summary>
        /// <param name="type">The type to get an editor for.</param>
        /// <returns>A <see cref="EditorInfo"/> object holding information about the editor.</returns>
        /// <remarks>
        /// This method allocates a delegate and is kept separate from <see cref="GetOrCreateEditorInfo"/>
        /// in order to avoid this allocation in the common case where we already have an editor.
        /// </remarks>
        private static EditorInfo GetOrCreateEditorInfoSlow(Type type)
        {
            return s_editors.GetValue(type, t =>
            {
                if (t.IsArray)
                {
                    return GetArrayEditorInfo(t);
                }
                else
                {
                    return GetFieldEditorInfo(t);
                }
            });
        }

        /// <summary>
        /// Gets an editor that traverses into reference typed fields of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get an editor for.</param>
        /// <returns>An editor for the specified <paramref name="type"/>.</returns>
        private static EditorInfo GetFieldEditorInfo(Type type)
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
            // which is beneficial when we want to inline the editing logic in a bigger editor, e.g. to edit
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
            // the list of the types in the hierarchy). This improves locality because an editor will access the
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
            // If the type has no reference typed fields, we can simply return the the no-op editor. Note that
            // this optimization can be propagated, e.g. to array types whose element types have a no-op editor.
            //

            if (traverse.Count == 0)
            {
                return s_nopInfo;
            }
            else
            {
                //
                // We're gonna keep the traverse collection alive in a static, so we better be space efficient.
                // Given that we only calculate editors once per type, we can afford to spend a few more cycles
                // to compact the representation by using an appropriately sized array. A nice side-effect is
                // that the expression factory built below can loop over an array rather than a list.
                //

                var traverseArray = traverse.ToArray();

                //
                // For not-trivial types, we'll build an expression factory. This will enable use to inline the
                // editor elsewhere, e.g. to edit the elements of an array.
                //

                var createExpression = new CreateEditor(/* [ref traverseArray] */ (objExpr, editExpr, enqueueExpr) =>
                {
                    var exprs = new List<Expression>(traverseArray.Length * 4);

                    for (var i = 0; i < traverseArray.Length; i++)
                    {
                        var traverseExpr = traverseArray[i].ToExpression(objExpr);

#if NO_HIJACK
                        if (!CanWrite(traverseExpr))
                        {
                            exprs.Add(
                                Expression.Invoke(
                                    enqueueExpr,
                                    traverseExpr
                                )
                            );
                        }
                        else
#endif
                        {
                            exprs.Add(
                                Expression.Assign(
                                    Expressions.s_objBefore,
                                    traverseExpr
                                )
                            );

                            exprs.Add(
                                Expression.Assign(
                                    Expressions.s_objEdited,
                                    Expression.Invoke(
                                        editExpr,
                                        Expressions.s_objBefore
                                    )
                                )
                            );

                            var convertedEdited =
                                Expression.Convert(
                                    Expressions.s_objEdited,
                                    traverseExpr.Type
                                );

                            var assign = default(Expression);

#if !NO_HIJACK
                            //
                            // If "hijacking" is enabled, we can assign even to read-only fields. Even though
                            // the expression tree APIs don't allow us to do so, we can go an alternative
                            // route through reflection or reflection emit to achieve this.
                            //

                            if (!CanWrite(traverseExpr))
                            {
                                //
                                // NB: We know that traversals can only be deemed non-writeable in case they
                                //     are field accesses, so the casts below are safe.
                                //

                                var memberExpr = (MemberExpression)traverseExpr;
                                var memberObjExpr = memberExpr.Expression;
                                var field = (FieldInfo)memberExpr.Member;

                                //
                                // If the target is a value type, we'd be mutating a copy if we were to pass
                                // the member expression by value, so we need to perform write-backs.
                                //

                                if (memberObjExpr.Type.IsValueType)
                                {
                                    var hijack = FieldHijacker.GetFieldSetter(field);

                                    var boxedTarget = Expression.Parameter(typeof(object));

                                    var writeBack = default(Expression);

                                    if (CanWrite(memberObjExpr))
                                    {
                                        writeBack =
                                            Expression.Assign(
                                                memberObjExpr,
                                                Expression.Convert(
                                                    boxedTarget,
                                                    memberObjExpr.Type
                                                )
                                            );
                                    }
                                    else
                                    {
                                        writeBack = Expression.Empty(); // TODO: Write-backs are shallow right now
                                    }

                                    assign =
                                        Expression.Block(
                                            typeof(void),
                                            new[] { boxedTarget },
                                            Expression.Assign(
                                                boxedTarget,
                                                Expression.Convert(
                                                    memberObjExpr,
                                                    typeof(object)
                                                )
                                            ),
                                            Expression.Invoke(
                                                Expression.Constant(hijack),
                                                boxedTarget,
                                                convertedEdited
                                            ),
                                            writeBack
                                        );
                                }
                                else
                                {
                                    var hijack = FieldHijacker.GetFieldSetter(field);

                                    assign =
                                        Expression.Invoke(
                                            Expression.Constant(hijack),
                                            memberObjExpr,
                                            convertedEdited
                                        );
                                }
                            }
                            else
#endif
                            {
                                assign =
                                    Expression.Assign(
                                        traverseExpr,
                                        convertedEdited
                                    );
                            }

                            exprs.Add(
                                Expression.IfThen(
                                    Expression.ReferenceNotEqual(
                                        Expressions.s_objBefore,
                                        Expressions.s_objEdited
                                    ),
                                    assign
                                )
                            );

                            exprs.Add(
                                Expression.Invoke(
                                    enqueueExpr,
                                    Expressions.s_objEdited // REVIEW: Do we want to walk the result of edit?
                                )
                            );
                        }
                    }

                    return Expression.Block(typeof(void), new[] { Expressions.s_objBefore, Expressions.s_objEdited }, exprs);
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

                var fieldWalker = createExpression(converted, Expressions.s_editParameter, Expressions.s_enqueueParameter);

                var bodyExprs = new Expression[fieldWalker.Expressions.Count + 1];
                bodyExprs[0] = convert;
                fieldWalker.Expressions.CopyTo(bodyExprs, 1);

                var body = Expression.Block(typeof(void), new[] { converted, Expressions.s_objBefore, Expressions.s_objEdited }, bodyExprs);

                //
                // Finally, build a lambda expression representing the Editor delegate, compile it, and
                // return it together with the expression factory, so it can be reused for inlining purposes.
                //

                var lambda = Expression.Lambda<Editor>(body, Expressions.s_editorParameters);
                var walker = lambda.Compile();
                return new EditorInfo(walker, createExpression);
            }
        }

        /// <summary>
        /// Checks whether the specified <paramref name="expression"/> is writeable and can be used as
        /// lhs of a <see cref="BinaryExpression"/> of type <see cref="ExpressionType.Assign"/>.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns></returns>
        private static bool CanWrite(Expression expression)
        {
            if (expression is MemberExpression member)
            {
                switch (member.Member)
                {
                    case FieldInfo field:
                        return !field.IsLiteral && !field.IsInitOnly;
                    case PropertyInfo property:
                        return property.CanWrite;
                }
            }

            //
            // NB: We only expect MemberExpression and IndexExpression right now, we so don't have
            //     to add more cases.
            //

            return true;
        }

        /// <summary>
        /// Gets an editor for an array <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The array type to get an editor for.</param>
        /// <returns>An editor for the specified array <paramref name="type"/>.</returns>
        private static EditorInfo GetArrayEditorInfo(Type type)
        {
            Debug.Assert(type.IsArray);

            var elemType = type.GetElementType();

            //
            // First, get an editor for the element type. If there's nothing to walk for the elements of the
            // array, simply return the no-op editor here as well. Note that the array itself will still be
            // walked (that's how we got here in the first place), so it can be accounted for. ALso, reference
            // typed elements can't be skipped.
            //

            var elemEditor = GetOrCreateEditorInfo(elemType);

            if (elemType.IsValueType && elemEditor == s_nopInfo)
            {
                return s_nopInfo;
            }

            //
            // Differentiate between the different kinds of arrays and dispatch to a specialized helper
            // method to build the appropriate editor.
            //

            if (elemType.MakeArrayType() == type)
            {
                return GetVectorEditorInfo(type, elemType, elemEditor);
            }
            else
            {
                return GetMultidimensionalArrayEditorInfo(type, elemType, elemEditor);
            }
        }

        /// <summary>
        /// Gets an editor for a single-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the single-dimensional array to get an editor for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemEditor">The editor to be used for the elements.</param>
        /// <returns>An editor for a single-dimensional array of the specified <paramref name="type"/>.</returns>
        private static EditorInfo GetVectorEditorInfo(Type type, Type elemType, EditorInfo elemEditor)
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
                // we're already post the check for a no-op element editor and we're dealing with value typed elements,
                // it is expected we can get a non-trivial expresssion to walk and edit the elements.
                //

                Debug.Assert(elemEditor.ExpressionFactory != null);

                return GetVectorEditorInfo(type, elemType, elemEditor.ExpressionFactory);
            }
            else
            {
                //
                // If the elements of the array are not value types, we can safely cast the array to an object[]
                // due to array covariance. That makes it trivial (and fast) to loop over the elements.
                //

                return new EditorInfo(new Editor((obj, edit, enqueue) =>
                {
                    var array = (object[])obj;

                    for (var i = 0; i < array.Length; i++)
                    {
                        var element = array[i];

                        var newElement = edit(element);

                        if (newElement != element)
                        {
                            array[i] = newElement;
                            element = newElement;
                        }

                        enqueue(newElement); // REVIEW:  Do we want to walk the result of edit?
                    }
                }));
            }
        }

        /// <summary>
        /// Gets an editor for a single-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the single-dimensional array to get an editor for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemEditorFactory">The factory to generate an editor expression to walk and edit elements of the array.</param>
        /// <returns>An editor for a single-dimensional array of the specified <paramref name="type"/>.</returns>
        private static EditorInfo GetVectorEditorInfo(Type type, Type elemType, CreateEditor elemEditorFactory)
        {
            //
            // Generate code that's equivalent to this:
            //
            //     (object obj, Func<object, object> edit, Action<object> enqueue) =>
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
            // Evaluate the element type editor factory in order to achieve inlining here. By default,
            // we avoid creating a copy of the array element to a local which can cause a lot of copying
            // which is unnecessary. This behavior can be tweaked using USE_LOCAL.
            //

#if USE_LOCAL
            var elem = Expression.Parameter(elemType, "elem");
#else
            var elem = Expression.ArrayAccess(arr, i);
#endif
            var editElemExpr = elemEditorFactory(elem, Expressions.s_editParameter, Expressions.s_enqueueParameter);

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
                editElemExpr.Expressions.Count;

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
            editElemExpr.Expressions.CopyTo(loopBodyExprs, 1);
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
                new[] { arr, i, len, elem, Expressions.s_objBefore, Expressions.s_objEdited },
#else
                new[] { arr, i, len, Expressions.s_objBefore, Expressions.s_objEdited },
#endif
                exprs
            );

            //
            // Construct and compile the lambda expression.
            //

            var lambda = Expression.Lambda<Editor>(body, Expressions.s_editorParameters);

            var editor = lambda.Compile();

            //
            // We only return the editor delegate, without an expression factory. The reason is that
            // arrays are reference types, so we'll never be interested in inlining the expression in
            // another bigger editor.
            //

            return new EditorInfo(editor);
        }

        /// <summary>
        /// Gets an editor for a multi-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the multi-dimensional array to get a walker for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemEditor">The editor to be used for the elements.</param>
        /// <returns>An editor for a multi-dimensional array of the specified <paramref name="type"/>.</returns>
        private static EditorInfo GetMultidimensionalArrayEditorInfo(Type type, Type elemType, EditorInfo elemEditor)
        {
            Debug.Assert(type.IsArray && type.GetElementType() == elemType);

            if (elemType.IsValueType)
            {
                //
                // First, check if we ever had an expression to walk objects of the array's element type. Given that
                // we're already post the check for a no-op element walker and we're dealing with value typed elements,
                // it is expected we can get a non-trivial expresssion to walk the elements.
                //

                Debug.Assert(elemEditor.ExpressionFactory != null);

                return GetMultidimensionalArrayEditorInfo(type, elemType, elemEditor.ExpressionFactory);
            }
            else
            {
                return GetMultidimensionalArrayEditorInfo(type, elemType, default(CreateEditor));
            }
        }

        /// <summary>
        /// Gets an editor for a multi-dimensional array of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the multi-dimensional array to get an editor for.</param>
        /// <param name="elemType">The element type of the array.</param>
        /// <param name="elemEditorFactory">The factory to generate an editor expression to walk and edit elements of the array, or <c>null</c> for reference types.</param>
        /// <returns>An editor for a multi-dimensional array of the specified <paramref name="type"/>.</returns>
        private static EditorInfo GetMultidimensionalArrayEditorInfo(Type type, Type elemType, CreateEditor elemEditorFactory)
        {
            //
            // Generate code that's equivalent to this:
            //
            //     (object obj, Func<object, object> edit, Action<object> enqueue) =>
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
            //                 arr[i0, ..., in] = edit(arr[i0, ..., in]);
            //                 enqueue(arr[i0, ..., in]);
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
                2 /* objBefore, objEdited */ +
                1 /* arr */ +
                3 * rank /* indexes, lowerBounds, upperBounds */
                ;

            var variables = new ParameterExpression[variableCount];
            var varIndex = 0;

            variables[varIndex++] = Expressions.s_objBefore;
            variables[varIndex++] = Expressions.s_objEdited;

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

            BlockExpression loopBody;

            if (elemEditorFactory == null)
            {
                loopBody =
                    Expression.Block(
                        typeof(void),
                        Expression.Assign(
                            Expressions.s_objBefore,
                            elem
                        ),
                        Expression.Assign(
                            Expressions.s_objEdited,
                            Expression.Invoke(
                                Expressions.s_editParameter,
                                Expressions.s_objBefore
                            )
                        ),
                        Expression.IfThen(
                            Expression.ReferenceNotEqual(
                                Expressions.s_objBefore,
                                Expressions.s_objEdited
                            ),
                            Expression.Assign(
                                elem,
                                Expression.Convert(
                                    Expressions.s_objEdited,
                                    elemType
                                )
                            )
                        ),
                        Expression.Invoke(
                            Expressions.s_enqueueParameter,
                            Expressions.s_objEdited
                        )
                    );

            }
            else
            {
                var walkElemExpr = elemEditorFactory(elem, Expressions.s_editParameter, Expressions.s_enqueueParameter);

                loopBody = walkElemExpr;
            }

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

            var lambda = Expression.Lambda<Editor>(body, Expressions.s_editorParameters);

            var editor = lambda.Compile();

            //
            // We only return the editor delegate, without an expression factory. The reason is that
            // arrays are reference types, so we'll never be interested in inlining the expression in
            // another bigger editor.
            //

            return new EditorInfo(editor);
        }

        /// <summary>
        /// Delegate type for object heap editors.
        /// </summary>
        /// <param name="obj">The object being visited.</param>
        /// <param name="edit">The callback used to perform an edit on an object.</param>
        /// <param name="enqueue">The callback used to enqueue the next objects to walk.</param>
        private delegate void Editor(object obj, Func<object, object> edit, Action<object> enqueue);

        /// <summary>
        /// Delegate type for code generation of editor delegates.
        /// </summary>
        /// <param name="obj">The expression representing the object being visited.</param>
        /// <param name="edit">The expression representing the callback used to perform an edit on an object.</param>
        /// <param name="enqueue">The expression representing the callback used to the enqueue the next objects to walk.</param>
        /// <returns>An expression representing an <see cref="Editor"/>.</returns>
        private delegate BlockExpression CreateEditor(Expression obj, Expression edit, Expression enqueue);

        /// <summary>
        /// Class holding information about an editor for a type. Besides holding a <see cref="Editor"/> delegate, a value
        /// of this type may also hold an expression factory which can be used to inline the logic for an editor in a bigger
        /// editor, e.g. for editing the elements of an array whose element type is a value type.
        /// </summary>
        private sealed class EditorInfo
        {
            /// <summary>
            /// Creates a new editor information instance for an editor without an expression factory.
            /// </summary>
            /// <param name="editor">The editor delegate.</param>
            public EditorInfo(Editor editor)
            {
                Delegate = editor;
                ExpressionFactory = null;
            }

            /// <summary>
            /// Creates a new editor information instance for an editor that has a corresponding expression factory.
            /// </summary>
            /// <param name="editor">The editor delegate.</param>
            /// <param name="expressionFactory">The expression factory that can be used to create an editor delegate.</param>
            public EditorInfo(Editor editor, CreateEditor expressionFactory)
            {
                Delegate = editor;
                ExpressionFactory = expressionFactory;
            }

            /// <summary>
            /// Gets the editor delegate. This property is guaranteed to be non-<c>null</c>.
            /// </summary>
            public Editor Delegate { get; }

            /// <summary>
            /// Gets the factory that can be used to create an editor delegate. This property can be <c>null</c>.
            /// </summary>
            public CreateEditor ExpressionFactory { get; }
        }

        /// <summary>
        /// Provides a set of shared expression tree fragments in order to reduce memory allocations.
        /// </summary>
        private static class Expressions
        {
            /// <summary>
            /// The first parameter of an expression representing an <see cref="Editor"/> delegate.
            /// </summary>
            public static readonly ParameterExpression s_objParameter = Expression.Parameter(typeof(object), "obj");

            /// <summary>
            /// The second parameter of an expression representing an <see cref="Editor"/> delegate.
            /// </summary>
            public static readonly ParameterExpression s_editParameter = Expression.Parameter(typeof(Func<object, object>), "edit");

            /// <summary>
            /// The third parameter of an expression representing an <see cref="Editor"/> delegate.
            /// </summary>
            public static readonly ParameterExpression s_enqueueParameter = Expression.Parameter(typeof(Action<object>), "enqueue");

            /// <summary>
            /// The collection of parameters to use for an expression representing an <see cref="Editor"/> delegate.
            /// </summary>
            public static readonly ReadOnlyCollection<ParameterExpression> s_editorParameters = Expression.Lambda(Expression.Empty(), s_objParameter, s_editParameter, s_enqueueParameter).Parameters;

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
            /// A variable of type <see cref="object"/> used to store the result of performing a traversal.
            /// </summary>
            public static readonly ParameterExpression s_objBefore = Expression.Parameter(typeof(object));

            /// <summary>
            /// A variable of type <see cref="object"/> used to store the result of applying the edit function to <see cref="s_objBefore"/>.
            /// </summary>
            public static readonly ParameterExpression s_objEdited = Expression.Parameter(typeof(object));

            /// <summary>
            /// A <see cref="MethodInfo"/> object representing the <see cref="Array.GetLowerBound(int)"/> method.
            /// </summary>
            public static readonly MethodInfo s_getLowerBound = typeof(Array).GetMethod(nameof(Array.GetLowerBound));

            /// <summary>
            /// A <see cref="MethodInfo"/> object representing the <see cref="Array.GetUpperBound(int)"/> method.
            /// </summary>
            public static readonly MethodInfo s_getUpperBound = typeof(Array).GetMethod(nameof(Array.GetUpperBound));
        }

        /// <summary>
        /// Provides a set of helper methods to be able to assign to read-only fields.
        /// </summary>
        private static class FieldHijacker
        {
            /// <summary>
            /// Gets a field setter for the specified field, using a late-bound <see cref="FieldInfo.SetValue(object, object)"/>.
            /// </summary>
            /// <param name="field">The field to get a setter for.</param>
            /// <returns>A setter delegate for the specified field.</returns>
            public static Action<object, object> GetFieldSetter(FieldInfo field)
            {
                //
                // NB: Only reflection is able to assign to a read-only field. The use of LCG fails here
                //     with a VerificationException due to the attempted stfld on the initonly field.
                //

                return field.SetValue;
            }

            //
            // REVIEW: The following method seems worthless. We'll need to box a value prior to passing
            //         it to the first parameter of the delegate, so we'll be mutating a copy.
            //

            /// <summary>
            /// Gets a field setter for the specified field, using a late-bound <see cref="FieldInfo.SetValue(object, object)"/>,
            /// where the target instance is passed by reference.
            /// </summary>
            /// <param name="field">The field to get a setter for.</param>
            /// <returns>A setter delegate for the specified field.</returns>
            public static ByRefFieldSetter GetFieldSetterByRef(FieldInfo field)
            {
                return (ref object obj, object value) => field.SetValue(obj, value);
            }
        }

        /// <summary>
        /// Field setter delegate where the target object is passed by reference.
        /// </summary>
        /// <param name="obj">The target object whose field to set.</param>
        /// <param name="value">The value to assign to the field.</param>
        private delegate void ByRefFieldSetter(ref object obj, object value);
    }
}
