// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/10/2017 - Created this type.
//

using System.Collections.Generic;
using System.Memory.Diagnostics;
using System.Reflection;

namespace System.Memory
{
    /// <summary>
    /// Optimizer for object graphs that works by eliminating copies of objects that are considered immutable
    /// and equivalent.
    /// </summary>
    public class HeapOptimizer : HeapEditor
    {
        /// <summary>
        /// Singleton instance of an editor that doesn't make any edits.
        /// </summary>
        private static readonly HeapEditor s_nop = new NopEditor();

        /// <summary>
        /// The <see cref="MethodInfo"/> object representing the open generic <see cref="CreateEditorCore{T}"/> method.
        /// </summary>
        private static readonly MethodInfo s_create = typeof(HeapOptimizer).GetMethod(nameof(CreateEditorCore), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Dictionary mapping types onto editors. If a type does not have an editor, an entry with <see cref="s_nop"/>
        /// is put in the dictionary as to avoid re-resolving an editor.
        /// </summary>
        private readonly Dictionary<Type, HeapEditor> _editors = new();

        /// <summary>
        /// Replaces the specified object in <paramref name="obj"/> by a shared copy if its type passes
        /// the <see cref="IsImmutable(Type)"/> check and another instance of the type has been found
        /// that's deemed equivalent according to <see cref="GetEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="obj">The object to optionally replace.</param>
        /// <returns>The original object or a replacement that's deemed equivalent.</returns>
        public override object Edit(object obj)
        {
            if (obj != null)
            {
                var type = obj.GetType();

                if (!_editors.TryGetValue(type, out var editor))
                {
                    if (IsImmutable(type))
                    {
                        editor = CreateEditor(type);
                    }
                    else
                    {
                        editor = s_nop;
                    }

                    _editors.Add(type, editor);
                }

                return editor.Edit(obj);
            }

            return obj;
        }

        /// <summary>
        /// Checks if instances of the specified <paramref name="type"/> are immutable.
        /// </summary>
        /// <param name="type">The type to check for immutability.</param>
        /// <returns><c>true</c> if instances of the type are immutable; otherwise, <c>false</c>.</returns>
        protected virtual bool IsImmutable(Type type)
        {
            if (type != null && (type.IsPrimitive || type == typeof(string)))
            {
                return true;
            }

            // TODO: support generics

            return false;
        }

        /// <summary>
        /// Gets an equality comparer for the type specified in <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get an equality comparer for.</typeparam>
        /// <returns>An equality comparer for the type specified in <typeparamref name="T"/>.</returns>
        protected virtual IEqualityComparer<T> GetEqualityComparer<T>()
        {
            return EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Creates an editor for the specified <paramref name="type"/>. A call to this method only takes place
        /// when <see cref="_editors"/> does not yet have an entry for the type and the type is considered to
        /// be immutable according to <see cref="IsImmutable(Type)"/>.
        /// </summary>
        /// <param name="type">The type to create an editor for.</param>
        /// <returns>An editor for the specified <paramref name="type"/>.</returns>
        private HeapEditor CreateEditor(Type type)
        {
            try
            {
                return (HeapEditor)s_create.MakeGenericMethod(type).Invoke(this, parameters: null);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        /// <summary>
        /// Creates a strongly typed editor for the type specified in <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to create an editor for.</typeparam>
        /// <returns>An editor for the type specified in <typeparamref name="T"/>.</returns>
        private HeapEditor CreateEditorCore<T>()
        {
            var eq = GetEqualityComparer<T>();

            return new Editor<T>(eq);
        }

        /// <summary>
        /// Strongly typed implementation of an editor for instances of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object instances being edited.</typeparam>
        private sealed class Editor<T> : HeapEditor
        {
            /// <summary>
            /// Dictionary mapping object instances on shared copies. Upon encountering an instance of the type,
            /// this dictionary is checked to determine if an equivalent instance of the type exists. If so, the
            /// original instance is replaced by the equivalent shared instance. If not, the first occurrence of
            /// the instance is added to the dictionary.
            /// </summary>
            private readonly Dictionary<T, T> _map;

            /// <summary>
            /// Creates a new editor using the specified <paramref name="comparer"/> to check whether instances
            /// of the type are deemed equivalent.
            /// </summary>
            /// <param name="comparer">The compare used to determine whether instances are equivalent.</param>
            public Editor(IEqualityComparer<T> comparer)
            {
                _map = new Dictionary<T, T>(comparer);
            }

            /// <summary>
            /// Optionally replaces the specified object by a shared copy.
            /// </summary>
            /// <param name="obj">The object to replace.</param>
            /// <returns>The original object if no equivalent instance is found; otherwise, a replacement.</returns>
            public override object Edit(object obj) => Edit((T)obj);

            /// <summary>
            /// Strongly typed implemented of <see cref="Edit(object)"/>.
            /// </summary>
            /// <param name="obj">The object to replace.</param>
            /// <returns>The original object if no equivalent instance is found; otherwise, a replacement.</returns>
            private T Edit(T obj)
            {
                if (!_map.TryGetValue(obj, out var res))
                {
                    res = obj;
                    _map.Add(obj, obj);
                }

                return res;
            }
        }

        /// <summary>
        /// Implementation of a <see cref="HeapEditor"/> that doesn't make any edits.
        /// </summary>
        private sealed class NopEditor : HeapEditor
        {
            /// <summary>
            /// No-op.
            /// </summary>
            /// <param name="obj">The object passed to edit.</param>
            /// <returns>The original object, <paramref name="obj"/>.</returns>
            public override object Edit(object obj) => obj;
        }
    }
}
