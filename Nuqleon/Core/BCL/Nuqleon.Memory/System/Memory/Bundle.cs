// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 10/27/2014 - Created bundle functionality.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace System.Memory
{
    /// <summary>
    /// Provides a set of factory methods to create bundles of objects which are stored in a memory-efficient manner.
    /// </summary>
    public static partial class Bundle
    {
        private static readonly MethodInfo[] s_createMethods = typeof(Bundle).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.IsGenericMethod && m.Name == nameof(Create)).OrderBy(m => m.GetGenericArguments().Length).ToArray();

        private const int MAX = 16;
        private const int LEN = MAX - 1;

        /// <summary>
        /// Creates a bundle with the specified items.
        /// </summary>
        /// <param name="items">Items to include in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create(params object[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            return CreateImpl(items);
        }

        /// <summary>
        /// Creates a bundle with the specified items.
        /// </summary>
        /// <param name="items">Items to include in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create(IEnumerable<object> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var list = items as IList<object>;
            return CreateImpl(list ?? items.ToArray());
        }

        private static IReadOnlyIndexed CreateImpl(IList<object> items)
        {
            var count = items.Count;

            if (count <= LEN)
            {
                if (count == 0)
                {
                    return EmptyBundle.Instance;
                }

                return CreateImpl(items, 0, count);
            }
            else
            {
                var chunks = count / LEN;
                var index = chunks * LEN;
                var len = count - index;

                if (len == 0)
                {
                    index -= LEN;
                    len = LEN;
                }

                var res = default(object);

                while (index >= 0)
                {
                    res = CreateImpl(items, index, len, res);

                    index -= LEN;
                    len = LEN;
                }

                return (IReadOnlyIndexed)res;
            }
        }

        private static IReadOnlyIndexed CreateImpl(IList<object> items, int index, int length, object rest = null)
        {
            var n = length;
            if (rest != null)
            {
                n++;
            }

            var createMtd = s_createMethods[n - 1];

            var args = new Type[n];
            var values = new object[n];

            for (var i = 0; i < length; i++)
            {
                var value = items[index + i];
                values[i] = value;

                var type = typeof(object);
                if (value != null)
                {
                    type = value.GetType();
                }

                args[i] = type;
            }

            if (rest != null)
            {
                values[length] = rest;
                args[length] = rest.GetType();

                Debug.Assert(rest.GetType().IsValueType);
            }

            var create = createMtd.MakeGenericMethod(args);

            return (IReadOnlyIndexed)create.Invoke(obj: null, values);
        }

        private readonly struct EmptyBundle : IReadOnlyIndexed
        {
            public static readonly IReadOnlyIndexed Instance = new EmptyBundle();

            public object this[int index] => throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}
